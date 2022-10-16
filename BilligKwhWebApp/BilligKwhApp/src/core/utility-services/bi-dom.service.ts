import { DOCUMENT } from "@angular/common";
import {
  ApplicationRef,
  ComponentFactoryResolver,
  ComponentRef, EmbeddedViewRef,
  Inject, Injectable, Injector, Type,
  ViewContainerRef
} from "@angular/core";
import { Observable, ReplaySubject } from "rxjs";
// import * as pdfMake from "pdfmake/build/pdfmake";
// import * as pdfFonts from "pdfmake/build/vfs_fonts";
// (pdfMake as any).vfs = pdfFonts.pdfMake.vfs;
// import htmlToPdfmake from "html-to-pdfmake";

@Injectable({
  providedIn: "root"
})
export class BiDomService {
  private insertedComponents: { [id: string]: ComponentRef<any> } = {};

  private _loadedLibraries: { [url: string]: ReplaySubject<any> } = {};

  constructor(
    private componentFactoryResolver: ComponentFactoryResolver,
    private appRef: ApplicationRef,
    private injector: Injector,
    @Inject(DOCUMENT) private readonly document: any
  ) { }

  /**
   * Dynamically creates a component, sets in/out properties (if any) and inserts the component's HTML to a container or to body (clears what's already inside).
   * NB: One and only one of the parameters "viewContainer" and "containerId" MUST be  provided!
   * @param component The type name of the component you want to insert. IMPORTANT: if this component has outputs, they should just
   * be defiend like normal functions and not using @Output(). This makes it possible to easy set both output/input props and not having to subscribe to eventemitters.
   * @param componentId A unique id for this component so it can be retrieved later when it should be deleted/removed.
   * @param inOutProps Optional object of input and/or output properties provided as object. These will automatically be set on the created component. Property names in component must match with the keys in these objects
   * @param viewContainer Optional reference to a view container (as ViewContainerRef)
   * @param containerId Optional HTMl id (excluding the #) of the container element (e.g. a div) to which the component should be appended
   */
  public insertComponent(
    component: Type<any>,
    componentId: string,
    inOutProps?: InOutProperties,
    viewContainer?: ViewContainerRef,
    containerId?: string
  ) {
    if (!componentId || componentId === "") throw new Error("You must provide an id for your component!");
    if (!viewContainer && !containerId) throw new Error("DomService says: You haven't told me where to insert your component!");

    const componentRef = this.componentFactoryResolver.resolveComponentFactory(component).create(this.injector);

    // Set component's in/out props if there are any
    if (inOutProps) this.setComponentInOutProperties(componentRef, inOutProps);

    // Store this component for later retrieval
    this.insertedComponents[componentId] = componentRef;

    // Attach this component to our Angular app's environment, so that change detection is also performed on this
    this.appRef.attachView(componentRef.hostView);

    // Now append to either the provided view container or the body - first clear content before inserting new
    if (viewContainer) {
      viewContainer.clear();
      viewContainer.insert(componentRef.hostView);
    } else {
      // insert the component in the container with id as provided
      const element = document.getElementById(`${containerId}`);
      if (element) {
        element.innerHTML = "";
        element.appendChild((componentRef.hostView as EmbeddedViewRef<any>).rootNodes[0]);
      } else throw new Error("DomService says: I couldn't find any container element with id '" + containerId + "'");
    }
  }

  /**
   * Removes an inserted component from the view OR clears inner html of a container element depending on passed arguments.
   * @param componentId Id of the component as provided in the method for appending component component
   * @param containerId Id of the container in which html as been inserted. That is, when it's not a component that was inserted.
   */
  public removeComponentOrHtml(componentId?: string, containerId?: string) {
    if (componentId) {
      this.appRef.detachView(this.insertedComponents[componentId].hostView);
      this.insertedComponents[componentId].destroy();
    } else document.getElementById(containerId).innerHTML = "";
  }

  /**
   * For inserting HTML snippets into an html container element, overwriting the inner html of it. Html must be a jQuery object so that you use event handlers in html.
   * @param viewContainer A ref to a viewcontainer in which this html should be appended
   * @param elementId Id of an HTML element to which we append (excluding the #).
   * @param elementClassName The class name of an HTML element to which we append (appending on the first instance with that class name)
   */
  public insertHtml(jQueryObject: JQuery<HTMLElement>, viewContainer?: ViewContainerRef, elementId?: string, elementClassName?: string) {
    let container;
    if (viewContainer) container = viewContainer.element.nativeElement;
    else if (elementId) {
      container = document.getElementById(elementId);
    } else if (elementClassName) container = document.getElementsByClassName(elementClassName)[0];
    else throw new Error("DomService.appendHtml says: you must provide a viewcontainer, element ID or element class name");

    if (container) {
      $(container).html("").append(jQueryObject);
    } else {
      throw new Error("DomService.appendHtml says: I couldn't find element!");
    }
  }

  /**
   * Remove html from the dom
   */
  public removeHtml(viewContainer?: ViewContainerRef, elementId?: string, elementClassName?: string) {
    if (viewContainer) viewContainer.element.nativeElement.innerHTML = "";
    else if (elementId) document.getElementById(elementId).innerHTML = "";
    else if (elementClassName) (document.getElementsByClassName(elementClassName)[0] as HTMLElement).innerHTML = "";
    else throw new Error("DomService.removeHtml says: I don't know where to remove html.");
  }

