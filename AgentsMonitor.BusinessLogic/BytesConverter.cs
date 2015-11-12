namespace AgentsMonitor.BusinessLogic
{
    #region

    using System;

    #endregion

    public static class BytesConverter
    {
        public static double BytesToGigaBytes(long bytes)
        {
            return bytes / Math.Pow(2, 3 * 10);
        }
    }
}