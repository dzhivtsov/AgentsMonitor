namespace AgentsMonitor.Domain.Entities
{
    #region

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class Server
    {
        public virtual long UsedSpace { get; set; }

        public virtual long FreeSpace { get; set; }

        public virtual User User { get; set; }

        public virtual string Name { get; set; }

        public virtual string MacAddress { get; set; }

        [Key]
        public virtual int Id { get; set; }
    }
}