import {
  HttpErrorResponse,
  HttpHandler,
  HttpHeaderResponse,
  HttpInterceptor,
  HttpProgressEvent,
  HttpRequest,
  HttpResponse,
  HttpSentEvent,
  HttpUserEvent
} from "@angular/common/http";
import { Injectable, Injector } from "@angular/core";
import { BehaviorSubject, empty, Observable, throwError as observableThrowError } from "rxjs";
import { catchError, filter, switchMap, take } from "rxjs/operators";
import { ApiRoutes } from "../shared/classes/ApiRoutes";
import { AUTHENTICATION_SERVICE_TOKEN, TokenAuthenticationService } from "./security/TokenAuthenticationService";

/**
 * Http interceptor used to add middleware for all Http requests. When 401's occur, automatic handling of token refresh and rerequest/retry is performed. 400's are also handled.
 * When the Http error is not something that can automatically be handled, we throw the HttpErrorResponse to our global ErrorHandler (BiErrorHandlerService).
 * With inspiration from here: https://www.intertech.com/Blog/angular-4-tutorial-handling-refresh-token-with-new-httpinterceptor/
 */
@Injectable()
export class BiHttpInterceptor implements HttpInterceptor {

  /**
   * Wether the access token is currently being refreshed (for preventing multiple refresh tries)
   */
  isRefreshingToken = false;

  /**
   * Used so requests can wait until the token is set. "True" means set, "false" means not set
   */
  private isTokenSetSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  private authService: TokenAuthenticationService;

  constructor(private injector: Injector) {
    this.authService = injector.get(AUTHENTICATION_SERVICE_TOKEN); // to prevent circular dependency between AuthenticationService and this Http interceptor, we manually use the injector here
  }

  private addAuthHeader(req: HttpRequest<any>, token: string): HttpRequest<any> {
    return req.clone({ setHeaders: { Authorization: "Bearer " + token } });
  }

  /**
   * Handles incomming requests (e.i. adds the necessary middleware logic)
   */
  public intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpSentEvent | HttpHeaderResponse | HttpProgressEvent | HttpResponse<any> | HttpUserEvent<any>> {
    const token = this.authService.accessTokenModel();

    // Now handle request. If no token, don't add it
    return next.handle(token ? this.addAuthHeader(req, token.accessToken) : req).pipe(
      catchError((error: Error) => {
        if (error instanceof HttpErrorResponse) {
          if (error.status === 401) {
            // if 401, it could be a login attempt - then just throw error.
            if (req.url.includes(ApiRoutes.userRoutes.login) || req.url.includes(ApiRoutes.userRoutes.twoFactorAuthenticate)) return observableThrowError(error);

            return this.handle401Error(req, next); // could be invalid access token OR refresh token
          }
        }
        // If we reached this, just return the error
        return observableThrowError(error);
      })
    );
  }

  private handle401Error(req: HttpRequest<unknown>, next: HttpHandler) {
    // if not already refreshing access token
    if (!this.isRefreshingToken) {
      this.isRefreshingToken = true;

      // Other requests must wait until the token is set. Setting this false here does the job
      this.isTokenSetSubject.next(false);

      // If profileid is part of the url, use this (Used when linking to new windows with target=_blank)
      const url = new URL(window.location.href);
      let profileID = 0;

      //If param "ap" is present - we know to read profile param from url
      if (url.searchParams.get("ap"))
        profileID = Number(url.searchParams.get("profileid"));

      let smsGroupId: number;

      if (url.searchParams.get("smsGroupId"))
        smsGroupId = Number(url.searchParams.get("smsGroupId"));

      return this.authService.refresh().pipe(
        switchMap(() => {
          this.isTokenSetSubject.next(true);
          this.isRefreshingToken = false;
          console.clear(); // as alot of 401 could be filling the console right now
          return next.handle(this.addAuthHeader(req, this.authService.accessTokenModel().accessToken)); // Access token refreshed - proceed with org. request
        })
      );
    } else {
      // If an error happens when trying to refresh access token, it's very bad. we must let auth service handle this
      if (req.url.includes(ApiRoutes.userRoutes.get.refresh)) {
        this.isTokenSetSubject.next(false);
        this.isRefreshingToken = false;
        this.authService.handleFailedAccessTokenRefresh();
        return empty();
      }
      // wait until token has refreshed
      return this.isTokenSetSubject.pipe(
        filter((tokenSet) => tokenSet === true),
        take(1),
        switchMap(() => next.handle(this.addAuthHeader(req, this.authService.accessTokenModel().accessToken)))
      );
    }
  }
}
