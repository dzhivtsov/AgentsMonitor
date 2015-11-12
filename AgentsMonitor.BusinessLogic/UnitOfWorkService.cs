using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AgentsMonitor.BusinessLogic
{
    #region Using

    using DataAccess;
    using Domain.Entities;

    #endregion

    public class UnitOfWorkService : IUnitOfWorkService
    {
        #region Fields

        private readonly AgentsMonitorContext context = new AgentsMonitorContext();

        #endregion

        #region UnitOfWorkService

        private IServersRepository serversRepository;

        public IServersRepository ServersRepository
        {
            get
            {
                return this.serversRepository ?? (this.serversRepository = new ServersRepository(this.context));
            }
        }

        private UsersManager usersManager;

        public UsersManager UsersManager
        {
            get
            {
                return this.usersManager ?? (this.usersManager = new UsersManager(new UserStore<User>(this.context)));
            }
        }

        #endregion

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        #region IDispose

        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}