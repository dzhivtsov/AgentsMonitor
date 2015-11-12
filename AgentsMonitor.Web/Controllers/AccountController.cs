namespace AgentsMonitor.Web.Controllers
{
    #region

    using System.Web.Mvc;

    using AgentsMonitor.BusinessLogic;
    using AgentsMonitor.Domain.Entities;
    using AgentsMonitor.Web.Infrastructure.ActionResults;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;

    #endregion

    public class AccountController : Controller
    {
        private readonly IAuthenticationManager authenticationManager;

        private readonly LoginManager loginManager;

        private readonly UsersManager usersManager;

        public AccountController(
            LoginManager loginManager,
            UsersManager usersManager,
            IAuthenticationManager authenticationManager)
        {
            this.loginManager = loginManager;
            this.usersManager = usersManager;
            this.authenticationManager = authenticationManager;
        }

        public ActionResult Login(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(
                provider,
                this.Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            ExternalLoginInfo loginInfo = this.authenticationManager.GetExternalLoginInfo();
            if (loginInfo == null)
            {
                return this.RedirectToAction("Login");
            }

            SignInStatus result = this.loginManager.ExternalSignIn(loginInfo, false);
            return this.ProcessSignInStatus(returnUrl, result, loginInfo);
        }

        private ActionResult ProcessSignInStatus(string returnUrl, SignInStatus result, ExternalLoginInfo loginInfo)
        {
            switch (result)
            {
                case SignInStatus.Success:
                    return this.RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return this.View("Lockout");
                default:
                    this.CreateAndSignInNewUser(loginInfo);
                    return this.RedirectToLocal(returnUrl);
            }
        }

        private void CreateAndSignInNewUser(ExternalLoginInfo loginInfo)
        {
            var user = new User { UserName = loginInfo.DefaultUserName, Email = loginInfo.Email };
            IdentityResult identityResult = this.usersManager.Create(user);

            if (!identityResult.Succeeded)
            {
                return;
            }

            identityResult = this.usersManager.AddLogin(user.Id, loginInfo.Login);

            if (identityResult.Succeeded)
            {
                this.loginManager.SignIn(user, false, false);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }
    }
}