using System;

namespace Atomia.OrderPage.UI.Infrastructure
{
    public interface ILogger
    {
        void LogException(Exception ex);
        void LogException(Exception ex, string shortMessage);
    }
}
