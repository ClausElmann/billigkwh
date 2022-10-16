import { ActivatedRouteSnapshot, DetachedRouteHandle, RouteReuseStrategy } from "@angular/router";

/**
 * Custom reuse strategy intended to be used for master/detail page flows. It will ensure that the master (index) page will be reused
 * and that the whole component doesn't have to be reinitialized.
 */
export class MasterDetailReuseStrategy implements RouteReuseStrategy {
  private storedRoutes = new Map<string, DetachedRouteHandle>();

  shouldDetach(route: ActivatedRouteSnapshot): boolean {
    // Only if on an index/list page and route is not an exception
    return (route.routeConfig.path.indexOf("index") > -1
      || route.routeConfig.path.indexOf("list") > -1)
      && route.data.shouldNotReuse === false
  }

  store(route: ActivatedRouteSnapshot, handle: DetachedRouteHandle): void {
    this.storedRoutes.set(route.routeConfig.path, handle);
  }

  shouldAttach(route: ActivatedRouteSnapshot): boolean {
    // Only reattach if there's is anything to reattach for this route!
    return route.routeConfig && this.storedRoutes.get(route.routeConfig.path) != null;
  }

  retrieve(route: ActivatedRouteSnapshot): DetachedRouteHandle {
    if (!route.routeConfig) return null;
    if (route.routeConfig.loadChildren) return null; // Lazy loaded child routes can't be reattached - Angular throws error
    return this.storedRoutes.get(route.routeConfig.path);
  }


  shouldReuseRoute(future: ActivatedRouteSnapshot, curr: ActivatedRouteSnapshot): boolean {
    // Use default implementation...
    return future.routeConfig === curr.routeConfig;
  }
}