  /**
   * Finds the caret position in an input, copies the input's value, inserts the supplied value string at the
   * position of the caret and then returns that new string. Intended for merge fields.
   * DOES NOT modify the input's value.
   * @returns The new value for the input after field has been inserted
   */
  public getNewTextAfterMergefieldInsertedAtCaretPos(value: string, input: HTMLInputElement, position?: number): string {

    if (position && position >= 0) {
      return input.value.substring(0, position) + value + " " + input.value.substring(position, input.value.length);
    }

    //IE support
    if ((document as any).selection) {
      input.focus();
      const sel = (document as any).selection.createRange();
      sel.text = value + " ";
      return input.value;
    }
    //MOZILLA and others
    else {
      const startPos = input.selectionStart;
      const endPos = input.selectionEnd;
      return input.value.substring(0, startPos) + value + " " + input.value.substring(endPos, input.value.length);
    }
  }

  /*
** Returns the caret (cursor) position of the specified text field (oField).
** Return value range is 0-oField.value.length.
*/
  public getCaretPosition(input: HTMLInputElement): number {
    return input.selectionStart;
  }

  public setCaretPosition(input: HTMLInputElement, position: number) {
    input.setSelectionRange(position, position, "none");
  }

  /**
   * Dynamically creates a style tag with the provided css and adds it to <head> element
   * @param cssString Pure css as you write in a .css file. But as a string
   */
  public insertGlobalCss(cssString: string) {
    $(`<style type="text/css">${cssString}</style>`).appendTo("head");
  }

  public loadScript(url: string, id: string): Observable<any> {
    if (this._loadedLibraries[id]) {
      return this._loadedLibraries[id].asObservable();
    }

    this._loadedLibraries[id] = new ReplaySubject();

    const script = this.document.createElement("script");
    script.type = "text/javascript";
    script.async = true;
    script.src = url;
    script.onload = () => {
      this._loadedLibraries[id].next(undefined);
      this._loadedLibraries[id].complete();
    };

    this.document.body.appendChild(script);

    return this._loadedLibraries[id].asObservable();
  }

  public loadStyle(url: string): Observable<any> {
    if (this._loadedLibraries[url]) {
      return this._loadedLibraries[url].asObservable();
    }

    this._loadedLibraries[url] = new ReplaySubject();

    const style = this.document.createElement("link");
    style.type = "text/css";
    style.href = url;
    style.rel = "stylesheet";
    style.onload = () => {
      this._loadedLibraries[url].next(undefined);
      this._loadedLibraries[url].complete();
    };

    const head = document.getElementsByTagName("head")[0];
    head.appendChild(style);

    return this._loadedLibraries[url].asObservable();
  }

  /**
   *
   * @param fileContent The file data as binary data, ArrayBuffer
   * @param fileName The name given to the file to download
   * @param fileType The MIME type of the data. See the list of MIME types here: https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types.
   * For Excel, the type should be application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.
   *
   */
  public downloadFile(fileContent: ArrayBuffer, fileName: string, fileType: string) {
    // Push the Binary result for Blob 'Creation'
    const binaryData = [];
    binaryData.push(fileContent);

    // Create Downloadlink to append to the dom
    const downloadLink = document.createElement("a");
    downloadLink.href = window.URL.createObjectURL(new Blob(binaryData, { type: fileType }));
    downloadLink.setAttribute("download", fileName);

    // Append the link, simulate click to trigger native download feature in browser and remove the link from DOM again (clean up)
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
  }

  // public downloadAsPdf(content: string, fileName: string) {
  //   const pdf = htmlToPdfmake(content);
  //   pdfMake.createPdf({ content: pdf }).download(fileName);
  // }
  /**
   * Downloads a canvas as a png
   * @param fileName Name of file without extension
   */
  public downloadCanvas(canvas: HTMLCanvasElement, fileName: string) {
    const context = canvas.getContext("2d");

    // Refer - https://stackoverflow.com/questions/50104437/set-background-color-to-save-canvas-chart/50126796#50126796
    context.save();
    context.globalCompositeOperation = "destination-over";
    context.fillStyle = "white";
    context.fillRect(0, 0, canvas.width, canvas.height);
    context.restore();

    const base64Url = canvas.toDataURL("image/png"),
      hiddenElement = document.createElement("a");

    // Create Download Link
    hiddenElement.href = base64Url;
    hiddenElement.target = "_blank";
    hiddenElement.download = `${fileName}.png`;
    hiddenElement.click();
  }

  /**
   * Helper for setting in/out props on a component.
   * @param component The component with inout/output properties. IMPOTANT: this component shouldn't define outputs with @Output() but just define normal, callable functions
   * @param componentInOutProps The input/output properties defined as objects in an object
   */
  private setComponentInOutProperties(component: ComponentRef<any>, componentInOutProps: InOutProperties) {
    const inputs = componentInOutProps.inputs;
    const outputs = componentInOutProps.outputs;
    if (inputs) {
      for (const field of Object.keys(inputs)) {
        component.instance[field] = inputs[field];
      }
    }
    if (outputs) {
      for (const field of Object.keys(outputs)) {
        component.instance[field] = outputs[field];
      }
    }
  }
}

/**
 * Defines an object of input and output properties being objects of key/value pairs
 */
export interface InOutProperties {
  inputs?: { [key: string]: any };
  /**
   * object of output functions where key is the function name. IMPORTANT: when you provide the function to an output, remember to call the "bind()" method on it. Example:
   * {
   *   someOutputName: (() => { //do stuff here }).bind(this)
   * }
   * If you don't do this, the "this" context will not refer to the component defininf the function resulting in nuclear war
   */
  // eslint-disable-next-line @typescript-eslint/ban-types
  outputs?: { [key: string]: Function };
}
