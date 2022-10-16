import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { CdkDrag, CdkDragDrop, CdkDropList, moveItemInArray, transferArrayItem } from "@angular/cdk/drag-drop";
import { MessageService } from "primeng/api";
import { SektionElKomponentDto } from "@apiModels/sektionElKomponentDto";
import { EltavleService } from "@core/services/eltavle.service";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { cloneObject } from "@globals/helper-functions";
import { ActivatedRoute, Router } from "@angular/router";
import { switchMap, take } from "rxjs";
import * as htmlToImage from "html-to-image";
import { FormControl, FormGroup } from "@angular/forms";
import { ElTavleDto } from "@apiModels/elTavleDto";
import { BiLocalizationHelperService } from "@core/utility-services/bi-localization-helper.service";

@UntilDestroy()
@Component({
  selector: "app-drag-drop",
  templateUrl: "./switchboard-edit-configuration.component.html",
  styleUrls: ["./switchboard-edit-configuration.component.scss"]
})
export class SwitchboardEditConfigurationComponent implements OnInit {
  constructor(
    private messageService: MessageService,
    private eltavleService: EltavleService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    private cd: ChangeDetectorRef,
    private localizor: BiLocalizationHelperService
  ) {}

  public komponentLines: number;
  public lineWidth: number;
  public numberOfLines: number[];
  public lines: SektionElKomponentDto[] = [];
  public tavleId: number;
  public extendToLines = 0;
  public mainForm: FormGroup;
  public selectedItem: SektionElKomponentDto;
  public eltavle: ElTavleDto;

  public komponentPlaceringer: SektionElKomponentDto[] = [];
  public lastValidKomponentPlaceringer: SektionElKomponentDto[] = [];

  public disp: SektionElKomponentDto[] = [];

  public displayPrintDialog = false;
  public dataUrl = "";
  public dispCss = "component-box";

  private initDisp() {
    this.disp = [
      { id: 0, line: 0, modul: 1, komponentNavn: "Disp. plads 1M", komponentID: 83, erExtraDisp: true, angivetNavn: false, navn: "" },
      { id: 0, line: 0, modul: 2, komponentNavn: "Disp. plads 2M", komponentID: 44, erExtraDisp: true, angivetNavn: false, navn: "" },
      { id: 0, line: 0, modul: 4, komponentNavn: "Disp. plads 4M", komponentID: 45, erExtraDisp: true, angivetNavn: false, navn: "" }
    ];
    console.log("init disp");
  }
  public someOtherVariable: SektionElKomponentDto[][] = [];

  ngOnInit() {
    this.activeRoute.parent.params
      .pipe(
        untilDestroyed(this),
        take(1),
        switchMap(params => {
          this.tavleId = +params["id"];
          return this.eltavleService.getEltavleConfiguration(this.tavleId);
        })
      )
      .subscribe(data => {
        this.initDisp();
        if (data) {
          this.komponentLines = data.antalSkinner;
          this.lineWidth = data.modulPrSkinne;
          this.orderLines(data.komponenter);
          this.eltavle = data.elTavle;
          this.getSortedLines();
        }
      });

    this.initFormGroup();
  }

  public orderLines(lines: SektionElKomponentDto[]) {
    let line = 1;
    let width = 0;
    let sektionNr = 1;

    lines.forEach(item => {
      if (item.id != 0) {
        sektionNr = item.sektion;
      }

      if (item.id === 0) {
        item.sektion = sektionNr;
      }

      if (width + item.modul < this.lineWidth + 1) {
        item.line = line;
        width += item.modul;
      } else {
        line++;
        item.line = line;
        width = item.modul;
      }
    });
    this.komponentPlaceringer = lines;

    if (line > this.komponentLines) {
      this.extendToLines = line;
      alert("Komponenterne kan ikke være på de nuværende skinner, udvid venligst!");
    }

    for (let index = 0; index < this.komponentLines; index++) {
      this.someOtherVariable[index] = this.komponentPlaceringer.filter(f => f.line == index + 1);
    }

    this.someOtherVariable = this.someOtherVariable.slice().reverse();
  }

