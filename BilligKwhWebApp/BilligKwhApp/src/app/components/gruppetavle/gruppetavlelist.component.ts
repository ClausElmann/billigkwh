import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from "@angular/core";
import { BiCountryId } from "@enums/BiLanguageAndCountryId";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ConfirmationService, MessageService, SelectItem } from "primeng/api";
import { BehaviorSubject, finalize, Observable, tap, take } from "rxjs";
import { ActivatedRoute, Router } from "@angular/router";
import { EltavleService } from "@core/services/eltavle.service";
import { ElTavleDto } from "@apiModels/elTavleDto";
import { TableColumnPrimeNg } from "@shared/interfaces-and-enums/TableColumnPrimeNg";
import moment from "moment";
import { BiLocalizationHelperService } from "@core/utility-services/bi-localization-helper.service";
import { FormGroup } from "@angular/forms";
import { UserService } from "@core/services/user.service";
import { CustomerService } from "@core/services/customer.service";
import { CustomerModel } from "@apiModels/customerModel";
import { UserModel } from "@apiModels/UserModel";

export interface TableColumnPrimeNgExt extends TableColumnPrimeNg {
  sortField?: string;
  sortOrder?: number;
}

export interface ElTavleDtoExt extends ElTavleDto {
  beregnetDatoLokalForSort: moment.Moment;
  beregnetDatoLokal: string;
  bestiltDatoLokalForSort?: moment.Moment;
  bestiltDatoLokal?: string;
}

@UntilDestroy()
@Component({
  templateUrl: "./gruppetavlelist.component.html",
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [MessageService, ConfirmationService]
})
export class GruppetavleListComponent implements OnInit {
  public loading = true;

  private gruppetavles = new BehaviorSubject<ElTavleDto[]>([]);
  public gruppetavles$: Observable<ElTavleDto[]> = this.gruppetavles.asObservable();

  public countryFilterItems: Array<SelectItem> = [
    { value: BiCountryId.DK, label: "Danmark" },
    { value: BiCountryId.SE, label: "Sverige" },
    { value: BiCountryId.FI, label: "Finland" },
    { value: BiCountryId.NO, label: "Norge" }
  ];

  public selectedCountryItem = this.countryFilterItems.find(obj => {
    return obj.value === this.eltavleService.countryFilter;
  });

  public ordreStatusFilterItems: Array<SelectItem> = [
    { value: "Bestilte", label: "Bestilte" },
    { value: "Fakturerede", label: "Fakturerede" },
    { value: "Kladder", label: "Kladder" },
    { value: "Slettede", label: "Slettede" }
  ];

  public selectedOrdreStatusItem = this.ordreStatusFilterItems.find(obj => {
    return obj.value === this.eltavleService.ordreStatusFilter;
  });

  public vareTypeFilterItems: Array<SelectItem> = [
    { value: "GruppeTavler", label: "Gruppetavler" },
    { value: "FordelingsTavler", label: "Fordelingstavler" },
    { value: "Loesdele", label: "Løsdelsalg" }
  ];

  public selectedVareTypeItem = this.vareTypeFilterItems.find(obj => {
    return obj.value === this.eltavleService.vareTypeFilter;
  });

  //public selectedCountryItem: SelectItem;

  //selectedValue: string;

  public selectedItem: ElTavleDto;

  opretTavleDialog: boolean;

  public customers: Array<CustomerModel>;
  public users: Array<UserModel>;
  public oprettetAfBrugere: Array<SelectItem>;

  public valgtKunde: CustomerModel;
  public valgtBruger: UserModel;

  takForBestillingDialog = false;
  viHarNuSendtDinTavleDialog = false;

  public mainForm: FormGroup;
  public showFormErrorMessage: boolean;

  public sortField = "tavleNr";
  public sortOrder = -1;

  //private countryId = BiCountryId.DK;

  private customerId: number;

  selectedGruppetavle: ElTavleDto;

  public cols = new Array<TableColumnPrimeNgExt>();

  _selectedColumns: TableColumnPrimeNgExt[];

  showDeleted: boolean;
  onlyNonOrdered: boolean;
  public errorMessage = "";

  constructor(
    private eltavleService: EltavleService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    private messageService: MessageService,
    public localizor: BiLocalizationHelperService,
    private cd: ChangeDetectorRef,
    private userService: UserService,
    private customerService: CustomerService
  ) {}

