import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { DokumentDto } from "@apiModels/dokumentDto";
import { ElKomponentDto } from "@apiModels/elKomponentDto";
import { ElKomponentItemDto } from "@apiModels/elKomponentItemDto";
import { EltavleConfigurationDto } from "@apiModels/eltavleConfigurationDto";
import { ElTavleDto } from "@apiModels/elTavleDto";
import { EltavleLaageConfigurationDto } from "@apiModels/eltavleLaageConfigurationDto";
import { ElTavleSektionDto } from "@apiModels/elTavleSektionDto";
import { EmailModel } from "@apiModels/emailModel";
import { LaageElKomponentDto } from "@apiModels/laageElKomponentDto";
import { SektionElKomponentDto } from "@apiModels/sektionElKomponentDto";
import { SektionKomponentDto } from "@apiModels/sektionKomponentDto";
import { BiCountryId } from "@enums/BiLanguageAndCountryId";
import { ApiRoutes } from "@shared/classes/ApiRoutes";
import { catchError, map, Observable, tap } from "rxjs";
//import { Observable } from "rxjs";

@Injectable()
export class EltavleService {
  constructor(private http: HttpClient) {}

  private _countryFilter = BiCountryId.DK;
  get countryFilter(): number {
    return this._countryFilter;
  }
  set countryFilter(value: number) {
    this._countryFilter = value;
  }

  private _vareTypeFilter = "GruppeTavler";
  get vareTypeFilter(): string {
    return this._vareTypeFilter;
  }
  set vareTypeFilter(value: string) {
    this._vareTypeFilter = value;
  }

  private _ordreStatusFilter = "Bestilte";
  get ordreStatusFilter(): string {
    return this._ordreStatusFilter;
  }
  set ordreStatusFilter(value: string) {
    this._ordreStatusFilter = value;
  }

  private _komponentStatusFilter = "Aktive";
  get komponentStatusFilter(): string {
    return this._komponentStatusFilter;
  }
  set komponentStatusFilter(value: string) {
    this._komponentStatusFilter = value;
  }

  private _komponentKategoriFilter = 0;
  get komponentKategoriFilter(): number {
    return this._komponentKategoriFilter;
  }
  set komponentKategoriFilter(value: number) {
    this._komponentKategoriFilter = value;
  }

  public getKomponentPlaceringer(tavleId?: number) {
    return this.http.get<SektionElKomponentDto[]>(ApiRoutes.eltavleRoutes.get.getKomponentPlaceringer, {
      params: {
        tavleId: tavleId.toString()
      }
    });
  }

  public getEltavleConfiguration(tavleId?: number) {
    return this.http.get<EltavleConfigurationDto>(ApiRoutes.eltavleRoutes.get.getEltavleConfiguration, {
      params: {
        tavleId: tavleId.toString()
      }
    });
  }

  public getEltavleLaageConfiguration(laageId?: number) {
    return this.http.get<EltavleLaageConfigurationDto>(ApiRoutes.eltavleRoutes.get.getEltavleLaageConfiguration, {
      params: {
        laageId: laageId.toString()
      }
    });
  }

  public getAllSektionElKomponents(tavleId?: number) {
    return this.http.get<SektionKomponentDto[]>(ApiRoutes.eltavleRoutes.get.getAllSektionElKomponents, {
      params: {
        tavleId: tavleId.toString()
      }
    });
  }

  public alleKomponenter() {
    return this.http.get<ElKomponentItemDto[]>(ApiRoutes.eltavleRoutes.get.alleKomponenter, {
      params: {}
    });
  }

  public alleElKomponenter(filter: string, komponentKategoriId: string) {
    const params: { [key: string]: string } = {
      filter,
      komponentKategoriId
    };

    return this.http.get<ElKomponentDto[]>(ApiRoutes.eltavleRoutes.get.alleElKomponenter, {
      params: params
    });
  }

