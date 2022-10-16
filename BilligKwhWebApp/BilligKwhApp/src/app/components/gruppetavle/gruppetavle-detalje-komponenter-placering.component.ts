import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ConfirmationService, MessageService } from "primeng/api";
import { finalize, Observable, switchMap, take } from "rxjs";
import { ActivatedRoute, Router } from "@angular/router";
import { EltavleService } from "@core/services/eltavle.service";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { SektionElKomponentDto } from "@apiModels/sektionElKomponentDto";

@UntilDestroy()
@Component({
  selector: "app-gruppetavle-komponenter-placering",
  templateUrl: "./gruppetavle-detalje-komponenter-placering.component.html",
  providers: [MessageService, ConfirmationService]
})
export class GruppetavleDetaljeKomponenterPlaceringComponent implements OnInit {
  public loading = true;
  //customer: CustomerModel;
  public komponentPlaceringer: Array<SektionElKomponentDto> = [];
  public komponentPlaceringer$: Observable<Array<SektionElKomponentDto>>;

  public selectedItem: SektionElKomponentDto;
  public mainForm: FormGroup;
  public showFormErrorMessage: boolean;

  public errorMessage = "";

  cols: any[];

  showDeleted: boolean;

  editDialog: boolean;

  public invoicingDone = false;

  constructor(private eltavleService: EltavleService, private activeRoute: ActivatedRoute, private router: Router, private cd: ChangeDetectorRef, private messageService: MessageService) {}

  ngOnInit() {
    // this.route.parent.data.subscribe(data => {
    //   this.customer = data["resolvedData"].customer;
    // });
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

    this.initializeKomponentPlaceringer();

    this.cols = [
      { field: "navn", header: "Navn" },
      { field: "title", header: "Komponent" },
      { field: "width", header: "Moduler" },
      { field: "id", header: "Id" }
      // { field: "timeZoneId", header: "timeZoneId" },
      // { field: "languageId", header: "languageId" }
    ];
  }

  onRowSelect(event) {
    this.router.navigate(["switchboards", this.activeRoute.parent.snapshot.params.id]);
  }

  public onShowActiveChange(checked: boolean) {
    if (checked) {
      this.showDeleted = true;
      this.initializeKomponentPlaceringer();
    } else {
      this.showDeleted = false;
      this.initializeKomponentPlaceringer();
    }
  }

  private initializeKomponentPlaceringer() {
    this.komponentPlaceringer$ = this.eltavleService.getKomponentPlaceringer(this.activeRoute.parent.snapshot.params.id).pipe(
      untilDestroyed(this),
      finalize(() => (this.loading = false))
    );
  }

  editItem(item: SektionElKomponentDto) {
    this.selectedItem = { ...item };
    this.initFormGroup();
    this.editDialog = true;
  }

  private initFormGroup() {
    this.mainForm = new FormGroup({
      navn: new FormControl(this.selectedItem.navn, [Validators.required])
    });
  }

  public get navn() {
    return this.mainForm.get("navn");
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

  hideDialog() {
    this.editDialog = false;
  }

  saveItem() {
    if (!this.checkAndValidateForm()) return;

    this.komponentPlaceringer$.subscribe(sektionListe => {
      this.selectedItem.navn = this.navn.value;

      this.eltavleService.updateKomponentPlaceringNavn(this.selectedItem).subscribe({
        next: () => {
          this.messageService.add({
            severity: "success",
            summary: "Success",
            detail: "Data blev gemt"
          });

          this.initializeKomponentPlaceringer();
          this.editDialog = false;
          this.selectedItem = {};
          this.cd.detectChanges();
        },
        error: err => (this.errorMessage = err)
      });
    });
  }
}
