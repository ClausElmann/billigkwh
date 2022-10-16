import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { CdkDrag, CdkDragDrop, CdkDropList, moveItemInArray, transferArrayItem } from "@angular/cdk/drag-drop";
import { MessageService } from "primeng/api";
import { LaageElKomponentDto } from "@apiModels/laageElKomponentDto";
import { EltavleService } from "@core/services/eltavle.service";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ActivatedRoute, Router } from "@angular/router";
import { switchMap, take } from "rxjs";
import * as htmlToImage from "html-to-image";
import { FormControl, FormGroup } from "@angular/forms";
import { ElTavleDto } from "@apiModels/elTavleDto";
import { BiLocalizationHelperService } from "@core/utility-services/bi-localization-helper.service";

export interface NotPlacedComponent {
  komponentID?: number;
  modul?: number;
  komponentNavn?: string;
  antal: number;
}

export enum KredsKomponentKategori {
  DISP = -1,
  HPFI = 1,
  Kombirelæ = 2,
  Automat_sikring_3p = 3,
  Sikring = 4,
  Måler = 5,
  Transient_beskyttelse = 6,
  Energi_maaler = 7,
  Automat_sikring_1p = 8,
  Ur = 10,
  Kontaktor = 11,
  Kiprelæ = 12,
  MCB_lille = 19,
  MCB = 20
}

@UntilDestroy()
@Component({
  selector: "app-drag-drop",
  templateUrl: "./circuit-edit-configuration.component.html",
  styleUrls: ["./circuit-edit-configuration.component.scss"]
})
export class CircuitEditConfigurationComponent implements OnInit {
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
  public lines: LaageElKomponentDto[] = [];
  public laageId: number;
  public tavleId: number;
  public extendToLines = 0;
  public mainForm: FormGroup;
  public selectedItem: LaageElKomponentDto;
  public eltavle: ElTavleDto;
  public komponenter = "";
  public komponentPlaceringer: LaageElKomponentDto[] = [];
  public lastValidKomponentPlaceringer: LaageElKomponentDto[] = [];
  public restKomponenter: NotPlacedComponent[] = [];
  public tavleKomponenter: LaageElKomponentDto[] = [];

  public disp: LaageElKomponentDto[] = [];

  public displayPrintDialog = false;
  public indsaetKomponenterDialog = false;
  public dataUrl = "";
  public dispCss = "component-box";
  public errorMessage = "";

  private initDisp() {
    this.disp = [
      { id: 0, line: 0, modul: 1, komponentNavn: "Disp. plads 1M", komponentID: 83, erExtraDisp: true, angivetNavn: false, navn: "" },
      { id: 0, line: 0, modul: 2, komponentNavn: "Disp. plads 2M", komponentID: 44, erExtraDisp: true, angivetNavn: false, navn: "" },
      { id: 0, line: 0, modul: 4, komponentNavn: "Disp. plads 4M", komponentID: 45, erExtraDisp: true, angivetNavn: false, navn: "" }
    ];
    console.log("init disp");
  }
  //public someOtherVariable: LaageElKomponentDto[][] = [];

