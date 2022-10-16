import { trigger, state, style, transition, animate, query, stagger } from "@angular/animations";

/**
 * All custom animations used in the app
 * Angular IO doc for animations: https://angular.io/api/animations/transition
 * About parameters that can be specificed for an animation (as well as many  other things): https://www.yearofmoo.com/2017/06/new-wave-of-animation-features.html
 */
export class BiCustomAnimations {
  /**
   * Animation for folding in/out an element by adjusting height while also adding fade-effect. Used, for example, for AddressSearchItems, StdReceiverSearchItems etc..
   * Usage: add [@foldInOut]="someProperty" to an element which height should be animated. "someProperty" must be a string toggling between "in" and "out"
   */
  public static zeroToAutoHeightAnimation = trigger("foldInOut", [
    state(
      "out",
      style({
        height: "*", // instead of using "auto", this works better
        margin: "*",
        opacity: 1
      })
    ),
    state(
      "in",
      style({
        height: "0px",
        opacity: "0",
        overflow: "hidden",
        margin: "0"
      })
    ),
    transition("out <=> in", animate("400ms ease-in-out"))
  ]);

  public static zeroToAutoWidthAnimation = trigger("autoWidth", [
    transition(
      ":enter",
      [
        style({
          width: "0px",
          opacity: "0",
          overflow: "hidden",
          margin: "0"
        }),
        animate("400ms linear", style({ opacity: "1", width: "{{width}}", margin: "*" }))
      ],
      { params: { width: "*" } }
    ),
    transition(
      ":leave",
      [
        style({
          margin: "*",
          width: "{{width}}",
          opacity: 1
        }),
        animate(
          "400ms linear",
          style({
            width: "0px",
            opacity: "0",
            margin: "0"
          })
        )
      ],
      { params: { width: "*" } }
    )
  ]);

  /**
   * Folds in/out an element by adjusting height and opacity.
   * NB: if used on a component host element, the component must have css display property set (typically block).
   * Add [@foldInOut] to the element
   */
  public static foldInOutEnterLeave = trigger("foldInOut", [
    transition(":enter", [style({ opacity: "0", height: "0" }), animate("{{speed}}ms ease-out", style({ opacity: "1", height: "*" }))], {
      params: { speed: 400 }
    }),
    transition(":leave", [style({ opacity: "1", height: "*" }), animate("{{speed}}ms ease-out", style({ opacity: "0", height: "0" }))], {
      params: { speed: 400 }
    })
  ]);

  public static bounceInOutEnterLeave = trigger("bounceInOut", [
    transition(":enter", [style({ opacity: "0", height: "0" }), animate("{{speed}}ms cubic-bezier(.46,1.18,.82,1.09)", style({ opacity: "1", height: "*" }))], {
      params: { speed: 400 }
    }),
    transition(":leave", [style({ opacity: "1", height: "*" }), animate("{{speed}}ms ease-out", style({ opacity: "0", height: "0" }))], {
      params: { speed: 400 }
    })
  ]);


  /**
   * Animation for fading in/out an element being displayed "block".
   * Usage: add [@fadeInOutBlock]="{value: expression, params: {speed: speed, opacity: 1} }" to an element. "expression" must be a something resolving to
   * string toggling between "in" and "out", and "speed" is ms. "opacity" param is opacity after fade in (default 1).
   */
  public static fadeInOutBlockElement = trigger("fadeInOutBlock", [
    state(
      "in",
      style({
        height: "*",
        opacity: "{{ opacity }}"
      }),
      { params: { opacity: 1 } }
    ),
    state(
      "out",
      style({
        height: "0",
        margin: "0",
        maxHeight: "0",
        overflow: "hidden",
        opacity: "0"
      })
    ),

    transition("out <=> in", animate("{{ speed }}ms ease-in-out"), {
      params: { speed: 400 }
    })
  ]);

  /**
   * Animation for fading in/out an element being displayed "inline-block". 2 optional params can be provided: 1) opacity after element is faded in and
   * 2) the animation speed in ms. Default values are 1 and 400ms, respectively.
   * Usage example: <div [@fadeInOutInlineBlock]="{value: somePropertyOrExpression, params: {opacity: .75, speed: 500}}">I'm being animated!</div>
   * "somePropertyOrExpression" must be a something resolving to a string toggling between "in" and "out. "
   */
  public static fadeInOutInlineBlock = trigger("fadeInOutInlineBlock", [
    state(
      "in",
      style({
        display: "inline-block",
        opacity: "{{ opacity }}"
      }),
      { params: { opacity: 1 } }
    ),
    state(
      "out",
      style({
        display: "none",
        opacity: "0"
      })
    ),
    transition("out <=> in", animate("{{speed}}ms ease-in-out"), {
      params: { speed: 400 }
    })
  ]);