  ngOnInit() {
    this.showDeleted = false;
    this.onlyNonOrdered = true;

    this.initializeGruppetavles();

    this.cols = [
      { field: "antal", header: "antal" },
      { field: "kundeNavn", header: "Kunde" },
      { field: "tavleNr", header: "TavleNr" },
      { field: "rekvisition", header: "Rekv." },
      { field: "kommentar", header: "Kommentar" },
      { field: "bestiltDatoLokal", header: "Bestilt", sortField: "bestiltDatoLokalForSort" },
      { field: "komponenterPris", header: "Brutto" },
      { field: "nettoPris", header: "Netto" },
      { field: "fragt", header: "Fragt" },
      { field: "adresse", header: "Adresse" },
      { field: "optjentBonus", header: "Op bonus" },
      { field: "udbetaltBonus", header: "Ud bonus" },
      { field: "beregnetDatoLokal", header: "Beregnet dato", sortField: "beregnetDatoLokalForSort" }
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

  public selectedCountryItemChange() {
    this.eltavleService.countryFilter = this.selectedCountryItem.value;
    this.initializeGruppetavles();
  }

  public selectedOrdreStatusItemChange() {
    this.eltavleService.ordreStatusFilter = this.selectedOrdreStatusItem.value;
    if (this.selectedOrdreStatusItem.value === "Kladder") {
      this.sortField = "beregnetDatoLokal";
      this.sortOrder = 1;
    } else {
      this.sortField = "tavleNr";
      this.sortOrder = -1;
    }
    this.initializeGruppetavles();
  }

  public selectedVareTypeItemChange() {
    this.eltavleService.vareTypeFilter = this.selectedVareTypeItem.value;
    this.initializeGruppetavles();
  }

  private stringDivider(str, width, spaceReplacer) {
    if (str.length > width) {
      let p = width;
      // eslint-disable-next-line no-empty
      for (; p > 0 && str[p] != " "; p--) {}
      if (p > 0) {
        const left = str.substring(0, p);
        const right = str.substring(p + 1);
        return left + spaceReplacer + this.stringDivider(right, width, spaceReplacer);
      }
    }
    return str;
  }

  private initializeGruppetavles() {
    this.gruppetavles$ = this.eltavleService.getAllElTavleDto(this.selectedOrdreStatusItem.value, this.selectedVareTypeItem.value, this.selectedCountryItem.value, this.customerId).pipe(
      tap((data: Array<ElTavleDtoExt>) => {
        data.forEach(element => {
          element.beregnetDatoLokal = this.localizor.localizeDateTime(element.beregnetDato);
          element.beregnetDatoLokalForSort = moment(element.beregnetDato);
          if (element.bestiltDato) {
            element.bestiltDatoLokal = this.localizor.localizeDateTime(element.bestiltDato);
            element.bestiltDatoLokalForSort = moment(element.bestiltDato);
          }
          if (element.economicBookedInvoiceNumber !== null) {
            element.tavleNr = element.tavleNr + "(fa)";
          } else if (element.economicDraftInvoiceNumber !== null) {
            element.tavleNr = element.tavleNr + "(fk)";
          } else if (element.economicOrderNumber !== null) {
            element.tavleNr = element.tavleNr + "(fø)";
          }
          element.kommentar = this.stringDivider(element.kommentar, 20, "<br />");
          if (element.billeder > 0) {
            element.kommentar = "(Billeder)<br />" + element.kommentar;
          }
        });
        this.gruppetavles.next(data);
      }),
      untilDestroyed(this),
      finalize(() => (this.loading = false))
    );
  }

  editItem(item: ElTavleDto) {
    this.router.navigate([item.id, "edit"], { relativeTo: this.activeRoute });
  }

  public opretTavle() {
    this.customerService
      .getCustomers(1, false)
      .pipe(take(1))
      .subscribe(c => {
        this.customers = c;
        this.valgtKunde = this.customers[1];
        this.opretTavleDialog = true;
        this.cd.detectChanges();
      });
  }

  onKundeChange(event: CustomerModel) {
    this.userService
      .getUsersByCustomer(this.valgtKunde.id, false)
      .pipe(take(1))
      .subscribe(c => {
        this.users = c;
        this.cd.detectChanges();
      });
  }

  opretFordelingstavle() {
    const eltavle = this.initializeElTavle();

    if (this.valgtBruger === null) return;
    if (this.valgtKunde === null) return;
    eltavle.oprettetAfBrugerID = this.valgtBruger.id;
    eltavle.kundeID = this.valgtKunde.id;

    if (this.eltavleService.vareTypeFilter === "FordelingsTavler") eltavle.typeID = 1;
    if (this.eltavleService.vareTypeFilter === "Loesdele") eltavle.typeID = 9;

    this.eltavleService.opretTavle(eltavle).subscribe({
      next: id => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Data blev oprettet"
        });

        this.router.navigate(["/eltavler", id, "edit"]);
      },
      error: err => (this.errorMessage = err)
    });
  }

  public initializeElTavle(): ElTavleDto {
    // Return an initialized object
    return {
      id: 0,
      adresse: "",
      rekvisition: "",
      fragt: 0,
      antal: 1,
      nettoPris: 0
    };
  }
}
