// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuditLogger.cs" company="Atomia AB">
//   Copyright (C) 2016 Atomia AB. All rights reserved
// </copyright>
// <summary>
//   Defines the IAuditLogger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Defines the IAuditLogger type.
    /// </summary>
    public interface IAuditLogger
    {
        /// <summary>
        /// Logs the specified action type.
        /// </summary>
        /// <param name="actionType">Type of the action.</param>
        /// <param name="message">The message.</param>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="details">The details.</param>
        void Log(string actionType, string message, string accountId, string username, string objectId, Dictionary<string, object> details);
    }
}