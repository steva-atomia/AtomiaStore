using Atomia.Store.Core;
using Atomia.Web.Base.ActionTrail;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;

namespace Atomia.Store.Services.ActionTrail
{
    /// <summary>
    /// Helper class for Action Trail logging
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>
        /// Logs the order page exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public void LogException(Exception ex)
        {
            var shortMessage = string.Format("Atomia Order Page v2 threw an exception.\r\n {0}", ex.Message + "\r\n" + ex.StackTrace);

            this.LogException(ex, shortMessage);
        }

        /// <summary>
        /// Logs the order page exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="shortMessage">The short message.</param>
        public void LogException(Exception ex, string shortMessage)
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            if (HttpContext.Current.Application["ActionTrailPowerSwitch"] != null)
            {
                bool actionTrailPowerSwitch;
                if (Boolean.TryParse(HttpContext.Current.Application["ActionTrailPowerSwitch"].ToString().ToLower(), out actionTrailPowerSwitch))
                {
                    if (actionTrailPowerSwitch)
                    {
                        string accountId = null;

                        var user = Thread.CurrentPrincipal.Identity.IsAuthenticated ? Thread.CurrentPrincipal.Identity.Name : null;

                        WebBaseLogger.CreateActionTrailExceptionLog(ex, "Atomia Order Page v2", accountId, user, shortMessage, new List<string> { "Atomia Order Page v2 Exceptions" });
                    }
                }
            }

            if (HttpContext.Current.Application["ElmahPowerSwitch"] == null)
            {
                return;
            }

            bool elmahPowerSwitch;
            if (!Boolean.TryParse(HttpContext.Current.Application["ElmahPowerSwitch"].ToString().ToLower(), out elmahPowerSwitch))
            {
                return;
            }

            if (elmahPowerSwitch)
            {
                WebBaseLogger.CreateElmahExceptionLog(ex);
            }
        }
    }
}
