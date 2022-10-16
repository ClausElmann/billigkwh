import { RouteNames } from "./../shared/classes/RouteNames";
import { Component } from "@angular/core";
import { AppMainComponent } from "./app.main.component";
import { MenuItem } from "primeng/api";
import { finalize, map, shareReplay } from "rxjs";
import { UserService } from "@core/services/user.service";
import { Router } from "@angular/router";

@Component({
  selector: "app-topbar",
  templateUrl: "./app.topbar.component.html"
})
export class AppTopBarComponent {

  items: MenuItem[];

  constructor(private userService: UserService, private router: Router, public appMain: AppMainComponent) { }

  public isLoggedIn = this.userService.userHasLoggedIn();

  public onLogoutClicked() {
    this.userService
      .logout()
      .pipe(
        finalize(() => {
          // this.currentProfile = null;
          // this.operationalMessages = [];
          this.router.navigate([RouteNames.landing]);
        })
      )
      .subscribe();
  }

  public currentUser$ = this.userService.state$.pipe(
    map((s) => s.currentUser),
    shareReplay(1)
  );

  public onProfileClicked() {
    this.router.navigate(["my-user","edit"]);
  }

  public onTabDemoClicked() {
    this.router.navigate(["tabdemo","tab1"]);
  }

}
