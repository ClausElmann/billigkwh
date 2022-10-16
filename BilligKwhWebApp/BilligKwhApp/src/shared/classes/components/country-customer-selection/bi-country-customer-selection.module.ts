import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { DropdownModule } from "primeng/dropdown";
import { ProgressSpinnerModule } from "primeng/progressspinner";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { ButtonModule } from "primeng/button";
import { TranslateModule } from "@ngx-translate/core";
import { BiCountryCustomerSelectionComponent } from "./bi-country-customer-selection.component";

@NgModule({
  declarations: [BiCountryCustomerSelectionComponent],
  imports: [CommonModule, FormsModule, ReactiveFormsModule, TranslateModule.forChild(), DropdownModule, ProgressSpinnerModule, ButtonModule],
  exports: [BiCountryCustomerSelectionComponent]
})
export class BiCountryCustomerSelectionModule { }