  ngOnInit() {
    this.activeRoute.parent.params
      .pipe(
        untilDestroyed(this),
        take(1),
        switchMap(params => {
          this.laageId = +params["id"];
          return this.eltavleService.getEltavleLaageConfiguration(this.laageId);
        })
      )
      .subscribe(data => {
        this.initDisp();
        if (data) {
          //this.komponentLines = data.antalSkinner;
          //this.lineWidth = data.modulPrSkinne;
          //this.orderLines(data.komponenter.filter(komponent => komponent.elTavleLaageID !== null));
          this.komponentPlaceringer = data.komponenter.filter(komponent => komponent.elTavleLaageID !== null);
          this.tavleId = data.elTavle.id;
          this.tavleKomponenter = data.komponenter; //.filter(komponent => komponent.elTavleLaageID === null);
          this.restKomponenter = [];
          const flags = [];
          for (let i = 0; i < this.tavleKomponenter.length; i++) {
            const comp = this.tavleKomponenter[i];
            if (flags[comp.komponentID]) {
              const foundIndex = this.restKomponenter.findIndex(x => x.komponentID == comp.komponentID);
              const item = this.restKomponenter[foundIndex];

              if (comp.elTavleLaageID === null) {
                item.antal++;
              }
              this.restKomponenter[foundIndex] = item;
              continue;
            }
            flags[this.tavleKomponenter[i].komponentID] = true;
            this.restKomponenter.push({ modul: comp.modul, komponentNavn: comp.komponentNavn, komponentID: comp.komponentID, antal: comp.elTavleLaageID === null ? 1 : 0 });
          }

          //console.log(this.tavleKomponenter);

          this.eltavle = data.elTavle;
          //this.getSortedLines();
        }
      });

    this.initFormGroup();
  }

  // public orderLines(lines: LaageElKomponentDto[]) {
  //   let line = 1;
  //   let width = 0;
  //   let sektionNr = 1;

  //   lines.forEach(item => {
  //     if (item.id != 0) {
  //       sektionNr = item.tavleSektionNr;
  //     }

  //     if (item.id === 0) {
  //       item.tavleSektionNr = sektionNr;
  //     }

  //     if (width + item.modul < this.lineWidth + 1) {
  //       item.line = line;
  //       width += item.modul;
  //     } else {
  //       line++;
  //       item.line = line;
  //       width = item.modul;
  //     }
  //   });
  //   this.komponentPlaceringer = lines;

  //   if (line > this.komponentLines) {
  //     this.extendToLines = line;
  //     alert("Komponenterne kan ikke være på de nuværende skinner, udvid venligst!");
  //   }

  //   for (let index = 0; index < this.komponentLines; index++) {
  //     this.someOtherVariable[index] = this.komponentPlaceringer.filter(f => f.line == index + 1);
  //   }

  //   this.someOtherVariable = this.someOtherVariable.slice();
  // }

  // private getSortedLines() {
  //   const komponentPlaceringer: LaageElKomponentDto[] = [];

  //   let lineNo = 1;
  //   let lineWidth = 0;

  //   for (let i = 0; i < this.someOtherVariable.length; i++) {
  //     const line = this.someOtherVariable[i];

  //     for (let j = 0; j < line.length; j++) {
  //       const item = line[j];
  //       if (item.modul + lineWidth < this.lineWidth + 1) {
  //         lineWidth += item.modul;
  //       } else {
  //         lineNo++;
  //         lineWidth = item.modul;
  //         if (this.komponentLines && lineNo > this.komponentLines) {
  //           this.messageService.add({
  //             severity: "error",
  //             summary: "Komponenten kunne ikke placeres!",
  //             detail: "Komponenten kan ikke placeres på den ønskede position, da der ikke er plads nok i tavlen"
  //           });
  //           this.orderLines(this.lastValidKomponentPlaceringer);
  //           return;
  //         }
  //       }
  //       item.line = lineNo;
  //       komponentPlaceringer.push(item);
  //     }
  //   }

  //   this.lastValidKomponentPlaceringer = cloneObject(komponentPlaceringer);

  //   this.orderLines(komponentPlaceringer);
  // }

  public onUdvidClicked(): void {
    const moduler = this.extendToLines * this.lineWidth;

    this.eltavleService.updateFrame(this.laageId, moduler).subscribe({
      next: () => {
        this.messageService.add({
          severity: "normal",
          summary: "Tavlen er nu blevet udvidet",
          detail: "Tavlen er nu blevet udvidet!"
        });

        this.router.navigateByUrl("/eltavler", { skipLocationChange: true }).then(() => this.router.navigate(["/circuits", this.laageId]));
      },
      error: err => console.log(err)
    });
  }

