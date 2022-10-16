import { APP_INITIALIZER, NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { AppRoutingModule } from "./app-routing.module";
import { StyleClassModule } from "primeng/styleclass";
import { AccordionModule } from "primeng/accordion";
import { AutoCompleteModule } from "primeng/autocomplete";
import { AvatarModule } from "primeng/avatar";
import { AvatarGroupModule } from "primeng/avatargroup";
import { BadgeModule } from "primeng/badge";
import { BreadcrumbModule } from "primeng/breadcrumb";
import { ButtonModule } from "primeng/button";
import { CalendarModule } from "primeng/calendar";
import { CardModule } from "primeng/card";
import { CarouselModule } from "primeng/carousel";
import { CascadeSelectModule } from "primeng/cascadeselect";
import { ChartModule } from "primeng/chart";
import { CheckboxModule } from "primeng/checkbox";
import { ChipModule } from "primeng/chip";
import { ChipsModule } from "primeng/chips";
import { CodeHighlighterModule } from "primeng/codehighlighter";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { ConfirmPopupModule } from "primeng/confirmpopup";
import { ColorPickerModule } from "primeng/colorpicker";
import { ContextMenuModule } from "primeng/contextmenu";
import { DataViewModule } from "primeng/dataview";
import { DialogModule } from "primeng/dialog";
import { DividerModule } from "primeng/divider";
import { DropdownModule } from "primeng/dropdown";
import { FieldsetModule } from "primeng/fieldset";
import { FileUploadModule } from "primeng/fileupload";
import { GalleriaModule } from "primeng/galleria";
import { ImageModule } from "primeng/image";
import { InplaceModule } from "primeng/inplace";
import { InputNumberModule } from "primeng/inputnumber";
import { InputMaskModule } from "primeng/inputmask";
import { InputSwitchModule } from "primeng/inputswitch";
import { InputTextModule } from "primeng/inputtext";
import { InputTextareaModule } from "primeng/inputtextarea";
import { KnobModule } from "primeng/knob";
import { LightboxModule } from "primeng/lightbox";
import { ListboxModule } from "primeng/listbox";
import { MegaMenuModule } from "primeng/megamenu";
import { MenuModule } from "primeng/menu";
import { MenubarModule } from "primeng/menubar";
import { MessagesModule } from "primeng/messages";
import { MessageModule } from "primeng/message";
import { MultiSelectModule } from "primeng/multiselect";
import { OrderListModule } from "primeng/orderlist";
import { OrganizationChartModule } from "primeng/organizationchart";
import { OverlayPanelModule } from "primeng/overlaypanel";
import { PaginatorModule } from "primeng/paginator";
import { PanelModule } from "primeng/panel";
import { PanelMenuModule } from "primeng/panelmenu";
import { PasswordModule } from "primeng/password";
import { PickListModule } from "primeng/picklist";
import { ProgressBarModule } from "primeng/progressbar";
import { RadioButtonModule } from "primeng/radiobutton";
import { RatingModule } from "primeng/rating";
import { RippleModule } from "primeng/ripple";
import { ScrollPanelModule } from "primeng/scrollpanel";
import { ScrollTopModule } from "primeng/scrolltop";
import { SelectButtonModule } from "primeng/selectbutton";
import { SidebarModule } from "primeng/sidebar";
import { SkeletonModule } from "primeng/skeleton";
import { SlideMenuModule } from "primeng/slidemenu";
import { SliderModule } from "primeng/slider";
import { SplitButtonModule } from "primeng/splitbutton";
import { SplitterModule } from "primeng/splitter";
import { StepsModule } from "primeng/steps";
import { TabMenuModule } from "primeng/tabmenu";
import { TableModule } from "primeng/table";
import { TabViewModule } from "primeng/tabview";
import { TagModule } from "primeng/tag";
import { TerminalModule } from "primeng/terminal";
import { TieredMenuModule } from "primeng/tieredmenu";
import { TimelineModule } from "primeng/timeline";
import { ToastModule } from "primeng/toast";
import { ToggleButtonModule } from "primeng/togglebutton";
import { ToolbarModule } from "primeng/toolbar";
import { TooltipModule } from "primeng/tooltip";
import { TreeModule } from "primeng/tree";
import { TreeSelectModule } from "primeng/treeselect";
import { TreeTableModule } from "primeng/treetable";
import { VirtualScrollerModule } from "primeng/virtualscroller";
import { BlockViewer } from "./components/blockviewer/blockviewer.component";

import { AppCodeModule } from "./components/app-code/app.code.component";
import { AppComponent } from "./app.component";
import { AppMainComponent } from "./app.main.component";
import { AppTopBarComponent } from "./app.topbar.component";
import { AppFooterComponent } from "./app.footer.component";
import { AppConfigComponent } from "./app.config.component";
import { AppMenuComponent } from "./app.menu.component";
import { AppMenuitemComponent } from "./app.menuitem.component";

import { DashboardComponent } from "./components/dashboard/dashboard.component";
import { FormLayoutComponent } from "./components/formlayout/formlayout.component";
import { FloatLabelComponent } from "./components/floatlabel/floatlabel.component";
import { InvalidStateComponent } from "./components/invalidstate/invalidstate.component";
import { InputComponent } from "./components/input/input.component";
import { ButtonComponent } from "./components/button/button.component";
import { TableComponent } from "./components/table/table.component";
import { ListComponent } from "./components/list/list.component";
import { TreeComponent } from "./components/tree/tree.component";
import { PanelsComponent } from "./components/panels/panels.component";
import { OverlaysComponent } from "./components/overlays/overlays.component";
import { MediaComponent } from "./components/media/media.component";
import { MenusComponent } from "./components/menus/menus.component";
import { MessagesComponent } from "./components/messages/messages.component";
import { MiscComponent } from "./components/misc/misc.component";
import { EmptyComponent } from "./components/empty/empty.component";
import { ChartsComponent } from "./components/charts/charts.component";
import { FileComponent } from "./components/file/file.component";
import { DocumentationComponent } from "./components/documentation/documentation.component";
import { CrudComponent } from "./components/crud/crud.component";
import { TimelineComponent } from "./components/timeline/timeline.component";
import { IconsComponent } from "./components/icons/icons.component";
import { BlocksComponent } from "./components/blocks/blocks.component";
import { PaymentComponent } from "./components/menus/payment.component";
import { ConfirmationComponent } from "./components/menus/confirmation.component";
import { PersonalComponent } from "./components/menus/personal.component";
import { SeatComponent } from "./components/menus/seat.component";
import { LandingComponent } from "./components/landing/landing.component";

import { UserService } from "@core/services/user.service";
import { CountryService } from "./service/countryservice";
import { CustomerServiceNG } from "./service/customerservice";
import { EventService } from "./service/eventservice";
import { IconService } from "./service/iconservice";
import { NodeService } from "./service/nodeservice";
import { PhotoService } from "./service/photoservice";
import { ProductService } from "./service/productservice";
import { MenuService } from "./service/app.menu.service";
import { ConfigService } from "./service/app.config.service";
import { TranslateLoader, TranslateModule, TranslateService } from "@ngx-translate/core";
import { FeaturesModule } from "@features/features.module";
import { AUTHENTICATION_SERVICE_TOKEN } from "@core/security/TokenAuthenticationService";
import { AppCanActivateGuard } from "@core/routing/guards/app-can-activate.guard";
import { AppCanDeactivateGuard } from "@core/routing/guards/app-can-deactivate.guard";
import { RouteReuseStrategy } from "@angular/router";
import { MasterDetailReuseStrategy } from "@core/routing/MasterDetailReuseStrategy";
import { billigkwhAppInitializer } from "src/billigkwhAppInitializer.factory";
import { Observable } from "rxjs";
import { BiCountryId } from "@enums/BiLanguageAndCountryId";
import { ApiRoutes } from "@shared/classes/ApiRoutes";
import { ToastrModule } from "ngx-toastr";
import { CoreModule } from "@core/core.module";
import { SharedModule } from "@shared/shared.module";
import { CustomerService } from "@core/services/customer.service";
import { ErrorComponent } from "./components/error/error.component";
import { NotfoundComponent } from "./components/notfound/notfound.component";
import { AccessComponent } from "./components/access/access.component";
import { LoginComponent } from "./components/login/login.component";
//import { CustomerModule } from "./components/customer/customer.module";
import { SwitchboardModule } from "./components/gruppetavle-configurator/switchboard.module";
import { ConfirmationService, MessageService } from "primeng/api";
import { DragDropComponent } from "./components/drag-drop/drag-drop.component";
import { UserModule } from "./components/user/user.module";
import { SuperAdminCustomerModule } from "./components/superadmin/customer/superadmin-customer.module";
import { EltavleService } from "@core/services/eltavle.service";
import { DoorModule } from "./components/door-configurator/door.module";
import { CircuitModule } from "./components/circuit-configurator/circuit.module";

/**
 * Custom translations loader that gets the translations from backend.
 */
export class ElHttpTranslateLoader implements TranslateLoader {
  constructor(private http: HttpClient) {}
  getTranslation(lang: string): Observable<any> {
    let languageId = BiCountryId.DK;
    if (lang === "se") languageId = BiCountryId.SE;
    if (lang === "en") languageId = BiCountryId.EN;
    if (lang === "fi") languageId = BiCountryId.FI;
    if (lang === "no") languageId = BiCountryId.NO;
    //console.log("ElHttpTranslateLoader")
    return this.http.get(ApiRoutes.commonRoutes.localizationRoutes.getResourcesJson, {
      params: { languageId: languageId.toString() }
    });
  }
}

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
    AppMainComponent,
    AppTopBarComponent,
    AppFooterComponent,
    AppConfigComponent,
    AppMenuComponent,
    AppMenuitemComponent,
    DashboardComponent,
    FormLayoutComponent,
    FloatLabelComponent,
    InvalidStateComponent,
    InputComponent,
    ButtonComponent,
    TableComponent,
    ListComponent,
    TreeComponent,
    PanelsComponent,
    OverlaysComponent,
    MenusComponent,
    MessagesComponent,
    MessagesComponent,
    MiscComponent,
    ChartsComponent,
    EmptyComponent,
    FileComponent,
    IconsComponent,
    DocumentationComponent,
    CrudComponent,
    TimelineComponent,
    BlocksComponent,
    BlockViewer,
    MediaComponent,
    PaymentComponent,
    ConfirmationComponent,
    PersonalComponent,
    SeatComponent,
    LandingComponent,
    //CustomerListComponent,
    LoginComponent,
    ErrorComponent,
    NotfoundComponent,
    AccessComponent,
    DragDropComponent
    // TabdemoComponent,
    // Tab1Component,
    // Tab2Component
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    AccordionModule,
    AutoCompleteModule,
    AvatarModule,
    AvatarGroupModule,
    BadgeModule,
    BreadcrumbModule,
    ButtonModule,
    CalendarModule,
    CardModule,
    CarouselModule,
    CascadeSelectModule,
    ChartModule,
    CheckboxModule,
    ChipsModule,
    ChipModule,
    CodeHighlighterModule,
    ConfirmDialogModule,
    ConfirmPopupModule,
    ColorPickerModule,
    ContextMenuModule,
    DataViewModule,
    DialogModule,
    DividerModule,
    DropdownModule,
    FieldsetModule,
    FileUploadModule,
    GalleriaModule,
    ImageModule,
    InplaceModule,
    InputNumberModule,
    InputMaskModule,
    InputSwitchModule,
    InputTextModule,
    InputTextareaModule,
    KnobModule,
    LightboxModule,
    ListboxModule,
    MegaMenuModule,
    MenuModule,
    MenubarModule,
    MessageModule,
    MessagesModule,
    MultiSelectModule,
    OrderListModule,
    OrganizationChartModule,
    OverlayPanelModule,
    PaginatorModule,
    PanelModule,
    PanelMenuModule,
    PasswordModule,
    PickListModule,
    ProgressBarModule,
    RadioButtonModule,
    RatingModule,
    RippleModule,
    ScrollPanelModule,
    ScrollTopModule,
    SelectButtonModule,
    SidebarModule,
    SkeletonModule,
    SlideMenuModule,
    SliderModule,
    SplitButtonModule,
    SplitterModule,
    StepsModule,
    TagModule,
    TableModule,
    TabMenuModule,
    TabViewModule,
    TerminalModule,
    TieredMenuModule,
    TimelineModule,
    ToastModule,
    ToggleButtonModule,
    ToolbarModule,
    TooltipModule,
    TreeModule,
    TreeSelectModule,
    TreeTableModule,
    VirtualScrollerModule,
    AppCodeModule,
    StyleClassModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useClass: ElHttpTranslateLoader,
        deps: [HttpClient]
      }
    }),
    ToastrModule.forRoot(),
    CoreModule.forRoot(),
    SharedModule,
    AppRoutingModule,
    FeaturesModule,
    SuperAdminCustomerModule,
    SwitchboardModule,
    DoorModule,
    CircuitModule,
    UserModule
  ],

  providers: [
    // https://github.com/coreui/coreui-free-angular-admin-template/issues/51
    // { provide: LocationStrategy, useClass: HashLocationStrategy },
    CountryService,
    CustomerServiceNG,
    EventService,
    IconService,
    NodeService,
    PhotoService,
    ProductService,
    MenuService,
    ConfigService,
    NodeService,
    MessageService,
    ConfirmationService,
    EltavleService,
    {
      provide: APP_INITIALIZER,
      useFactory: billigkwhAppInitializer,
      deps: [TranslateService, UserService, CustomerService, AUTHENTICATION_SERVICE_TOKEN],
      multi: true
    },
    AppCanActivateGuard,
    AppCanDeactivateGuard,
    {
      provide: RouteReuseStrategy,
      useClass: MasterDetailReuseStrategy
    }
  ]
  //bootstrap: [AppComponent]
})
export class AppModule {}
