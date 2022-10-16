/////////// COMMON FUNCTIONS USED FOR CREATING DYNAMIC HTML THAT SHOULD BE INSERTED IN DOM \\\\\\\\\\\\\\\\\\\\\\\
////////////  OBS: AVOID WHEN YOU CAN (AS IT IS ANTI-ANGULAR WAY BUT MANY TIMES NECESSARY) \\\\\\\\\\\\\\\\\\\\

/**
 * Helper for creating the HTML for select input (dropdown) following our styleguide's HTML for dropdown component
 * @param value The value that should be set dropdown
 * @param selectValues Array of key/value pairs used for the <option> tags. key is the option's text, value is the option's value
 * @param name Optional name for the created <select> element. This will be set in an HTML5-attribute called "name"
 * @param onChangeCallback Optional eventhandler for when dropdown value changes
 */
export function createDropdownHtml(
  value: string | number,
  selectValues: Array<[string, string | number]>,
  name?: string,
  onChangeCallback?: (value: string) => void
): JQuery<HTMLElement> {
  const formField = $("<div class='FormField FormField--select'></div>");
  const formContainer = $("<div class='FormContainer'></div>");
  const selectEl = $(`<select id="dynamic-table-fieldItem" class="fieldItem" data-old-value="${value}" data-name="${name ? name : "select"}"></select>`); // store the old value in attribute "oldValue" so it's possible to cancel selection.
  // append the options
  for (let i = 0; i < selectValues.length; i++) {
    selectEl.append(
      `<option value="${selectValues[i][1]}">
          ${selectValues[i][0]}
       </option>`
    );
  }
  selectEl.val(value); // because this column has type "select", the data cell's data value is stored in HTML5-attribute "value"

  if (onChangeCallback)
    selectEl.on("change", () => {
      onChangeCallback(selectEl.val() as string); // in js, when getting a select's value, it's always a string
    });

  selectEl.on("click", (e) => e.stopPropagation()); // to be sure nothing else is triggered on click

  formContainer.append(selectEl);
  formContainer.append(`
                <span class="Icon">
                     <svg class="svg-icon">
                          <use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/StaticFiles/images/svg/svg-sprite.svg#arrow-down-white"></use>
                    </svg>
                </span>`);
  formField.append(formContainer);

  return formField;
}

/**
 * Helper function for dynamically adding a tooltip to an element.
 * @param el The element you want to add a tooltip for (wrapped as a jQuery object)
 * @param toolTipText What might this be? :p
 * @param cssTopPercent As this function was developed for a specific need where a default "top"-value was used for the
 * tooltip position, the position might not be correct everywhere you use this function. Therefore, you can see the value here.
 */
export function setupHoverTooltipForElement(el: JQuery<HTMLElement>, toolTipText: string, cssTopPercent?: number) {
  const _toolTip = $(`
  <div class="p-tooltip p-widget p-tooltip-top">
    <div class="p-tooltip-arrow"></div>
    <div class="p-tooltip-text p-shadow p-corner-all">${toolTipText}</div>
  </div>`).css({
    "display": "inline-block",
    "z-index": "1047",
    "right": "50%",
    "transform": "translateX(50%)",
    "width": `${toolTipText.length * 0.8}em`,
    "top": `${cssTopPercent ? cssTopPercent : "-190"}%`
  });

  const divWrapper = $("<div style='position: relative'></div>");
  divWrapper.insertBefore(el);
  el.appendTo(divWrapper);

  // Setup the hover feature
  el.hover(function () {
    $(this).parent().prepend(_toolTip);
  })
  .mouseleave(function () {
    $(this).prev().remove();
  });
}


export function setupHoverTooltipForRow(el: JQuery<HTMLElement>, toolTipText: string, messageHeadline: string, affectedStreets: Array<string>, areaHeadline: string) {
  // Create Tooltip Wrapper
  const divWrapper = $("<div style='position: relative'></div>");
  divWrapper.insertBefore(el);
  el.appendTo(divWrapper);

  // Slice the Array to 5 entries only
  if (affectedStreets.length > 5) 
    affectedStreets = affectedStreets.slice(0, 4);

  // Format the Addresses into a 'View String'
  const affectedStreetsString = affectedStreets.map(x => " " + x);

  // Td to Inject Tooltip
  const injector = el.children(".row-hover").first().css({ "position": "relative" });
  const _toolTip = $(`
    <div class="p-tooltip p-widget p-tooltip-top">
      <div class=""></div>
      <div class="p-tooltip-text p-shadow p-corner-all">
        <h3 style="margin: 0;">${ messageHeadline }:</h3>
        <p>${ toolTipText }</p>
        <hr style="margin: 0;">
        <h3 style="margin: 0;">${ areaHeadline }</h3>
        <p>${ affectedStreetsString }</p>
    </div>
    </div>`).css({
      "display": "inline-block",
      "z-index": "1047",
      "position": "absolute",
      "bottom": "100%",
      "left": "-1em",
      "min-width": "600px"
  });

  // Setup the hover feature
  el.hover(function () {
    injector.append(_toolTip);
  }).mouseleave(function () {
    _toolTip.remove();
  });
}
