namespace AgentsMonitor.Web.Models
{
    public class SpaceStatisticsModel
    {
        public int ServerId { get; set; }

        public double FreeSpace { get; set; }

        public double UsedSpace { get; set; }
    }
}