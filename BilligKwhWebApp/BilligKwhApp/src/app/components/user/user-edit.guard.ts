import { Injectable } from "@angular/core";
import { CanDeactivate } from "@angular/router";
import { Observable } from "rxjs";
import { UserEditComponent } from "./user-edit/user-edit.component";

@Injectable({
  providedIn: "root"
})
export class UserEditGuard implements CanDeactivate<UserEditComponent>  {

  canDeactivate(component: UserEditComponent): boolean | Observable<boolean> | Promise<boolean> {

    if (component.isDirty) {
      const userName = component.user.email || "New User";
      return confirm(`Navigate away and lose all changes to ${userName}?`);
    }
    return true;
  }

}
