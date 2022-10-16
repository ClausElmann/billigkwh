import { Injectable, Type } from "@angular/core";
import { BiDomService, InOutProperties } from "./bi-dom.service";
import { TranslateService } from "@ngx-translate/core";
import { isStringNullOrEmpty } from "@shared/variables-and-functions/helper-functions";

/**
 * Service used for opening/closing the global modal window and dynamically insert components as content.
 * This service assumes that the folowing HTML exists somewhere in page:
 *  <div id="bi-modal-container">
        <div id="bi-modal-content"></div>
    </div>
    <div id="bi-modal-overlay"></div>
 *
 * Ids can be modified if you want but do remember to also update these ids inside this service.
 * It is also assumed that when adding a css class on the outer modal container (e.g. "bi-show"), then the modal
 * will become visible. Use some global styling for styling modal window. NB: for correct animation, you must specify host-animation for
 * the dynamic component you want to insert. E.g. insert this in component's metadata: host: {'[@fadeInOut]': '{value: "", params: {speed: 200}}'}
 */
@Injectable()
export class BiModalService {

  /**
   * Html id of the outer container for modal window (this is used for setting class on it so the inner content can be animated).
   * NB: Use this ID as the modal component id when opening modal
   */
  private modalContainerElementId = "bi-modal-container";

  /**
   * HTML id of the element that should contain the modal content.
   * NB: Use this ID when opening modal window so that content will be added to the correct element
   */
  private modalContentElementId = "bi-modal-content";

  /**
   * Flag telling whether the current content in modal window is a component (needed when we close modal)
   */
  private modalContentIsComponent: boolean;

  constructor(private domService: BiDomService, private translator: TranslateService) {
    // Setup keyboard enter event for the modal close button
    $("#bi-modal-container").keyup(keyEvent => {
      if (keyEvent.key === "Enter" && document.activeElement.id === "modal-close-button")
        this.close();
    });
  }

  /**
   * Opens modal window and automatically show backdrop/overlay
   * @param componentTypeOrjQueryObject Type of the component you want to insert OR a an object created using jQuery so that eventhandlers can be used
   * @param inOutProps The input/output props for the component (if you provided component type and it has in/out props that should be set)
   */
  public open(componentTypeOrjQueryObject: Type<unknown> | JQuery<HTMLElement>, inOutProps?: InOutProperties, closeCallback?: () => void) {
    if ((componentTypeOrjQueryObject as any).jquery) {
      this.domService.insertHtml(componentTypeOrjQueryObject as JQuery<HTMLElement>, null, this.modalContentElementId);
      this.modalContentIsComponent = false;
    } else {
      this.domService.insertComponent(
        componentTypeOrjQueryObject as Type<any>,
        this.modalContainerElementId,
        inOutProps,
        null,
        this.modalContentElementId
      );
      this.modalContentIsComponent = true;
    }

    const containerEl = document.getElementById(this.modalContainerElementId);
    containerEl.className = "bi-show";
    $("#modal-close-button")
      .off("click")
      .on("click", () => (closeCallback ? closeCallback() : this.close()));

    // Focus modal window
    $("#bi-modal-container").attr("tabindex", "-1").focus();

  }

  /**
   * Opens modal window with a header and dismiss button inside. This is a simple modal that we use alot of places and therefore we have this
   * helper funtion.
   * @param callback Optional callback - if not passed, modal window will close itself. NB: you must close the modal yourself in the callback!
   */
  public openSimpleDismissable(title: string, text?: string, callback?: () => void) {
    const $modalHtml = $("<div/>").css({ "padding-top": "2em", "text-align": "center" });

    if (!isStringNullOrEmpty(title)) $modalHtml.append($(`<h3>${title}</h3>`));

    if (!isStringNullOrEmpty(text)) $modalHtml.append($(`<p>${text}</p>`));

    const okButton = $(`
    <button pButton type="button" style="width: 8.5em" class="p-button p-component p-ripple margin-top-1">
      <span class="p-button-label">OK</span>
    </button>`);

    okButton.on("click", () => (callback ? callback() : this.close()));
    $modalHtml.append(okButton);

    this.open($modalHtml, undefined, callback);
  }

