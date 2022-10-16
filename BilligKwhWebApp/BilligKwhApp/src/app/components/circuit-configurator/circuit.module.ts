import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { SharedModule } from "@shared/shared.module";
import { RouterModule } from "@angular/router";
import { ToolbarModule } from "primeng/toolbar";
import { TableModule } from "primeng/table";
import { DialogModule } from "primeng/dialog";
import { ToastModule } from "primeng/toast";
import { CircuitEditConfigurationComponent } from "./circuit-edit/circuit-edit-configuration.component";
import { DragDropModule } from "@angular/cdk/drag-drop";
import { NgxPrintModule } from "ngx-print";
import { BadgeModule } from "primeng/badge";

const primeNgModules = [ToolbarModule, TableModule, DialogModule, ToastModule, BadgeModule];

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    NgxPrintModule,
    RouterModule.forChild([
      {
        path: "",
        component: CircuitEditConfigurationComponent
      }
    ]),
    ...primeNgModules,
    DragDropModule
  ],
  declarations: [CircuitEditConfigurationComponent]
})
export class CircuitModule {}
