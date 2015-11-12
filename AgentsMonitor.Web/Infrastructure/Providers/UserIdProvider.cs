namespace AgentsMonitor.Web.Infrastructure.Providers
{
    #region

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;

    #endregion

    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            return request.User.Identity.GetUserId();
        }
    }
}