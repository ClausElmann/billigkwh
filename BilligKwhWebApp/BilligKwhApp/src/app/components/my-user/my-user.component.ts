import { Component, OnInit } from "@angular/core";
import { MenuItem } from "primeng/api";

@Component({
  selector: "app-myuser",
  templateUrl: "./my-user.component.html",
  styleUrls: ["./my-user.component.scss"]
})
export class MyUserComponent implements OnInit {

  constructor() {
    //
  }

  items: MenuItem[];

  activeItem: MenuItem;

  ngOnInit() {
    this.items = [
      { label: "User Profile", routerLink: "profile" },
      { label: "User Security", routerLink: "security" }
    ];

    this.activeItem = this.items[0];
  }

}
