namespace AgentsMonitor.BusinessLogic
{
    #region

    using AgentsMonitor.Domain.Entities;

    #endregion

    public class AgentUpdateInfo
    {
        public bool IsNew { get; set; }

        public Server Server { get; set; }

        public string UserId { get; set; }
    }
}