import { Component, OnInit } from "@angular/core";
import { CustomerService } from "@core/services/customer.service";
import { BiCountryId } from "@enums/BiLanguageAndCountryId";
import { CustomerModel } from "@apiModels/CustomerModel";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ConfirmationService, MessageService, SelectItem } from "primeng/api";
import { finalize, Observable, take } from "rxjs";
import { ActivatedRoute, Router } from "@angular/router";

@UntilDestroy()
@Component({
  templateUrl: "./superadmin-customerlist.component.html",
  providers: [MessageService, ConfirmationService]
})
export class SuperAdminCustomerListComponent implements OnInit {
  public loading = true;

  public customers: Array<CustomerModel> = [];
  public customers$: Observable<Array<CustomerModel>>;

  public countries: Array<SelectItem> = [
    { value: BiCountryId.DK, label: "Danmark" },
    { value: BiCountryId.SE, label: "Sverige" },
    { value: BiCountryId.FI, label: "Finland" },
    { value: BiCountryId.NO, label: "Norge" }
  ];

  public selectedCountryCode: string;

  selectedValue: string;

  private countryId = BiCountryId.DK;

  selectedCustomer: CustomerModel;

  cols: any[];

  showDeleted: boolean;

  constructor(private customerService: CustomerService, private activeRoute: ActivatedRoute, private router: Router) {}

  ngOnInit() {
    const currentCustomer = this.customerService.getCurrentStateValue().currentCustomer;
    this.countryId = currentCustomer.countryId;
    this.showDeleted = false;

    this.initializeCustomers();

    this.cols = [
      { field: "id", header: "Id" },
      { field: "name", header: "Navn" },
      { field: "displayAddress", header: "Adresse" },
      { field: "zipcode", header: "PostNr" },
      { field: "city", header: "By" },
      { field: "invoiceMail", header: "Faktura mail" },
      { field: "companyRegistrationId", header: "Cvr" },
      { field: "economicId", header: "E-conomicId" }
    ];
  }

  onRowSelect(event) {
    this.router.navigate([event.data.id, "main"], { relativeTo: this.activeRoute });
  }

  public onCreateNewCustomer() {
    this.router.navigate([0, "main"], { relativeTo: this.activeRoute });
  }

  public selectedCountryCodeChange(item: SelectItem) {
    this.countryId = +item.value;
    this.initializeCustomers();
  }

  public onShowActiveChange(checked: boolean) {
    if (checked) {
      this.showDeleted = true;
      this.initializeCustomers();
    } else {
      this.showDeleted = false;
      this.initializeCustomers();
    }
  }

  private initializeCustomers() {
    this.customers$ = this.customerService.getCustomers(this.countryId, this.showDeleted).pipe(
      untilDestroyed(this),
      finalize(() => (this.loading = false))
    );
  }

  sendMail(messageId: string) {
    if (messageId) {
      this.customerService
        .sendMail(+messageId)
        .pipe(take(1))
        .subscribe(c => {
          console.log(c);
        });
    }
  }
}
