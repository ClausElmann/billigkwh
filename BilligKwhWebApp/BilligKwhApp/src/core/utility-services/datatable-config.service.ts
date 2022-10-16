import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { cloneObject } from "@globals/helper-functions";
/**
 * Core service used for configuration of jQuery DataTables throughout the web app
 */
import { DataTableResultModel } from "@models/common/DataTableResultModel";
import { PagedResultRequest, PageResultOrdering } from "@models/common/PagedResultRequest";
import { TranslateService } from "@ngx-translate/core";
import { createDropdownHtml } from "@shared/variables-and-functions/dynamic-html-functions";
import { isStringNullOrEmpty } from "@shared/variables-and-functions/helper-functions";
import moment from "moment-timezone";
import { map, take } from "rxjs/operators";
import { BiLocalizationHelperService } from "./bi-localization-helper.service";

@Injectable()
export class DataTableConfigService {

  /**
   * Used for Prime NG table's "currentPageReportTemplate" property
   */
  public datatablePaginatorText$ = this.translator.get(["dataTables.Shows", "dataTables.of", "dataTables.rows"])
    .pipe(map(translatedKeys => {
      return `${translatedKeys["dataTables.Shows"]} {last} ${translatedKeys["dataTables.of"]} {totalRecords} ${translatedKeys["dataTables.rows"]}`
    }));

  constructor(private translator: TranslateService, private httpClient: HttpClient, private dateConverter: BiLocalizationHelperService) { }

  /**
   * Gets the general configuratons for a DataTable used in this app
   * @param globalSearch Wether global search functionality should be enabled (default true)
   * @param usePagination Wether pagination functionality should be enabled (default true). If false, both the length menu dropdown well as the pagination at bottom of table will be removed
   * @param hasButtons Whether buttons will be used for the table. If true, an excel button will be added automatically.
   */
  public getGeneralDataTableConfig(globalSearch = true, usePagination = true, hasButtons = true): DataTables.Settings {
    let domProperty = "Bfltrip";
    if (!hasButtons) domProperty = domProperty.replace("B", "");
    if (!globalSearch) domProperty = domProperty.replace("f", ""); // no global search => remove search/filter input
    if (!usePagination) domProperty = domProperty.replace("l", "").replace("p", ""); // no pagination => remove length menu and pagination controls

    return {
      dom: domProperty,
      language: {
        lengthMenu:
          this.translator.instant("dataTables.Show") +
          " _MENU_ " +
          this.translator.instant("dataTables.rows") +
          " " +
          this.translator.instant("dataTables.perPage"),
        search: "",
        info:
          this.translator.instant("dataTables.Shows") +
          " _END_ " +
          this.translator.instant("dataTables.of") +
          " _TOTAL_ " +
          this.translator.instant("dataTables.rows"),
        infoEmpty:
          this.translator.instant("dataTables.Shows") +
          " _END_ " +
          this.translator.instant("dataTables.of") +
          " _TOTAL_ " +
          this.translator.instant("dataTables.rows"),
        infoFiltered: this.translator.instant("datatables.InfoFiltered", {
          count: "_MAX_"
        }),
        paginate: {
          previous: this.translator.instant("dataTables.Last"),
          next: this.translator.instant("dataTables.Next"),
          first: "",
          last: ""
        },
        processing: `<i class="fas fa-spinner fa-spin fa-3x"></i><span class="sr-only"></span>`,
        emptyTable: this.translator.instant("dataTables.NoDataFound")
      },
      lengthMenu: [
        [25, 50, 200, 500, -1],
        [25, 50, 200, 500, this.translator.instant("shared.All")]
      ],
      retrieve: true
    };
  }

