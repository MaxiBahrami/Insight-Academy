using EasyRepository.EFCore.Abstractions;
using EasyRepository.EFCore.Generic;
using InsightAcademy.Dtos;
using InsightAcademy.Entities;
using InsightAcademy.Helper;
using InsightAcademy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Net.Mail;
using System.Net;

namespace InsightAcademy.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly Usershelper _usershelper;
        private readonly IStmpServices _stmpServices;
        public AuthenticationController(
            IUnitOfWork unitOfWork,
            IAuthService authService,
            Usershelper usershelper,
            IStmpServices stmpServices
            )
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _usershelper = usershelper;
            _stmpServices = stmpServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signup(UserDto user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User Userentity= _usershelper.DtotoEntity(user);
                    Userentity.CreationDate= DateTime.Now;
                    Userentity.CreatedBy = 1;
                    await _unitOfWork.Repository.AddAsync(Userentity);
                    await _unitOfWork.Repository.CompleteAsync();
                    
                    return RedirectToAction("Login", "Authentication");
                }
                catch (Exception ex)
                {
                    // Log the exception for further investigation
                    Console.WriteLine($"Error occurred during signup: {ex}");

                    return View("ErrorOccurred");
                }
            }
            else
            {
                // Return a JSON response indicating validation errors
                return new JsonResult("Please fill all the required fields.");
            }
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
                if (user.Email == null && user.Password==null)
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
        public IActionResult ResetPassword(int userid)
        {
            User user = _unitOfWork.Repository.GetQueryable<User>().Where(m => m.Id == userid).FirstOrDefault();
             if(user != null)
            {
            return View(user.Id);

            }
             else { return RedirectToAction("Lostpassword", "Authentication"); }
        }
        public IActionResult UpdateNewPassword(int userid, string NewPassword)
        {
			User user = _unitOfWork.Repository.GetQueryable<User>().Where(m=>m.Id==userid).FirstOrDefault();

			if (user != null)
			{
				// Update the password property of the user entity
				user.Password = NewPassword;

				try
				{
                    // Save the changes to persist the updated password
                    _unitOfWork.Repository.CompleteAsync();
				}
				catch (Exception ex)
				{
					// Handle any potential exceptions during the save operation
					Console.WriteLine($"Error updating password: {ex.Message}");
					throw; // Optionally, rethrow the exception or handle it accordingly
				}
			}
			else
			{
				// Handle the case where the user with the specified ID does not exist
				Console.WriteLine("User not found.");
				// You can throw an exception or handle it according to your application's logic
			}

			return RedirectToAction("Login");
        }
        public IActionResult Lostpassword()
        {
            string message = TempData["Message"] as string;
       
            ViewBag.Message = message;

            return View();
        }
        public IActionResult Recoverpassword(string email)
        {
           var validemail= _unitOfWork.Repository.GetQueryable<User>().Where(e=>e.Email == email).FirstOrDefault();
			if (validemail != null)
			{
              var message=  _stmpServices.sendStmpEmail(email, validemail.Id);

                return RedirectToAction("Login");
			}
			else
			{
				return RedirectToAction("Lostpassword");
			}

		}
		public IActionResult Signout()
        {
            _authService.ClearHttpContextItems();
            return RedirectToAction("Login");
        }

    }
}
