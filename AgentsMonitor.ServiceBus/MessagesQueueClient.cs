namespace AgentsMonitor.ServiceBus
{
    #region

    using System;

    using Microsoft.ServiceBus.Messaging;

    #endregion

    public class MessagesQueueClient<T> : IMessagesQueueClient<T>
    {
        private readonly QueueClient queueClient;

        public MessagesQueueClient(string connectionString, string queueName)
        {
            this.queueClient = QueueClient.CreateFromConnectionString(connectionString, queueName, ReceiveMode.PeekLock);
        }

        public void Send(T message)
        {
            this.queueClient.Send(new BrokeredMessage(message));
        }

        public void Subscribe(Action<T> messageProcessor)
        {
            this.queueClient.OnMessage(brokeredMessage => messageProcessor(brokeredMessage.GetBody<T>()));
        }
    }
}