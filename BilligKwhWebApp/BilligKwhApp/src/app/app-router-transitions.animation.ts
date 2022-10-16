

import { AnimationTriggerMetadata, transition, trigger } from "@angular/animations";
import { routeStateSlideLeftAnimationMetaData, routeStateSlideRightAnimationMetaData, slideEnterLeave } from "app-globals/classes/router-animation-definitions";

/**
 * Route animation used when switching between child routes. This is copied from Angular doc: https://angular.io/guide/route-animations
 */
export const routeStateSlideAnimation =
  trigger("routeAnimations", [
    // Slide left when going to create/edit and slide right the other way around
    transition("benchmarkIndexPage => benchmarkCreateEditPage", routeStateSlideLeftAnimationMetaData),
    transition("* => benchmarkIndexPage", routeStateSlideRightAnimationMetaData),

    // Slide in create/edit page when comming from a master page to a create/edit page
    transition("master => createEdit", routeStateSlideLeftAnimationMetaData),
    transition("createEdit => master", routeStateSlideRightAnimationMetaData)


  ]);



export const appRouterAnimations: AnimationTriggerMetadata = trigger("appRouterTransition", [
  // animation when you go from main admin menu to some admin submenu
  transition("adminMain <=> adminSub", slideEnterLeave),
  transition("superAdminMain <=> superAdminSub", slideEnterLeave),
  transition("settingsMain <=> settingsSub", slideEnterLeave)
]);
