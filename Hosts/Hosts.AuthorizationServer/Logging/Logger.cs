using NLog;
using NLog.Config;
using NLog.Targets;

namespace Hosts.AuthorizationServer.Logging
{
    public static class Logger
    {
        public static void SetupLogger()
        {
            // IdentityServer uses LibLog, this means NLog is automatically detected.
            // We just need to setup NLog and it will receive logmessages from IdentityServer

            var loggingConfiguration = new LoggingConfiguration();

            // Setup debugger target which logs to debugger, log statements will be visible in output window of Visual Studio
            var debuggerTarget = new DebuggerTarget();
            loggingConfiguration.AddTarget("DebuggerTarget", debuggerTarget);
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, debuggerTarget));

            LogManager.Configuration = loggingConfiguration;

            LogManager.ThrowExceptions = true;
        }
    }
}