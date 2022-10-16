import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiRoutes } from "@shared/classes/ApiRoutes";

/**
 * Angular service for creating and sending SMS messages
 */
@Injectable()
export class SupportService {
  constructor(private http: HttpClient) {
  }

  /**
   * Creates and sends the support case.
   */
  public sendSupportCase(caseModel: SupportCaseModel) {
    // WEB api expects formdata and therefore reads body as a Form object with formdata.
    const formData: FormData = new FormData();
    // add each file to formdata.
    if (caseModel.filesToUpload && caseModel.filesToUpload.length > 0)
      caseModel.filesToUpload.forEach((f) => formData.append("caseFiles", f, f.name));

    if (caseModel.name) formData.append("caseName", caseModel.name);

    formData.append("caseEmail", caseModel.email);
    formData.append("caseText", caseModel.text);
    formData.append("clientAppVersion", caseModel.clientAppVersion);
    return this.http.post(ApiRoutes.supportRoutes.createSupportCase, formData);
  }

}

export class SupportCaseModel {
  constructor(public text: string, public email: string, public clientAppVersion: string, public name?: string, public filesToUpload?: Array<File>) { }
}
