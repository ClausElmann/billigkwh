import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from "@angular/core";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ConfirmationService, MessageService, SelectItem } from "primeng/api";
import { BehaviorSubject, finalize, Observable, tap, take } from "rxjs";
import { Router } from "@angular/router";
import { EltavleService } from "@core/services/eltavle.service";
import { ElKomponentDto } from "@apiModels/elKomponentDto";
import { TableColumnPrimeNg } from "@shared/interfaces-and-enums/TableColumnPrimeNg";
import moment from "moment";
import { BiLocalizationHelperService } from "@core/utility-services/bi-localization-helper.service";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { CustomerModel } from "@apiModels/customerModel";
import { UserModel } from "@apiModels/UserModel";
import { PrimeNgUtilities } from "@shared/variables-and-functions/primeNg-utilities";

export interface TableColumnPrimeNgExt extends TableColumnPrimeNg {
  sortField?: string;
}

export interface ElKomponentDtoExt extends ElKomponentDto {
  beregnetDatoLokalForSort: moment.Moment;
  beregnetDatoLokal: string;
  bestiltDatoLokalForSort?: moment.Moment;
  bestiltDatoLokal?: string;
}

@UntilDestroy()
@Component({
  templateUrl: "./komponentlist.component.html",
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [MessageService, ConfirmationService]
})
export class KomponentListComponent implements OnInit {
  public loading = true;

  private komponents = new BehaviorSubject<ElKomponentDto[]>([]);
  public komponents$: Observable<ElKomponentDto[]> = this.komponents.asObservable();

  public komponentStatusFilterItems: Array<SelectItem> = [
    { value: "Aktive", label: "Aktive" },
    { value: "Slettede", label: "Slettede" }
  ];

  public selectedKomponentStatusItem = this.komponentStatusFilterItems.find(obj => {
    return obj.value === this.eltavleService.komponentStatusFilter;
  });

  public komponentKategoriFilterItems: Array<SelectItem> = [
    { value: 0, label: "Alle" },
    { value: 1, label: "D02 Forsikring" },
    { value: 2, label: "Transient beskyttelse" },
    { value: 5, label: "HPFI" },
    { value: 7, label: "LK Ug 150" },
    { value: 8, label: "UM" },
    { value: 11, label: "LK PGE Planforsænket med låg" },
    { value: 12, label: "Hager Gamma" },
    { value: 13, label: "Hager Volta Planforsænketmed låg" },
    { value: 14, label: "Sneider Resi9" },
    { value: 104, label: "Automat sikringer 3p+n" },
    { value: 105, label: "Automat sikringer 1p+n" },
    { value: 106, label: "Kombirelæ 3p+n" },
    { value: 107, label: "Kombirelæ 1p+n" },
    { value: 110, label: "Øvrige komponenter" }
  ];

  public selectedkomponentKategoriItem = this.komponentKategoriFilterItems.find(obj => {
    return obj.value === this.eltavleService.komponentKategoriFilter;
  });

  selectedValue: string;

  public selectedItem: ElKomponentDto;

  editDialog: boolean;
  retTimePrisDialog: boolean;

  public customers: Array<CustomerModel>;
  public users: Array<UserModel>;
  public oprettetAfBrugere: Array<SelectItem>;
  public timeLoen: number;
  public exportCSV = PrimeNgUtilities.exportCSV;

  takForBestillingDialog = false;
  viHarNuSendtDinTavleDialog = false;

  public mainForm: FormGroup;
  public showFormErrorMessage: boolean;

  selectedKomponent: ElKomponentDto;

  public cols = new Array<TableColumnPrimeNgExt>();

  _selectedColumns: TableColumnPrimeNgExt[];

  showDeleted: boolean;
  onlyNonOrdered: boolean;
  public errorMessage = "";

