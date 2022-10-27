import { Component, OnInit } from "@angular/core";
import { UserService } from "@core/services/user.service";
import { AppMainComponent } from "./app.main.component";

@Component({
  selector: "app-menu",
  template: `
    <div class="layout-menu-container">
      <ul class="layout-menu" role="menu" (keydown)="onKeydown($event)">
        <li app-menu class="layout-menuitem-category" *ngFor="let item of model; let i = index" [item]="item" [index]="i" [root]="true" role="none">
          <div class="layout-menuitem-root-text" [attr.aria-label]="item.label">{{ item.label }}</div>
          <ul role="menu">
            <li app-menuitem *ngFor="let child of item.items" [item]="child" [index]="i" role="none"></li>
          </ul>
        </li>
        <!-- <a href="https://www.primefaces.org/primeblocks-ng/#/">
          <img src="assets/layout/images/{{ appMain.config.dark ? 'banner-primeblocks-dark' : 'banner-primeblocks' }}.png" alt="Prime Blocks" class="w-full mt-3" />
        </a> -->
      </ul>
    </div>
  `
})
export class AppMenuComponent implements OnInit {
  model: any[];

  constructor(public appMain: AppMainComponent, private userService: UserService) {}

  hasModule: true;

  ngOnInit() {
    const isSuperAdmin = this.userService.getCurrentStateValue().currentUser.isSuperAdmin;

    this.model = [
      {
        label: "Home",
        items: [
          {
            label: "Dashboard",
            icon: "pi pi-fw pi-home",
            routerLink: ["/"]
          },
          {
            label: "ElectricityPrices",
            icon: "pi pi-fw pi-globe",
            routerLink: ["/superadmin/customers/elpriser"],
            visible: this.hasModule
          },
          {
            label: "Enheder",
            icon: "pi pi-fw pi-globe",
            routerLink: ["/devices"],
            visible: this.hasModule
          }
        ]
      }
    ];
    if (isSuperAdmin) {
      this.model.unshift({
        label: "Administration",
        visible: this.hasModule,
        items: [{ label: "Kunder", icon: "pi pi-fw pi-globe", routerLink: ["/superadmin/customers"] }]
      });
    }
  }

  onKeydown(event: KeyboardEvent) {
    const nodeElement = <HTMLDivElement>event.target;
    if (event.code === "Enter" || event.code === "Space") {
      nodeElement.click();
      event.preventDefault();
    }
  }
}
