// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditLogger.cs" company="Atomia AB">
//   Copyright (C) 2016 Atomia AB. All rights reserved
// </copyright>
// <summary>
//   Defines the AuditLogger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Web;

using Atomia.Store.Core;
using Atomia.Web.Base.AuditLog;

namespace Atomia.Store.ActionTrail.Adapters
{
    /// <summary>
    /// Defines the AuditLogger type.
    /// </summary>
    /// <seealso cref="Atomia.Store.Core.IAuditLogger" />
    public class AuditLogger : IAuditLogger
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
        public void Log(string actionType, string message, string accountId, string username, string objectId, Dictionary<string, object> details)
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            if (HttpContext.Current.Application["AuditLogPowerSwitch"] != null)
            {
                bool auditLogPowerSwitch;
                if (bool.TryParse(HttpContext.Current.Application["AuditLogPowerSwitch"].ToString().ToLower(), out auditLogPowerSwitch))
                {
                    if (auditLogPowerSwitch)
                    {
                        WebBaseAuditLogger.CreateAuditLog("Atomia Store", actionType, message, accountId, username, objectId, details);
                    }
                }
            }
        }
    }
}