  constructor(
    private eltavleService: EltavleService,
    private router: Router,
    private messageService: MessageService,
    public localizor: BiLocalizationHelperService,
    private cd: ChangeDetectorRef,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit() {
    this.showDeleted = false;
    this.onlyNonOrdered = true;

    this.initializeKomponents();

    this.cols = [
      { field: "navn", header: "Navn" },
      { field: "kostKomponent", header: "Kost komp" },
      { field: "kostHjaelpeMat", header: "Hjælpemat" },
      { field: "montageMinutter", header: "Mont. min." },
      { field: "kostLoen", header: "Kost løn" },
      { field: "daekningsBidrag", header: "DB proc" },
      { field: "bruttoPris", header: "Salgspris" },
      { field: "effekt", header: "Effekt(W)" },
      { field: "modul", header: "Modul" }
    ];
    this._selectedColumns = this.cols;
  }

  @Input() get selectedColumns(): TableColumnPrimeNgExt[] {
    return this._selectedColumns;
  }

  set selectedColumns(val: TableColumnPrimeNgExt[]) {
    //restore original order
    this._selectedColumns = this.cols.filter(col => val.includes(col));
  }

  beregnetDatoLokalForSort: moment.Moment;
  beregnetDatoLokal: string;
  bestiltDatoLokalForSort?: moment.Moment;
  bestiltDatoLokal?: string;

  clonedProducts: { [s: string]: ElKomponentDto } = {};

  public selectedKomponentStatusItemChange() {
    this.eltavleService.komponentStatusFilter = this.selectedKomponentStatusItem.value;
    this.initializeKomponents();
  }

  public selectedkomponentKategoriItemChange() {
    this.eltavleService.komponentKategoriFilter = this.selectedkomponentKategoriItem.value;
    this.initializeKomponents();
  }

  private initializeKomponents() {
    this.komponents$ = this.eltavleService.alleElKomponenter(this.selectedKomponentStatusItem.value, this.selectedkomponentKategoriItem.value).pipe(
      tap((data: Array<ElKomponentDtoExt>) => {
        data.forEach(element => {
          //
        });
        this.komponents.next(data);
      }),
      untilDestroyed(this),
      finalize(() => (this.loading = false))
    );
  }

  private initFormGroup() {
    this.mainForm = new FormGroup({
      navn: new FormControl(this.selectedItem.navn, [Validators.required]),
      placering: new FormControl(this.selectedItem.placering, [Validators.required]),
      modul: new FormControl(this.selectedItem.modul, [Validators.required]),
      kostKomponent: new FormControl(this.selectedItem.kostKomponent, [Validators.required]),
      kostHjaelpeMat: new FormControl(this.selectedItem.kostHjaelpeMat, [Validators.required]),
      daekningsBidrag: new FormControl(this.selectedItem.daekningsBidrag, [Validators.required]),
      montageMinutter: new FormControl(this.selectedItem.montageMinutter, [Validators.required]),
      effekt: new FormControl(this.selectedItem.effekt, [Validators.required])
    });
  }

  public get navn() {
    return this.mainForm.get("navn");
  }

  public get placering() {
    return this.mainForm.get("placering");
  }

  public get modul() {
    return this.mainForm.get("modul");
  }

  public get kostKomponent() {
    return this.mainForm.get("kostKomponent");
  }

  public get kostHjaelpeMat() {
    return this.mainForm.get("kostHjaelpeMat");
  }

  public get daekningsBidrag() {
    return this.mainForm.get("daekningsBidrag");
  }

  public get montageMinutter() {
    return this.mainForm.get("montageMinutter");
  }

  public get effekt() {
    return this.mainForm.get("effekt");
  }

  editItem(item: ElKomponentDto) {
    this.selectedItem = item;
    this.initFormGroup();
    this.editDialog = true;
  }

  retTimePris() {
    this.eltavleService
      .getKomponentTimeLoen()
      .pipe(take(1))
      .subscribe(c => {
        this.timeLoen = c;
        this.cd.detectChanges();
      });

    this.retTimePrisDialog = true;
  }

  gemTimepris(event: Event) {
    this.confirmationService.confirm({
      target: event.target,
      message: `Er du sikker på du ønsker at ændre komponent timelønnen til: ${this.timeLoen} kr. i timen?`,
      icon: "pi pi-exclamation-triangle",
      accept: () => {
        this.eltavleService.gemKomponentTimeLoen(this.timeLoen).subscribe({
          next: () => {
            this.messageService.add({
              severity: "success",
              summary: "Success",
              detail: "Data blev opdateret"
            });

            this.retTimePrisDialog = false;
            this.router.navigateByUrl("/", { skipLocationChange: true }).then(() => this.router.navigate(["/komponenter"]));
          },
          error: err => (this.errorMessage = err)
        });
      },
      reject: () => {
        //reject action
      }
    });
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

  saveItem() {
    if (!this.checkAndValidateForm()) return;

    this.selectedItem.navn = this.navn.value;
    this.selectedItem.placering = this.placering.value;
    this.selectedItem.kostKomponent = this.kostKomponent.value;
    this.selectedItem.kostHjaelpeMat = this.kostHjaelpeMat.value;
    this.selectedItem.daekningsBidrag = this.daekningsBidrag.value;
    this.selectedItem.montageMinutter = this.montageMinutter.value;
    this.selectedItem.effekt = +this.effekt.value;

    this.eltavleService.updateElKomponent(this.selectedItem).subscribe({
      next: komponent => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Data blev gemt"
        });

        const index = this.komponents.value.findIndex(r => r.id === this.selectedItem.id);
        if (index > -1) {
          this.komponents.value[index] = komponent;
          this.komponents.next(this.komponents.value);
        }
        this.editDialog = false;
        this.selectedItem = {};
        this.cd.detectChanges();
      },
      error: err => (this.errorMessage = err)
    });
  }

  public initializeKomponent(): ElKomponentDto {
    // Return an initialized object
    return {
      id: 0,
      navn: "",
      kategoriId: 0,
      placering: 0,
      modul: 0,
      dinSkinner: 0,
      kostKomponent: 0,
      kostHjaelpeMat: 0,
      kostLoen: 0,
      bruttoPris: 0,
      daekningsBidrag: 0,
      montageMinutter: 0
    };
  }
}
