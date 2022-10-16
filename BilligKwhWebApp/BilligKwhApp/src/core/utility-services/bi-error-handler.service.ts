import { HttpErrorResponse } from "@angular/common/http";
import { ErrorHandler, Injectable, Injector } from "@angular/core";
import { BiNotificationConfig, BiToastNotificationService, NotificationType } from "./bi-toast-notification.service";
import { isRunningInIE } from "@globals/helper-functions";
import { TranslateService } from "@ngx-translate/core";

/**
 * Custom, global error handler for Angular that we use to log front end errors!
 */
@Injectable()
export class BiErrorHandlerService implements ErrorHandler {
  /**
   * Ctor. We need the Angular Injector as some dependencies in this service would cause cyclic dependency if defined in ctor
   * (e.i. auto inject by Angular DI) - they must manually be injected when needed!
   * Info here: https://medium.com/@amcdnl/global-error-handling-with-angular2-6b992bdfb59c
   */
  constructor(private injector: Injector, private translator: TranslateService) {}

  public handleError(error: Error): void {
    // If HttpErrorResponse, it's a server error and we handle that differently
    if (error instanceof HttpErrorResponse) this.handleServerResponse(error);
    else this.handleClientError(error);
  }

  /**
   * Helper for handling server response during development
   */
  private handleServerResponse(response: HttpErrorResponse) {
    console.error("SERVER RESPONSE: ", response);
    // if unauthorized, access token will be refreshed so don't do anything.
    // Or if error 400 and it was regarding the refreshtoken, don't do anytning - login prompt should be shown by now.
    if (response.status === 401 || (response.status === 400 && response.error?.errorMessage?.indexOf && response.error.errorMessage.indexOf("token") !== -1)) return;

    // Some other server error occured. Show to user...
    const notifier = this.injector.get(BiToastNotificationService);
    // If we have log id, this must be included in message
    const errorMessage = response.error.logId
      ? this.translator.instant("errorMessages.GeneralErrorWithLogId", {
          logId: response.error.logId
        })
      : this.translator.instant("errorMessages.ErrorOccurred");

    const notificationConfig = new BiNotificationConfig(NotificationType.WARNING, this.translator.instant("errorMessages.Error"), errorMessage);

    notificationConfig.sticky = true;
    notifier.createNotification(notificationConfig);
  }

  /**
   * Helper for handling client/browser errors. Unlike server errors, these errors are also logged in backend.
   */
  private handleClientError(err: Error) {
    // As of Angular 9, unsafe resource usage in DOM always throws an errror even though we use ByPassSecurityTrust... methods so don't
    // anything if the error is one like that
    if (err.message && err.message.indexOf("unsafe value used in a resource URL") !== -1) return;
    if (err.message && err.message.indexOf("GEOCODER_GEOCODE") !== -1) return;

    console.error("CLIENT ERROR: ", err);
    // IE throws many type errors and we just want to suppress these
    if (!isRunningInIE() || ((err as any).stack && (err as any).stack.indexOf("TypeError") === -1)) {
      const notifier = this.injector.get(BiToastNotificationService);
      const notificationConfig = new BiNotificationConfig(
        NotificationType.WARNING,
        "Der skete desværre en uventet fejl. Prøv venligst igen senere, eller kontakt support. fejl: " + (err as any).message,
        undefined
      );
      notificationConfig.timeout = 10000;
      notifier.createNotification(notificationConfig);
    }
  }
}
