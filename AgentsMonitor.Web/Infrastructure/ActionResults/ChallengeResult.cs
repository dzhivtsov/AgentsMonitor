namespace AgentsMonitor.Web.Infrastructure.ActionResults
{
    #region

    using System.Web;
    using System.Web.Mvc;

    using Microsoft.Owin.Security;

    #endregion

    internal class ChallengeResult : HttpUnauthorizedResult
    {
        private const string XsrfKey = "XsrfId";

        private readonly string loginProvider;

        private readonly string redirectUri;

        private readonly string userId;

        public ChallengeResult(string provider, string redirectUri)
            : this(provider, redirectUri, null)
        {
        }

        public ChallengeResult(string provider, string redirectUri, string userId)
        {
            this.loginProvider = provider;
            this.redirectUri = redirectUri;
            this.userId = userId;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            AuthenticationProperties properties = new AuthenticationProperties { RedirectUri = this.redirectUri };

            if (this.userId != null)
            {
                properties.Dictionary[XsrfKey] = this.userId;
            }

            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, this.loginProvider);
        }
    }
}