using EasyRepository.EFCore.Generic;
using InsightAcademy.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsightAcademy.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        public List<User> _users=new List<User>();
        public AdminController(UnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        public IActionResult Index()
        {
            _users=_unitOfWork.Repository.GetQueryable<User>().Where(m=>m.Role==eRole.Admin).ToList();
            return View(_users);
        }
    }
}