  /**
* Fade out/in animation used for animating the enter/leave state of element. Will fade in/out element by adjusting opacity and height.
* Usage example (you don't need an ngFor - you could as well just add it on some single element):
<div>
      <div *ngFor="let item of items" [@fadeInOut]>
          {{ item.name }}
      </div>
</div>

* Now each of the list items will become an animation when entering/leaving the view. Passing the length of the ngFor items array to the animation expression ensures that
* the animation is run each time this length changes.
*/
  public static fadeInOut = trigger("fadeInOut", [
    transition(":enter", [style({ opacity: "0" }), animate("{{speed}}ms ease-out", style({ opacity: "1" }))], {
      params: { speed: 400 }
    }),
    transition(":leave", [style({ opacity: "1" }), animate("{{speed}}ms ease-out", style({ opacity: "0" }))], {
      params: { speed: 400 }
    })
  ]);

  /**
   * Animation used for only fading IN an element and not out.
   */
  public static fadeIn = trigger("fadeIn", [transition(":enter", [style({ opacity: 0 }), animate(500, style({ opacity: 1 }))])]);

  /**
   * Fade out/in animation used for the entering/leaving of elements - typically in ngFor and ngIf. Will fade in/out elements by adjusting opacity and height.
   * This must be used on a container which CHILDREN should be animated.
   * Usage example:
   * <div [@fadeInOutChildren]="listItems.length">
   *      <div *ngFor="let item of items">
   *          {{ item }}
   *      </div>
   * </div>
   *
  * Now each of the list items will become an animation when entering/leaving the view. Passing the length of the ngFor items array to the animation expression ensures that
  * the animation is run each time this length changes.
   */
  public static fadeInOutChildren = trigger("fadeInOutChildren", [
    transition("* <=> *", [
      // animate elements entering the view
      query(":enter", [style({ opacity: 0 }), animate("300ms ease-in", style({ opacity: 1 }))], { optional: true }),

      // animate elements leaving the view
      query(":leave", [style({ opacity: 1 }), animate("300ms ease-out", style({ opacity: 0 }))], { optional: true })
    ])
  ]);

  /**
   * For adding fade in animation with small slide from top. Added to entering and leaving child elements of a container. Typically for elements using *ngIf or *ngFor directives
   * Usage example:
   * <div *ngIf="somePropertyOrWhatEver" [@fadeInDown]>
   *      <p>I'm being animated when I become visible </p>
   * </div>
   */
  public static fadeInDown = trigger("fadeInDown", [
    transition(":enter", [
      style({ opacity: "0", transform: "translateY(-10px)" }),
      animate(".3s ease-in", style({ opacity: "1", transform: "translateY(0px)" }))
    ]),
    transition(":leave", [
      style({ opacity: "1", transform: "translateY(0px)" }),
      animate(".3s ease-out", style({ opacity: "0", transform: "translateY(-10px)" }))
    ])
  ]);

  /**
   * Zooms in/out an element ('in' means element becomes visible and 'out' means element disappears).
   * Usage example: <div [@zoomInOut]="expression">I will be zoomed in and out</div>. Expressions must reoslve to either "in" or "out"
   */
  public static zoomInOut = trigger("zoomInOut", [
    state("in", style({ transform: "scale(1)", fontSize: "*" })),
    state("out", style({ transform: "scale(0)", fontSize: "0" })),
    transition("in <=> out", animate("400ms ease-in-out"))
  ]);
  /**
   * Animation for rotating an element. Default rotation is 90deg - use parameter to change.
   * Usage: add [@rorate]="{value: expression, params: {degrees: nrOfDegrees} }" to an element. "expression" must be a something resolving to
   * string toggling between "default" and "rotated".
   */
  public static rotate = trigger("rotate", [
    state(
      "default",
      style({
        transform: "rotate(0)"
      })
    ),
    state(
      "rotated",
      style({
        transform: "rotate({{ degrees }}deg)"
      }),
      { params: { degrees: 90 } }
    ),

    transition("default <=> rotated", animate("200ms ease-in-out"))
  ]);

  /**
   * Animation used for switching between tabs
   * Usage: add [@tabSwitch]="somePropertyThatChanges" to the parent element hosting the tab content. We use query()
   * to query :enter and :leave children.
   */
  public static tabSwitch = trigger("tabSwitch", [
    transition("* <=> *", [
      query(":enter", [style({ opacity: 0, zIndex: 0 }), animate("300ms ease-in", style({ opacity: 1, zIndex: 100 }))], { optional: true })
    ])
  ]);