  public onSaveClicked(refresh: boolean): void {
    //debugger;
    this.eltavleService.gemLaageKomponentPlaceringer(this.laageId, this.komponentPlaceringer).subscribe({
      next: () => {
        this.messageService.add({
          severity: "normal",
          summary: "Data er gemt",
          detail: "Data er nu blevet gemt!"
        });

        if (refresh) {
          this.router.navigateByUrl("/eltavler", { skipLocationChange: true }).then(() => this.router.navigate(["/circuits", this.laageId]));
        } else {
          this.router.navigateByUrl("/eltavler", { skipLocationChange: true }).then(() => this.router.navigate(["/circuits", this.laageId]));
          //      this.router.navigate(["/eltavler", this.laageId, "parts"]);
        }
      },
      error: err => console.log(err)
    });
  }

  clickme(value: any) {
    console.log(value);
  }

  add(count: number, data: NotPlacedComponent) {
    //this.lines.push({ line: 1, modul: 500, komponentNavn: "500" });
    //debugger;
    for (let index = 0; index < count; index++) {
      const foundKomponentIndex = this.tavleKomponenter.findIndex(x => x.elTavleLaageID === null && x.komponentID == data.komponentID);
      const foundRestIndex = this.restKomponenter.findIndex(x => x.komponentID == data.komponentID);

      const foundKomponentItem = this.tavleKomponenter[foundKomponentIndex];
      const foundRestItem = this.restKomponenter[foundRestIndex];

      foundRestItem.antal--;
      // if (foundRestItem.antal === 0) this.restKomponenter.splice(foundRestIndex, 1);
      // else
      this.restKomponenter[foundRestIndex] = foundRestItem;

      //this.someOtherVariable[this.someOtherVariable.length - 1].push(foundKomponentItem);

      //this.tavleKomponenter[foundKomponentIndex] = foundKomponentItem;

      this.komponentPlaceringer.push(foundKomponentItem);

      //this.tavleKomponenter.push(foundKomponentItem);
      //this.tavleKomponenter.splice(foundKomponentIndex, 1);

      foundKomponentItem.elTavleLaageID = this.laageId;
      this.tavleKomponenter[foundKomponentIndex] = foundKomponentItem;

      // const item = this.someOtherVariable[this.someOtherVariable.length - 1].push({
      //   id: 0,
      //   line: this.someOtherVariable.length - 1,
      //   modul: 2,
      //   komponentNavn: "TESTER",
      //   komponentID: 83,
      //   erExtraDisp: true,
      //   angivetNavn: false,
      //   navn: ""
      // });
    }

    //this.getSortedLines();
    this.resort();
    this.cd.detectChanges();
  }

  rest() {
    return this.restKomponenter.filter(f => f.antal > -1);
  }

  remove(komponent: LaageElKomponentDto) {
    // debugger;

    const foundIndex = this.komponentPlaceringer.findIndex(x => x.id == komponent.id);
    const foundKomponentIndex = this.tavleKomponenter.findIndex(x => x.id == komponent.id);
    const foundRestIndex = this.restKomponenter.findIndex(x => x.komponentID == komponent.komponentID);

    const foundKomponentItem = this.tavleKomponenter[foundKomponentIndex];
    const foundRestItem = this.restKomponenter[foundRestIndex];

    foundKomponentItem.elTavleLaageID = null;
    this.tavleKomponenter[foundKomponentIndex] = foundKomponentItem;

    if (foundRestItem) {
      foundRestItem.antal++;
    } else {
      //const x: NotPlacedComponent;
      this.restKomponenter.push({ modul: foundKomponentItem.modul, komponentNavn: foundKomponentItem.komponentNavn, komponentID: foundKomponentItem.komponentID, antal: 1 });
      //this.restKomponenter.push(foundKomponentItem);
    }

    foundKomponentItem.elTavleLaageID = null;

    this.komponentPlaceringer.splice(foundIndex, 1);

    //this.tavleKomponenter.push(foundKomponentItem);
    //this.tavleKomponenter.splice(foundKomponentIndex, 1);

    // remove from list at add to not placed components.
    this.resort();
    this.cd.detectChanges();
  }

