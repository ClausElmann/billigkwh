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
import { KomponentListComponent } from "./komponentlist.component";
import { MultiSelectModule } from "primeng/multiselect";
import { KomponentDetaljeEditComponent } from "./komponent-detalje-edit.component";
import { NgxPrintModule } from "ngx-print";
import { NgxExtendedPdfViewerModule } from "ngx-extended-pdf-viewer";
import { KomponentDetaljeComponent } from "./komponent-detalje.component";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { ConfirmPopupModule } from "primeng/confirmpopup";

const primeNgModules = [
  ToolbarModule,
  TableModule,
  DialogModule,
  ToastModule,
  InputTextareaModule,
  InputTextModule,
  ButtonModule,
  TabMenuModule,
  MultiSelectModule,
  ConfirmDialogModule,
  ConfirmPopupModule
];

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([
      {
        path: "",
        component: KomponentListComponent
      },
      {
        path: ":id",
        component: KomponentDetaljeComponent,
        children: [
          // { path: "edit/:id", redirectTo: "main", pathMatch: "full" },
          { path: "edit", component: KomponentDetaljeEditComponent }
          // { path: "parts", component: KomponentDetaljeKomponenterComponent },
          // { path: "components", component: KomponentDetaljeKomponenterPlaceringComponent },
          // { path: "images", component: KomponentImagesComponent },
          // { path: "emails", component: KomponentDetaljeEmailsComponent }
        ]
      }
      // {
      //   path: "create",
      //   component: KomponentCreateEditComponent
      // }
    ]),
    ...primeNgModules,
    NgxPrintModule,
    NgxExtendedPdfViewerModule
  ],
  declarations: [KomponentListComponent, KomponentDetaljeComponent]
})
export class KomponentModule {}