  /**
   * Animation used for lists. Adds a nice fade in with stagger effect (e.i. animates 1 at a time). Usage (could also be a list of divs - <ul> is not required!):
   * <ul [@listAnim]="items.length">
   *      <li *ngFor="let i of items"> i.title </li>
   * </ul>
   */
  public static listAnimation = trigger("listAnimation", [
    // Each time the binding expression changes, run this animation
    transition("* <=> *", [
      query(":enter", [style({ opacity: 0, "max-height": 0 }), stagger(100, [animate("0.2s", style({ opacity: 1, "max-height": "100%" }))])], {
        optional: true
      })
    ])
  ]);

  /**
   * Animates a container with 2 states: "left" and "right". E.i. slides either left or right.
   * Usage ex.: <div [@slideLeftRightSingle]="leftRightExpr"> <div class="child1"></div> <div class="child2"></div> </div>
   */
  public static slideLeftRightSingle = trigger("slideLeftRightSingle", [
    state("left", style({ transform: "translateX(0)" })),
    state("right", style({ transform: "translateX(-100%)" })),
    transition("*=>*", animate("200ms ease-in-out"))
  ]);
  /**
   * Slides down an element. Just add [@slideDown]="expression" where "expression" must resolve to either "down" or "up".
   * You can add parameters for specifying start and end value of css "top" property like this (default values used):
   * [@slideDown]="{value: expression, params: {startTop: '-20px', endTop: '0px'}}"
   */
  public static slideDown = trigger("slideDown", [
    state("up", style({ opacity: 0, top: "{{startTop}}", height: "0" }), {
      params: { startTop: "-20px" }
    }),
    state("down", style({ top: "{{endTop}}", opacity: 1, display: "block" }), {
      params: { endTop: "0px" }
    }),
    transition("up => down", animate("200ms ease-in-out")), //cubic-bezier(.47,.73,.46,1.27)
    transition("down => up", animate("200ms ease-out"))
  ]);

  public static slideUp

  /**
   * Slides down an element as entering animation and up as leaving animation. It never removes the element, only hides by setting height and opacity to 0 when "up".
   * Just add [@slideDownHide]="expression" where "expression" must resolve to either "down" or "up".
   * You can add parameters for specifying start and end value of css "top" property like this (default values used):
   * [@slideDown]="{value: expression, params: {startTop: '-20px', endTop: '0px'}}"
   */
  public static slideDownHide = trigger("slideDownHide", [
    state(
      "up",
      style({
        opacity: 0,
        top: "{{startTop}}",
        height: "0",
        overflow: "hidden"
      }),
      { params: { startTop: "-20px" } }
    ),
    state("down", style({ top: "{{endTop}}", opacity: 1, height: "*" }), {
      params: { endTop: "0px" }
    }),
    transition("up => down", animate("200ms ease-in-out")),
    transition("down => up", animate("200ms ease-out"))
  ]);

  /**
   * Slides in an element from the right. Used with ngIf or, for example, for elements in ngFor.
   * Usage: <p *ngIf="someVar" [@slideInElRight]="''">Smooth right slide for me!</p>
   */
  public static slideInElRight = trigger("slideInElRight", [
    transition(":enter", [
      style({ opacity: "0", transform: "translateX(10px)" }),
      animate(".3s ease-in", style({ opacity: "1", transform: "translateX(0)" }))
    ])
  ]);


  /**
   * Slides an element in from the left and then slides it back to the right when leaving
   */
  public static slideInFromLeft = trigger("slideInFromLeft", [
    transition(":enter", [
      style({ opacity: "0", transform: "translateX(-100%)" }),
      animate(".1s ease-in", style({ opacity: "1", transform: "translateX(0)" }))
    ]),
    transition(":leave", [
      style({ opacity: "1", transform: "translateX(0)" }),
      animate(".1s ease-in", style({ opacity: "0", transform: "translateX(-100%)" }))
    ])
  ]);


  /**
   * Animation intended to be used via HostBinding on a routed component for a nice entering animation.
   * Usage in component: @HostBinding("@slideInPageRight") get slideInHost() { return true; }
   * Note: You can also use a parameter called "speed" by returning an object like this:
   * {value: true, params: {speed: "500ms"}} - by default, spped is 400ms
   */
  public static slideInPageRight = trigger("slideInPageRight", [
    transition(
      ":enter",
      [style({ opacity: "0", transform: "translateX(50%)" }), animate("{{speed}} ease-in", style({ opacity: "1", transform: "translateX(0)" }))],
      { params: { speed: "200ms" } }
    ),
    transition(
      ":leave",
      [
        style({
          opacity: "1",
          transform: "translateX(0)",
          display: "block",
          position: "absolute",
          top: "25%"
        }),
        animate("{{speed}} ease-in", style({ opacity: "0", transform: "translateX(-50%)" }))
      ],
      { params: { speed: "200ms" } }
    )
  ]);

  //#endregion
}
