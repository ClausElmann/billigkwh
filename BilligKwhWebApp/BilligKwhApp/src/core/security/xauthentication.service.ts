// import { HttpClient, HttpHeaders } from "@angular/common/http";
// import { Injectable } from "@angular/core";
// import { Params } from "@angular/router";
// import { ApiRoutes } from "../../shared/classes/ApiRoutes";
// import moment from "moment-timezone";
// import { EMPTY, Observable } from "rxjs";
// import { map, tap } from "rxjs/operators";
// import { GlobalStateAndEventsService } from "../global-state-and-events.service";
// import { LocalStorageItemNames } from "../../shared/variables-and-functions/LocalStorageItemNames";
// import { WindowSessionStorageNames } from "@shared/variables-and-functions/WindowSessionStorageNames";
// import { refreshTokenExpireWarnig } from "../general-variables";
// import { TokenModel } from "@models/user/TokenModel";
// import { UserRefreshToken } from "@models/user/UserRefreshToken";
// import { TwoFactorModel } from "@models/user/TwoFactorModel";

// @Injectable({ providedIn: "root" })
// export class xAuthenticationService {
//   private _accessTokenModel: TokenModel = {} as any;
//   private _userRefreshToken: UserRefreshToken;
//   private _userId: number;
//   private _customerId: number;
//   private _impersonateFromUserId: number;
//   private _tokenTimer: NodeJS.Timeout;

//   constructor(private http: HttpClient, private eventsManager: GlobalStateAndEventsService) {
//     this._userRefreshToken = JSON.parse(localStorage.getItem(LocalStorageItemNames.userRefreshToken));
//     this._accessTokenModel = JSON.parse(window.sessionStorage.getItem(WindowSessionStorageNames.accessTokenModel));
//     this._customerId = +window.sessionStorage.getItem(WindowSessionStorageNames.customerId);
//     this._userId = +window.sessionStorage.getItem(WindowSessionStorageNames.userId);
//     this._impersonateFromUserId = +window.sessionStorage.getItem(WindowSessionStorageNames.impersonateFromUserId);
//   }

//   public login(email: string, password: string): Observable<TokenModel> {
//     const emailPass: Params = {
//       email,
//       password
//     };

//     return this.http.post<TokenModel>(ApiRoutes.userRoutes.login, emailPass).pipe(tap(tokenModel => {
//       this.saveTokenModel(tokenModel);
//     }));
//   }

//   public logout() {
//     console.log("ApiRoutes.userRoutes.logout")

//     return this.http.
//       post(ApiRoutes.userRoutes.logout, {}).
//       pipe(
//         tap(() => {
//           console.log("ApiRoutes.userRoutes.logout was called")
//           this.clearData();
//           this.eventsManager.loggedOut.next();
//         })
//       );
//   }

//   public accessTokenModel(): TokenModel {
//     return this._accessTokenModel;
//   }

//   public refreshToken(): string {
//     if (this._userRefreshToken && moment(this._userRefreshToken.dateExpiresUtc) > moment.utc()) {
//       return this._userRefreshToken.token;
//     }
//     else {
//       return null;
//     }
//   }

//   public userId(): number {
//     return this._userId;
//   }

//   public customerId(): number {
//     return this._customerId;
//   }

//   public get isImpersonating() {
//     return this._impersonateFromUserId !== this._userId;
//   }

//   public impersonateFromUserId(): number {
//     return this._impersonateFromUserId;
//   }

//   public get loggedIn() {
//     return this.accessTokenModel() !== null;
//   }

//   public refresh(): Observable<TokenModel> {
//     const token = this.refreshToken();

//     if (token) {
//       let headers = new HttpHeaders();
//       headers = headers.set("Content-Type", "application/json"); // IMPORTANT! Angular will set "plain text" as Content Type, but it MUST be json!
//       return this.http
//         .post<TokenModel>(ApiRoutes.userRoutes.get.refresh, { refreshToken: token, userId: this.userId() }, { headers: headers })
//         .pipe(
//           map(tokenModel => {
//             this.saveTokenModel(tokenModel);
//             return tokenModel;
//           })
//         );
//     }
//     return EMPTY;
//   }

