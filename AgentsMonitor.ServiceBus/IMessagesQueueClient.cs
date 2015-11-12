namespace AgentsMonitor.ServiceBus
{
    #region

    using System;

    #endregion

    public interface IMessagesQueueClient<T>
    {
        void Send(T message);

        void Subscribe(Action<T> messageProcessor);
    }
}