import { NgModule } from "@angular/core";
import { SharedModule } from "@shared/shared.module";
import { InputTextModule } from "primeng/inputtext";
import { CheckboxModule } from "primeng/checkbox";
import { DropdownModule } from "primeng/dropdown";
import { TabdemoComponents, TabdemoRoutingModule } from "./tabdemo-routing.module";
import { TabMenuModule } from "primeng/tabmenu";

const primeNgModules = [InputTextModule, CheckboxModule, DropdownModule, TabMenuModule];

@NgModule({
  imports: [SharedModule, TabdemoRoutingModule, ...primeNgModules],
  providers: [],
  declarations: [...TabdemoComponents]
})
export class TabdemoModule { }
