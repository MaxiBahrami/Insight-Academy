using EasyRepository.EFCore.Generic;
using InsightAcademy.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsightAcademy.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public List<User> _users=new List<User>();
        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        public IActionResult Index()
        {
            _users=_unitOfWork.Repository.GetQueryable<User>().ToList();
            return View(_users);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Repository.Add(user);
                _unitOfWork.Repository.Complete();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            User user = _unitOfWork.Repository.GetQueryable<User>().Where(m=>m.Id==id).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Repository.Update(user);
                _unitOfWork.Repository.Complete();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            User user = _unitOfWork.Repository.GetQueryable<User>().Where(m=>m.Id==id).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _unitOfWork.Repository.HardDelete<User>(id);
            _unitOfWork.Repository.Complete();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetUser(int id)
        {
            User user = _unitOfWork.Repository.GetQueryable<User>().Where(m=>m.Id==id).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            return Json(user);
        }

        [HttpPost]
        public IActionResult SaveUser(User user)
        {
            if (ModelState.IsValid)
            {
                if (user.Id == 0)
                {
                    _unitOfWork.Repository.Add(user);
                }
                else
                {
                    _unitOfWork.Repository.Update(user);
                }
                _unitOfWork.Repository.Complete();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            _unitOfWork.Repository.HardDelete<User>(id);
            _unitOfWork.Repository.Complete();
            return Json(new { success = true });
        }


    }
}