  private getSortedLines() {
    const komponentPlaceringer: SektionElKomponentDto[] = [];

    let lineNo = 1;
    let lineWidth = 0;

    for (let i = this.someOtherVariable.length - 1; i >= 0; i--) {
      const line = this.someOtherVariable[i];

      for (let j = 0; j < line.length; j++) {
        const item = line[j];
        if (item.modul + lineWidth < this.lineWidth + 1) {
          lineWidth += item.modul;
        } else {
          lineNo++;
          lineWidth = item.modul;
          if (this.komponentLines && lineNo > this.komponentLines) {
            this.messageService.add({
              severity: "error",
              summary: "Komponenten kunne ikke placeres!",
              detail: "Komponenten kan ikke placeres på den ønskede position, da der ikke er plads nok i tavlen"
            });
            this.orderLines(this.lastValidKomponentPlaceringer);
            return;
          }
        }
        item.line = lineNo;
        komponentPlaceringer.push(item);
      }
    }

    this.lastValidKomponentPlaceringer = cloneObject(komponentPlaceringer);

    this.orderLines(komponentPlaceringer);
  }

  public onUdvidClicked(): void {
    const moduler = this.extendToLines * this.lineWidth;

    this.eltavleService.updateFrame(this.tavleId, moduler).subscribe({
      next: () => {
        this.messageService.add({
          severity: "normal",
          summary: "Tavlen er nu blevet udvidet",
          detail: "Tavlen er nu blevet udvidet!"
        });

        this.router.navigateByUrl("/eltavler", { skipLocationChange: true }).then(() => this.router.navigate(["/switchboards", this.tavleId]));
      },
      error: err => console.log(err)
    });
  }

  public onSaveClicked(refresh: boolean): void {
    this.eltavleService.gemKomponentPlaceringer(this.tavleId, this.lastValidKomponentPlaceringer).subscribe({
      next: () => {
        this.messageService.add({
          severity: "normal",
          summary: "Data er gemt",
          detail: "Data er nu blevet gemt!"
        });

        if (refresh) {
          this.router.navigateByUrl("/eltavler", { skipLocationChange: true }).then(() => this.router.navigate(["/switchboards", this.tavleId]));
        } else {
          this.router.navigate(["/eltavler", this.tavleId, "parts"]);
        }
      },
      error: err => console.log(err)
    });
  }

  add() {
    this.lines.push({ line: 1, modul: 500, komponentNavn: "500" });
  }

  move() {
    transferArrayItem(this.someOtherVariable[0], this.someOtherVariable[1], this.someOtherVariable[0].length, 0);
  }

  shuffle() {
    this.lines.sort(function () {
      return 0.5 - Math.random();
    });
  }

  getDivWidth(moduler: number) {
    return moduler * 3 + "vw";
  }

  canDrop(item: CdkDrag, list: CdkDropList) {
    console.log(item && item.data);
    console.log(list && list.getSortedItems().length && list.getSortedItems()[0].dropContainer.data.reduce((sum, current) => sum + current.width, 0));
    return list && list.getSortedItems().length && list.getSortedItems().length > 3;
  }

  generateImage() {
    this.dispCss = "component-box-hidden";
    this.cd.detectChanges();

    this.displayPrintDialog = true;
    const node: HTMLElement = document.getElementById("tavle");

    // const filter = node => {
    //   if (!node.classList) return true;
    //   const exclusionClasses = ["disp"];
    //   return !exclusionClasses.some(classname => node.classList.contains(classname));
    // };

    htmlToImage
      .toPng(node)
      //.toPng(node, { filter: filter }) //,
      .then(function (dataUrl) {
        //this.dataUrl = dataUrl;
        const img = document.getElementById("image") as HTMLImageElement;
        img.src = dataUrl;
        // document.body.appendChild(img);
      })
      .catch(function (error) {
        console.error("oops, something went wrong!", error);
      });
  }

