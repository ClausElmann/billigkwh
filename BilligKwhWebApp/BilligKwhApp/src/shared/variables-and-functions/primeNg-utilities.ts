import { ObjectUtils } from "primeng/utils";

export class PrimeNgUtilities {

  public static exportCSV(table: any, options?: any) {
    let data: any[];
    let csv = "";

    const fieldBoundary = (options && !options.fieldBoundary) ? "" : "\"";

    const columns = table.frozenColumns ? [...table.frozenColumns, ...table.columns] : table.columns;

    if (options && options.selectionOnly) {
      data = table.selection || [];
    }
    else {
      data = table.filteredValue || table.value;

      if (table.frozenValue) {
        data = data ? [...table.frozenValue, ...data] : table.frozenValue;
      }
    }

    //headers
    for (let i = 0; i < columns.length; i++) {
      const column = columns[i];
      if (column.exportable !== false && column.field) {
        csv += fieldBoundary + (column.header || column.field) + fieldBoundary;

        if (i < (columns.length - 1)) {
          csv += table.csvSeparator;
        }
      }
    }

    //body
    data.forEach((record) => {
      csv += "\n";
      for (let i = 0; i < columns.length; i++) {
        const column = columns[i];
        if (column.exportable !== false && column.field) {
          let cellData = ObjectUtils.resolveFieldData(record, column.field);

          if (cellData != null) {
            if (table.exportFunction) {
              cellData = table.exportFunction({
                data: cellData,
                field: column.field
              });
            }
            else
              cellData = String(cellData).replace(/"/g, "\"\"");
          }
          else
            cellData = "";

          csv += fieldBoundary + cellData + fieldBoundary;

          if (i < (columns.length - 1)) {
            csv += table.csvSeparator;
          }
        }
      }
    });

    const bom = new Uint8Array(3);
    bom[0] = 239;
    bom[1] = 187;
    bom[2] = 191;

    const file = (options && options.bom) ? [bom, csv] : [csv];

    const blob = new Blob(file, {
      type: "text/csv;charset=utf-8;"
    });

    if ((window.navigator as any).msSaveOrOpenBlob) {
      (window.navigator as any).msSaveOrOpenBlob(blob, table.exportFilename + ".csv");
    }
    else {
      const link = document.createElement("a");
      link.style.display = "none";
      document.body.appendChild(link);
      if (link.download !== undefined) {
        link.setAttribute("href", URL.createObjectURL(blob));
        link.setAttribute("download", table.exportFilename + ".csv");
        link.click();
      }
      else {
        csv = "data:text/csv;charset=utf-8," + csv;
        window.open(encodeURI(csv));
      }
      document.body.removeChild(link);
    }
  }
}
