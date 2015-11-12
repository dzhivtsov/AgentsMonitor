namespace AgentsMonitor.DataAccess
{
    #region

    using System.Collections.Generic;
    using System.Linq;

    using AgentsMonitor.Domain.Entities;

    #endregion

    public class ServersRepository : IServersRepository
    {
        private readonly AgentsMonitorContext context;

        public ServersRepository(AgentsMonitorContext context)
        {
            this.context = context;
        }

        public IEnumerable<Server> GetServersByUserId(string userId)
        {
            return this.context.Servers.Where(server => server.User.Id == userId);
        }

        public Server GetServerById(int serverId)
        {
            return this.context.Servers.Find(serverId);
        }

        public Server GetServerByMacAddress(string macAddress)
        {
            return this.context.Servers.FirstOrDefault(server => server.MacAddress == macAddress);
        }

        public Server Save(Server server)
        {
            if (server.Id != default(int))
            {
                this.context.Servers.Attach(server);
            }
            else
            {
                this.context.Servers.Add(server);
            }
            this.context.SaveChanges();
            return server;
        }
    }
}