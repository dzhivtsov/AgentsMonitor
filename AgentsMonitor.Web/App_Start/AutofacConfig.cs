namespace AgentsMonitor.Web
{
    #region

    using System.Configuration;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;

    using AgentsMonitor.BusinessLogic;
    using AgentsMonitor.DataAccess;
    using AgentsMonitor.Domain.Entities;
    using AgentsMonitor.ServiceBus;
    using AgentsMonitor.ServiceBus.Messages;
    using AgentsMonitor.Web.Hubs;
    using AgentsMonitor.Web.Infrastructure;

    using Autofac;
    using Autofac.Integration.Mvc;

    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;

    using Owin;

    #endregion

    public static class AutofacConfig
    {
        #region Registration

        public static void RegisterDependency(IAppBuilder appBuilder)
        {
            Initialize(appBuilder);
        }

        #endregion

        #region Helpers

        private static void Initialize(IAppBuilder appBuilder)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModule(new AutofacWebTypesModule());

            RegisterServices(builder);

            RegisterNotifier(builder);

            IContainer container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            appBuilder.UseAutofacMiddleware(container);
            appBuilder.UseAutofacMvc();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<AgentsMonitorContext>();

            builder.RegisterType<ServersRepository>().As<IServersRepository>().InstancePerRequest();

            builder.Register(ctx => HttpContext.Current.GetOwinContext()).As<IOwinContext>();

            builder.Register(c => new UsersManager(new UserStore<User>(c.Resolve<AgentsMonitorContext>())));
            builder.Register(c => c.Resolve<IOwinContext>().Authentication).As<IAuthenticationManager>();
            builder.Register(c => new LoginManager(c.Resolve<UsersManager>(), c.Resolve<IOwinContext>().Authentication));

            builder.RegisterType<UpdatesNotifierStarter>();
            builder.RegisterType<QueueMessageReceiver>();
            builder.RegisterType<ClientsNotifier>();
        }

        private static void RegisterNotifier(ContainerBuilder builder)
        {
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            string queueName = ConfigurationManager.AppSettings["QueueName"];

            UpdatesNotifierStarter updatesNotifierStarter =
                new UpdatesNotifierStarter(
                    new QueueMessageReceiver(new MessagesQueueClient<AgentUpdateMessage>(connectionString, queueName)),
                    new ClientsNotifier());

            builder.RegisterInstance(updatesNotifierStarter).SingleInstance();
        }

        #endregion
    }
}