/**
 * Enum defining the user roles that this app operates with.
 * This defines the NAMES of these roles
 */
export enum UserRoleEnum {
  API = "API",
  Bearer = "Bearer",
  SuperAdmin = "SuperAdmin",
  ManageCustomer = "ManageCustomer",
  ManageUsers = "ManageUsers",
  ManageProfiles = "ManageProfiles",
  ManageReports = "ManageReports",
  ManageMessages = "ManageMessages",
  ManageRecurringMessages = "ManageRecurringMessages",
  ManageBenchmarks = "ManageBenchmarks",
  TwoFactorAuthenticate = "TwoFactorAuthenticate",
  ADLogin = "ADLogin",

  Benchmark = "Benchmark", // Benchmark is visible on admin page
  CustomerSetup = "CustomerSetup", // CustomerSetup is visible on admin page
  Searching = "Searching", // Searching is visible on admin page
  WEBMessages = "WEBMessages", // Web Messages is visible on admin page
  StandardReceivers = "StandardReceivers", // StandardReceiversis visible on admin page
  SubscriptionModule = "SubscriptionModule", // SubscriptionModule (Subscribe/Unsubscribe) is visible on admin page
  MessageTemplates = "MessageTemplates", // MessageTemplates is visible on admin page
  CanSetupStatstidende = "CanSetupStatstidende", // Statstidende menu is visible on admin page
  CanSetupSubscriptionReminders = "CanSetupSubscriptionReminders", // Subscription Notification Tab visible on the Page
  CanCreateScheduledBroadcasts = "CanCreateScheduledBroadcasts",
  WeatherWarning = "WeatherWarning",
  RequiresApproval = "RequiresApproval",
  AlwaysTestMode = "AlwaysTestMode"
}
