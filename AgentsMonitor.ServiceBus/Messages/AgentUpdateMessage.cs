namespace AgentsMonitor.ServiceBus.Messages
{
    public class AgentUpdateMessage
    {
        public string UserId { get; set; }

        public string MacAddress { get; set; }

        public string Name { get; set; }

        public long FreeSpace { get; set; }

        public long UsedSpace { get; set; }
    }
}