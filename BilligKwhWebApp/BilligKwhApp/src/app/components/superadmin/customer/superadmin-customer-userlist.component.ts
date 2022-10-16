import { Component, OnInit } from "@angular/core";
import { UserService } from "@core/services/user.service";
import { UserModel } from "@apiModels/UserModel";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ConfirmationService, MessageService } from "primeng/api";
import { finalize, Observable } from "rxjs";
//import { CustomerModel } from "@apiModels/customerModel";
import { ActivatedRoute, Router } from "@angular/router";

@UntilDestroy()
@Component({
  selector: "app-user-list",
  templateUrl: "./superadmin-customer-userlist.component.html",
  providers: [MessageService, ConfirmationService]
})
export class SuperAdminCustomerUserListComponent implements OnInit {
  public loading = true;
  private customerId: number;
  public users: Array<UserModel> = [];
  public users$: Observable<Array<UserModel>>;

  selectedUser: UserModel;

  cols: any[];

  showDeleted: boolean;

  constructor(private userService: UserService, private activeRoute: ActivatedRoute, private router: Router) {}

  ngOnInit() {
    // this.route.parent.data.subscribe(data => {
    //   this.customer = data["resolvedData"].customer;
    // });
    this.showDeleted = false;

    this.initializeUsers();

    this.cols = [
      { field: "email", header: "email" },
      { field: "firstname", header: "Fornavn" },
      { field: "lastname", header: "Efternavn" },
      { field: "phone", header: "phone" },
      { field: "mobile", header: "mobile" }
    ];
  }

  onRowSelect(event) {
    this.router.navigate(["users", event.data.id, "edit"]);
  }

  public onAddNewUserClick() {
    //[routerLink] = "['/users/0/edit']";

    // if (!this.checkCustomerUserLicenseExceedment()) return;

    // const sharedState = this.superCustomerSharedService.getCurrentStateValue();

    const routeParams: { [key: string]: string | number } = {
      customerId: this.customerId
    };

    this.router.navigate(["users", 0, "edit", routeParams]);

    //if (sharedState.selectedCustomer) routeParams["customerId"] = sharedState.selectedCustomer.id;

    // navigate to create user page
    // this.router.navigate([routeParams], {
    //   relativeTo: this.activeRoute
    // });
  }

  public onShowActiveChange(checked: boolean) {
    if (checked) {
      this.showDeleted = true;
      this.initializeUsers();
    } else {
      this.showDeleted = false;
      this.initializeUsers();
    }
  }

  private initializeUsers() {
    this.customerId = this.activeRoute.parent.snapshot.params.id;
    this.users$ = this.userService.getUsersByCustomer(this.customerId, this.showDeleted).pipe(
      untilDestroyed(this),
      finalize(() => (this.loading = false))
    );
  }
}
