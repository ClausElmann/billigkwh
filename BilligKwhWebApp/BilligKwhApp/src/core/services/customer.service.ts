// import { Injectable } from "@angular/core";
// import { BiStore } from "@globals/classes/BiStore";
// import { Customer } from "@models/customer/Customer";
// import { Observable } from "rxjs";

// @Injectable({
//   providedIn: "root"
// })

// class CustomerState {
//   currentCustomer: Customer;
//   countryToCustomersMap: { [countryId: number]: Array<Customer> } = {};
// }

// export class CustomerService extends BiStore<CustomerState> {

//   constructor() {
//     ///
//   }

//   public userHasLoggedIn(): boolean {
//     return true;// !isStringNullOrEmpty(this.authService.refreshToken());
//   }

//   public getCustomer(id?: number, publicId?: string): Observable<Customer> {
//     return null;
//   }

// }

import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
//import { Customer } from "@apiModels/Customer";
//import { CustomerEdit } from "@models/customer/CustomerEdit";
import { CustomerUserRole } from "@models/user/UserRole";
import { ApiRoutes } from "@shared/classes/ApiRoutes";
import { BiStore } from "@globals/classes/BiStore";
import { BiCountryId } from "@globals/enums/BiLanguageAndCountryId";
import { isStringNullOrEmpty } from "@shared/variables-and-functions/helper-functions";
import moment from "moment-timezone";
import { Observable } from "rxjs";
import { catchError, map, take, tap } from "rxjs/operators";
import { GlobalStateAndEventsService } from "../global-state-and-events.service";
import { cloneObject } from "@globals/helper-functions";
//import { CustomerEdit } from "@models/customer/CustomerEdit";
import { CustomerModel } from "src/code-gen/model/customerModel";
import { UserModel } from "@apiModels/UserModel";
import { EmailModel } from "@apiModels/emailModel";
import { ElprisModel } from "@apiModels/elprisModel";

class CustomerState {
  currentCustomer: CustomerModel;
  countryToCustomersMap: { [countryId: number]: Array<CustomerModel> } = {};
}

/**
 * Service for handling everything regarding customers
 */
@Injectable()
export class CustomerService extends BiStore<CustomerState> {
  private customersLastRefreshed: moment.Moment;
  private customersCacheExpirationHours = 2;

  public currentCustomerId: number;

  constructor(private http: HttpClient, eventsManager: GlobalStateAndEventsService) {
    super(new CustomerState());
    eventsManager.loginEvent.subscribe(newUser => {
      // make sure that customer is set before moving on! The all components needing current customer can
      // just get it from cache
      if (!this.state.value.currentCustomer || newUser.customerId !== this.state.value.currentCustomer.id) this.getCustomer().pipe(take(1)).subscribe();
    });

    eventsManager.customerChanged$.subscribe(_cId => this.getCustomer().pipe(take(1)).subscribe());
  }

  /**
   * Get customer by either customer ID or the public GUID id. If no id is provided, current customer is returned.
   */
  public getCustomer(id?: number, publicId?: string): Observable<CustomerModel> {
    const params: { [key: string]: string } = {};
    if (id) params.id = id.toString();
    if (!isStringNullOrEmpty(publicId)) params.publicId = publicId;

    return this.http.get<CustomerModel>(ApiRoutes.customerRoutes.get.getCustomer, id ? { params: params } : undefined).pipe(
      tap(c => {
        if (id) return c;

        const stateClone = cloneObject(this.state.value);
        if (stateClone.countryToCustomersMap[c.countryId]) {
          const idx = stateClone.countryToCustomersMap[c.countryId].findIndex(cust => cust.id === c.id);
          if (idx !== -1) stateClone.countryToCustomersMap[c.countryId][idx] = c;
          else stateClone.countryToCustomersMap[c.countryId].push(c);
        } else stateClone.countryToCustomersMap[c.countryId] = [c];

        stateClone.currentCustomer = c;
        this.state.next(stateClone);
      })
    );
  }

  public sendMail(messageId: number) {
    const params: { [key: string]: string } = {};
    params.messageId = messageId.toString();

    return this.http.get(ApiRoutes.commonRoutes.sendMail, { params: params }).pipe(
      tap(c => {
        console.log("test");
      })
    );
  }

  public getEmails(fromDateUtc: Date, toDateUtc: Date, customerId?: number): Observable<EmailModel[]> {
    const params: { [key: string]: string } = {
      fromDateUtc: fromDateUtc.toDateString(),
      toDateUtc: toDateUtc.toDateString()
    };

    if (customerId) params["customerId"] = customerId.toString();

    return this.http
      .get<EmailModel[]>(ApiRoutes.commonRoutes.get.getEmails, {
        params: params
      })
      .pipe(
        map(emails => {
          return emails;
        })
      );
  }

  public getElpriser(fromDateUtc: Date, toDateUtc: Date): Observable<ElprisModel[]> {
    const params: { [key: string]: string } = {
      fromDateUtc: fromDateUtc.toDateString(),
      toDateUtc: toDateUtc.toDateString()
    };

    return this.http
      .get<ElprisModel[]>(ApiRoutes.commonRoutes.get.getElpriser, {
        params: params
      })
      .pipe(
        map(emails => {
          return emails;
        })
      );
  }

  public sendUnsentMails(): Observable<boolean> {
    return this.http.post<boolean>(ApiRoutes.customerRoutes.post.sendUnsentEmails, null, { params: {} });
  }

