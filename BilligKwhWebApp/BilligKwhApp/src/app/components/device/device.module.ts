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
import { RouterModule } from "@angular/router";
import { NgxPrintModule } from "ngx-print";
import { NgxExtendedPdfViewerModule } from "ngx-extended-pdf-viewer";
import { DeviceListComponent } from "./devicelist.component";
import { MultiSelectModule } from "primeng/multiselect";
import { DevicedetailComponent } from "./device-detail.component";
import { DevicedetailEditComponent } from "./device-detail-edit.component";
import { DeviceDetailConsumptionComponent } from "./device-detail-consumption.component";

const primeNgModules = [ToolbarModule, TableModule, DialogModule, ToastModule, InputTextareaModule, InputTextModule, ButtonModule, TabMenuModule, MultiSelectModule];

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([
      {
        path: "",
        component: DeviceListComponent
      },
      {
        path: ":id",
        component: DevicedetailComponent,
        children: [
          // { path: "edit/:id", redirectTo: "main", pathMatch: "full" },
          { path: "edit", component: DevicedetailEditComponent },
          // { path: "parts", component: GruppetavledetailKomponenterComponent },
          // // { path: "components", component: GruppetavledetailKomponenterPlaceringComponent },
          // { path: "images", component: GruppetavleImagesComponent },
          { path: "consumptions", component: DeviceDetailConsumptionComponent }
        ]
      }
      // {
      //   path: "create",
      //   component: GruppetavleCreateEditComponent
      // }
    ]),
    ...primeNgModules,
    NgxPrintModule,
    NgxExtendedPdfViewerModule
  ],
  declarations: [DeviceListComponent, DevicedetailComponent, DevicedetailEditComponent, DeviceDetailConsumptionComponent]
})
export class DeviceModule {}
