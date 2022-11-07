//========  All route configuration here ===============\\

import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { AppCanActivateGuard } from "@core/routing/guards/app-can-activate.guard";
import { RouteNames } from "@shared/classes/RouteNames";

import { DashboardComponent } from "./components/dashboard/dashboard.component";
import { FormLayoutComponent } from "./components/formlayout/formlayout.component";
import { PanelsComponent } from "./components/panels/panels.component";
import { OverlaysComponent } from "./components/overlays/overlays.component";
import { MediaComponent } from "./components/media/media.component";
import { MessagesComponent } from "./components/messages/messages.component";
import { MiscComponent } from "./components/misc/misc.component";
import { EmptyComponent } from "./components/empty/empty.component";
import { ChartsComponent } from "./components/charts/charts.component";
import { FileComponent } from "./components/file/file.component";
import { DocumentationComponent } from "./components/documentation/documentation.component";
import { AppMainComponent } from "./app.main.component";
import { InputComponent } from "./components/input/input.component";
import { ButtonComponent } from "./components/button/button.component";
import { TableComponent } from "./components/table/table.component";
import { ListComponent } from "./components/list/list.component";
import { TreeComponent } from "./components/tree/tree.component";
import { CrudComponent } from "./components/crud/crud.component";
import { BlocksComponent } from "./components/blocks/blocks.component";
import { FloatLabelComponent } from "./components/floatlabel/floatlabel.component";
import { InvalidStateComponent } from "./components/invalidstate/invalidstate.component";
import { TimelineComponent } from "./components/timeline/timeline.component";
import { IconsComponent } from "./components/icons/icons.component";
import { LandingComponent } from "./components/landing/landing.component";
import { ElLoginComponent } from "@features/login/el-login.component";
import { ErrorComponent } from "./components/error/error.component";
import { NotfoundComponent } from "./components/notfound/notfound.component";
import { AccessComponent } from "./components/access/access.component";
import { LoginComponent } from "./components/login/login.component";
import { DragDropComponent } from "./components/drag-drop/drag-drop.component";
import { PasswordResetCreateComponent } from "./components/password-reset-create/password-reset-create.component";
import { ForgotPasswordComponent } from "@features/forgot-password/forgot-password.component";
//import { DragDropComponent } from "./components/drag-drop/drag-drop.component";
//import { DragDropComponent } from "./components/drag-drop/drag-drop.component";

const routes: Routes = [
  // { path: "", pathMatch: "full", redirectTo: RouteNames.frontPage },
  {
    path: RouteNames.login,
    component: ElLoginComponent
    //canActivate: [AppCanActivateGuard]
  },
  {
    path: RouteNames.resetPassword,
    component: PasswordResetCreateComponent,
    canActivate: [AppCanActivateGuard]
  },
  {
    path: RouteNames.newPassword,
    component: PasswordResetCreateComponent,
    canActivate: [AppCanActivateGuard]
  },
  {
    path: RouteNames.forgotPassword,
    component: ForgotPasswordComponent
    //canActivate: [AppCanActivateGuard]
  },
  // {
  //   path: RouteNames.protected,
  //   component: ProtectedPageComponent,
  //   canActivate: [AppCanActivateGuard]
  // },
  {
    path: "",
    component: AppMainComponent,
    children: [
      { path: "", component: DashboardComponent, canActivate: [AppCanActivateGuard] },
      { path: "dragdrop", component: DragDropComponent },
      {
        path: "superadmin/customers",
        canActivate: [AppCanActivateGuard],
        data: { preload: false },
        loadChildren: () => import("./components/superadmin/customer/superadmin-customer.module").then(m => m.SuperAdminCustomerModule)
      },
      { path: "uikit/formlayout", component: FormLayoutComponent },
      { path: "uikit/input", component: InputComponent },
      { path: "uikit/floatlabel", component: FloatLabelComponent },
      { path: "uikit/invalidstate", component: InvalidStateComponent },
      { path: "uikit/button", component: ButtonComponent },
      { path: "uikit/table", component: TableComponent },
      { path: "uikit/list", component: ListComponent },
      { path: "uikit/tree", component: TreeComponent },
      { path: "uikit/panel", component: PanelsComponent },
      { path: "uikit/overlay", component: OverlaysComponent },
      { path: "uikit/media", component: MediaComponent },
      { path: "uikit/menu", loadChildren: () => import("./components/menus/menus.module").then(m => m.MenusModule) },
      { path: "uikit/message", component: MessagesComponent },
      { path: "uikit/misc", component: MiscComponent },
      { path: "uikit/charts", component: ChartsComponent },
      { path: "uikit/file", component: FileComponent },
      { path: "pages/crud", component: CrudComponent },
      { path: "pages/timeline", component: TimelineComponent },
      { path: "pages/empty", component: EmptyComponent },
      { path: "icons", component: IconsComponent },
      { path: "blocks", component: BlocksComponent },
      { path: "documentation", component: DocumentationComponent },
      {
        path: "tabdemo",
        loadChildren: () => import("./components/tabdemo/tabdemo.module").then(m => m.TabdemoModule)
      },
      {
        path: "my-user",
        loadChildren: () => import("./components/my-user/my-user.module").then(m => m.MyUserModule)
      },
      {
        path: "users",
        canActivate: [AppCanActivateGuard],
        loadChildren: () => import("./components/user/user.module").then(m => m.UserModule)
      },
      {
        path: "devices",
        canActivate: [AppCanActivateGuard],
        data: { preload: false },
        loadChildren: () => import("./components/device/device.module").then(m => m.DeviceModule)
      }
    ]
  },

  { path: "pages/landing", component: LandingComponent },
  { path: "pages/login", component: LoginComponent },
  { path: "pages/error", component: ErrorComponent },
  { path: "pages/notfound", component: NotfoundComponent },
  { path: "pages/access", component: AccessComponent },
  { path: "**", redirectTo: "pages/notfound" }

  // { path: "landing", component: LandingComponent },
  // { path: "login", component: ButtonComponent },
  // { path: "**", redirectTo: "pages/empty" }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      useHash: false,
      onSameUrlNavigation: "reload",
      scrollPositionRestoration: "enabled"
      // enableTracing: true
    })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}

// Export all the components with a route
export const routedComponents = [];
