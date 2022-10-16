import { Observable, of } from "rxjs";
import { Inject, Injectable } from "@angular/core";
import { CanActivate, CanActivateChild, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { UserService } from "@core/services/user.service";
import { GlobalStateAndEventsService } from "@core/global-state-and-events.service";
import { RouteNames } from "@shared/classes/RouteNames";
import { AUTHENTICATION_SERVICE_TOKEN, TokenAuthenticationService } from "@core/security/TokenAuthenticationService";

/**
 * Global route guard for application. Must be used with componentless route that uses canActivateChild and defines child routes
 */
@Injectable()
export class AppCanActivateGuard implements CanActivate, CanActivateChild {
  constructor(private userService: UserService, private router: Router, private globalStateManager: GlobalStateAndEventsService, @Inject(AUTHENTICATION_SERVICE_TOKEN) private authenticationService: TokenAuthenticationService) { }
  canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.checkUserAndRoute(state.url);
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.checkUserAndRoute(state.url);
  }

  /**
   * Checks users existence and if route requires authentication and acts accordingly1
   * @param navigatedRoute The route we're about to navigate to)
   */
  private checkUserAndRoute(navigatedRoute: string): Observable<boolean> {
    // User logged on or navigating to a route requiring no authentication?
    if (this.userService.userHasLoggedIn() || routeRequiresNoAuth(navigatedRoute))
      return of(true);
    else {
      if (navigatedRoute.indexOf(RouteNames.landing) !== -1) {
        this.router.navigate([RouteNames.landing]);
        return of(false);
      }
      // Set the route user tried to visit so we can go back to it after login
      if (!this.globalStateManager.getCurrentStateValue().routeAfterLogin)
        this.globalStateManager.setState({
          ...this.globalStateManager.getCurrentStateValue(),
          routeAfterLogin: navigatedRoute
        });

      this.router.navigate([RouteNames.landing]);

      return of(false);
    }
  }
}

/**
 * Helper method for determining wether a route url is one that requires user to be logged in
 */
const routeRequiresNoAuth = (routeUrl: string) => {
  if (RouteNames.noAuthRoutes.findIndex((r) => routeUrl.indexOf(r) > -1) > -1) return true;

  return false;
};
