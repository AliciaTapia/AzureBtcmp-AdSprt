// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0052:Remove unread private members", Justification = "Attendees will write code that uses this value later", Scope = "member", Target = "~F:ContosoSupport.Controllers.SupportCasesController.logger")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "This specific exception is what will be thrown when code is running outside of Azure", Scope = "member", Target = "~M:ContosoSupport.Services.VmMetadataService.GetComputeLocationAsync(System.String)~System.Threading.Tasks.Task{System.String}")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Internal log messages, not user facing, no localization required", Scope = "type", Target = "ContosoSupport.Middleware.PerfMetricsMiddleware")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Internal log messages, not user facing, no localization required", Scope = "type", Target = "ContosoSupport.InstrumentationHelpers.IfxMetricsHelper")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "<Pending>", Scope = "type", Target = "ContosoSupport.Middleware.BehaviorConfig")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Dummy parameters to simulate values that might be passed to an RP", Scope = "type", Target = "ContosoSupport.Controllers.SupportCasesController")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Dummy parameters to simulate values that might be passed to an RP", Scope = "type", Target = "ContosoSupport.Controllers.SupportCasesController")]
