import { Injectable } from "@angular/core";
import { ActiveToast, IndividualToastrConfig, ToastrService } from "ngx-toastr";
import { take } from "rxjs/operators";
import { uniqueID } from "@shared/variables-and-functions/helper-functions";

/**
 * Sevice wrapping the NGX Toastr library. Many of the configuration options from the
 * library are used and some renamed to more convenient names. The functionality hasn't changed, though, so you can see more info
 * on the documentation page of the library (or just by inspecting the types used which have the documentation already!).
 *
 */
@Injectable()
export class BiToastNotificationService {
  private activeToasts: Map<string, ActiveToast<any>> = new Map();

  constructor(private toaster: ToastrService) {}

  /**
   * Creates a new notification using passed config object for configuration.
   */
  public createNotification(config: BiNotificationConfig): ActiveToast<any> {
    const toastConfig: Partial<IndividualToastrConfig> = {
      positionClass: `toast-${config.positionClass}`,
      enableHtml: config.enableHtml,
      closeButton: true
    };
    if (config.sticky) toastConfig.disableTimeOut = true;
    else toastConfig.timeOut = config.timeout;

    let toast: ActiveToast<any>;
    switch (config.type) {
      case NotificationType.SUCCESS:
        toast = this.toaster.success(config.message, config.title, toastConfig);
        break;

      case NotificationType.INFO:
        toast = this.toaster.info(config.message, config.title, toastConfig);
        break;

      case NotificationType.ERROR:
        toast = this.toaster.error(config.message, config.title, toastConfig);
        break;

      case NotificationType.WARNING:
        toast = this.toaster.warning(config.message, config.title, toastConfig);
        break;
      default:
        toast = this.toaster.success(config.message, config.title, toastConfig);
        break;
    }
    // Add toast to map. If an id was provided, use that. Else we generate a random
    const toastId = config.id ? config.id : uniqueID();
    this.activeToasts.set(toastId, toast);

    // Delete the toast from our map when it's dismissed
    toast.onHidden.pipe(take(1)).subscribe(() => this.activeToasts.delete(toastId));

    return toast;
  }

  public getNotification(id: string) {
    return this.activeToasts.get(id);
  }

  /**
   * Updates a notification's message text if it exists.
   * @param id The id of the notification
   * @param message New message for the notification
   */
  public updateNotificationMessage(id: string, message?: string) {
    const toast = this.activeToasts.get(id);
    if (toast) toast.message = message;
  }
}

/**
 * Represents a configuration object used for configuring an Ngx Toastr toast notification
 */
export class BiNotificationConfig {
  /**
   * Set true if notification should not auto dismiss. Default false.
   */
  public sticky = false;

  /**
   * Time before notification is auto dismissed (not effective if sticky is true). Default is 3000 ms.
   */
  public timeout = 3000;

  /**
   * Class setting the position of the notification. Format along with all possiblitites:  "{top/bottom/center}-{left/center/right}". Default is "top-center".
   */
  public positionClass = "top-center";

  /**
   * To allow HTML content to be inserted in the notification's message. Default false
   */
  public enableHtml: boolean;

  /**
   * @param id Optional id for the toast in case you need to get it using the getNotification() method.
   * @param type Type of notification. Defined as enum and sets a matching color. Default is NotificationType.SUCCESS.
   */
  constructor(public type = NotificationType.SUCCESS, public title?: string, public message?: string, public id?: string) {}
}

export enum NotificationType {
  SUCCESS,
  INFO,
  ERROR,
  WARNING
}
