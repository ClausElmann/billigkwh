import { map, take } from "rxjs/operators";
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, Router, RouterStateSnapshot, CanLoad, Route, UrlSegment } from "@angular/router";
import { UserService } from "@core/services/user.service";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import { RouteNames } from "../../../shared/classes/RouteNames";
import { UserRoleEnum } from "src/UserRoleEnum";

/**
 * Guarding routes against existence of roles on current user. Required roles must be passed as route data
 * to the route configuration. Access is denied when required role isn't present on user.
 * When access is denied, this guard navigates back to the default page that is shown when logged in
 */
@Injectable()
export class UserRoleGuard implements CanActivate, CanActivateChild, CanLoad {
  constructor(private userService: UserService, private router: Router) { }

  public canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.checkUserHasRole(route.data["roles"]);
  }

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.checkUserHasRole(route.data["roles"]);
  }

  public canLoad(route: Route, segments: UrlSegment[]): boolean | Observable<boolean> | Promise<boolean> {
    return this.checkUserHasRole(route.data["roles"]);
  }

  private checkUserHasRole(requiredRoleNames: Array<UserRoleEnum>) {
    if (!this.userService.userHasLoggedIn()) return true;

    // Superadmin have super powers and can do it all!
    if (this.userService.doesUserHaveRole(UserRoleEnum.SuperAdmin)) return true;

    return this.userService.userRoles$.pipe(
      take(1),
      map((userRoles) => {
        for (let i = 0; i < requiredRoleNames.length; i++) {
          if (!this.userService.doesUserHaveRole(requiredRoleNames[i])) {
            this.router.navigate([RouteNames.frontPage]);
            return false;
          }
        }
        return true;
      })
    );
  }
}