  /**
   * Opens modal window used for something to confirm. This is a modal that we use alot of places and therefore we have this
   * helper funtion.
   * @param config Config. object for the modal window. Contains the following:
   * title: title for the modal Window
   * hasRedOkButton: Set this true to make the "yes" button red. This is useful for when this buton has a negative outcome like deleting something.
   * ok and cancel action callbacks to set logic for when user hits "ok" and "cancel" buttons. NOTE: the modal closes by default if cancelling.
   * buttonsTranslationKeys: To control the text being displayed on the buttons for yes/no. Default is keys for "yes" and "no"
   */
  public openConfirm(config: {
    title: string,
    message?: string,
    hasRedOkButton: boolean,
    okAction: () => void,
    cancelAction?: () => void,
    buttonTranslationKeys?: { okKey: string, cancelKey: string }
  }) {
    if (isStringNullOrEmpty(config.title)) throw ("Not title provided for modal!");


    const $modalHtml = $("<div style='padding-top: 1.5em; align-items: center'/>").addClass("flex-column");
    // First add title:
    $modalHtml.append($(`<h3>${config.title}</h3>`));
    if (!isStringNullOrEmpty(config.message))
      $modalHtml.append(`<p id="modal-text">${config.message}</p>`);


    // Then create buttons
    const $buttonGroup = $("<div style='margin-top: 1.5em; text-align: center; width: 100%'/>");
    const okButton$ = $(`
    <button pButton type="button" style="width: calc(50% - 0.25em); max-width: 12em; white-space: nowrap;overflow: hidden;text-overflow: ellipsis" class="p-button p-component p-ripple">
      <span class="p-button-label">${this.translator.instant(config.buttonTranslationKeys ? config.buttonTranslationKeys.okKey : "shared.Yes")}</span>
    </button>`);

    okButton$.on("click", () => config.okAction());
    if (config.hasRedOkButton) okButton$.addClass("p-button-danger");
    const cancelButton$ = $(`
    <button pButton type="button" style="width: calc(50% - 0.25em); max-width: 12em" class="p-button p-button-secondary p-component p-ripple margin-left-half">
      <span class="p-button-label">${this.translator.instant(config.buttonTranslationKeys ? config.buttonTranslationKeys.cancelKey : "shared.No")}</span>
    </button>`);
    cancelButton$.on("click", () => {
      if (config.cancelAction) config.cancelAction();
      this.close();
    });

    // Add buttons to button group and finally add this to the modal html
    $buttonGroup.append(okButton$).append(cancelButton$);
    $modalHtml.append($buttonGroup);
    this.open($modalHtml);
  }

  public close() {
    document.getElementById(this.modalContainerElementId).className = "";
    if (this.modalContentIsComponent) this.domService.removeComponentOrHtml(this.modalContainerElementId);
    else this.domService.removeComponentOrHtml(null, this.modalContentElementId);
  }

  /**
   * For updating what's shown in the modal content container
   * @param componentTypeOrjQueryObject Type of the component you want to insert OR just a plain html string
   * @param inOutProps The input/output props for the component (if you provided component type and it has in/out props that should be set)
   */
  public updateModalContent(componentTypeOrjQueryObject: Type<any> | JQuery<HTMLElement>, inOutProps?: InOutProperties) {
    if ((componentTypeOrjQueryObject as any).jquery) {
      this.domService.insertHtml(componentTypeOrjQueryObject as JQuery<HTMLElement>, null, this.modalContentElementId);
      this.modalContentIsComponent = false;
    } else {
      this.domService.insertComponent(
        componentTypeOrjQueryObject as Type<any>,
        this.modalContainerElementId,
        inOutProps,
        null,
        this.modalContentElementId
      );
      this.modalContentIsComponent = true;
    }
  }

  public updateModalText(textContent: string) {
    $("#modal-text").html(textContent);
  }


}
