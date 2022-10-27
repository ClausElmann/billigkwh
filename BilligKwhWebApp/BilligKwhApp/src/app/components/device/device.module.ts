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
import { DeviceDetaljeComponent } from "./device-detalje.component";
import { DeviceDetaljeEditComponent } from "./device-detalje-edit.component";

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
        component: DeviceDetaljeComponent,
        children: [
          // { path: "edit/:id", redirectTo: "main", pathMatch: "full" },
          { path: "edit", component: DeviceDetaljeEditComponent }
          // { path: "parts", component: GruppetavleDetaljeKomponenterComponent },
          // // { path: "components", component: GruppetavleDetaljeKomponenterPlaceringComponent },
          // { path: "images", component: GruppetavleImagesComponent },
          // { path: "emails", component: GruppetavleDetaljeEmailsComponent }
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
  declarations: [DeviceListComponent, DeviceDetaljeComponent, DeviceDetaljeEditComponent]
})
export class DeviceModule {}
