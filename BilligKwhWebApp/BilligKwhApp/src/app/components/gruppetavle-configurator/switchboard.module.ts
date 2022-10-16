import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { SharedModule } from "@shared/shared.module";
import { RouterModule } from "@angular/router";
import { ToolbarModule } from "primeng/toolbar";
import { TableModule } from "primeng/table";
import { DialogModule } from "primeng/dialog";
import { ToastModule } from "primeng/toast";
import { SwitchboardEditConfigurationComponent } from "./switchboard-edit/switchboard-edit-configuration.component";
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
        component: SwitchboardEditConfigurationComponent
      }
    ]),
    ...primeNgModules,
    DragDropModule
  ],
  declarations: [SwitchboardEditConfigurationComponent]
})
export class SwitchboardModule {}
