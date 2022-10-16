import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ConfirmationService, MessageService, SelectItem } from "primeng/api";
import { BehaviorSubject, finalize, map, Observable, take, tap, switchMap } from "rxjs";
import { ActivatedRoute, Router } from "@angular/router";
import { EltavleService } from "@core/services/eltavle.service";
import { NavigationService } from "@core/services/navigation.service";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { PrimeNgUtilities } from "@shared/variables-and-functions/primeNg-utilities";
import { SektionElKomponentDto } from "@apiModels/sektionElKomponentDto";

@UntilDestroy()
@Component({
  selector: "app-gruppetavle-komponenter",
  templateUrl: "./gruppetavle-detalje-komponenter.component.html",
  providers: [MessageService, ConfirmationService]
})
export class GruppetavleDetaljeKomponenterComponent implements OnInit {
  public loading = true;

  private komponenter = new BehaviorSubject<SektionElKomponentDto[]>([]);
  public komponenter$: Observable<SektionElKomponentDto[]> = this.komponenter.asObservable();
  cols: any[];
  showDeleted: boolean;

  public invoicingDone = false;

  public selectedItem: SektionElKomponentDto;
  public mainForm: FormGroup;
  public showFormErrorMessage: boolean;
  editDialog: boolean;
  deleteDialog = false;
  public errorMessage = "";
  public exportCSV = PrimeNgUtilities.exportCSV;

  constructor(
    private eltavleService: EltavleService,
    private activeRoute: ActivatedRoute,
    private navigation: NavigationService,
    private cd: ChangeDetectorRef,
    private messageService: MessageService,
    private router: Router
  ) {}

  ngOnInit() {
    this.showDeleted = false;

    this.activeRoute.parent.params
      .pipe(
        untilDestroyed(this),
        take(1),
        switchMap(params => {
          return this.eltavleService.getEltavle(+params["id"]);
        })
      )
      .subscribe(data => {
        if (data) {
          this.invoicingDone = data.economicBookedInvoiceNumber !== null;
        }
      });

    this.initializeSektionKomponenter();

    this.cols = [
      { field: "sektion", header: "Sektion nr" },
      { field: "navn", header: "Navn" },
      { field: "komponentNavn", header: "Komponent" },
      { field: "bruttoPris", header: "Pris stk." }
    ];
  }

  onRowSelect(event) {
    this.router.navigate(["switchboards", this.activeRoute.parent.snapshot.params.id]);
  }

  public elKomponenter$ = this.eltavleService.alleKomponenter().pipe(
    take(1),
    map(types => {
      const selectItems = types.map(t => <SelectItem>{ label: t.navn, value: t.id });
      return selectItems;
    })
  );

  public sektioner$ = this.eltavleService.alleSektioner(this.activeRoute.parent.snapshot.params.id).pipe(
    take(1),
    map(types => {
      const selectItems = types.map(t => <SelectItem>{ label: t.placering.toString(), value: t.id });
      // selectItems.unshift({
      //   label: "Ikke i sektion",
      //   value: null
      // });

      return selectItems;
    })
  );

  // public sektioner: Array<SelectItem> = [
  //   // { value: null, label: "Vælg sprog" },
  //   { value: 1, label: "Dansk" },
  //   { value: 2, label: "Svensk" },
  //   { value: 3, label: "Finsk" },
  //   { value: 4, label: "Norsk" }
  // ];

  // public elKomponenter: Array<SelectItem> = [
  //   // { value: null, label: "Vælg land" },
  //   { value: 1, label: "Danmark" },
  //   { value: 2, label: "Sverige" },
  //   { value: 3, label: "Finland" },
  //   { value: 4, label: "Norge" }
  // ];

  private initFormGroup() {
    this.mainForm = new FormGroup({
      komponentID: new FormControl(this.selectedItem.komponentID, [Validators.required]),
      elTavleSektionID: new FormControl(this.selectedItem.elTavleSektionID, [Validators.required]),
      navn: new FormControl(this.selectedItem.navn),
      angivetNavn: new FormControl(this.selectedItem.angivetNavn)
    });
  }

  public get komponentID() {
    return this.mainForm.get("komponentID");
  }

  public get elTavleSektionID() {
    return this.mainForm.get("elTavleSektionID");
  }

  public get navn() {
    return this.mainForm.get("navn");
  }

  public get angivetNavn() {
    return this.mainForm.get("angivetNavn");
  }

  onAngivetNavnChanged(event) {
    if (!event.checked) {
      this.navn.setValue(this.selectedItem.serverNavn);
      this.navn.disable();
      //this.cd.detectChanges();
    } else {
      this.navn.enable();
      //this.cd.detectChanges();
    }
  }

