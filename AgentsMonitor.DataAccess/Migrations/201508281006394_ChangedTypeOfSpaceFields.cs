namespace AgentsMonitor.DataAccess.Migrations
{
    #region

    using System.Data.Entity.Migrations;

    #endregion

    public partial class ChangedTypeOfSpaceFields : DbMigration
    {
        public override void Up()
        {
            this.AddColumn("dbo.Servers", "UsedSpace", c => c.Long(false));
            this.AddColumn("dbo.Servers", "FreeSpace", c => c.Long(false));
        }

        public override void Down()
        {
            this.DropColumn("dbo.Servers", "FreeSpace");
            this.DropColumn("dbo.Servers", "UsedSpace");
        }
    }
}