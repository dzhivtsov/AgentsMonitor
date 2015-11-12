namespace AgentsMonitor.Web.Infrastructure
{
    #region

    using AgentsMonitor.BusinessLogic;
    using AgentsMonitor.Web.Hubs;

    #endregion

    public class UpdatesNotifierStarter
    {
        private readonly ClientsNotifier clientsNotifier;

        private readonly QueueMessageReceiver queueMessageReceiver;

        public UpdatesNotifierStarter(QueueMessageReceiver queueMessageReceiver, ClientsNotifier clientsNotifier)
        {
            this.queueMessageReceiver = queueMessageReceiver;
            this.clientsNotifier = clientsNotifier;

            this.Start();
        }

        private void Start()
        {
            this.queueMessageReceiver.Subscribe();
            this.queueMessageReceiver.ServerInfoUpdated += ((o, info) => this.clientsNotifier.NotifyClients(info));
        }
    }
}