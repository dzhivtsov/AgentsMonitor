namespace AgentsMonitor.BusinessLogic
{
    #region

    using AgentsMonitor.Domain.Entities;

    using Microsoft.AspNet.Identity;

    #endregion

    public class UsersManager : UserManager<User>
    {
        public UsersManager(IUserStore<User> usersStore)
            : base(usersStore)
        {
        }
    }
}