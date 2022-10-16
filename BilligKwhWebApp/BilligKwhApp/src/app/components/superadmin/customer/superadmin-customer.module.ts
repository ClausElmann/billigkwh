import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { SharedModule } from "@shared/shared.module";
import { ToolbarModule } from "primeng/toolbar";
import { TableModule } from "primeng/table";
import { DialogModule } from "primeng/dialog";
import { ToastModule } from "primeng/toast";
import { InputTextareaModule } from "primeng/inputtextarea";
import { InputTextModule } from "primeng/inputtext";
import { ButtonModule } from "primeng/button";
import { TabMenuModule } from "primeng/tabmenu";
import { SuperAdminCustomerListComponent } from "./superadmin-customerlist.component";
import { RouterModule } from "@angular/router";
import { SuperAdminCustomerIdComponent } from "./superadmin-customer-id.component";
import { SuperAdminCustomerCreateEditComponent } from "./superadmin-customer-createedit.component";
import { SuperAdminCustomerUserListComponent } from "./superadmin-customer-userlist.component";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { ConfirmPopupModule } from "primeng/confirmpopup";
import { SuperAdminEmailListComponent } from "./superadmin-emaillist.component";
import { CalendarModule } from "primeng/calendar";
import { EditorModule } from "primeng/editor";
import { SuperAdminElprisListComponent } from "./superadmin-elprislist.component";

const primeNgModules = [
  DialogModule,
  ConfirmDialogModule,
  ConfirmPopupModule,
  ToastModule,
  ToolbarModule,
  TableModule,
  InputTextareaModule,
  InputTextModule,
  ButtonModule,
  TabMenuModule,
  CalendarModule,
  EditorModule
];

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([
      {
        path: "",
        component: SuperAdminCustomerListComponent
      },
      {
        path: "emails",
        component: SuperAdminEmailListComponent
      },
      {
        path: "elpriser",
        component: SuperAdminElprisListComponent
      },
      {
        path: ":id",
        component: SuperAdminCustomerIdComponent,
        children: [
          // { path: "edit/:id", redirectTo: "main", pathMatch: "full" },
          { path: "main", component: SuperAdminCustomerCreateEditComponent },
          { path: "users", component: SuperAdminCustomerUserListComponent }
        ]
      },
      {
        path: "create",
        component: SuperAdminCustomerCreateEditComponent
      }
    ]),
    ...primeNgModules
  ],
  declarations: [
    SuperAdminCustomerListComponent,
    SuperAdminCustomerIdComponent,
    SuperAdminCustomerCreateEditComponent,
    SuperAdminCustomerUserListComponent,
    SuperAdminEmailListComponent,
    SuperAdminElprisListComponent
  ]
})
export class SuperAdminCustomerModule {}
