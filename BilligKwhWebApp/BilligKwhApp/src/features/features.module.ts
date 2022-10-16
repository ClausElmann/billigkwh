// Native
import { NgModule } from "@angular/core";
//import { ModalWindowContentComponentsModule } from "@shared/components/modal-content/modal-window-content-components.module";
// Shared
import { SharedModule } from "@shared/shared.module";
import { InputTextareaModule } from "primeng/inputtextarea";
import { ProgressSpinnerModule } from "primeng/progressspinner";
//import { SupportCaseComponent } from "./support/support-case/support-case.component";
import { MessagesModule } from "primeng/messages";
import { MessageModule } from "primeng/message";
import { DynamicDialogModule } from "primeng/dynamicdialog";
import { FeaturesRoutingModule, routedComponents } from "./features-routing.module";

const primeNgModules = [MessagesModule, MessageModule, InputTextareaModule, ProgressSpinnerModule, DynamicDialogModule];

@NgModule({
  imports: [SharedModule, FeaturesRoutingModule, ...primeNgModules],
  declarations: [...routedComponents]
})
export class FeaturesModule { }
