namespace AgentsMonitor.BusinessLogic
{
    #region

    using AgentsMonitor.Domain.Entities;

    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;

    #endregion

    public class LoginManager : SignInManager<User, string>
    {
        public LoginManager(UsersManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }
    }
}