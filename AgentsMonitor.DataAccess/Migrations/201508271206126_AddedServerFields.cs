namespace AgentsMonitor.DataAccess.Migrations
{
    #region

    using System.Data.Entity.Migrations;

    #endregion

    public partial class AddedServerFields : DbMigration
    {
        public override void Up()
        {
            this.AddColumn("dbo.Servers", "Name", c => c.String());
            this.AddColumn("dbo.Servers", "MacAddress", c => c.String());
        }

        public override void Down()
        {
            this.DropColumn("dbo.Servers", "MacAddress");
            this.DropColumn("dbo.Servers", "Name");
        }
    }
}