  /**
   * Returns config for the Excel/csv button. See config iptions here: https://datatables.net/reference/button/csv
   * @param columnsToExport can be a string like ":visible" or ":hidden" or an array of columns number like [0,2,4]
   * @param nameOfExportFile Name of the exported file
   * @returns
   */
  public getExcelButtonConfig(
    columnsToExport: Array<number> | string,
    nameOfExportFile: string,
    excelButtonTranslateKey: string,
    excelFormatter?: (cellData: any, cellIndex: number, colIndex: number, cell: HTMLTableCellElement) => any
  ) {
    return <DataTables.ButtonSettings>{
      extend: "csv",
      className: "p-button-success p-button-raised bi-excel-button p-button p-component",
      text: `<span class="p-button-icon p-button-icon-left fas fa-file-excel" aria-hidden="true"></span>
      <span class="p-button-label">${this.translator.instant(excelButtonTranslateKey)}</span>`,
      filename: nameOfExportFile,
      exportOptions: {
        columns: columnsToExport,
        // Make sure that null values are made empty strings in the export
        format: {
          // eslint-disable-next-line @typescript-eslint/no-unused-vars
          body: excelFormatter ? excelFormatter : (cellData: any, cellIndex, colIndex, theCell: HTMLTableCellElement) => {
            if (cellData && typeof cellData === "string")
              return cellData.replace(String.fromCharCode(13), "").replace(/\n/g, "");
            else return cellData;
          }
        }
      },
      fieldSeparator: ";",
      fieldBoundary: "",
      bom: true
    };
  }

  /**
   * Function to be used for the "render" callback for datatables columns for dates. The cellData MUST be a Date object
   * (or valid date string that was returned from C#) or a Moment object.
   * It ensures that the date will be correctly formated for display, filter, sorting and export data requests (orthogonal data)
   * See doc. of the render callback function here: https://datatables.net/reference/option/columns.render
   * @returns The value to be rendered in datatable
   */
  public dateColumnRenderFunction(date?: string | moment.Moment, showSeconds = false) {
    if (moment.isMoment(date)) return date.isValid() ? this.dateConverter.localizeDateTime(date, showSeconds) : "";

    return date ? this.dateConverter.localizeDateTime(date, showSeconds) : "";
  }

  /**
   * Helper function used to add the server side processing configuration for a  datatable
   * @param tableConfig The whole datatable configuration settings object that we will modify here
   * @param ajaxUrl The API url from which data is fetched. This will automatically be set on the DataTable Setting's "ajaxUrl" property
   * @param dataTable Reference to the datatable Api object. If table is not yet initialized, it will probably be null but that doesn't matter.
   * @param modifyRequestBodyCallback? Optional callback function making it possible to modify the body of the ajax request.
   * Could either overwrite the body object or just add extra properties. in this callback, you could also change the request url by changing ajax url setting on the table)
   * Notice: the paging details are automatically added to body object so don't provide these here.
   */
  public setupServerSideProcessing(
    tableConfig: DataTables.Settings,
    ajaxUrl: string,
    modifyRequestBodyCallback?: (bodyData: PagedResultRequest) => void
  ) {
    tableConfig.serverSide = true;
    tableConfig.processing = true;
    tableConfig.ajaxUrl = ajaxUrl; // then we can retrieve the url from the table's Settings object when it's initialized
    tableConfig.ajax = (body: any, callback, settings) => {

      if (!isStringNullOrEmpty(body.search.value)) body.search.value = body.search.value.trim();
      // To see what properties DataTables automatically adds to the HTTP body data, refer to this: https://datatables.net/manual/server-side
      // Here, we just do casting to get intellisense for the things we wanna add
      (body as PagedResultRequest).page = body.start / body.length;
      (body as PagedResultRequest).pageSize = body.length;
      if (body.order) {
        (body as PagedResultRequest).ordering = [];
        // For each of the specified columns for ordering, add ordering spec object to body
        (body.order as Array<{ column: number, dir: string }>).forEach(colAndDir => {
          // get the name of property defined by the "data" setting
          (body as PagedResultRequest).ordering.push(new PageResultOrdering(body.columns[colAndDir.column].data, <"asc" | "desc">colAndDir.dir));
        });
      }

      let dataTable: DataTables.Api;
      // Through the passed settings object, we can access the dataTable. First we check if it's initialized
      if ($.fn.dataTable.isDataTable(settings.nTable as any)) dataTable = $(settings.nTable).dataTable().api();

      if (modifyRequestBodyCallback) modifyRequestBodyCallback(body);
      else {
        // add the search and filter values (if null, Object.assign doesn't add them)
        if (dataTable.columnSearchValues) Object.assign(body, dataTable.columnSearchValues);

        if (dataTable.columnFilterValues) Object.assign(body, dataTable.columnFilterValues);
      }

      const url = dataTable ? dataTable.settings().init().ajaxUrl : ajaxUrl; // read url from the datatable's settings object (if initialized)

      this.httpClient
        .post<DataTableResultModel<Record<string, unknown>>>(url, body)
        .pipe(take(1))
        .subscribe((result) => {
          callback({
            recordsTotal: result.total,
            recordsFiltered: result.total,
            data: result.data
          });
        });
    };
  }

