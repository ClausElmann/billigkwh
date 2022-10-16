import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { ErrorHandler, ModuleWithProviders, NgModule } from "@angular/core";
import { BiHttpInterceptor } from "./BiHttpInterceptor";
import { GlobalStateAndEventsService } from "./global-state-and-events.service";
import { AuthenticationService } from "./security/authentication.service";
import { AUTHENTICATION_SERVICE_TOKEN } from "./security/TokenAuthenticationService";
import { CustomerService } from "./services/customer.service";
import { LogService } from "./services/log.service";
import { SupportService } from "./services/support.service";
import { UserService } from "./services/user.service";
import { BiDateTimePickerConfigService } from "./utility-services/bi-date-time-picker-config.service";
import { BiErrorHandlerService } from "./utility-services/bi-error-handler.service";
import { BiLocalizationHelperService } from "./utility-services/bi-localization-helper.service";
import { BiModalService } from "./utility-services/bi-modal.service";
import { BiToastNotificationService } from "./utility-services/bi-toast-notification.service";
import { DataTableConfigService } from "./utility-services/datatable-config.service";

@NgModule({})
export class CoreModule {
  public static forRoot(): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule,
      providers: [
        {
          provide: HTTP_INTERCEPTORS,
          useClass: BiHttpInterceptor,
          multi: true
        },
        {
          provide: AUTHENTICATION_SERVICE_TOKEN,
          useClass: AuthenticationService
        },
        LogService,
        { provide: ErrorHandler, useClass: BiErrorHandlerService }, // use our own error handler instead of Angular's default
        GlobalStateAndEventsService,
        UserService,
        CustomerService,
        SupportService,
        BiDateTimePickerConfigService,
        DataTableConfigService,
        BiToastNotificationService,
        BiLocalizationHelperService,
        BiModalService
      ]
    };
  }
}
