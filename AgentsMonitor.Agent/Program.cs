namespace AgentsMonitor.Agent
{
    #region

    using System.ServiceProcess;

    using Autofac;

    #endregion

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            IContainer container = ConfigureContainer();

            ServiceBase[] servicesToRun = { container.Resolve<AgentService>() };
            ServiceBase.Run(servicesToRun);
        }

        private static IContainer ConfigureContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<AgentServiceSettings>().As<IAgentServiceSettings>();
            builder.RegisterType<AgentService>();
            IContainer container = builder.Build();
            return container;
        }
    }
}