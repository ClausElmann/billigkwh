//============================= All WEB API routes ==================================\\
//=== Route definitions are grouped in objects named after .NET Controller classes ==\\
export class ApiRoutes {
  //#region Variables for the different API endpoint areas
  private static api = "api/";

  private static userEndpoint = ApiRoutes.api + "User/";

  private static logEndpoint = ApiRoutes.api + "Log/";

  private static customerEndpoint = ApiRoutes.api + "Customer/";

  private static commonEndpoint = ApiRoutes.api + "Common/";

  private static supportEndpoint = ApiRoutes.api + "Support/";

  private static eltavleEndpoint = ApiRoutes.api + "Eltavle/";

  private static deviceEndpoint = ApiRoutes.api + "SmartDevice/";

  /**
   * Api endpoints for user
   */
  public static userRoutes = {
    login: ApiRoutes.userEndpoint + "Login",
    loginAD: ApiRoutes.userEndpoint + "LoginAD",
    loginCookie: ApiRoutes.userEndpoint + "LoginCookie",
    logout: ApiRoutes.userEndpoint + "Logout",
    sendPinCodeBySms: ApiRoutes.userEndpoint + "SendPinCodeBySms",
    sendPinCodeByEmail: ApiRoutes.userEndpoint + "SendPinCodeByEmail",
    twoFactorAuthenticate: ApiRoutes.userEndpoint + "AuthenticateTwoFactor",

    get: {
      refresh: ApiRoutes.userEndpoint + "Refresh",
      getCurentUser: ApiRoutes.userEndpoint + "GetCurentUser",
      getUserById: ApiRoutes.userEndpoint + "GetUserById",
      //getUsersByCountry: ApiRoutes.userEndpoint + "GetUsersByCountry",
      getUsersByCustomer: ApiRoutes.userEndpoint + "GetUsersByCustomer",
      //getUserInfo: ApiRoutes.userEndpoint + "GetUserInformation",
      getCustomer: ApiRoutes.customerEndpoint + "GetCustomer",
      getUserRoles: ApiRoutes.userEndpoint + "GetUserRoles",
      getUserRoleAccess: ApiRoutes.userEndpoint + "GetUserRoleAccess",
      getWorkingLanguage: ApiRoutes.userEndpoint + "GetWorkingLanguage",
      downloadUsersWithProfileName: ApiRoutes.userEndpoint + "DownloadUsersWithProfileName",
      impersonateUser: ApiRoutes.userEndpoint + "ImpersonateUser",
      cancelImpersonation: ApiRoutes.userEndpoint + "CancelImpersonation"
    },
    post: {
      updateUser: ApiRoutes.userEndpoint + "UpdateUser"
    },
    update: {
      //setUserInfo: ApiRoutes.userEndpoint + "SetUserInformation",
      updateUserInfo: ApiRoutes.userEndpoint + "UpdateUserInformation",
      setUserRoleAccess: ApiRoutes.userEndpoint + "SetUserRoleAccess",
      changePassword: ApiRoutes.userEndpoint + "ChangePassword",
      toggleTestMode: ApiRoutes.userEndpoint + "ToggleTestMode",
      resetPassword: ApiRoutes.userEndpoint + "ResetPassword",
      reactivateUser: ApiRoutes.userEndpoint + "ReactivateUser"
    },
    deleteUser: ApiRoutes.userEndpoint + "DeleteUser",
    requestResetPasswordToken: ApiRoutes.userEndpoint + "RequestResetPasswordToken",
    verifyResetPasswordToken: ApiRoutes.userEndpoint + "VerifyResetPasswordToken",
    resendPasswordEmail: ApiRoutes.userEndpoint + "ResendPasswordEmail",
    sendNewUserEmail: ApiRoutes.userEndpoint + "SendNewUserEmail"
  };

