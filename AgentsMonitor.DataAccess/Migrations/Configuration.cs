namespace AgentsMonitor.DataAccess.Migrations
{
    #region

    using System.Data.Entity.Migrations;

    #endregion

    internal sealed class Configuration : DbMigrationsConfiguration<AgentsMonitorContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
            this.ContextKey = "AgentsMonitor.Web.DataAccess.AgentsMonitorContext";
        }
    }
}