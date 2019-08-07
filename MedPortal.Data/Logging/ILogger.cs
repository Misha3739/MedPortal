using System;
using System.Collections.Generic;
using System.Text;

namespace MedPortal.Data.Logging {
    public interface ILogger {
        void LogInfo(string message);

        void LogError(string message, Exception e);
    }
}
