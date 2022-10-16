import { HttpErrorResponse } from "@angular/common/http";

/**
 * An Http Error Response with added type info for the "error" property: logId and errorMessage.
 * We expect this info to come from server.
 */
export interface BiHttpErrorResponse extends HttpErrorResponse {
  error: {
    logId: number;
    errorMessage: string;
  };
}
