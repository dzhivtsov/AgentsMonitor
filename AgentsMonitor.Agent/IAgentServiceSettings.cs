namespace AgentsMonitor.Agent
{
    public interface IAgentServiceSettings
    {
        string QueueConnectionString { get; }

        string QueueName { get; }

        int TimerInterval { get; }

        string UserId { get; }
    }
}