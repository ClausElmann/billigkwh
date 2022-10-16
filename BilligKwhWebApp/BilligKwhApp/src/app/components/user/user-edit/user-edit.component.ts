import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { UserModel } from "@apiModels/userModel";
import { MenuItem, MessageService } from "primeng/api";
//import { UserResolved } from "../user-resolved";

@Component({
  selector: "app-user-edit",
  templateUrl: "./user-edit.component.html",
  styleUrls: ["./user-edit.component.scss"],
  providers: [MessageService]
})
export class UserEditComponent implements OnInit {
  pageTitle = "Bruger redigering";
  errorMessage: string;

  get isDirty(): boolean {
    return JSON.stringify(this.originalUser) !== JSON.stringify(this.currentUser);
  }

  private currentUser: UserModel;
  private originalUser: UserModel;

  get user(): UserModel {
    return this.currentUser;
  }

  set user(value: UserModel) {
    this.currentUser = value;
    // Clone the object to retain a copy
    this.originalUser = value ? { ...value } : null;
  }

  constructor(private route: ActivatedRoute) {}

  items: MenuItem[];

  activeItem: MenuItem;

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      //const resolvedData: UserResolved = data["resolvedData"];
      //this.errorMessage = resolvedData.error;
      //this.onUserRetrieved(resolvedData.user);
    });

    this.items = [
      { label: "Basis Information", icon: "pi pi-fw pi-home", routerLink: "main" },
      { label: "Tab Info", icon: "pi pi-fw pi-file", routerLink: "info" }
      //{ label: "Brugere", icon: "pi pi-fw pi-file", routerLink: "users" }
    ];

    this.activeItem = this.items[0];
  }

  onUserRetrieved(user: UserModel): void {
    this.user = user;

    if (!this.user) {
      this.pageTitle = "Bruger ikke fundet";
    } else {
      if (this.user.id === 0) {
        this.pageTitle = "Tilføj ny bruger";
      } else {
        this.pageTitle = `Rediger bruger: ${this.user.firstname + " " + this.user.lastname}`;
      }
    }
  }
}
