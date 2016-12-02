using System.Web.Mvc;
using MyApp.RefLib;

namespace MyWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly SettingsPrinter _printer;

        public HomeController(SettingsPrinter printer)
        {
            _printer = printer;
        }

        public HomeController() : this(new SettingsPrinter(new RefLibConfig()))
        {
        }

        public ActionResult Index()
        {
            var settings = _printer.GetSettingsList();
            return View(settings);
        }
    }
}