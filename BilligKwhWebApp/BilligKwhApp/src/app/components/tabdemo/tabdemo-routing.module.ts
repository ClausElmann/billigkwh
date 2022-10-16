import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { Tab2Component } from "./tab2/tab2.component";
import { Tab1Component } from "./tab1/tab1.component";
import { TabdemoComponent } from "./tabdemo.component";

/**
 * Routes for My User feature area
 */
const tabdemoRoutes: Routes = [
  {
    path: "",
    component: TabdemoComponent,
    children: [
      {
        path: "",
        redirectTo: "tab1",
        pathMatch: "full"
      },
      {
        path: "tab1",
        component: Tab1Component
      },
      {
        path: "tab2",
        component: Tab2Component
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(tabdemoRoutes)],
  exports: [RouterModule]
})
export class TabdemoRoutingModule { }

// Export array of the MyUserComponents  - to be used in the declarations array of the feature module
export const TabdemoComponents = [TabdemoComponent, Tab1Component, Tab2Component];
