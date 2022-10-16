// Extended Type definitions for JQuery DataTables  1.10

//<reference types="jquery" />
/// <reference types="datatables.net"/>

declare namespace DataTables {
  interface ColumnSettings {
    /**
     * The type of input to be used when cell is is transformed to edit mode. Can be either "text", "number" or "select". Default is "text".
     * Note: If you set "select", you must also set the "dropdownValues"-property! Also, <td> tags in this column MUST get an HTML5-attribute called "value" with the value set to the cell's data
     */
    editInputType?: string;

    /**
     * Array of key/value pairs used for the <option> tags in the <select> element inserted if editInputType is "select" and cell is in edit mode. The "key" will be used for the <option> text,
     * while the value will be used for... you guessed right!
     */
    selectValues?: Array<[string, string | number]>;

    /**
     * Wether the cell requires a value. Useful when cell is being edited and validation should be performed.
     */
    required?: boolean;
  }

  interface Api {
    /**
     * Current values of the inputs for searchable columns. Keys are the names of the columns's data source (property name) and values are the value of the search input in the column
     */
    columnSearchValues: { [key: string]: string | number };

    /**
     * Current values of the dropdowns for filterable columns. Keys are the names of the columns's data source (property name) and values are the value of the filter dropdown/select in the column
     */
    columnFilterValues: { [key: string]: string | number };
  }

  interface Settings {
    /**
     * Url used for data requests when server side processing is active. This property is usefull if a function is passed to the
     * DataTable's "ajax" setting. It makes it possible to change the ajax url dynamically.
     */
    ajaxUrl?: string;
  }

  interface RowMethods {
    /**
     * See the row in datable by display the right pagination page
     */
    show(): RowMethods;
  }

  interface StaticFunctions {
    moment(format: string): void;
  }
}