  /**
   * Api endpoints for customer backend logic
   */
  public static customerRoutes = {
    get: {
      getCustomer: ApiRoutes.customerEndpoint + "GetCustomer",
      //getCustomerForEdit: ApiRoutes.customerEndpoint + "GetCustomerForEdit",
      getCustomers: ApiRoutes.customerEndpoint + "GetCustomers",
      getCustomerUsers: ApiRoutes.customerEndpoint + "GetCustomerUsers",
      getCustomerLogs: ApiRoutes.customerEndpoint + "GetCustomerLogs",
      getCustomerAccount: ApiRoutes.customerEndpoint + "GetCustomerAccount",
      getCustomerUserRoleAccess: ApiRoutes.customerEndpoint + "GetCustomerUserRoleAccess",
      getAllProfileRoles: ApiRoutes.customerEndpoint + "GetAllProfileRoles",
      getPublicProfileRoles: ApiRoutes.customerEndpoint + "GetPublicProfileRoles",
      getSubscriptions: ApiRoutes.customerEndpoint + "GetSubscriptions",
      getCustomerMapModuleSettings: ApiRoutes.customerEndpoint + "GetCustomerMapModuleSettings",
      updateCustomerMapModuleSettings: ApiRoutes.customerEndpoint + "UpdateCustomerMapModuleSettings",
      customerHasAnyProfileWithRoles: ApiRoutes.customerEndpoint + "CustomerHasAnyProfileWithRoles"
    },
    post: {
      createCustomerLog: ApiRoutes.customerEndpoint + "CreateCustomerLog",
      createCustomerAccount: ApiRoutes.customerEndpoint + "CreateCustomerAccount",
      createCustomerAccountLog: ApiRoutes.customerEndpoint + "CreateCustomerAccountLog",
      updateCustomer: ApiRoutes.customerEndpoint + "UpdateCustomer",
      sendUnsentEmails: ApiRoutes.customerEndpoint + "SendUnsentEmails"
    },
    put: {
      updateCustomerLog: ApiRoutes.customerEndpoint + "UpdateCustomerLog",
      updateCustomerAccount: ApiRoutes.customerEndpoint + "UpdateCustomerAccount"
    },
    patch: {
      updateCustomerMapModuleSettings: ApiRoutes.customerEndpoint + "UpdateCustomerMapModuleSettings",
      updateCustomerDriftsStatusModuleSettings: ApiRoutes.customerEndpoint + "updateCustomerDriftsStatusModuleSettings",
      updateCustomerSubscriptionModuleSettings: ApiRoutes.customerEndpoint + "UpdateCustomerSubscriptionModuleSettings"
    },
    update: {
      importData: ApiRoutes.customerEndpoint + "ImportData"
    }
  };

  public static eltavleRoutes = {
    get: {
      getKomponentPlaceringer: ApiRoutes.eltavleEndpoint + "GetKomponentPlaceringer",
      getEltavler: ApiRoutes.eltavleEndpoint + "GetEltavler",
      getEltavle: ApiRoutes.eltavleEndpoint + "GetEltavle",
      getEltavleConfiguration: ApiRoutes.eltavleEndpoint + "GetEltavleConfiguration",
      getAllSektionElKomponents: ApiRoutes.eltavleEndpoint + "GetAllSektionElKomponents",
      getEltavleDokumenter: ApiRoutes.eltavleEndpoint + "GetElTavleDokumenter",
      alleKomponenter: ApiRoutes.eltavleEndpoint + "AlleKomponenter",
      alleSektioner: ApiRoutes.eltavleEndpoint + "AlleSektioner",
      getSentOrderPdf: ApiRoutes.eltavleEndpoint + "GetSentOrderPdf",
      getDraftInvoicePdf: ApiRoutes.eltavleEndpoint + "GetDraftInvoicePdf",
      getBookedInvoicePdf: ApiRoutes.eltavleEndpoint + "GetBookedInvoicePdf",
      alleElKomponenter: ApiRoutes.eltavleEndpoint + "AlleElKomponenter",
      getKomponentTimeLoen: ApiRoutes.eltavleEndpoint + "GetKomponentTimeLoen",
      getEltavleTable: ApiRoutes.eltavleEndpoint + "GetEltavleTable",
      getVarmeberegning: ApiRoutes.eltavleEndpoint + "GetVarmeberegning",

      getEltavleLaageConfiguration: ApiRoutes.eltavleEndpoint + "GetEltavleLaageConfiguration"
    },
    post: {
      createOrUpdateOrder: ApiRoutes.eltavleEndpoint + "CreateOrUpdateOrder",
      createInvoiceDraft: ApiRoutes.eltavleEndpoint + "CreateInvoiceDraft",
      bookInvoice: ApiRoutes.eltavleEndpoint + "BookInvoice",
      opretTavle: ApiRoutes.eltavleEndpoint + "opretTavle",
      gemKomponentTimeLoen: ApiRoutes.eltavleEndpoint + "GemKomponentTimeLoen",
      bestilTavle: ApiRoutes.eltavleEndpoint + "BestilTavle"
    },
    update: {
      gemKomponentPlaceringer: ApiRoutes.eltavleEndpoint + "GemKomponentPlaceringer",
      updateEltavle: ApiRoutes.eltavleEndpoint + "UpdateEltavle",
      updateSektionKomponent: ApiRoutes.eltavleEndpoint + "UpdateSektionKomponent",
      flytKunde: ApiRoutes.eltavleEndpoint + "FlytKunde",
      updateElKomponent: ApiRoutes.eltavleEndpoint + "UpdateElKomponent",
      updateKomponentPlaceringNavn: ApiRoutes.eltavleEndpoint + "UpdateKomponentPlaceringNavn",
      updateFrame: ApiRoutes.eltavleEndpoint + "UpdateFrame",
      gemLaageKomponentPlaceringer: ApiRoutes.eltavleEndpoint + "GemLaageKomponentPlaceringer",
      importKomponenter: ApiRoutes.eltavleEndpoint + "ImportKomponenter"
    },
    delete: {
      deleteSektionKomponent: ApiRoutes.eltavleEndpoint + "DeleteSektionKomponent"
    },
    sendTavleMail: ApiRoutes.eltavleEndpoint + "SendTavleMail",
    //sendVarmeberegningMail: ApiRoutes.eltavleEndpoint + "sendVarmeberegningMail",
    genberegnKabinetter: ApiRoutes.eltavleEndpoint + "GenberegnKabinetter",
    sendFakturaMail: ApiRoutes.eltavleEndpoint + "SendFakturaMail"
  };

