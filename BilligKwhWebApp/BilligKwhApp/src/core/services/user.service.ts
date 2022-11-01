import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Language } from "@models/common/Language";
import { TwoFactorModel } from "@models/user/TwoFactorModel";
import { UserInfo } from "@models/user/UserInfo";
import { UserRole } from "@models/user/UserRole";
import { ApiRoutes } from "@shared/classes/ApiRoutes";
import { BiStore } from "@globals/classes/BiStore";
import { BiCountryId, BiLanguageId } from "@globals/enums/BiLanguageAndCountryId";
import { UserRoleEnum } from "@shared/interfaces-and-enums/UserRoleEnum";
import { isStringNullOrEmpty } from "@shared/variables-and-functions/helper-functions";
import { LocalStorageItemNames } from "@shared/variables-and-functions/LocalStorageItemNames";
import { BehaviorSubject, forkJoin, Observable } from "rxjs";
import { catchError, map, switchMap, tap } from "rxjs/operators";
import { GlobalStateAndEventsService } from "../global-state-and-events.service";
import { AUTHENTICATION_SERVICE_TOKEN, TokenAuthenticationService } from "../security/TokenAuthenticationService";
import { cloneObject } from "@globals/helper-functions";
import { UserModel } from "@apiModels/UserModel";
import { UserEditModel } from "@apiModels/userEditModel";

interface UserServiceState {
  currentUser?: UserModel;
  userRoles: Array<UserRole>;
  countryToUsersMap: { [countryId: number]: Array<UserModel> };
  currentLanguageId: BiLanguageId;
}

@Injectable()
export class UserService extends BiStore<UserServiceState> {
  /**
   * The current user's roles
   */
  private userRoles = new BehaviorSubject<UserRole[]>([]);

  /**
   * The current roles that user has
   */
  public userRoles$ = this.userRoles.asObservable();

  private countryToUsersMap = new BehaviorSubject<{
    [countryId: number]: Array<UserModel>;
  }>({});

  constructor(
    private http: HttpClient,
    private eventsManager: GlobalStateAndEventsService,
    @Inject(AUTHENTICATION_SERVICE_TOKEN)
    private authService: TokenAuthenticationService
  ) {
    super({
      currentUser: undefined,
      userRoles: [],
      countryToUsersMap: {},
      currentLanguageId: null
    });
    eventsManager.failedAccessTokenRefresh.subscribe(() => this.state.next({ ...this.state.value, currentUser: undefined }));

    // WHen customer changes, we must update ther current customer id on the user
    eventsManager.customerChanged$.subscribe(cId => this.state.next({ ...this.state.value, currentUser: { ...this.state.value.currentUser, customerId: cId } }));
  }

  public login(email: string, password: string): Observable<UserModel> {
    return this.authService.login(email, password).pipe(
      // Get both the user and his/her roles
      switchMap(() => {
        return this.refreshUserState();
      })
    );
  }

  public logInTwoFactor(email: string, pinCode: number): Observable<UserModel> {
    return this.authService.twoFactorAuthentication(<TwoFactorModel>{ email, pinCode }).pipe(
      switchMap(() => {
        return this.refreshUserState();
      })
    );
  }

  public loginAD(token: string, smsGroupId?: number): Observable<UserModel> {
    return this.authService.loginAD(token, smsGroupId).pipe(
      // Get both the user and his/her roles
      switchMap(() => {
        return this.refreshUserState();
      })
    );
  }

  public loginCookie(): Observable<UserModel> {
    return this.authService.loginCookie().pipe(
      // Get both the user and his/her roles
      switchMap(() => {
        return this.refreshUserState();
      })
    );
  }

  public sendCodeByEmail(email: string) {
    return this.http.get(ApiRoutes.userRoutes.sendPinCodeByEmail, { params: { email: email } });
  }

  public sendCodeBySms(email: string) {
    return this.http.get(ApiRoutes.userRoutes.sendPinCodeBySms, { params: { email: email } });
  }

  public refreshUserState() {
    return forkJoin([this.getCurentUser(), this.getUserRoles()]).pipe(
      map(([user, roles]) => {
        this.userRoles.next(roles);
        this.state.next({ ...this.state.value, currentUser: user });

        if (user.languageId !== this.state.value.currentLanguageId)
          this.state.next({
            ...this.state.value,
            currentLanguageId: user.languageId
          });

        return user;
      })
    );
  }

