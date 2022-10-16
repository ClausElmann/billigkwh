import { InjectionToken } from "@angular/core";
import { Observable } from "rxjs";
import { TokenModel } from "@models/user/TokenModel";
import { TwoFactorModel } from "@models/user/TwoFactorModel";

/**
 * Authentication service using JWTs as authenticationstrategy
 */
export interface TokenAuthenticationService {
  /**
   * Performs a login and returns id of the new logged in user
   */
  login(email: string, password: string): Observable<TokenModel>;

  loginAD(token: string, smsGroupId?: number): Observable<TokenModel>;

  loginCookie(): Observable<TokenModel>;

  twoFactorAuthentication(twoFactorModel: TwoFactorModel);

  /**
   * Performs a logout
   */
  logout: () => Observable<any>;

  /**
   * Returns stored access token (either from LocalStorage, session or whatever)
   */
  accessTokenModel: () => TokenModel;

  /**
   * Returns the refresh token
   */
  refreshToken: () => string;

  /**
   * Refreshes tokens (access and refresh token)
   */
  refresh: () => Observable<TokenModel>;

  /**
   * Refreshes access token when impersonating
   */
  impersonateUser: (userId: number) => Observable<TokenModel>;

  /**
   * Refreshes access token when canceling impersonation
   */
  cancelImpersonation: () => Observable<TokenModel>;

  /**
   * For handling the event when server cannot return a new accesstoken (either during login or when a refresh is performed)
   */
  handleFailedAccessTokenRefresh: () => void;

  updateTokenExpiration: () => void;

  clearData: () => void;

  //profileId: () => number;

  //selectProfile: () => boolean;

  customerId: () => number;

  userId: () => number;

  impersonateFromUserId: () => number;

  isImpersonating: boolean;

  reloadRefreshTokenFromStorage: () => void;
}

// Export injectiontoken so we don't have to use the "new Inject(someToken)"" syntax.
// More info here: https://blog.thoughtram.io/angular/2016/05/23/opaque-tokens-in-angular-2.html#the-problem-with-string-tokens
export const AUTHENTICATION_SERVICE_TOKEN = new InjectionToken<TokenAuthenticationService>("TokenAuthenticationService");
