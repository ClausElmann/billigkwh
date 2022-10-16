import { ApplicationRef, ComponentFactoryResolver, ComponentRef, Injectable, InjectionToken, Injector } from "@angular/core";
import {
  ComponentPortal,
  DomPortalOutlet
} from "@angular/cdk/portal";
import { CustomerDataOverviewComponent, CUSTOMER_OVERVIEW_DATA_TOKEN } from "@features/administration/super-administration/customers/customer-data-overview/customer-data-overview.component";
@Injectable({
  providedIn: "root"
})
export class BiComponentPortalCreatorService {
  private styleSheetElement: HTMLLinkElement;

  private portalWindowInstance: Window;


  constructor(
    private injector: Injector,
    private componentFactoryResolver: ComponentFactoryResolver,
    private applicationRef: ApplicationRef
  ) { }

  public openComponentInNewTab(componentData: any) {
    const windowInstance = this.openNewTabOnce(
      "assets/blank-portal-outlet.html",
      "COMPONENT_TAB"
    );
    // Wait for window instance to be created
    setTimeout(() => {
      this.createComponentPortal(componentData, CUSTOMER_OVERVIEW_DATA_TOKEN, windowInstance);
    }, 3000);
  }

  public focusPortalWindow() {
    if (this.portalWindowInstance) this.portalWindowInstance.focus();
  }

  public closePortalWindow() {
    if (this.portalWindowInstance) {
      this.portalWindowInstance.close();
      this.portalWindowInstance = undefined;
    }

  }

  private openNewTabOnce(url: string, tabName: string) {
    // Open a blank window/tab or get the reference to the existing
    const winRef = window.open("", tabName, "");
    // If the window was just opened, change its url
    if (winRef.location.href === "about:blank") {
      winRef.location.href = url;
    }
    this.portalWindowInstance = winRef;
    return winRef;
  }

  /**
   * Creates a Component Portal and attaches it to a Portal Outlet inside a new browser tab
   * @param componentData data to make available via DI in the component ctor
   * @param dataInjectionToken The InjectionToken to be used for providing the component data via DI
   * @param windowInstance An instance of a browser Window
   */
  private createComponentPortal(componentData: any, dataInjectionToken: InjectionToken<any>, windowInstance: Window) {
    if (windowInstance) {
      // Create a PortalOutlet with the body of the new window document
      const outlet = new DomPortalOutlet(
        windowInstance.document.body,
        this.componentFactoryResolver,
        this.applicationRef,
        this.injector
      );
      // Copy styles from parent window
      document.querySelectorAll("style").forEach(htmlElement => {
        windowInstance.document.head.appendChild(htmlElement.cloneNode(true));
      });
      // Copy stylesheet link from parent window
      this.styleSheetElement = this.getStyleSheetElement();
      windowInstance.document.head.appendChild(this.styleSheetElement);

      this.styleSheetElement.onload = () => {
        // Clear popout modal content
        windowInstance.document.body.innerText = "";

        // Create an injector with modal data
        const injector = this.createInjector(componentData, dataInjectionToken);

        // Attach the portal
        // if (componentData.modalName === "CUSTOMER_DATA_OVERVIEW") {
        windowInstance.document.title = "Customer data overview";
        this.attachCustomerOverviewComponent(outlet, injector);
        // }

      };
    }
  }

  private attachCustomerOverviewComponent(outlet: DomPortalOutlet, injector: Injector) {
    const containerPortal = new ComponentPortal(
      CustomerDataOverviewComponent,
      null,
      injector
    );
    const componentRef: ComponentRef<CustomerDataOverviewComponent> = outlet.attach(
      containerPortal
    );
    return componentRef.instance;
  }

  private createInjector(componentData: any, dataInjectionToken: InjectionToken<any>): Injector {
    // Define the data to be injectable in the component portal (e.i. that can be injected through ctor in component)
    const injectionTokens = new WeakMap();
    injectionTokens.set(dataInjectionToken, componentData);
    return Injector.create({ parent: this.injector, providers: [{ provide: dataInjectionToken, useValue: componentData }] });
  }

  private getStyleSheetElement() {
    const styleSheetElement = document.createElement("link");
    document.querySelectorAll("link").forEach(htmlElement => {
      if (htmlElement.rel === "stylesheet") {
        const absoluteUrl = new URL(htmlElement.href).href;
        styleSheetElement.rel = "stylesheet";
        styleSheetElement.href = absoluteUrl;
      }
    });
    return styleSheetElement;
  }
}
