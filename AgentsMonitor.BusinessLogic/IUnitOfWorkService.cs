using System;

namespace AgentsMonitor.BusinessLogic
{
    #region Using

    using DataAccess;

    #endregion

    public interface IUnitOfWorkService : IDisposable
    {
        IServersRepository ServersRepository { get; }

        UsersManager UsersManager { get; }

        void SaveChanges();
    }
}