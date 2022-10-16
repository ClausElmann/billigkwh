import { Injectable } from "@angular/core";
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";

/**
 * CanDeactivate guard to be used by any component that must ensure something before deactivation is allowed. Routes using this guard must use a component that implements
 * CanDeactivateComponent interface so that this guard can just call this method when calling canDeactivate.
 * This pattern can be read about here: https://www.concretepage.com/angular-2/angular-candeactivate-guard-example#canDeactivate-method
 */
@Injectable()
export class AppCanDeactivateGuard implements CanDeactivate<CanDeactivateComponent> {
  canDeactivate(
    component: CanDeactivateComponent,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState: RouterStateSnapshot
  ) {
    return component.canIDeactivate(nextState);
  }
}

export interface CanDeactivateComponent {
  canIDeactivate(nextState: RouterStateSnapshot): Observable<boolean>;
}