  drop(event: CdkDragDrop<string[]>) {
    // console.log((event.container.data as unknown as ComponentDetails[]).reduce((sum, current) => sum + current.width, 0))
    // console.log(event.previousIndex)

    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      //const source = event.previousContainer.data as unknown as ComponentDetails[];
      // const itemWidth = source[event.previousIndex].width;
      // const targetWidth = (event.container.data as unknown as ComponentDetails[]).reduce((sum, current) => sum + current.width, 0);

      // if (itemWidth + targetWidth > 12) {
      //   //no room on target
      //   this.messageService.add({ severity: "error", summary: "Komponenten kunne ikke placeres!", detail: "Komponenten kan ikke placeres på den ønskede position, da der ikke er plads nok" });
      //   return;
      // }

      //  const source = event.previousContainer.data as unknown as ComponentDetails[];
      //  const item = source[event.previousIndex];
      // item.line = this.someOtherVariable.length - (+event.container.id.substring(14));

      if (event.container.id === "cdk-drop-list-0") {
        const source = event.previousContainer.data as unknown as SektionElKomponentDto[];
        const item = source[event.previousIndex];

        if (!item.erExtraDisp) {
          this.messageService.add({
            severity: "error",
            summary: "Kan ikke slettes",
            detail: "Det er kun disponible komponenter kan fjernes!"
          });
          this.initDisp();
          return;
        }
      }

      transferArrayItem(event.previousContainer.data, event.container.data, event.previousIndex, event.currentIndex);
    }

    // if (event.previousContainer.id === "cdk-drop-list-0" || event.container.id === "cdk-drop-list-0") {
    //   this.initDisp();
    // }
    this.initDisp();
    this.getSortedLines();
  }

  public handleClick(item: SektionElKomponentDto): void {
    this.mainForm.reset();
    this.selectedItem = item;
    this.navn.setValue(this.selectedItem?.navn);
    this.angivetNavn.setValue(this.selectedItem?.angivetNavn);
    if (!this.selectedItem.angivetNavn) {
      this.navn.disable();
    } else {
      this.navn.enable();
    }
  }

  private initFormGroup() {
    this.mainForm = new FormGroup({
      navn: new FormControl(),
      angivetNavn: new FormControl(false)
    });
  }

  public get navn() {
    return this.mainForm.get("navn");
  }

  public get angivetNavn() {
    return this.mainForm.get("angivetNavn");
  }

  onAngivetNavnChanged(event) {
    if (!event.checked) {
      this.navn.setValue(this.selectedItem?.serverNavn);
      this.navn.disable();
      this.cd.detectChanges();
    } else {
      this.navn.enable();
      this.cd.detectChanges();
    }
  }

  saveItem() {
    this.selectedItem.navn = this.navn.value;
    this.selectedItem.angivetNavn = this.angivetNavn.value;

    this.mainForm.reset();
    this.selectedItem = null;
    this.getSortedLines();
  }

  public bestiltDato() {
    if (this.eltavle === null) return "";
    return this.localizor.formatUtcDateTime(this.eltavle?.beregnetDato, null, null, "DD/MM/YYYY");
  }

  public fabrikat() {
    if (!this.eltavle) return "";
    switch (this.eltavle.tavlefabrikatID) {
      case 1: {
        return "LK UG 150";
      }
      case 2: {
        return "LK PGE planforsænket med låg";
      }
      case 3: {
        return "Hager Gamma";
      }
      case 4: {
        return "Hager planforsænket med låg";
      }
      case 5: {
        return "Schneider Resi9";
      }
      case 6: {
        return "Hager Vector IP 65";
      }
      default: {
        return "";
      }
    }
    return "";
  }
}

function tab(): import("rxjs").OperatorFunction<SektionElKomponentDto[], unknown> {
  throw new Error("Function not implemented.");
}
/**  Copyright 2022 Google LLC. All Rights Reserved.
    Use of this source code is governed by an MIT-style license that
    can be found in the LICENSE file at https://angular.io/license */
