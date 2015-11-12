namespace AgentsMonitor.Web.Hubs
{
    #region

    using AgentsMonitor.BusinessLogic;
    using AgentsMonitor.Web.Models;

    using Microsoft.AspNet.SignalR;

    #endregion

    public class ClientsNotifier
    {
        private readonly IHubContext hub;

        public ClientsNotifier()
        {
            this.hub = GlobalHost.ConnectionManager.GetHubContext<ServersHub>();
        }

        public void NotifyClients(AgentUpdateInfo agentUpdateInfo)
        {
            dynamic client = this.hub.Clients.User(agentUpdateInfo.UserId);

            if (agentUpdateInfo.IsNew)
            {
                AddServer(agentUpdateInfo, client);
            }
            else
            {
                UpdateSpaceStatistics(agentUpdateInfo, client);
            }
        }

        private static void UpdateSpaceStatistics(AgentUpdateInfo agentUpdateInfo, dynamic client)
        {
            double freeSpace = BytesConverter.BytesToGigaBytes(agentUpdateInfo.Server.FreeSpace);
            double usedSpace = BytesConverter.BytesToGigaBytes(agentUpdateInfo.Server.UsedSpace);
            var spaceStatisticsModel = new SpaceStatisticsModel
                                           {
                                               ServerId = agentUpdateInfo.Server.Id,
                                               FreeSpace = freeSpace,
                                               UsedSpace = usedSpace
                                           };
            client.updateSpaceStatistics(spaceStatisticsModel);
        }

        private static void AddServer(AgentUpdateInfo agentUpdateInfo, dynamic client)
        {
            var serverModel = new ServerModel
                                  {
                                      MacAddress = agentUpdateInfo.Server.MacAddress,
                                      Name = agentUpdateInfo.Server.Name,
                                      ServerId = agentUpdateInfo.Server.Id
                                  };
            client.addServer(serverModel);
        }
    }
}