  /**
   * Helper for automatically setting up searchable and filterable columns in a datatable. Columns that are to be made searchable must have the class "searchable",
   * while filterable columns must have class "filterable".
   * CALL THIS FUNCTION AFTER TABLE IS INITIALIZED
   * @param table The datatable
   * @param valueChangeCallback? Optional callback function fired after input/select value has changed in a column. Takes the input's/select's value along with the datatable column index
   */
  public setupSearchableAndFilterableColumns(
    table: DataTables.Api,
    valueChangeCallback?: (value: string | number, colIndex: number) => void
  ) {
    // Go through all columns having "searchable" or "filterable" class
    table.columns(".searchable, .filterable").every((colIdx) => {
      const column = table.column(colIdx);
      const columnSettings = table.settings().init().columns[colIdx];
      // Is this column a searchable column? If yes, create input. If not, create select
      if (columnSettings.className.indexOf("searchable") !== -1) {
        // Construct input with label conforming to the Web App's styleguide's input component
        const $formField = $(`<div class="FormField FormField--text"></div>`);

        const $input = $(`<input type="${columnSettings.editInputType === "number" ? "number" : "text"}" class="table-input fieldItem margin-top-half"
                placeholder="${this.translator.instant("shared.SearchOn", {
          searchOn: columnSettings.title.toLowerCase()
        })}"/>`);
        // Setup input keyup event with a delay build in
        ($input as any).timer = null; // store timer in the jQuery object for the input so we have access to that variable
        $input.on("keyup", () => {
          if (($input as any).timer) clearTimeout(($input as any).timer);

          ($input as any).timer = setTimeout(() => {
            // save the current input value in the table's current search input values object! using the column's data source as the property name/key
            if (!table.columnSearchValues) table.columnSearchValues = {};
            table.columnSearchValues[column.dataSrc() as string] = $input.val() as string | number;

            // timeout - trigger column search!
            if (valueChangeCallback) valueChangeCallback($input.val() as string | number, colIdx);
            // string or number depending on input type
            else table.ajax.reload();
          }, 1500);
        });

        $input.on("click", (e) => e.stopPropagation()); // make sure table sorting is not run when clicking input!

        // add elements together before inserting into table header cell
        $formField.append($input);

        column.header().appendChild($formField[0]); // finally, add this input to the column header. As we need the native js element, we use [0]
      } else {
        // this must be a filterable column - but make a check first!
        if (columnSettings.className.indexOf("filterable") !== -1 && columnSettings.selectValues) {
          // Construct a select element conforming to the styleguide of ths app
          const $formField = $(`<div class="FormField FormField--select"></div>`);

          // Create dropdown. Make a copy of the select values (important!)
          const selectValues = cloneObject(columnSettings.selectValues);
          selectValues.unshift([this.translator.instant("shared.All"), ""]); // As the column's "selectValues" only contains the different values, we need to add an option for selecting all values (e.g. no filter)
          const $select = createDropdownHtml(selectValues[0][1], selectValues, null, (val) => {
            if (!table.columnFilterValues) table.columnFilterValues = {};

            table.columnFilterValues[column.dataSrc() as string] = val; // save the selected value in the table's current filter values object. The column's data source used as property name
            if (valueChangeCallback) valueChangeCallback(val, colIdx);
            else table.ajax.reload(); // will make a new HTTP. When we setup the serverside processing, we modify the HTTp request to include to current filter values in body
          });
          $select.addClass("margin-top-half"); // a bit of margin from the column header

          // add elements together before inserting into table header cell
          $formField.append($select);
          column.header().appendChild($formField[0]);
        }
      }
    });
  }

