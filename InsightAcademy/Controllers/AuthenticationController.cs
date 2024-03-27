using Microsoft.AspNetCore.Mvc;

namespace InsightAcademy.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Lostpassword()
        {
            return View();
        }

        public IActionResult Signout()
        {
            return RedirectToAction("Login");
        }

    }
}