  public getTavleEmails(tavleID?: number): Observable<EmailModel[]> {
    const params: { [key: string]: string } = {
      tavleID: tavleID.toString()
    };

    return this.http
      .get<EmailModel[]>(ApiRoutes.commonRoutes.get.getTavleEmails, {
        params: params
      })
      .pipe(
        map(emails => {
          return emails;
        })
      );
  }

  public initializeCustomerModel(): CustomerModel {
    // Return an initialized object
    return {
      id: 0,
      name: "",
      address: "",
      city: "",
      zipcode: 0,
      countryId: 1,
      deleted: false,
      timeZoneId: null,
      languageId: 1,
      companyRegistrationId: null,
      hourWage: 450,
      coveragePercentage: 1.3
    };
  }

  public updateCustomer(customer: CustomerModel) {
    return this.http.post<number>(ApiRoutes.customerRoutes.post.updateCustomer, customer).pipe(
      catchError(err => {
        throw err;
      }),
      tap(cId => {
        customer.id = cId;
        const stateClone = cloneObject(this.getCurrentStateValue());
        // Update the local customer object in store if it exists
        if (stateClone.countryToCustomersMap[customer.countryId]) {
          const indexOfLocalCust = stateClone.countryToCustomersMap[customer.countryId].findIndex(c => c.id === customer.id);
          const customerClone = stateClone.countryToCustomersMap[customer.countryId][indexOfLocalCust];
          stateClone.countryToCustomersMap[customer.countryId][indexOfLocalCust] = { ...customerClone, ...customer };
        }

        // Was this customer the current one? Then this also needs an update
        if (stateClone.currentCustomer.id === customer.id)
          stateClone.currentCustomer = {
            ...stateClone.currentCustomer,
            ...customer
          };
        this.state.next(stateClone);
      })
    );
  }

  /**
   * Returns a list of all the users that current customer has
   */
  public getCustomerUsers(): Observable<UserModel[]> {
    return this.http.get<UserModel[]>(ApiRoutes.customerRoutes.get.getCustomerUsers).pipe(
      map((users: UserModel[]) => {
        return users.map(user => {
          // server serializes dates as strings so convert to what we want - moment js objects!
          // user.lastLoginUtc = moment(user.lastLoginUtc);
          // user.dateCreatedUtc = moment(user.dateCreatedUtc);
          return user;
        });
      })
    );
  }

  /**
   * Does the customer has any profiles with 1 or more of the specified profile roles?
   * @param roleNames Comma separated list of profile role names
   * @param customerId Optional customerId. If not provided, current customer's id is used
   * @returns Observable<boolean>
   */
  public customerHasAtleast1ProfileWith1OfRoles(roleNames: string, customerId?: number) {
    return this.http.get<boolean>(ApiRoutes.customerRoutes.get.customerHasAnyProfileWithRoles, {
      params: {
        customerId: customerId ? customerId.toString() : this.state.value.currentCustomer.id.toString(),
        roleNames: roleNames
      }
    });
  }

  //#region SUPER ADMIN
  /**
   * Returns all existing customers for a country. If local state already has these customers, a clone of these is
   * returned without making http call.
   */
  public getCustomers(countryId?: BiCountryId, onlyDeleted = false): Observable<CustomerModel[]> {
    // Check if we have local state that hasn't expired and return that
    // if (
    //   !inclDeleted &&
    //   this.customersLastRefreshed &&
    //   this.customersLastRefreshed.diff(moment(), "hours") <
    //   this.customersCacheExpirationHours
    // ) {
    //   // At the beginning, the "currentCustomer" could be the only customer in a country - meaning that all customers hasn't been fetched before
    //   if (
    //     this.state.value.countryToCustomersMap[countryId] &&
    //     this.state.value.countryToCustomersMap[countryId].length > 1
    //   ) {
    //     this.state.next({ ...this.state.value });
    //     return of(
    //       cloneObject(this.state.value.countryToCustomersMap[countryId])
    //     );
    //   }
    // }
    //console.log("calling: ApiRoutes.customerRoutes.get.getCustomers")

    //console.log("calling: ApiRoutes.customerRoutes.get.getCustomers", countryId, onlyDeleted)

    let params: { [key: string]: string };
    if (countryId)
      params = {
        countryId: countryId.toString(),
        onlyDeleted: onlyDeleted ? "true" : "false"
      };
    return this.http
      .get<CustomerModel[]>(ApiRoutes.customerRoutes.get.getCustomers, {
        params: params
      })
      .pipe(
        map(customers => {
          const map = this.state.value.countryToCustomersMap;
          map[countryId] = customers;
          this.customersLastRefreshed = moment();
          this.state.next({ ...this.state.value, countryToCustomersMap: map });
          return cloneObject(customers);
        })
      );
  }

  //#region Various stuff.
  /**
   * Returns a list of all possible user roles along with a flag telling whether users will become the role or not when created.dataTable
   * @param onlyHasAccess Set true if only the user roles that the customer's user has access to should be returned
   */
  public getCustomerUserRoleAccess(customerId: number, onlyHasAccess?: boolean) {
    const params: { [key: string]: string } = {
      customerId: customerId.toString()
    };
    if (onlyHasAccess) params.onlyHasAccess = "true";

    return this.http.get<Array<CustomerUserRole>>(ApiRoutes.customerRoutes.get.getCustomerUserRoleAccess, { params: params });
  }
}