  public logout(): Observable<void> {
    return this.authService.logout().pipe(
      tap(() => {
        this.state.next({
          ...this.state.value,
          currentUser: undefined,
          userRoles: []
        });
      })
    );
  }

  /**
   * Returns a specific user or just the current one depending on the optional parameter.
   * @param inclDeleted Whether the server should look among deleted users as well
   */
  public getUser(id: number, inclDeleted?: boolean): Observable<UserModel> {
    const params: { [key: string]: string } = {};
    params.id = id.toString();

    return this.http.get<UserModel>(ApiRoutes.userRoutes.get.getUserById, { params: params }).pipe(
      tap(user => {
        // If id is not provided, current user is returned and must be set in local state. Also, if there's no Id set for
        // current user, do the same
        if (!this.state.value.currentUser?.id)
          this.state.next({
            ...this.state.value,
            currentUser: user,
            currentLanguageId: user.languageId
          });
      })
    );
  }

  /**
   * Returns a specific user or just the current one depending on the optional parameter.
   * @param inclDeleted Whether the server should look among deleted users as well
   */
  public getCurentUser(): Observable<UserModel> {
    return this.http.get<UserModel>(ApiRoutes.userRoutes.get.getCurentUser, {}).pipe(
      tap(user => {
        // If id is not provided, current user is returned and must be set in local state. Also, if there's no Id set for
        // current user, do the same
        if (!this.state.value.currentUser?.id)
          this.state.next({
            ...this.state.value,
            currentUser: user,
            currentLanguageId: user.languageId
          });
      })
    );
  }

  // /**
  //  * Returns all users by country
  //  */
  // public getUsersByCountry(countryId?: number, inclDeleted = false, ignoreCache = false): Observable<UserModel[]> {
  //   if (!ignoreCache || (!ignoreCache && this.countryToUsersMap.value[countryId])) return of(this.countryToUsersMap.value[countryId]);

  //   return this.http
  //     .get<UserModel[]>(ApiRoutes.userRoutes.get.getUsersByCountry, {
  //       params: {
  //         countryId: countryId.toString(),
  //         inclDeleted: inclDeleted ? "true" : "false"
  //       }
  //     })
  //     .pipe(
  //       tap(u => {
  //         // Update state
  //         const countryUsersMap = cloneObject(this.countryToUsersMap.value);
  //         countryUsersMap[countryId] = u;
  //         this.countryToUsersMap.next(countryUsersMap);
  //       })
  //     );
  // }

  public getUsersByCustomer(customerId?: number, onlyDeleted = false, userId = -100): Observable<UserModel[]> {
    return this.http.get<UserModel[]>(ApiRoutes.userRoutes.get.getUsersByCustomer, {
      params: {
        customerId: customerId.toString(),
        onlyDeleted: onlyDeleted ? "true" : "false",
        userId: userId
      }
    });
  }

  public downloadUsersWithProfileName(customerId: number) {
    return this.http.get(ApiRoutes.userRoutes.get.downloadUsersWithProfileName, {
      params: { customerId: customerId.toString() },
      responseType: "arraybuffer"
    });
  }

  /**
   * Deletes a user
   */
  public deleteUser(id: number, countryId: BiCountryId): Observable<any> {
    return this.http
      .delete(ApiRoutes.userRoutes.deleteUser, {
        params: { id: id.toString() }
      })
      .pipe(
        tap(() => {
          const countryToUsersMapClone = cloneObject(this.countryToUsersMap.value);
          const users = countryToUsersMapClone[countryId];
          if (users) {
            const idx = users.findIndex(u => u.id === id);
            if (idx > -1) {
              users.splice(idx, 1);
              this.countryToUsersMap.next(countryToUsersMapClone);
            }
          }
        })
      );
  }

