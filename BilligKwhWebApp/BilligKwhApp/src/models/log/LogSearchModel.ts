import { PagedResultRequest } from "../common/PagedResultRequest";

/**
 * Model class used when we query logs from backend. This can be used with datatables
 */
export class LogSearchModel extends PagedResultRequest {
  public createdOnFrom?: string;

  public createdOnTo?: string;

  public message: string;

  public logLevelId?: number;

  public availableLogLevels: Array<any>;

}
