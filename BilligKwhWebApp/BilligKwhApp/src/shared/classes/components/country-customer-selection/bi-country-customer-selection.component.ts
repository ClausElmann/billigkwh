import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, HostBinding, Input, OnInit, Output } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { GlobalStateAndEventsService } from "@core/global-state-and-events.service";
import { CustomerService } from "@core/services/customer.service";
import { BiLocalizationHelperService } from "@core/utility-services/bi-localization-helper.service";
import { Customer } from "@models/customer/Customer";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { SelectItem } from "primeng/api/selectitem";
import { forkJoin, Observable, of } from "rxjs";
import { debounceTime, distinctUntilChanged, filter, finalize, map, take, tap } from "rxjs/operators";
import { BiCountryId } from "@globals/enums/BiLanguageAndCountryId";
import { BiCustomAnimations } from "@shared/classes/BICustomAnimations";

/**
 * Model containing country,- customer and maybe profile id.
 */
export interface CountryCustomer {
  countryId: BiCountryId;
  customer?: Customer
}

@UntilDestroy()
@Component({
  selector: "bi-country-customer-selection",
  templateUrl: "./bi-country-customer-selection.component.html",
  animations: [BiCustomAnimations.fadeIn, BiCustomAnimations.fadeInDown],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BiCountryCustomerSelectionComponent implements OnInit {
  private _customerRequired: boolean;
  public get customerRequired() {
    return this._customerRequired;
  }
  @Input() public set customerRequired(val: boolean) {
    this._customerRequired = val;
    if (this.form) {
      if (val) {
        this.customerCtrl.setValidators([Validators.required]);
        this.customerCtrl.updateValueAndValidity();
      } else {
        this.customerCtrl.clearValidators();
        this.customerCtrl.updateValueAndValidity();
      }
    }
  }

  /**
   * showClear give the user the option to clear all fields in the form.
   */
  @Input() showClear = false;

  /**
   * The Country dropdown is not required to have anything selected.
   */
  @Input() requireCountry = true;

  /**
   *  The country dropdown will have the country the user is in selected by default.
   */
  @Input() selectDefaultTheUsersCountry = true;


  @Input() hasOkButton = false;
  @Input() okButtonTitle = "OK";

  private _hasCustomerSelection = true;
  @Input() public set hasCustomerSelection(yesIHave: boolean) {
    this._hasCustomerSelection = yesIHave;

    if (yesIHave) {
      this.getCustomers();
    }
  }
  public get hasCustomerSelection() { return this._hasCustomerSelection; }

  @Output() onOkClicked = new EventEmitter<CountryCustomer>();
  @Output() onSelectionChanged = new EventEmitter<CountryCustomer>();

  public countries$ = this.globalState.state$.pipe(map((s) => s.countries.map((c) => <SelectItem>{ label: c.name, value: c.countryId })));
  public customers: Array<SelectItem> = [];

  public form: FormGroup;

  @HostBinding("@fadeIn") get fadeInHost() {
    return true;
  }

  public loadingCustomers = false;
  public loadingProfiles = false;

  public inputGridClassName: string;

  constructor(
    private customerService: CustomerService,
    private localizor: BiLocalizationHelperService,
    private globalState: GlobalStateAndEventsService,
    private cd: ChangeDetectorRef
  ) {

  }

  public ngOnInit() {
    // Form Group
    this.initFormGroup();
    this.initCountryCtrl();
    this.initCustomerCtrl();
    this.initProfileCtrl();
    this.setInputGridClassName();
    this.getCustomers();
  }
  // Form Group
  public initFormGroup() {
    this.form = new FormGroup({
      countryId: new FormControl(this.selectDefaultTheUsersCountry ? this.localizor.customerCountryId : undefined, this.requireCountry ? Validators.required : Validators.nullValidator),
      customer: new FormControl(null, this.customerRequired ? Validators.required : undefined)
    });
  }

  public initCountryCtrl() {

    this.countryCtrl.valueChanges
      .pipe(
        untilDestroyed(this),
        filter((val) => val != null || !this.selectDefaultTheUsersCountry),
        distinctUntilChanged(),
        debounceTime(800)
      )
      .subscribe(() => this.onCountryChanged());
  }

  public initCustomerCtrl() {
    this.customerCtrl.valueChanges
      .pipe(
        untilDestroyed(this),
        distinctUntilChanged((oldCust, newCust) => {
          if ((!oldCust && newCust) || (oldCust && !newCust)) {
            return false;
          }
          if (oldCust && newCust) {
            return oldCust.id === newCust.id;
          }
        }),
        debounceTime(800)
      )
      .subscribe(() => {
        this.onCustomerChanged();
      });
  }

  public initProfileCtrl() {
    this.profileCtrl.valueChanges
      .pipe(
        untilDestroyed(this),
        filter((val) => val != null),
        distinctUntilChanged(),
        debounceTime(800)
      )
      .subscribe(() => {
        setTimeout(() => this.onSelectionChanged.emit(this.form.value), 0);
      });
  }

  private setInputGridClassName() {
    if (this.hasCustomerSelection) {
      this.inputGridClassName = "sm:col-6";
    }
    else {
      this.inputGridClassName = "sm:col-12";
    }
  }
  //===== Getters for easy FormControl access
  public get countryCtrl() {
    return this.form.get("countryId");
  }
  public get customerCtrl() {
    return this.form.get("customer");
  }
  public get profileCtrl() {
    return this.form.get("profile");
  }
  // =============================

  public onCountryChanged() {
    if (this.hasCustomerSelection) {
      this.getCustomers();
    }
    setTimeout(() => this.onSelectionChanged.emit(this.form.value), 0);
  }

  public onCustomerChanged() {
    setTimeout(() => this.onSelectionChanged.emit(this.form.value), 0);
  }

  public getCustomers(returnAsObservable?: boolean) {
    if (this.countryCtrl.value) {
      this.loadingCustomers = true;
      // Make sure to clear earlier selection of profile and the list of profiles, too
      this.profileCtrl.setValue(undefined, { emitEvent: false });
      this.profileCtrl.markAsPristine(); // So that any errors are gone
      this.customerCtrl.setValue(undefined, { emitEvent: false });
      this.customerCtrl.markAsPristine();
      const observable = this.customerService.getCustomers(this.countryCtrl.value).pipe(
        tap((cs) => {
          this.customers = cs.map(
            (c) =>
              <SelectItem>{
                label: `${c.name}`,
                value: c
              }
          );

        }),
        finalize(() => {
          this.loadingCustomers = false;
          this.cd.markForCheck();
        })
      );

      if (returnAsObservable) return observable;

      return observable.subscribe();
    }
  }

  public okClicked() {
    if (this.form.valid) {
      this.form.markAsPristine();
      this.onOkClicked.emit(this.form.value);
    }
  }

  //============ PARENT METHODS

  /**
   * Sets the current selection of country, customer and profile
   */
  public setSelection(selection: CountryCustomer, emitToParent = true) {
    const observables: Observable<any>[] = [of("")];
    // different country? Then get customers
    if (this.countryCtrl.value !== selection.countryId) {
      // Set country - only emit if param true AND there's not also a customer to be set
      this.countryCtrl.setValue(selection.countryId, { emitEvent: emitToParent && !selection.customer });
      observables.push((this.getCustomers(true) as Observable<any>).pipe(untilDestroyed(this), take(1)));
    }

    // different customer?
    let customerChanged = false;
    if (selection.customer && (!this.customerCtrl.value || (this.customerCtrl.value && selection.customer.id !== this.customerCtrl.value.id))) {
      customerChanged = true;
    }

    forkJoin(observables).subscribe(() => {
      if (customerChanged) {
        this.customerCtrl.setValue(selection.customer, {
          emitEvent: emitToParent
        });
      }
      this.form.markAsDirty();
    });
  }

  /**
   * Returns the current selection
   */
  public getSelection(): CountryCustomer {
    return this.form.value;
  }

  public reset() {
    this.form.reset();
  }
  public resetCustomerCtrl() {
    this.customerCtrl.reset();
  }

  public get isValid() {
    return this.form.valid;
  }

  //=================================
}