  private checkAndValidateForm() {
    if (this.mainForm.invalid) {
      Object.keys(this.mainForm.controls).forEach(cName => this.mainForm.controls[cName].markAsTouched());
      this.showFormErrorMessage = true;
      return false;
    }

    this.showFormErrorMessage = false;
    return true;
  }

  editItem(item: SektionElKomponentDto) {
    this.selectedItem = { ...item };
    this.initFormGroup();
    this.editDialog = true;
  }

  deleteItem(item: SektionElKomponentDto) {
    this.deleteDialog = true;
    this.selectedItem = { ...item };
  }

  addItem() {
    this.selectedItem = this.initializeSektionKomponent();
    this.initFormGroup();
    this.editDialog = true;
  }

  public initializeSektionKomponent(): SektionElKomponentDto {
    // Return an initialized object
    return {
      id: 0,
      navn: "",
      angivetNavn: false,
      elTavleID: this.activeRoute.parent.snapshot.params.id
    };
  }

  public back(): void {
    this.navigation.back();
  }

  public onShowActiveChange(checked: boolean) {
    if (checked) {
      this.showDeleted = true;
      this.initializeSektionKomponenter();
    } else {
      this.showDeleted = false;
      this.initializeSektionKomponenter();
    }
  }

  // private initializeUsers() {
  //   this.komponenter$ = this.eltavleService.getAllSektionElKomponents(this.activeRoute.parent.snapshot.params.id).pipe(
  //     untilDestroyed(this),
  //     finalize(() => (this.loading = false))
  //   );
  // }

  private initializeSektionKomponenter() {
    this.komponenter$ = this.eltavleService.getAllSektionElKomponents(this.activeRoute.parent.snapshot.params.id).pipe(
      tap((data: Array<SektionElKomponentDto>) => {
        data.forEach(element => {
          //
        });
        this.komponenter.next(data);
      }),
      untilDestroyed(this),
      finalize(() => (this.loading = false))
    );
  }

  confirmDelete() {
    this.eltavleService.deleteSektionKomponent(this.selectedItem).subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Data blev slettet"
        });
        // const index = this.komponenter.value.findIndex(r => r.id === this.selectedItem.id);

        // if (index > -1) {
        //   this.komponenter.value.splice(index, 1);
        //   this.komponenter.next(this.komponenter.value);
        // }

        this.initializeSektionKomponenter();
        this.deleteDialog = false;
        this.messageService.add({ severity: "success", summary: "Successful", detail: "Product Deleted", life: 3000 });
        this.selectedItem = {};
        this.cd.detectChanges();
      },
      error: err => (this.errorMessage = err)
    });
  }

  hideDialog() {
    this.editDialog = false;
  }

  saveItem() {
    if (!this.checkAndValidateForm()) return;

    this.sektioner$.subscribe(sektionListe => {
      this.selectedItem.komponentID = this.komponentID.value;
      this.selectedItem.elTavleSektionID = this.elTavleSektionID.value;
      this.selectedItem.navn = this.navn.value;
      this.selectedItem.angivetNavn = this.angivetNavn.value;

      if (this.elTavleSektionID.value) this.selectedItem.sektion = +sektionListe.find(x => x.value === this.elTavleSektionID.value).label;

      this.eltavleService.updateSektionKomponent(this.selectedItem).subscribe({
        next: () => {
          this.messageService.add({
            severity: "success",
            summary: "Success",
            detail: "Data blev gemt"
          });

          this.initializeSektionKomponenter();
          // const index = this.komponenter.value.findIndex(r => r.id === this.selectedItem.id);
          // if (index > -1) {
          //   this.komponenter.value[index] = this.selectedItem;
          //   this.komponenter.next(this.komponenter.value);
          // }
          this.editDialog = false;
          this.selectedItem = {};
          this.cd.detectChanges();
        },
        error: err => (this.errorMessage = err)
      });
    });

    //console.log(this.sektioner$..find(x => x.value == evt).label);

    //console.log(this.elTavleSektionID);
  }

  genberegn() {
    this.eltavleService.genberegnKabinetter(this.activeRoute.parent.snapshot.params.id).subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Data blev genberegnet"
        });
        // const index = this.komponenter.value.findIndex(r => r.id === this.selectedItem.id);

        // if (index > -1) {
        //   this.komponenter.value.splice(index, 1);
        //   this.komponenter.next(this.komponenter.value);
        // }

        this.initializeSektionKomponenter();
        this.deleteDialog = false;
        this.messageService.add({ severity: "success", summary: "Successful", detail: "Product Deleted", life: 3000 });
        this.selectedItem = {};
        this.cd.detectChanges();
      },
      error: err => (this.errorMessage = err)
    });
  }
}
