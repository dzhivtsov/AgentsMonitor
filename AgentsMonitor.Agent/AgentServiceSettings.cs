namespace AgentsMonitor.Agent
{
    #region

    using System;
    using System.Configuration;

    #endregion

    internal class AgentServiceSettings : IAgentServiceSettings
    {
        public string UserId => ConfigurationManager.AppSettings["UserId"];

        public string QueueConnectionString => ConfigurationManager.AppSettings["QueueConnectionString"];

        public string QueueName => ConfigurationManager.AppSettings["QueueName"];

        public int TimerInterval => Convert.ToInt32(ConfigurationManager.AppSettings["TimerInterval"]);
    }
}