#region

using AgentsMonitor.Web;

using Microsoft.Owin;

#endregion

[assembly: OwinStartup(typeof(Startup))]

namespace AgentsMonitor.Web
{
    #region

    using AgentsMonitor.Web.Infrastructure.Providers;

    using Microsoft.AspNet.SignalR;

    using Owin;

    #endregion

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
            app.MapSignalR("/signalr", new HubConfiguration());
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new UserIdProvider());
        }
    }
}