  /**
   * Reactivates a user that was deleted
   */
  public reactivateUser(user: UserModel, countryId: BiCountryId): Observable<any> {
    return this.http.post(ApiRoutes.userRoutes.update.reactivateUser, {}, { params: { id: user.id.toString() } }).pipe(
      tap(() => {
        // Update  local cache
        const countryToUsersMapClone = cloneObject(this.countryToUsersMap.value);
        const users = countryToUsersMapClone[countryId];
        if (users) {
          user.deleted = false;
          users.push(user);
          this.countryToUsersMap.next(countryToUsersMapClone);
        }
      })
    );
  }

  /**
   * Gets the user info for the current user
   */
  // public getUserInfo(): Observable<UserInfo> {
  //   return this.http.get<UserInfo>(ApiRoutes.userRoutes.get.getUserInfo);
  // }

  // /**
  //  * Saves the user info for the current user (Users own Page)
  //  */
  // public setUserInfo(info: UserInfo): Observable<any> {
  //   return this.http.post(ApiRoutes.userRoutes.update.setUserInfo, info).pipe(
  //     tap(() => {
  //       // Clone local user object, update it and emit event.
  //       const userCopy: UserModel = cloneObject(this.state.value.currentUser);
  //       userCopy.name = info.name;
  //       userCopy.email = info.email;
  //       userCopy.languageId = info.languageId;
  //       this.state.next({ ...this.state.value, currentUser: userCopy });

  //       if (info.languageId != this.state.value.currentLanguageId)
  //         this.state.next({
  //           ...this.state.value,
  //           currentLanguageId: info.languageId
  //         });
  //     })
  //   );
  // }

  public updateUser(user: UserModel, sendEmail = true) {
    debugger;
    const params: { [key: string]: string } = {
      //customerId: user.customerId.toString()
    };

    params["sendEmail"] = sendEmail ? "true" : "false";

    return this.http.post<number>(ApiRoutes.userRoutes.post.updateUser, user, { params: params }).pipe(
      catchError(err => {
        throw err;
      }),
      tap(uId => {
        user.id = uId;
        const stateClone = cloneObject(this.getCurrentStateValue());
        // Update the local user object in store if it exists
        if (stateClone.countryToUsersMap[user.countryId]) {
          const indexOfLocalCust = stateClone.countryToUsersMap[user.countryId].findIndex(c => c.id === user.id);
          const userClone = stateClone.countryToUsersMap[user.countryId][indexOfLocalCust];
          stateClone.countryToUsersMap[user.countryId][indexOfLocalCust] = { ...userClone, ...user };
        }

        // Was this user the current one? Then this also needs an update
        if (stateClone.currentUser.id === user.id)
          stateClone.currentUser = {
            ...stateClone.currentUser,
            ...user
          };
        this.state.next(stateClone);
      })
    );
  }

  // /**
  //  * Creates a new user
  //  * @param profilesUserCanAccess List of profile ids of the profiles that this user can choose after login
  //  * @param sendToEmails String being a semicolon-separated list of the email addresses to which password email should be sendToEmails
  //  * @param sendEmail Whether the password email should be sent or not
  //  */
  // public createUser(user: Partial<UserModel>, sendToEmails?: string, sendEmail = true, createStdReceiver = false): Observable<CreateUserResult> {
  //   const params: { [key: string]: string } = {
  //     customerId: user.customerId.toString()
  //   };
  //   if (!isStringNullOrEmpty(sendToEmails)) params["sendToEmails"] = sendToEmails;

  //   params["sendEmail"] = sendEmail ? "true" : "false";
  //   params["createStdReceiver"] = createStdReceiver ? "true" : "false";

  //   return this.http
  //     .post<CreateUserResult>(ApiRoutes.userRoutes.updateUser, user, { params: params })
  //     .pipe(
  //       tap((result: CreateUserResult) => {
  //         user.id = result.id;
  //         const countryToUsersMapClone = cloneObject(this.countryToUsersMap.value);
  //         // If no users stored for this country, create new entry for this country
  //         if (!countryToUsersMapClone[user.countryId]) countryToUsersMapClone[user.countryId] = [user as UserModel];
  //         else countryToUsersMapClone[user.countryId].push(user as UserModel);

  //         this.countryToUsersMap.next(countryToUsersMapClone);
  //       })
  //     );
  // }

