namespace AgentsMonitor.Web
{
    #region

    using System;

    using AgentsMonitor.BusinessLogic;
    using AgentsMonitor.Domain.Entities;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OAuth;

    using Owin;

    #endregion

    public partial class Startup
    {
        static Startup()
        {
            PublicClientId = "web";

            OAuthOptions = new OAuthAuthorizationServerOptions
                               {
                                   TokenEndpointPath = new PathString("/Token"),
                                   AuthorizeEndpointPath =
                                       new PathString("/Account/Authorize"),
                                   Provider = new OAuthAuthorizationServerProvider(),
                                   AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                                   AllowInsecureHttp = true
                               };
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; }

        public static string PublicClientId { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            AutofacConfig.RegisterDependency(app);

            app.UseCookieAuthentication(
                new CookieAuthenticationOptions
                    {
                        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                        LoginPath = new PathString("/Account/Login"),
                        Provider =
                            new CookieAuthenticationProvider
                                {
                                    OnValidateIdentity =
                                        SecurityStampValidator
                                        .OnValidateIdentity
                                        <UsersManager, User>(
                                            TimeSpan.FromMinutes(
                                                20),
                                            (manager, user) =>
                                            manager
                                                .CreateIdentityAsync
                                                (
                                                    new User(),
                                                    DefaultAuthenticationTypes
                                                .ApplicationCookie))
                                }
                    });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            app.UseOAuthBearerTokens(OAuthOptions);

            app.UseMicrosoftAccountAuthentication("000000004816789D", "mfPe08xs3uDoxaTOPfYwgA852zpfB0W8");
        }
    }
}