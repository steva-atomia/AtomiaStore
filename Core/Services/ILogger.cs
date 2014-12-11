using System;

namespace Atomia.Store.Core
{
    public interface ILogger
    {
        void LogException(Exception ex);

        void LogException(Exception ex, string shortMessage);
    }
}
