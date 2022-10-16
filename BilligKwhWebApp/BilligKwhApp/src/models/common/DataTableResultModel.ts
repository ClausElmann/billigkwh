/**
 * Interface defining model for datatable data returned from server.
 */
export interface DataTableResultModel<T> {
  extraData?: Record<string, unknown>;

  /**
   * The returned data for the datatable
   */
  data: Array<T>;

  errors: Record<string, unknown>;

  /**
   * The total number of resources. NOT number of returned items but count of ALL resources from where the result was fetched from
   */
  total: number;
}
