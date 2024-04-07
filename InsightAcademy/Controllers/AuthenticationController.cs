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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Google.Cloud.RecaptchaEnterprise.V1;
using Google.Api.Gax.ResourceNames;

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
            if (user.Email !="" && user.Password!=""&& user.firstName!="" && user.Role!=0)
            {
                var dbUser = _unitOfWork.Repository.GetQueryable<User>()
                               .FirstOrDefault(u => u.Email == user.Email);
                if (dbUser == null)
                {
                    try
                    {
                        User Userentity = _usershelper.DtotoEntity(user);
                        Userentity.CreationDate = DateTime.Now;
                        Userentity.CreatedBy = 1;
                        await _unitOfWork.Repository.AddAsync(Userentity);
                        await _unitOfWork.Repository.CompleteAsync();
                        TempData["message"] = user.Email + " Sign Up Successfully Log As";
                        return RedirectToAction("Login", "Authentication");
                    }
                    catch (Exception ex)
                    {
                        // Log the exception for further investigation
                        Console.WriteLine($"Error occurred during signup: {ex}");
                        TempData["message"] = "Error Occurred";
                        return RedirectToAction("Index", "Authentication");
                    }
                }
                else
                {
                    TempData["message"] =user.Email+ " Already Exits! Please LogIn ...";
                    return RedirectToAction("Login", "Authentication");
                }
            }
            else
            {
                TempData["message"] =" Please fill all the required fields.";
                return RedirectToAction("Index", "Authentication");              
            }
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
			string message = "";
			try
            {
                if (user.Email == null && user.Password==null)
                {
                    ModelState.AddModelError("InvalidRequest", "Invalid Client Request");
					message = "Invalid Client Request";
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
                        var claimss = new List<Claim>
                        {
                         new Claim(ClaimTypes.NameIdentifier, dbUser.Id.ToString()),
                       new Claim(ClaimTypes.Name, dbUser.LastName),
                         new Claim(ClaimTypes.Role, dbUser.Role.ToString())
                         };
                        var claimsIdentity = new ClaimsIdentity(claimss, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                        if (dbUser.Role == eRole.Admin)
                        {
                            message = "LogIn As Admin";
							TempData["message"] = message;
							return RedirectToAction("Index", "Admin");
                        }
                        else if(dbUser.Role == eRole.Student)
                        {
                            message = "LogIn As Student";
							TempData["message"] = message;
							return RedirectToAction("Profile", "Student");
                        }
                        else if (dbUser.Role == eRole.Teacher)
                        {
                            message = "LogIn As Teacher";
							TempData["message"] = message;
							return RedirectToAction("Profile", "Teacher");
                        }
						return RedirectToAction("Login", "Authentication");
					}
                }
                else
                {
                    ModelState.AddModelError("Unauthorized", "Invalid email or password.");
					message = "Invalid email or password.";
				}
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("ServerError", $"Error logging in user: {ex.Message}");
				message =( $"ServerError Error logging in user: {ex.Message}").ToString();
			}
			TempData["message"] = message;
			return View(user);

        }
        public IActionResult ResetPassword(int userid)
        {
            User user = _unitOfWork.Repository.GetQueryable<User>().Where(m => m.Id == userid).FirstOrDefault();
             if(user != null)
            {
                TempData["message"] = "Enter New Cridentials";
                return View(user.Id);

            }
             else {
                TempData["message"] = "No User Exiting ";
                return RedirectToAction("Lostpassword", "Authentication");
            }
        }
        public IActionResult UpdateNewPassword(int userid, string NewPassword)
        {
            var message = "";
          
            User user = _unitOfWork.Repository.GetById<User>(asNoTracking: false, id: userid);

            if (user != null)
			{
				// Update the password property of the user entity
				user.Password = NewPassword;

				try
				{
                    _unitOfWork.Repository.Update<User, int>(user);
                    // Save changes to the database
                    _unitOfWork.Repository.CompleteAsync();
                
                    message = "Password Updated LogIn with New Cridentials ";
                    TempData["message"] = message;
                    return RedirectToAction("Login");
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
                message = "User not found.";
             
			}

			return RedirectToAction("Lostpassword");
        }
        public IActionResult Lostpassword()
        {
            string message = TempData["Message"] as string;
       
            ViewBag.Message = message;

            return View();
        }
		[HttpPost]
		public IActionResult Recoverpassword(string email, string recaptchaToken)
		{
			// Verify the reCAPTCHA response
			bool captchaVerified = VerifyRecaptcha(recaptchaToken);
			if (captchaVerified)
			{
				var validemail = _unitOfWork.Repository.GetQueryable<User>().Where(e => e.Email == email).FirstOrDefault();
				if (validemail != null)
				{
					var message = _stmpServices.sendStmpEmail(email, validemail.Id);
					TempData["message"] = message;
					return RedirectToAction("Login");
				}
				else
				{
					TempData["message"] = "User Not found...";
					return RedirectToAction("Lostpassword");
				}
			}
            else
            {
			TempData["message"] = "reCAPTCHA verification failed. Please try again";

            }
			return RedirectToAction("Lostpassword", "Authentication");
		}

		public bool VerifyRecaptcha(string token)
		{
			string secretKey = "6LdkiKwpAAAAANyp08POHUeI0BJxwFy7tHpvG0Z7";
			string verificationUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={token}";

			using (var httpClient = new HttpClient())
			{
				var httpResponse = httpClient.GetAsync(verificationUrl).Result;
				if (httpResponse.IsSuccessStatusCode)
				{
					var responseContent = httpResponse.Content.ReadAsStringAsync().Result;
					dynamic verificationResult = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
					return (bool)verificationResult.success;
				}
				else
				{
					// Handle HTTP error
					return false;
				}
			}
		}

        public IActionResult Signout()
        {
            _authService.ClearHttpContextItems();
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
           var  message = "LogOut Successfully";
            TempData["message"] = message;
            return RedirectToAction("Login", "Authentication");
        }
        [HttpPost]
        public IActionResult SetMessage(string message)
        {
            TempData["Role"] = message;
            return Ok(); 
        }


    }
}
