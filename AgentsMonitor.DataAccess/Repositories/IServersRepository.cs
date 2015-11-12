namespace AgentsMonitor.DataAccess
{
    #region

    using System.Collections.Generic;

    using AgentsMonitor.Domain.Entities;

    #endregion

    public interface IServersRepository
    {
        IEnumerable<Server> GetServersByUserId(string userId);

        Server GetServerById(int serverId);

        Server GetServerByMacAddress(string macAddress);

        Server Save(Server server);
    }
}