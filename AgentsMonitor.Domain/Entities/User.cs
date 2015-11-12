namespace AgentsMonitor.Domain.Entities
{
    #region

    using System.Collections.Generic;

    using Microsoft.AspNet.Identity.EntityFramework;

    #endregion

    public class User : IdentityUser
    {
        public virtual ICollection<Server> Servers { get; set; }
    }
}