  result(items: LaageElKomponentDto[]) {
    return items.reduce((sum, current) => sum + current.modul, 0);
  }

  move() {
    //transferArrayItem(this.someOtherVariable[0], this.someOtherVariable[1], this.someOtherVariable[0].length, 0);
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

  resort() {
    //debugger;
    let lastSektionNr = 0;

    let q = 1;
    let f = 1;
    //          int qf = 1;
    //let p = 1;
    let d = 1;
    let row = 1;
    let lastIsComp = false;

    for (let index = 0; index < this.komponentPlaceringer.length; index++) {
      const element = this.komponentPlaceringer[index];

      const sektionNr = lastSektionNr;

      switch (element.komponentKategoriId) {
        case KredsKomponentKategori.DISP:
          break;
        case KredsKomponentKategori.HPFI:
          lastSektionNr++;
          element.navn = `Q${lastSektionNr}`;
          row = 1;
          lastIsComp = false;
          q++;
          break;
        case KredsKomponentKategori.Kombirelæ:
          lastSektionNr++;
          element.navn = `QF${lastSektionNr}`;
          row = 1;
          lastIsComp = false;
          q++;
          break;
        case KredsKomponentKategori.Automat_sikring_3p:
        case KredsKomponentKategori.Automat_sikring_1p:
          element.navn = `F${lastSektionNr}.${f}`;
          row = 2;
          lastIsComp = false;
          f++;
          break;
        case KredsKomponentKategori.Sikring:
          break;
        case KredsKomponentKategori.Måler:
          break;
        case KredsKomponentKategori.Transient_beskyttelse:
          break;
        case KredsKomponentKategori.Energi_maaler:
          element.navn = `D${lastSektionNr}${f > 1 ? "." + (f - 1) : ""}.${d}`;
          d++;
          break;
        case KredsKomponentKategori.Ur:
          element.navn = `D${lastSektionNr}${f > 1 ? "." + (f - 1) : ""}.${d}`;
          if (!lastIsComp) row++;
          lastIsComp = true;
          d++;
          break;
        case KredsKomponentKategori.Kontaktor:
          break;
        case KredsKomponentKategori.Kiprelæ:
          break;
        case KredsKomponentKategori.MCB_lille:
          break;
        case KredsKomponentKategori.MCB:
          break;
        default:
          break;
      }

      element.row = row;
      element.tavleSektionNr = lastSektionNr;

      if (sektionNr != lastSektionNr) {
        q = 1;
        f = 1;
        //qf = 1;
        //p = 1;
        d = 1;
      }
    }
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
      //debugger;
      if (event.container.id === "cdk-drop-list-0") {
        const source = event.previousContainer.data as unknown as LaageElKomponentDto[];
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
    //this.getSortedLines();
  }

  dropA(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.komponentPlaceringer, event.previousIndex, event.currentIndex);

    this.resort();
  }

  public handleClick(item: LaageElKomponentDto): void {
    this.remove(item);
    return;

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
    //this.getSortedLines();
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

  importKomponenter() {
    this.eltavleService.importKomponenter(this.eltavle.id, this.komponenter).subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Data blev importeret"
        });

        this.indsaetKomponenterDialog = false;
        this.mainForm.reset();
        this.router.navigateByUrl("/", { skipLocationChange: true }).then(() => this.router.navigate(["/circuits/", this.laageId]));
      },
      error: err => (this.errorMessage = err)
    });
  }

  showItemDialog(event) {
    console.log(event);
  }
}

function tab(): import("rxjs").OperatorFunction<LaageElKomponentDto[], unknown> {
  throw new Error("Function not implemented.");
}
/**  Copyright 2022 Google LLC. All Rights Reserved.
    Use of this source code is governed by an MIT-style license that
    can be found in the LICENSE file at https://angular.io/license */
