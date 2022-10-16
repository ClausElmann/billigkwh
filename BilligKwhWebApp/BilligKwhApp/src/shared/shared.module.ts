import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { TranslateModule } from "@ngx-translate/core";
// import { TextMaskModule } from "angular2-text-mask";
import { AccordionModule } from "primeng/accordion";
import { AutoCompleteModule } from "primeng/autocomplete";
import { ButtonModule } from "primeng/button";
// Prime Ng
import { ChartModule } from "primeng/chart";
import { CheckboxModule } from "primeng/checkbox";
import { DropdownModule } from "primeng/dropdown";
import { InputTextModule } from "primeng/inputtext";
import { ListboxModule } from "primeng/listbox";
import { ProgressBarModule } from "primeng/progressbar";
import { ProgressSpinnerModule } from "primeng/progressspinner";
import { RadioButtonModule } from "primeng/radiobutton";
import { SelectButtonModule } from "primeng/selectbutton";
import { StepsModule } from "primeng/steps";
import { TooltipModule } from "primeng/tooltip";
// import { BiAccordionModule } from "./components/bi-accordion/bi-accordion.module";
// import { BiCheckboxComponent } from "./components/bi-custom-inputs/bi-checkbox/bi-checkbox.component";
// import { BiDateAndTimeInputModule } from "./components/bi-custom-inputs/bi-date-and-time-input/bi-date-and-time-input.module";
// import { BiDropDownComponent } from "./components/bi-custom-inputs/bi-drop-down/bi-drop-down.component";
// import { BiRadioButtonComponent } from "./components/bi-custom-inputs/bi-radio-button/bi-radio-button.component";
// import { BiSearchInputComponent } from "./components/bi-custom-inputs/bi-search-input/bi-search-input.component";
// import { BiSwitchComponent } from "./components/bi-custom-inputs/bi-switch/bi-switch.component";
// import { BiTextInputComponent } from "./components/bi-custom-inputs/bi-text-input/bi-text-input.component";
// import { BiPasswordInputComponent } from "./components/bi-custom-inputs/password-input/password-input.component";
// import { BiFlyOutMenuItemComponent } from "./components/bi-fly-out-menu/bi-fly-out-menu-item.component";
// import { BiFlyOutMenuComponent } from "./components/bi-fly-out-menu/bi-fly-out-menu.component";
// import { BiMapModule } from "./components/bi-map/bi-map.module";
// import { BiMessageComponent } from "./components/bi-message.component";
// import { BiNotificationBarComponent } from "./components/bi-notification-bar/bi-notification-bar.component";
//import { BiTabsComponent } from "./classes/components/bi-tabs/bi-tabs.component";
// import { BoxWithCheckboxesComponent } from "./components/box-with-checkboxes/box-with-checkboxes.component";
// import { BiFileUploaderComponent } from "./components/file-uploader/bi-file-uploader.component";
// import { GoTopBottomComponent } from "./components/go-top-bottom/go-top-bottom.component";
// import { BiEditableListComponent } from "./components/lists/bi-editable-list/bi-editable-list.component";
// import { BiTemplateListComponent } from "./components/lists/bi-template-list/bi-template-list.component";
// import { BiSpinnerModule } from "./components/spinner/bi-spinner.module";
// import { BiTablesModule } from "./components/tables/bi-tables.module";
// import { BiAutofocusDirective } from "./directives/bi-autofocus.directive";
// import { BiDisableControlDirective } from "./directives/bi-disable-control.directive";
// import { BiDragDropDirective } from "./directives/bi-dragDrop.directive";
// import { BiRequireRolesDirective } from "./directives/bi-require-roles.directive";
// import { AddRowDirective } from "./directives/p-add-new-row.directive";
// import { PrimeEditorAutoScrollFixDirective } from "./directives/prime-editor-auto-scroll-fix.directive";
// import { BiPipesModule } from "./pipes/bi-pipes.module";
import { FileUploadModule } from "primeng/fileupload";
import { BackButtonDirective } from "./directives/backbutton.directive";


const exportedComponents = [
  // BiSearchInputComponent,
  // BiRadioButtonComponent,
  // BiCheckboxComponent,
  // BiDropDownComponent,
  // BiNotificationBarComponent,
  // BiFileUploaderComponent,

  // // Custom input controls
  // BiSwitchComponent,
  // BiTextInputComponent,
  // BiSearchInputComponent,
  // BiRadioButtonComponent,
  // BiCheckboxComponent,
  // BiDropDownComponent,
  // BiPasswordInputComponent,

  // BoxWithCheckboxesComponent,
  // GoTopBottomComponent,
  // BiMessageComponent,

  // BiFlyOutMenuComponent,
  // BiFlyOutMenuItemComponent,

  //BiTabsComponent

  // BiTemplateListComponent,
  // BiEditableListComponent
];

const exportedDirectives = [
  BackButtonDirective
  // BiDragDropDirective, BiRequireRolesDirective,
  //   BiDisableControlDirective, PrimeEditorAutoScrollFixDirective,
  //   BiAutofocusDirective, AddRowDirective
];

const importedExportedModules = [TranslateModule, FormsModule, ReactiveFormsModule, CommonModule
  // TextMaskModule, BiMapModule, BiAccordionModule, BiSpinnerModule, BiDateAndTimeInputModule, BiTablesModule
];

const primeNgModules = [
  CheckboxModule,
  ChartModule,
  RadioButtonModule,
  FileUploadModule,
  ButtonModule,
  ProgressSpinnerModule,
  InputTextModule,
  DropdownModule,
  ProgressBarModule,
  AccordionModule,
  AutoCompleteModule,
  TooltipModule,
  ListboxModule,
  SelectButtonModule,
  StepsModule
];

@NgModule({
  imports: [...primeNgModules, //BiPipesModule,
  ...importedExportedModules, RouterModule],
  declarations: [...exportedDirectives, ...exportedComponents],
  exports: [...importedExportedModules,
  //...exportedDirectives,
  ...exportedComponents,
    InputTextModule,
    TooltipModule,
    // BiPipesModule,
    FileUploadModule,
    ButtonModule,
    DropdownModule,
    RadioButtonModule,
    ChartModule,
    SelectButtonModule,
    CheckboxModule]
})
export class SharedModule { }
