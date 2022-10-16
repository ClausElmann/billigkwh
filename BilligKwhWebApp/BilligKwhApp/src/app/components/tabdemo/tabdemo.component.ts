import { Component, OnInit } from "@angular/core";
import { MenuItem } from "primeng/api";

@Component({
  selector: "app-tabdemo",
  templateUrl: "./tabdemo.component.html",
  styleUrls: ["./tabdemo.component.scss"]
})
export class TabdemoComponent implements OnInit {

  constructor() {
    //
  }

  items: MenuItem[];

  activeItem: MenuItem;

  ngOnInit() {
    this.items = [
      { label: "Tab1", icon: "pi pi-fw pi-home", routerLink: "tab1" },
      { label: "Tab2", icon: "pi pi-fw pi-file", routerLink: "tab2" }
    ];

    this.activeItem = this.items[0];
  }

}
