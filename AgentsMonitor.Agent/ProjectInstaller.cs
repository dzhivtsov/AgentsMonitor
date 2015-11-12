namespace AgentsMonitor.Agent
{
    #region

    using System.ComponentModel;
    using System.Configuration.Install;

    #endregion

    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            this.InitializeComponent();
        }
    }
}