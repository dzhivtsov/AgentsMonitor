namespace AgentsMonitor.DataAccess
{
    #region

    using System.Data.Entity;

    using AgentsMonitor.Domain.Entities;

    using Microsoft.AspNet.Identity.EntityFramework;

    #endregion

    public class AgentsMonitorContext : IdentityDbContext<User>
    {
        public DbSet<Server> Servers { get; set; }
    }
}