import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { SharedModule } from "@shared/shared.module";
import { RouterModule } from "@angular/router";
import { ToolbarModule } from "primeng/toolbar";
import { TableModule } from "primeng/table";
import { DialogModule } from "primeng/dialog";
import { ToastModule } from "primeng/toast";
import { DoorEditConfigurationComponent } from "./door-edit/door-edit-configuration.component";
import { DragDropModule } from "@angular/cdk/drag-drop";
import { NgxPrintModule } from "ngx-print";

const primeNgModules = [ToolbarModule, TableModule, DialogModule, ToastModule];

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    NgxPrintModule,
    RouterModule.forChild([
      {
        path: "",
        component: DoorEditConfigurationComponent
      }
    ]),
    ...primeNgModules,
    DragDropModule
  ],
  declarations: [DoorEditConfigurationComponent]
})
export class DoorModule {}