//   public twoFactorAuthentication(twoFactorModel: TwoFactorModel) {
//     return this.http
//       .post<TokenModel>(ApiRoutes.userRoutes.twoFactorAuthenticate, twoFactorModel)
//       .pipe(
//         tap((tokenModel) => {
//           this.saveTokenModel(tokenModel);
//           return tokenModel;
//         })
//       );
//   }

//   private saveTokenModel(tokenModel: TokenModel) {
//     const customerId = +window.sessionStorage.getItem(WindowSessionStorageNames.customerId);

//     window.sessionStorage.setItem(WindowSessionStorageNames.accessTokenModel, JSON.stringify(tokenModel));
//     localStorage.setItem(LocalStorageItemNames.userRefreshToken, JSON.stringify(tokenModel.refreshTokenModel));
//     this._userRefreshToken = tokenModel.refreshTokenModel;
//     this._accessTokenModel = tokenModel;
//     this.updateTokenExpiration();

//     // Customer changed?
//     if (!customerId || customerId !== tokenModel.customerId) {
//       this._customerId = tokenModel.customerId;
//       window.sessionStorage.setItem(WindowSessionStorageNames.customerId, tokenModel.customerId.toString());
//       this.eventsManager.fireCustomerChanged(tokenModel.customerId);
//     }

//     const userId = window.sessionStorage.getItem(WindowSessionStorageNames.userId);

//     if (!userId || userId !== tokenModel.userId.toString()) {
//       this._userId = tokenModel.userId;
//       window.sessionStorage.setItem(WindowSessionStorageNames.userId, tokenModel.userId.toString());
//       this.eventsManager.fireUserChanged(tokenModel.userId);
//     }

//     const impersonateFromUserId = window.sessionStorage.getItem(WindowSessionStorageNames.impersonateFromUserId);

//     if (!impersonateFromUserId || impersonateFromUserId !== tokenModel.impersonateFromUserId.toString()) {
//       this._impersonateFromUserId = tokenModel.impersonateFromUserId;
//       window.sessionStorage.setItem(WindowSessionStorageNames.impersonateFromUserId, tokenModel.impersonateFromUserId.toString());
//       this.eventsManager.fireimpersonateFromUserChanged(tokenModel.impersonateFromUserId);
//     }

//     this.eventsManager.refreshTokenExpireDate.next(moment(tokenModel.refreshTokenModel.dateExpiresUtc).toDate())
//   }

//   public handleFailedAccessTokenRefresh(): void {
//     this.clearData();
//     this.eventsManager.failedAccessTokenRefresh.next();
//   }

//   public updateTokenExpiration() {
//     if (!this._userRefreshToken) return;

//     this.eventsManager.refreshTokenExpireDate.next(moment(this._userRefreshToken.dateExpiresUtc).toDate())

//     const durationInSeconds = Math.round(moment.duration(moment(this._userRefreshToken.dateExpiresUtc).diff(moment())).asSeconds());

//     const udlob = (durationInSeconds - refreshTokenExpireWarnig) * 1000;

//     if (this._tokenTimer)
//       clearTimeout(this._tokenTimer);

//     this._tokenTimer = setTimeout(() => {
//       const duration = Math.round(moment.duration(moment(this._userRefreshToken.dateExpiresUtc).diff(moment())).asSeconds());
//       this.eventsManager.refreshTokenNearlyExpired.next(duration)
//     }, udlob)
//   }

//   public reloadRefreshTokenFromStorage() {
//     if (localStorage.getItem(LocalStorageItemNames.userRefreshToken)) {
//       this._userRefreshToken = JSON.parse(localStorage.getItem(LocalStorageItemNames.userRefreshToken));
//       this.updateTokenExpiration();
//     }
//   }

//   /**
//    * Clears both stored tokens in this service as well as all in localstorage
//    */
//   public clearData() {
//     this._accessTokenModel = null;
//     this._userRefreshToken = null;
//     this._customerId = null;
//     this._userId = null;
//     this._impersonateFromUserId = null;
//     window.sessionStorage.removeItem(WindowSessionStorageNames.accessTokenModel);
//     localStorage.removeItem(LocalStorageItemNames.userRefreshToken);
//     window.sessionStorage.removeItem(WindowSessionStorageNames.customerId);
//     window.sessionStorage.removeItem(WindowSessionStorageNames.userId);
//     window.sessionStorage.removeItem(WindowSessionStorageNames.impersonateFromUserId);
//   }
// }
