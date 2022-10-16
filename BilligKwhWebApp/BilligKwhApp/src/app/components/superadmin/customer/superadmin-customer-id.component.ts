import { Component, OnInit } from "@angular/core";
import { UntilDestroy } from "@ngneat/until-destroy";
import { MenuItem, MessageService } from "primeng/api";

@UntilDestroy()
@Component({
  selector: "app-customer-edit",
  templateUrl: "./superadmin-customer-id.component.html",
  providers: [MessageService]
})
export class SuperAdminCustomerIdComponent implements OnInit {

  items: MenuItem[];

  activeItem: MenuItem;

  ngOnInit(): void {

    this.items = [
      { label: "Basis Information", icon: "pi pi-fw pi-home", routerLink: "main" },
      { label: "Brugere", icon: "pi pi-fw pi-file", routerLink: "users" }
    ];
    this.activeItem = this.items[0];
  }
}