  /*
   * Update the User from Customer or SuperAdmin Page
   */
  public updateUserInformation(id: number, name: string, email: string, phoneNumber?: number, newsletterActive?: boolean, newPassword?: string): Observable<any> {
    // Create Dto
    const basicUserInfoModel: Partial<UserInfo> & { id: number } = {
      id,
      name,
      email
    };

    if (phoneNumber) basicUserInfoModel.mobileNumber = phoneNumber.toString();
    if (newPassword) basicUserInfoModel.newPassword = newPassword;

    return this.http.patch(ApiRoutes.userRoutes.update.updateUserInfo, basicUserInfoModel);
  }

  /**
   * Returns the current working language as a model with locale code and culture info.
   * NB: this is only to be called once - in the app initializer function. Afterwards, you can just use the property "currentLanguageId" (automatically set in this call)
   */
  public getWorkingLanguage() {
    return this.http.get<Language>(ApiRoutes.userRoutes.get.getWorkingLanguage).pipe(tap(l => this.state.next({ ...this.state.value, currentLanguageId: l.id })));
  }

  //#region ==== RESET PASSWORD FUNCTIONS ===============================================

  /**
   * Tells server to send a "Reset password token" to the given email. Email must be the one that's registered on user.
   */
  public requestResetPasswordToken(email: string): Observable<boolean> {
    return this.http.post<boolean>(ApiRoutes.userRoutes.requestResetPasswordToken, null, { params: { email: email } });
  }

  /**
   * Verifies match between "reset-password-token" and email.
   */
  public verifyResetPasswordToken(token: string, email: string): Observable<boolean> {
    return this.http.post<boolean>(ApiRoutes.userRoutes.verifyResetPasswordToken, null, {
      params: { token: token, email: email }
    });
  }

  /**
   * Queries server for resetting user's password
   */
  public resetPassword(token: string, email: string, newPass: string, newPassRepeat: string) {
    const resetPassModel = {
      email: email,
      token: token,
      newPassword: newPass,
      newPasswordRepeat: newPassRepeat,
      termsConditionsAccepted: true
    };
    return this.http.post<{ refreshToken: string }>(ApiRoutes.userRoutes.update.resetPassword, resetPassModel);
  }

  //#endregion ============================================================================

  /**
   * Sets a new password for the current user
   */
  public changePassword(newPassword: string, currentPassword: string): Observable<any> {
    // We must be sure to encode the passwords as they contain special characters
    const route = `${ApiRoutes.userRoutes.update.changePassword}?newPassword=${encodeURIComponent(newPassword)}&currentPassword=${encodeURIComponent(currentPassword)}`;
    return this.http.post(route, null);
  }

  /**
   * Togles the "test mode"-flag on current user. Returns a result telling whether test mode is active or not
   */
  public toggleTestMode() {
    return this.http.post<{ testMode: boolean }>(ApiRoutes.userRoutes.update.toggleTestMode, {}).pipe(
      tap(x =>
        this.state.next({
          ...this.state.value,
          currentUser: {
            ...this.state.value.currentUser
            //testMode: x.testMode
          }
        })
      )
    );
  }

  //#region ========= SUPER ADMIN ONLY =========================

  // /**
  //  * Creates a new user
  //  * @param profilesUserCanAccess List of profile ids of the profiles that this user can choose after login
  //  * @param sendToEmails String being a semicolon-separated list of the email addresses to which password email should be sendToEmails
  //  * @param sendEmail Whether the password email should be sent or not
  //  */
  // public createUser(user: Partial<UserModel>, sendToEmails?: string, sendEmail = true, createStdReceiver = false): Observable<CreateUserResult> {
  //   const params: { [key: string]: string } = {
  //     customerId: user.customerId.toString()
  //   };
  //   if (!isStringNullOrEmpty(sendToEmails)) params["sendToEmails"] = sendToEmails;

  //   params["sendEmail"] = sendEmail ? "true" : "false";
  //   params["createStdReceiver"] = createStdReceiver ? "true" : "false";

  //   return this.http
  //     .post<CreateUserResult>(ApiRoutes.userRoutes.updateUser, user, { params: params })
  //     .pipe(
  //       tap((result: CreateUserResult) => {
  //         user.id = result.id;
  //         const countryToUsersMapClone = cloneObject(this.countryToUsersMap.value);
  //         // If no users stored for this country, create new entry for this country
  //         if (!countryToUsersMapClone[user.countryId]) countryToUsersMapClone[user.countryId] = [user as UserModel];
  //         else countryToUsersMapClone[user.countryId].push(user as UserModel);

