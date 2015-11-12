namespace AgentsMonitor.BusinessLogic
{
    #region

    using System;

    using AgentsMonitor.Domain.Entities;
    using AgentsMonitor.ServiceBus;
    using AgentsMonitor.ServiceBus.Messages;

    using Microsoft.AspNet.Identity;

    #endregion

    public class QueueMessageReceiver
    {
        private readonly IMessagesQueueClient<AgentUpdateMessage> messagesQueueClient;

        public QueueMessageReceiver(IMessagesQueueClient<AgentUpdateMessage> messagesQueueClient)
        {
            this.messagesQueueClient = messagesQueueClient;
        }

        public event EventHandler<AgentUpdateInfo> ServerInfoUpdated;

        public void Subscribe()
        {
            this.messagesQueueClient.Subscribe(this.ProcessMessage);
        }

        private void ProcessMessage(AgentUpdateMessage message)
        {
            using (UnitOfWorkService unitOfWork = new UnitOfWorkService())
            {
                Server existingServer = unitOfWork.ServersRepository.GetServerByMacAddress(message.MacAddress)
                                        ?? new Server();
                AgentUpdateInfo agentUpdateInfo = GetAgentUpdateInfo(message, existingServer);

                this.UpdateServerFields(existingServer, message, unitOfWork.UsersManager);

                unitOfWork.ServersRepository.Save(existingServer);

                if (agentUpdateInfo != null)
                {
                    this.ServerInfoUpdated?.Invoke(this, agentUpdateInfo);
                }
            }
        }

        private static AgentUpdateInfo GetAgentUpdateInfo(AgentUpdateMessage message, Server existingServer)
        {
            bool isNew = existingServer.Id == default(int);

            bool spaceIsUpdated = existingServer.FreeSpace != message.FreeSpace
                                  || existingServer.UsedSpace != message.UsedSpace;

            if (!isNew && !spaceIsUpdated)
            {
                return null;
            }

            return new AgentUpdateInfo { IsNew = isNew, Server = existingServer, UserId = message.UserId };
        }

        private void UpdateServerFields(
            Server existingServer,
            AgentUpdateMessage agentUpdateMessage,
            UsersManager userManager)
        {
            existingServer.MacAddress = agentUpdateMessage.MacAddress;
            existingServer.Name = agentUpdateMessage.Name;
            existingServer.FreeSpace = agentUpdateMessage.FreeSpace;
            existingServer.UsedSpace = agentUpdateMessage.UsedSpace;
            existingServer.User = userManager.FindById(agentUpdateMessage.UserId);
        }
    }
}