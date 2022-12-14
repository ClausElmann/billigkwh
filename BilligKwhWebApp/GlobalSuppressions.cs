// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "ASP.Net core does not have a Context to configure for", Scope = "module")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>", Scope = "module")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "<Pending>", Scope = "member", Target = "~M:BilligKwhWebApp.Controllers.AdminController.GetAvailableInfoBipNumbers(System.Int32)~System.Threading.Tasks.Task{Microsoft.AspNetCore.Mvc.IActionResult}")]
