import { PasswordModule } from "primeng/password";
import { NgModule } from "@angular/core";
import { SharedModule } from "@shared/shared.module";
import { InputTextModule } from "primeng/inputtext";
import { CheckboxModule } from "primeng/checkbox";
import { DropdownModule } from "primeng/dropdown";
import { TabMenuModule } from "primeng/tabmenu";
import { MyUserComponents, MyUserRoutingModule } from "./my-user-routing.module";
import { InputTextareaModule } from "primeng/inputtextarea";

const primeNgModules = [InputTextModule, CheckboxModule, DropdownModule, TabMenuModule, PasswordModule, InputTextareaModule];

@NgModule({
  imports: [SharedModule, MyUserRoutingModule, ...primeNgModules],
  providers: [],
  declarations: [...MyUserComponents]
})
export class MyUserModule { }
