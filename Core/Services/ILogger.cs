using System;

namespace Atomia.Store.Core.Services
{
    public interface ILogger
    {
        void LogException(Exception ex);
        void LogException(Exception ex, string shortMessage);
    }
}
