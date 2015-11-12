namespace AgentsMonitor.Agent
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.ServiceProcess;
    using System.Timers;

    using AgentsMonitor.ServiceBus;
    using AgentsMonitor.ServiceBus.Messages;

    #endregion

    public partial class AgentService : ServiceBase
    {
        private readonly IAgentServiceSettings agentServiceSettings;

        private MessagesQueueClient<AgentUpdateMessage> messagesQueueClient;

        private Timer timer;

        public AgentService(IAgentServiceSettings settings)
        {
            this.InitializeComponent();
            this.agentServiceSettings = settings;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            this.messagesQueueClient.Send(this.GetAgentUpdateMessage());
        }

        private AgentUpdateMessage GetAgentUpdateMessage()
        {
            List<DriveInfo> drives = DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.Fixed).ToList();

            return new AgentUpdateMessage
                       {
                           UserId = this.agentServiceSettings.UserId,
                           MacAddress = GetMacAddress(),
                           Name = Environment.MachineName,
                           FreeSpace = drives.Sum(drive => drive.TotalFreeSpace),
                           UsedSpace =
                               drives.Sum(drive => drive.TotalSize)
                               - drives.Sum(drive => drive.TotalFreeSpace)
                       };
        }

        protected override void OnStart(string[] args)
        {
            this.messagesQueueClient =
                new MessagesQueueClient<AgentUpdateMessage>(
                    this.agentServiceSettings.QueueConnectionString,
                    this.agentServiceSettings.QueueName);

            this.timer = new Timer(this.agentServiceSettings.TimerInterval);
            this.timer.Elapsed += this.TimerOnElapsed;
            this.timer.Start();
        }

        protected override void OnStop()
        {
            this.timer.Stop();
        }

        private static string GetMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            return nics.Any() ? nics.First().GetPhysicalAddress().ToString() : string.Empty;
        }
    }
}