  public static smartDeviceRoutes = {
    get: {
      getSmartDevices: ApiRoutes.deviceEndpoint + "GetSmartDevices",
      getSmartDevice: ApiRoutes.deviceEndpoint + "GetSmartDevice",
      getSchedulesForToday: ApiRoutes.deviceEndpoint + "GetSchedulesForToday",
      getConsumptionsPeriod: ApiRoutes.deviceEndpoint + "GetConsumptionsPeriod",
      getTemperatureReadingsPeriod: ApiRoutes.deviceEndpoint + "GetTemperatureReadingsPeriod",
      getRecipes: ApiRoutes.deviceEndpoint + "GetRecipes"
    },
    post: {},
    update: {
      updateSmartDevice: ApiRoutes.deviceEndpoint + "UpdateSmartDevice",
      updateRecipe: ApiRoutes.deviceEndpoint + "UpdateRecipe"
    },
    delete: {}
  };

  /**
   * Api endpoints for support
   */
  public static supportRoutes = {
    createSupportCase: ApiRoutes.supportEndpoint + "CreateNewCase",
    getAssemblyVersion: ApiRoutes.supportEndpoint + "GetAssemblyVersion"
  };

  /**
   * All routes for the "Common" endpoint
   */
  public static commonRoutes = {
    get: {
      getEmails: ApiRoutes.commonEndpoint + "GetEmails",
      getTavleEmails: ApiRoutes.commonEndpoint + "GetTavleEmails",
      getElectricityPrices: ApiRoutes.commonEndpoint + "GetElectricityPrices"
    },
    errorLogging: {
      logJavascriptException: ApiRoutes.commonEndpoint + "LogJavascriptException",
      getJavascriptExceptions: ApiRoutes.commonEndpoint + "GetJavascriptExceptions"
    },
    sendMail: ApiRoutes.commonEndpoint + "SendMail",
    localizationRoutes: {
      getAllResources: ApiRoutes.commonEndpoint + "Resources",
      getResourcesJson: ApiRoutes.commonEndpoint + "GetResourcesJson"
    }
  };

  /**
   * All routes associated with logs
   */
  public static logRoutes = {
    logJavascriptException: ApiRoutes.logEndpoint + "logJavascriptException",
    getAllLogs: ApiRoutes.logEndpoint + "GetLogs",
    getLogLevels: ApiRoutes.logEndpoint + "GetLogLevels",
    clearAllLogs: ApiRoutes.logEndpoint + "ClearAll",
    // -----
    getById: ApiRoutes.logEndpoint + "GetById",
    getAllByDate: ApiRoutes.logEndpoint + "GetAllByDate",
    getAllByLoglevel: ApiRoutes.logEndpoint + "GetAllByLoglevel"
  };
}
