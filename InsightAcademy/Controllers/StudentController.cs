using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsightAcademy.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