  /**
   * Helper for adding a dropdown filter to a column so it can be sorted by a unique value.
   * SHOULD ONLY BE CALLED AFTER TABLE IS INITIALIZED
   * @param datatableApiInstance Reference to a DataTable Api instance
   * @param columnClasses String representing the classes of the columns for which dropdowns should be added. THe format should be like ".class1, .class2, .class3".
   */
  public addFilterDropdownToColumns(
    datatableApiInstance: DataTables.Api,
    columnClasses: string,
    optionDisplayValueMappings?: { [value: string]: string }
  ) {
    const translator = this.translator;
    datatableApiInstance.columns(columnClasses).every(function () {
      // eslint-disable-next-line @typescript-eslint/no-this-alias
      const column = this;
      const select = $(`
				<select style="transform: translateY(5px); display: block; margin: 0 auto">
					<option value="">${translator.instant("shared.All")}</option>
				</select>
			`);
      // Find all unique values of the column and add these as options
      column
        .data()
        .unique()
        .sort()
        .each((val) =>
          select.append(
            `<option value="${val}">${optionDisplayValueMappings && optionDisplayValueMappings[val] ? optionDisplayValueMappings[val] : val}</option>`
          )
        );
      // Perform search when selecting a value
      select.on("change", (e) => {
        const selectedValue = (e.target as HTMLSelectElement).value;
        column.search(selectedValue ? "^" + selectedValue + "$" : "", true, false).draw();
      });

      $(column.header()).append(select);
    });
  }

  /**
   * Adds eventhandlers to the "Click"-event of table edit- and delete buttons.
   * @param datatable Reference to an initialized datatable
   * @param tableHtmlId Id of the native table HTML element
   * @param onEditCallback Callback to be fired when Edit-button is clicked
   * @param onDeleteCallback Callback to be fired when Delete-button is clicked
   */
  public setupEditDeleteButtonClicks(
    datatable: DataTables.Api,
    tableHtmlId: string,
    onEditCallback: (rowData: any) => void,
    onDeleteCallback: (rowData: any) => void
  ) {
    $("#" + tableHtmlId + " tbody").on("click", ".table-edit-button, .table-delete-button", (e) => {
      e.stopPropagation();
      const button$ = $(e.currentTarget);
      const rowData = datatable.row(button$.parents("tr")).data();
      if (button$.hasClass("table-edit-button")) onEditCallback(rowData);
      else onDeleteCallback(rowData);
    });
  }

  public setUpToggleButtonClickHandler(datatable: DataTables.Api, tableHtmlId: string, onToggleCallBack: (row$: DataTables.RowMethods) => void) {
    $("#" + tableHtmlId + " tbody").on("click", ".table-toggle-button", (e) => {
      e.stopPropagation();
      const button$ = $(e.currentTarget);
      const row$ = datatable.row(button$.parents("tr"));

      if (button$.hasClass("table-toggle-button")) {
        onToggleCallBack(row$);
      }
    });
  }
}
