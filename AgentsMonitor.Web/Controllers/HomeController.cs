namespace AgentsMonitor.Web.Controllers
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AgentsMonitor.BusinessLogic;
    using AgentsMonitor.DataAccess;
    using AgentsMonitor.Domain.Entities;
    using AgentsMonitor.Web.Models;

    using Microsoft.AspNet.Identity;

    #endregion

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IServersRepository serversRepository;

        public HomeController(IServersRepository serversRepository)
        {
            this.serversRepository = serversRepository;
        }

        public ActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public ActionResult Servers()
        {
            IEnumerable<Server> servers = this.serversRepository.GetServersByUserId(this.User.Identity.GetUserId());
            IEnumerable<ServerModel> model =
                servers.Select(
                    server =>
                    new ServerModel { ServerId = server.Id, Name = server.Name, MacAddress = server.MacAddress });

            return this.Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SpaceStatistics(int serverId)
        {
            Server server = this.serversRepository.GetServerById(serverId);
            if (server == null)
            {
                return this.HttpNotFound();
            }

            if (server.User.Id != this.User.Identity.GetUserId())
            {
                return new HttpUnauthorizedResult();
            }

            return
                this.Json(
                    new SpaceStatisticsModel
                        {
                            FreeSpace = BytesConverter.BytesToGigaBytes(server.FreeSpace),
                            UsedSpace = BytesConverter.BytesToGigaBytes(server.UsedSpace)
                        },
                    JsonRequestBehavior.AllowGet);
        }

        public ActionResult Settings()
        {
            SettingsModel model = new SettingsModel { UserId = this.User.Identity.GetUserId() };
            return this.View(model);
        }
    }
}