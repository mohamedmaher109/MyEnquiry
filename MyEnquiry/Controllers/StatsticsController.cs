using Microsoft.AspNetCore.Mvc;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_BussniessLayer.Interface.InterfaceApi;

namespace MyEnquiry.Controllers
{
    public class StatsticsController : Controller
    {
        private IStatstics _IStatstics;

        public StatsticsController(IStatstics IStatstics)
        {
            _IStatstics = IStatstics;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DisplayGrid()
        {
            var result = _IStatstics.GetStatic();
            return PartialView("_DisplayStatics", result);
        }
    }
}
