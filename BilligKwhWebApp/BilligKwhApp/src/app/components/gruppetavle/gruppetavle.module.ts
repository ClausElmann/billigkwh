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
import { GruppetavleListComponent } from "./gruppetavlelist.component";
import { GruppetavleDetaljeComponent } from "./gruppetavle-detalje.component";
import { GruppetavleDetaljeKomponenterComponent } from "./gruppetavle-detalje-komponenter.component";
import { GruppetavleDetaljeKomponenterPlaceringComponent } from "./gruppetavle-detalje-komponenter-placering.component";
import { GruppetavleImagesComponent } from "./gruppetavle-detalje-images.component";
import { MultiSelectModule } from "primeng/multiselect";
import { GruppetavleDetaljeEditComponent } from "./gruppetavle-detalje-edit.component";
import { GruppetavleDetaljeEmailsComponent } from "./gruppetavle-detalje-emaillist.component";
import { NgxPrintModule } from "ngx-print";
import { NgxExtendedPdfViewerModule } from "ngx-extended-pdf-viewer";

const primeNgModules = [ToolbarModule, TableModule, DialogModule, ToastModule, InputTextareaModule, InputTextModule, ButtonModule, TabMenuModule, MultiSelectModule];

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([
      {
        path: "",
        component: GruppetavleListComponent
      },
      {
        path: ":id",
        component: GruppetavleDetaljeComponent,
        children: [
          // { path: "edit/:id", redirectTo: "main", pathMatch: "full" },
          { path: "edit", component: GruppetavleDetaljeEditComponent },
          { path: "parts", component: GruppetavleDetaljeKomponenterComponent },
          // { path: "components", component: GruppetavleDetaljeKomponenterPlaceringComponent },
          { path: "images", component: GruppetavleImagesComponent },
          { path: "emails", component: GruppetavleDetaljeEmailsComponent }
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
  declarations: [
    GruppetavleListComponent,
    GruppetavleDetaljeComponent,
    GruppetavleDetaljeKomponenterComponent,
    GruppetavleDetaljeKomponenterPlaceringComponent,
    GruppetavleImagesComponent,
    GruppetavleDetaljeEditComponent,
    GruppetavleDetaljeEmailsComponent
  ]
})
export class GruppetavleModule {}
