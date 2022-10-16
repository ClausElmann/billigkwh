import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { SharedModule } from "@shared/shared.module";
import { RouterModule } from "@angular/router";
//import { UserEditGuard } from "./user-edit/user-edit.guard";
import { ToolbarModule } from "primeng/toolbar";
import { TableModule } from "primeng/table";
import { DialogModule } from "primeng/dialog";
import { ToastModule } from "primeng/toast";
import { UserEditComponent } from "./user-edit/user-edit.component";
import { UserEditMainComponent } from "./user-edit/user-edit-main.component";
import { UserEditGuard } from "./user-edit.guard";
//import { UserResolver } from "./user-resolver.service";
import { InputTextareaModule } from "primeng/inputtextarea";
import { InputTextModule } from "primeng/inputtext";
import { ButtonModule } from "primeng/button";
import { TabMenuModule } from "primeng/tabmenu";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { ConfirmPopupModule } from "primeng/confirmpopup";

const primeNgModules = [DialogModule, ConfirmDialogModule, ConfirmPopupModule, ToastModule, ToolbarModule, TableModule, InputTextareaModule, InputTextModule, ButtonModule, TabMenuModule];

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([
      {
        path: ":id",
        component: UserEditComponent
        //resolve: { resolvedData: ProductResolver }
      },
      {
        path: ":id/edit",
        component: UserEditComponent,
        canDeactivate: [UserEditGuard],
        //resolve: { resolvedData: UserResolver },
        children: [
          //{ path: "", redirectTo: "main", pathMatch: "full" },
          { path: "", component: UserEditMainComponent }
          // { path: "info", component: UserEditInfoComponent }
        ]
      }
      // {
      //   path: "",
      //   component: UserEditComponent,
      //   //canDeactivate: [UserEditGuard],
      //   children: [
      //     { path: ":id", redirectTo: "main", pathMatch: "full" },
      //     { path: "main", component: UserEditMainComponent },
      //     { path: "info", component: UserEditInfoComponent }
      //   ]
      // }
    ]),
    ...primeNgModules
  ],
  declarations: [UserEditComponent, UserEditMainComponent]
})
export class UserModule {}
