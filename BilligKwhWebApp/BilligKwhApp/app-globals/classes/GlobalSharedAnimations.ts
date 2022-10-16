import { trigger, animate, transition, style } from "@angular/animations";

export class GlobalSharedAnimations {
  /**
    * Zoom in/out an element. Usage: apply [@zoomInOut] on an element along with an *ngIf directive
    */
  public static enterLeaveZoom = trigger("zoomInOut", [
    transition(":enter", [style({ transform: "scale(0)", fontSize: "0" }), animate(200, style({ transform: "scale(1)", fontSize: "*" }))]),
    transition(":leave", [style({ transform: "scale(1)", fontSize: "*" }), animate(200, style({ transform: "scale(0)", fontSize: "0" }))])
  ]);

}