  public alleSektioner(tavleId: number) {
    return this.http.get<ElTavleSektionDto[]>(ApiRoutes.eltavleRoutes.get.alleSektioner, {
      params: { tavleId: tavleId }
    });
  }

  public getAllElTavleDto(filter: string, varetype: string, countryId?: BiCountryId, customerId?: number): Observable<ElTavleDto[]> {
    const params: { [key: string]: string } = {
      filter,
      varetype
    };

    if (countryId) params["countryId"] = countryId.toString();

    if (customerId) params["customerId"] = customerId.toString();

    return this.http
      .get<ElTavleDto[]>(ApiRoutes.eltavleRoutes.get.getEltavler, {
        params: params
      })
      .pipe(
        map(customers => {
          return customers;
        })
      );
  }

  public getEltavle(id: number): Observable<ElTavleDto> {
    const params: { [key: string]: string } = {};
    params.id = id.toString();

    return this.http.get<ElTavleDto>(ApiRoutes.eltavleRoutes.get.getEltavle, {
      params: params
    });
  }

  public updateEltavle(eltavle: ElTavleDto) {
    return this.http.post<number>(ApiRoutes.eltavleRoutes.update.updateEltavle, eltavle).pipe(
      catchError(err => {
        throw err;
      })
    );
  }

  public updateElKomponent(komponent: ElKomponentDto) {
    return this.http.post<ElKomponentDto>(ApiRoutes.eltavleRoutes.update.updateElKomponent, komponent).pipe(
      catchError(err => {
        throw err;
      })
    );
  }

  public opretTavle(eltavle: ElTavleDto) {
    return this.http.post<number>(ApiRoutes.eltavleRoutes.post.opretTavle, eltavle).pipe(
      catchError(err => {
        throw err;
      })
    );
  }

  public bestilTavle(tavleId: number) {
    return this.http.post<number>(ApiRoutes.eltavleRoutes.post.bestilTavle, undefined, { params: { tavleId: tavleId } }).pipe(
      catchError(err => {
        throw err;
      })
    );
  }

  public flytKunde(tavleId: number, kundeId: number, brugerId: number) {
    return this.http.post(ApiRoutes.eltavleRoutes.update.flytKunde, undefined, { params: { tavleId: tavleId, kundeId: kundeId, brugerId: brugerId } }).pipe(
      catchError(err => {
        throw err;
      })
    );
  }

  public importKomponenter(tavleId: number, indhold: string) {
    return this.http.post(ApiRoutes.eltavleRoutes.update.importKomponenter, undefined, { params: { tavleId: tavleId, indhold: indhold } }).pipe(
      catchError(err => {
        throw err;
      })
    );
  }

  public updateSektionKomponent(dto: SektionKomponentDto) {
    return this.http.post<number>(ApiRoutes.eltavleRoutes.update.updateSektionKomponent, dto).pipe(
      catchError(err => {
        throw err;
      })
    );
  }

  public updateKomponentPlaceringNavn(dto: SektionElKomponentDto) {
    return this.http.post<number>(ApiRoutes.eltavleRoutes.update.updateKomponentPlaceringNavn, dto).pipe(
      catchError(err => {
        throw err;
      })
    );
  }

  public deleteSektionKomponent(dto: SektionKomponentDto) {
    return this.http.delete(ApiRoutes.eltavleRoutes.delete.deleteSektionKomponent, { params: { id: dto.id.toString() } });
  }

  public gemKomponentPlaceringer(tavleId: number, komponentPlaceringer: Array<SektionElKomponentDto>): Observable<any> {
    return this.http
      .post(ApiRoutes.eltavleRoutes.update.gemKomponentPlaceringer, komponentPlaceringer, {
        params: { tavleID: tavleId.toString() }
      })
      .pipe(
        tap(() => {
          // ...
        })
      );
  }