  //         this.countryToUsersMap.next(countryToUsersMapClone);
  //       })
  //     );
  // }

  /**
   * For requesting server to resend an email for creating new password
   * @param userEmail Email of the user for which a password is needed
   */
  public resendPasswordEmail(userEmail: string) {
    return this.http.get(ApiRoutes.userRoutes.resendPasswordEmail, {
      params: { userEmail: userEmail }
    });
  }

  public sendNewUserEmail(userId: number) {
    return this.http.get(ApiRoutes.userRoutes.sendNewUserEmail, {
      params: { userId: userId.toString() }
    });
  }

  /**
   * Gets the roles that a user has. If userId is not passed, we expect backend to use current user's id
   */
  public getUserRoles(userId?: number) {
    const params = userId ? { params: { userId: userId.toString() } } : undefined;

    return this.http.get<UserRole[]>(ApiRoutes.userRoutes.get.getUserRoles, params).pipe(tap(roles => this.userRoles.next(roles)));
  }

  /**
   * Returns a list of user role access models telling what roles the user has access to and not.
   * @param userId If set, ALL the roles that a user can have are returned along with flag telling if this user has the role or not. If not provided, current user's id is used.
   */
  public getUserRoleAccess(userId?: number) {
    return this.http.get<{ userRole: UserRole; hasAccess: boolean }[]>(
      ApiRoutes.userRoutes.get.getUserRoleAccess,
      userId
        ? {
            params: { userId: userId.toString() }
          }
        : undefined
    );
  }

  /**
   * Set roles for a user.
   * @param hasAccess Wether user should have this role or not
   * @param userRoles The roles that should be added or removed depending on "hasAccess"
   */
  public setUserRoleAccess(userId: number, userRoles: Array<UserRole>, hasAccess: boolean) {
    const params = {
      userId: userId.toString(),
      hasAccess: hasAccess ? "true" : "false"
    };
    return this.http
      .put(
        ApiRoutes.userRoutes.update.setUserRoleAccess,
        userRoles.map(u => u.id),
        { params: params }
      )
      .pipe(
        tap(() => {
          // If a change to a role of CURRENT user has been made, we must notifiy subscribers of the user roles
          if (userId === this.state.value.currentUser.id) {
            const newRoles: Array<UserRole> = cloneObject(this.userRoles.value);
            userRoles.forEach(role => {
              const roleIndex = this.userRoles.value.findIndex(r => r.id === role.id);
              if (roleIndex !== -1 && !hasAccess) newRoles.splice(roleIndex, 1);
              else if (roleIndex === -1 && hasAccess) newRoles.push(role);
            });
          }
        })
      );
  }

  //#endregion

  //#region ====================== HELPERS ==============================================

  /**
   * Has a user logged in?
   */
  public userHasLoggedIn(): boolean {
    return !isStringNullOrEmpty(this.authService.refreshToken());
  }

  /**
   * Helper function for clearing the current user and token data without performing logout on server
   */
  public clearUser() {
    localStorage.removeItem(LocalStorageItemNames.userRefreshToken);
    this.state.next({ ...this.state.value, currentUser: undefined });
  }

  public handleUserExists() {
    return forkJoin([this.getCurentUser(), this.getUserRoles()]).pipe(
      tap(([user, roles]) => {
        this.userRoles.next(roles);
        this.state.next({ ...this.state.value, currentUser: user });
        this.authService.updateTokenExpiration();
      })
    );
  }

  /**
   * Helper for determining whether a user has role or not. Only use this if you cannot use the BiRequireRoles directive!
   */
  public doesUserHaveRole(role: UserRoleEnum): boolean {
    return this.userRoles.value.findIndex(r => r.name === role) !== -1;
  }

  public initializeUserModel(): UserEditModel {
    // Return an initialized object
    return {
      id: 0,
      customerId: 0,
      name: "",
      email: "",
      countryId: 1,
      deleted: false,
      languageId: 1,
      administrator: false
    };
  }
  //#endregion =========================================================================
}
