using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atomia.Store.UI.Infrastructure
{

    /// <summary>
    /// The GlobalEventsDefaultHandler class
    /// </summary>
    public abstract class GlobalEventsHandler
    {
        /// <summary>
        /// Handles the Application_Init event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_Init(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Application_Disposed event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_Disposed(object sender, EventArgs e)
        {
        }
        
        /// <summary>
        /// Handles the Application_Error event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_Error(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Application_Start event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_Start(object sender, EventArgs e)
        { 
        }

        /// <summary>
        /// Handles the Application_End event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Application_BeginRequest event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Application_EndRequest event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_EndRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Application_PreRequestHandlerExecute event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Application_PostRequestHandlerExecute event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_PostRequestHandlerExecute(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Application_PreSendRequestHeaders event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Application_PreSendContent event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_PreSendContent(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Application_AcquireRequestState event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_AcquireRequestState(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the ReleaseRequestState event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_ReleaseRequestState(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the ResolveRequestCache event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_ResolveRequestCache(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the UpdateRequestCache event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_UpdateRequestCache(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the AuthenticateRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the AuthorizeRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Application_AuthorizeRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Start event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Session_Start(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the End event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void Session_End(object sender, EventArgs e)
        {
        }
    }
}