  public gemLaageKomponentPlaceringer(laageId: number, komponentPlaceringer: Array<LaageElKomponentDto>): Observable<any> {
    return this.http
      .post(ApiRoutes.eltavleRoutes.update.gemLaageKomponentPlaceringer, komponentPlaceringer, {
        params: { laageId: laageId.toString() }
      })
      .pipe(
        tap(() => {
          // ...
        })
      );
  }

  public updateFrame(tavleId: number, moduler: number): Observable<any> {
    return this.http
      .post(ApiRoutes.eltavleRoutes.update.updateFrame, null, {
        params: { tavleId: tavleId, moduler: moduler }
      })
      .pipe(
        tap(() => {
          // ...
        })
      );
  }

  public getEltavleDokumenter(tavleId: number) {
    return this.http.get<DokumentDto[]>(ApiRoutes.eltavleRoutes.get.getEltavleDokumenter, {
      params: {
        tavleID: tavleId.toString()
      }
    });
  }

  public getKomponentTimeLoen() {
    return this.http.get<number>(ApiRoutes.eltavleRoutes.get.getKomponentTimeLoen, {});
  }

  public sendTavleMail(tavleId: number, emailTemplateName: string): Observable<boolean> {
    return this.http.post<boolean>(ApiRoutes.eltavleRoutes.sendTavleMail, null, { params: { tavleId: tavleId, emailTemplateName: emailTemplateName } });
  }

  public sendFakturaMail(tavleId: number): Observable<boolean> {
    return this.http.post<boolean>(ApiRoutes.eltavleRoutes.sendFakturaMail, null, { params: { tavleId: tavleId } });
  }

  public getVarmeberegning(tavleId: number, emailTemplateName: string) {
    return this.http.get<EmailModel>(ApiRoutes.eltavleRoutes.get.getVarmeberegning, { params: { tavleId: tavleId, emailTemplateName: emailTemplateName } });
  }

  public genberegnKabinetter(tavleId: number): Observable<boolean> {
    return this.http.post<boolean>(ApiRoutes.eltavleRoutes.genberegnKabinetter, null, { params: { tavleId: tavleId } });
  }

  public createOrUpdateOrder(tavleId: number) {
    return this.http.post<number>(ApiRoutes.eltavleRoutes.post.createOrUpdateOrder, undefined, { params: { tavleId: tavleId } }).pipe(
      tap(orderNumber => {
        /// do nothing
      })
    );
  }

  public createInvoiceDraft(tavleId: number) {
    return this.http.post<number>(ApiRoutes.eltavleRoutes.post.createInvoiceDraft, undefined, { params: { tavleId: tavleId } }).pipe(
      tap(invoiceDraftNumber => {
        /// do nothing
      })
    );
  }

  public bookInvoice(tavleId: number) {
    return this.http.post<number>(ApiRoutes.eltavleRoutes.post.bookInvoice, undefined, { params: { tavleId: tavleId } }).pipe(
      tap(invoiceDraftNumber => {
        /// do nothing
      })
    );
  }

  public getSentOrderPdf(orderNumber: number) {
    return this.http.get(ApiRoutes.eltavleRoutes.get.getSentOrderPdf, {
      params: { orderNumber },
      responseType: "blob"
    });
  }

  public getDraftInvoicePdf(tavleId: number) {
    return this.http.get(ApiRoutes.eltavleRoutes.get.getDraftInvoicePdf, {
      params: { tavleId },
      responseType: "blob"
    });
  }

  public getBookedInvoicePdf(tavleId: number) {
    return this.http.get(ApiRoutes.eltavleRoutes.get.getBookedInvoicePdf, {
      params: { tavleId },
      responseType: "blob"
    });
  }

  public gemKomponentTimeLoen(komponentTimeLoen: number) {
    return this.http.post<number>(ApiRoutes.eltavleRoutes.post.gemKomponentTimeLoen, undefined, { params: { komponentTimeLoen: komponentTimeLoen } }).pipe(
      tap(invoiceDraftNumber => {
        /// do nothing
      })
    );
  }
}
