namespace AgentsMonitor.BusinessLogic
{
    using System.Configuration;
    using System.Linq;

    using AgentsMonitor.DataAccess;
    using AgentsMonitor.Domain.Entities;
    using AgentsMonitor.ServiceBus;
    using AgentsMonitor.ServiceBus.Messages;

    public class QueueMessageReceiver
    {
        private readonly MessagesQueueClient<AgentUpdateMessage> messagesQueueClient;
         
        public QueueMessageReceiver()
        {
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            const string QueueName = "AgentsQueue";
            this.messagesQueueClient = new MessagesQueueClient<AgentUpdateMessage>(connectionString, QueueName);
            this.messagesQueueClient.Subscribe(ProcessMessage);
        }

        private static void ProcessMessage(AgentUpdateMessage message)
        {
            using (AgentsMonitorContext context = new AgentsMonitorContext())
            {
                Server existingServer = GetExistingServer(context, message);
                UpdateServerFields(existingServer, message, context);
                context.SaveChanges();
            }
        }

        private static void UpdateServerFields(
            Server existingServer,
            AgentUpdateMessage agentUpdateMessage,
            AgentsMonitorContext context)
        {
            existingServer.MacAddress = agentUpdateMessage.MacAddress;
            existingServer.Name = agentUpdateMessage.Name;
            existingServer.FreeSpace = agentUpdateMessage.FreeSpace;
            existingServer.UsedSpace = agentUpdateMessage.UsedSpace;

            User user = context.Users.Find(agentUpdateMessage.UserId);
            existingServer.User = user;
        }

        private static Server GetExistingServer(AgentsMonitorContext context, AgentUpdateMessage agentUpdateMessage)
        {
            Server existingServer = context.Servers.FirstOrDefault(server => server.MacAddress == agentUpdateMessage.MacAddress);
            if (existingServer == null)
            {
                existingServer = context.Servers.Create();
                context.Servers.Add(existingServer);
            }
            return existingServer;
        }
    }

}