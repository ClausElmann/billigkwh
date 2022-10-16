import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { UserProfileComponent } from "./user-profile/user-profile.component";
import { MyUserComponent } from "./my-user.component";
import { UserSecurityComponent } from "./user-security/user-security.component";

/**
 * Routes for My User feature area
 */
const routes: Routes = [
  {
    path: "",
    component: MyUserComponent,
    children: [
      {
        path: "",
        redirectTo: "profile",
        pathMatch: "full"
      },
      {
        path: "profile",
        component: UserProfileComponent
      },
      {
        path: "security",
        component: UserSecurityComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MyUserRoutingModule { }

// Export array of the MyUserComponents  - to be used in the declarations array of the feature module
export const MyUserComponents = [MyUserComponent, UserProfileComponent, UserSecurityComponent];
