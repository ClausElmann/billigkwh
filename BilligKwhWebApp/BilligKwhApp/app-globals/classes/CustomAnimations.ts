import { trigger, style, transition, animate, query, AnimationMetadata, group } from "@angular/animations";

/**
 * All custom animations used in the app
 * Angular IO doc for animations: https://angular.io/api/animations/transition
 * About parameters that can be specificed for an animation (as well as many  other things): https://www.yearofmoo.com/2017/06/new-wave-of-animation-features.html
 */
export class CustomAnimations {
  //#region SLIDING ANIMATIONS
  private static leftSlide: AnimationMetadata[] = [
    query(":enter, :leave", style({ position: "absolute", width: "{{ width }}", zIndex: "{{ zIndex }}" }), { params: { zIndex: "1500", width: "100%" } }),
    query(":leave", style({ transform: "translateX(0%)", opacity: 1 })), // start at x pos 0
    query(":enter", style({ transform: "translateX(90%)", opacity: 0.2 })), // start from the right

    // run animation in parallel
    group([
      query(":leave", animate("400ms ease-out", style({ transform: "translateX(-100%)", opacity: 0.2 }))), // go out from middle -> left
      query(":enter", animate("400ms ease-in", style({ transform: "translateX(0%)", opacity: 1 }))) // come in from right -> left
    ])
  ];

  private static rightSlide: AnimationMetadata[] = [
    query(":enter, :leave", style({ position: "absolute", width: "{{ width }}", zIndex: "{{ zIndex }}" }), { params: { zIndex: "1500", width: "100%" } }),
    query(":enter", style({ transform: "translateX(-90%)", opacity: 0.2 })), // start at x pos 0
    query(":leave", style({ transform: "translateX(0%)", opacity: 1 })), // start from the right

    // run animation in parallel
    group([
      query(":enter", animate("400ms ease-out", style({ transform: "translateX(0%)", opacity: 1 }))), // come in from left ->  middle
      query(":leave", animate("400ms ease-in", style({ transform: "translateX(100%)", opacity: 0.2 }))) // go out from middle -> right
    ])
  ];

  /**
   * Animates entering and leaving content with a left or right slide effect depending on an index. This index is some number
   * that either increment or decrements. An increment will make a left slide (meaning content to the right should become visible), a decrement a right
   * slide (left content visible). This makes it possible to have multiple elements in a slide-container.
   * Usage example:
   * <div [@slideLeftRightMulti]="someIndex">
   *      <p *ngIf="">I'm being slided in when I become visible! </p>
   *      <p *ngIf="">I'm being slided in when I become visible! </p>
   *      <p *ngIf="">I'm being slided in when I become visible! </p>
   * </div>
   * <button (click)="someIndex -= someIndex">Prev</button> <button (click)="someIndex += someIndex">Next</button>
   */
  public static slideLeftRightMulti = trigger("slideLeftRightMulti", [
    transition(":increment", CustomAnimations.leftSlide),
    transition(":decrement", CustomAnimations.rightSlide)
  ]);

}
