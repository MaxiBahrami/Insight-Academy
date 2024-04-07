using EasyRepository.EFCore.Generic;
using InsightAcademy.Entities;
using InsightAcademy.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace InsightAcademy.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public List<User> _users=new List<User>();
        private readonly Usershelper _usershelper;
        public AdminController(IUnitOfWork unitOfWork, Usershelper usershelper)
        {
            _unitOfWork = unitOfWork;
            _usershelper = usershelper;
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
            if (user.Email != "" && user.Password != "" && user.FirstName != "" && user.Role != 0)
            {
                user.ProfileImage = new byte[0];
                if (user.Id== 0)
                {
                    var dbUser = _unitOfWork.Repository.GetQueryable<User>()
                            .FirstOrDefault(u => u.Email == user.Email);

                    if (dbUser == null)
                    {
                        try
                        {
                            user.CreationDate = DateTime.Now;
                            user.CreatedBy = 1;
                            _unitOfWork.Repository.Add(user);
                            _unitOfWork.Repository.Complete();
                            TempData["message"] = user.Email + " User saved.";
                        }
                        catch (Exception ex)
                        {
                            // Log the exception for further investigation
                            TempData["message"] = "Error Occurred in saving user: "+ex;
                        }
                    }
                    else
                    {
                        TempData["message"] = user.Email + " Already Exits!";
                    }
                   
                }
                else
                {
                    _unitOfWork.Repository.Update(user);
                    _unitOfWork.Repository.Complete();

                }
                return Json(new { success = true });
            }
            else
            {
                TempData["message"] = " Please fill all the required fields.";
                return Json(new { success = false });
            }
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
