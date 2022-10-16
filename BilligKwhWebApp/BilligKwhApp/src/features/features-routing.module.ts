//import { UserRoleGuard } from "@core/routing/guards/user-role.guard";
//========= ROUTE CONFIG FOR ALL FEATURE AREAS =======================\\
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AppCanActivateGuard } from "@core/routing/guards/app-can-activate.guard";
import { UserRoleGuard } from "@core/routing/guards/user-role.guard";
import { RouteNames } from "@shared/classes/RouteNames";
import { ElLoginComponent } from "@features/login/el-login.component";
import { PasswordResetCreateComponent } from "src/app/components/password-reset-create/password-reset-create.component";
import { ForgotPasswordComponent } from "./forgot-password/forgot-password.component";
// import { ProfileRoleCanActivateGuard } from "@core/routing/guards/profile-role.guard";
// import { UserRoleGuard } from "@core/routing/guards/user-role.guard";
// // All route names
// import { RouteNames } from "@shared/classes/RouteNames";
// import { UserRoleEnum } from "@shared/interfaces-and-enums/UserRoleEnum";
// import { BiLoginComponent } from "./bi-login/bi-login.component";
// import { TransparentLoginComponent } from "./bi-login/transparent-login.component";
// import { CanActivateWizardRoute } from "./message-wizard/message-wizard-routing.module";
// // Import components ================
// import { NewsComponent } from "./news/news.component";
// import { PasswordResetCreateComponent } from "./password-reset-create/password-reset-create.component";
// import { SupportComponent } from "./support/support.component";
// import { TermsAndConditionsComponent } from "./terms-and-conditions/terms-and-conditions.component";

//=================================

// Define feature site routes
const routes: Routes = [
  {
    path: "",
    canActivateChild: [AppCanActivateGuard],
    children: [
      // {
      //   path: RouteNames.broadcasting,
      //   loadChildren: () => import("./broadcasting/broadcasting.module").then((m) => m.BroadcastingModule)
      // },
      { path: RouteNames.login, component: ElLoginComponent },

      {
        path: "bla",
        component: ElLoginComponent
        //canActivate: [AppCanActivateGuard]
      },
      {
        path: RouteNames.newPassword,
        component: ForgotPasswordComponent
        //canActivate: [AppCanActivateGuard]
      }
      // { path: RouteNames.transparentLogin, component: TransparentLoginComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [UserRoleGuard]
})
export class FeaturesRoutingModule { }

// Export array of the routeds components - to be used in the declarations array of the feature module
export const routedComponents = [ElLoginComponent, PasswordResetCreateComponent, ForgotPasswordComponent];
