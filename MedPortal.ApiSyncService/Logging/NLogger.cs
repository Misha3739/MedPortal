using MedPortal.Data.Logging;
using NLog;
using System;
using ILogger = MedPortal.Data.Logging.ILogger;

namespace MedPortal.ApiSyncService.Logging {
    internal class NLogger : ILogger {
        Logger infoLogger = LogManager.GetLogger("info");
        Logger errorLogger = LogManager.GetLogger("error");
        public void LogError(string message, Exception e) {
            errorLogger.Error(e, message);
            infoLogger.Error(e, message);
        }

        public void LogInfo(string message) {
            infoLogger.Info(message);
        }
    }
}
