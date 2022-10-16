//==================================== Definition of router transition for the root AppComponent ================================\\
//==================================== To be used on a wrapper for the root <router-outlet> tag ================================\\

// We use states in animation trigger. For all the routes, where you want animation, you must set route-data property "state"
// so the root component can get this state and pass to the animation trigger. In this file, you can then specify what animation should be used between these routes.
// More info and inspiration from here:  https://stackoverflow.com/a/48012701

import { animate, animateChild, AnimationMetadata, group, query, style } from "@angular/animations";

/**
 * Defines slide animation for entering and leaving elements
 */
export const slideEnterLeave: AnimationMetadata[] = [
  query(":enter, :leave", style({ position: "fixed", width: "100%", zIndex: 1500 }), { optional: true }),

  // First styling of leaving/entering elements before animation
  query(":leave", [style({ transform: "translateX(0%)", opacity: 1 })], { optional: true }),
  query(":enter", [style({ opacity: 0 })], { optional: true }),

  // now define animations and what styling they animate against. Group these so they run in parallel
  group([
    // entering elements comes in from the right and leaving elements goes to the left
    query(":leave", animate("400ms ease-out", style({ transform: "translateX(-50%)", opacity: 0 })), { optional: true }),
    query(":enter", animate("400ms ease-in", style({ opacity: 1 })), { optional: true })
  ])
];


export const routeStateSlideLeftAnimationMetaData: AnimationMetadata[] = [
  style({ position: "relative" }),
  query(":enter, :leave", [
    style({
      position: "absolute",
      top: 0,
      left: 0,
      width: "100%"
    })
  ], { optional: true }),
  query(":enter", [
    style({ left: "100%" })
  ], { optional: true }),
  query(":leave", animateChild(), { optional: true }),
  group([
    query(":leave", [
      animate("300ms ease-out", style({ left: "-100%" }))
    ], { optional: true }),
    query(":enter", [
      animate("300ms ease-out", style({ left: "0%" }))
    ], { optional: true })
  ]),
  query(":enter", animateChild(), { optional: true })
];

export const routeStateSlideRightAnimationMetaData: AnimationMetadata[] = [
  style({ position: "relative" }),
  query(":enter, :leave", [
    style({
      position: "absolute",
      top: 0,
      left: 0,
      width: "100%"
    })
  ], { optional: true }),
  query(":enter", [
    style({ left: "-100%" })
  ], { optional: true }),
  query(":leave", animateChild(), { optional: true }),
  group([
    query(":leave", [
      animate("300ms ease-out", style({ left: "100%" }))
    ], { optional: true }),
    query(":enter", [
      animate("300ms ease-out", style({ left: "0%" }))
    ], { optional: true })
  ]),
  query(":enter", animateChild(), { optional: true })
];
