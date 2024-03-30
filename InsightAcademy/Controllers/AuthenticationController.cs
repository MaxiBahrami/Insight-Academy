using EasyRepository.EFCore.Abstractions;
using EasyRepository.EFCore.Generic;
using InsightAcademy.Entities;
using InsightAcademy.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace InsightAcademy.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public AuthenticationController(
            IUnitOfWork unitOfWork,
            IAuthService authService
            )
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            try
            {
                if (user == null)
                {
                    ModelState.AddModelError("InvalidRequest", "Invalid Client Request");
                    return View(user);
                }

                var dbUser = _unitOfWork.Repository.GetQueryable<User>()
                                .FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);

                if (dbUser != null)
                {
                    if (dbUser.IsDeleted)
                    {
                        ModelState.AddModelError("UserDeleted", "The user is deleted.");
                    }
                    else
                    {
                        var token = _authService.GenerateJwtToken(dbUser);
                        // Redirect to the desired page upon successful authentication
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("Unauthorized", "Invalid email or password.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ServerError", $"Error logging in user: {ex.Message}");
            }

            // If any errors occurred, return the view with the ModelState containing the error messages
            return View(user);
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
