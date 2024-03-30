using InsightAcademy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InsightAcademy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult HowItWorks()
        {
            return View();
        }
        public IActionResult Packages()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult TutorDetails()
        {
            return View();
        }
        public IActionResult Blogs()
        {
            return View();
        }
        public IActionResult Blogs2()
        {
            return View();
        }
        public IActionResult SearchListing()
        {
            return View();
        }
        public IActionResult SearchListing2()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
