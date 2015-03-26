using System;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Interface for logging exceptions
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log an <see cref="System.Exception"/>
        /// </summary>
        /// <param name="ex">The <see cref="System.Exception"/> to log.</param>
        void LogException(Exception ex);

        /// <summary>
        /// Log an <see cref="System.Exception"/> and a message.
        /// </summary>
        /// <param name="ex">The <see cref="System.Exception"/> to log</param>
        /// <param name="shortMessage">The message to log.</param>
        void LogException(Exception ex, string shortMessage);
    }
}
