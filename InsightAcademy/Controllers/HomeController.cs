using EasyRepository.EFCore.Generic;
using Google.Cloud.RecaptchaEnterprise.V1;
using InsightAcademy.Dtos;
using InsightAcademy.Entities;
using InsightAcademy.Helper;
using InsightAcademy.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using static Google.Cloud.RecaptchaEnterprise.V1.TransactionData.Types;

namespace InsightAcademy.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;      
        private readonly IHttpContextAccessor _context;
        private readonly TeacherHelper _teacherHelper;
        TeacherProfileDto profileDto = new TeacherProfileDto();
        TeacherProfile teacherprofile =new TeacherProfile();
        public HomeController(IUnitOfWork unitOfWork, IHttpContextAccessor context,TeacherHelper teacherHelper)
        {
           _unitOfWork = unitOfWork;
            _context = context;
            _teacherHelper= teacherHelper;
            
        }

    
    public IActionResult Index()
        {          
            return View();
        }
        public IActionResult MediaGallery()
        {
            return View();
        }
        public IActionResult SubjectIcanteach()
        {
            return View();
        }
        public IActionResult Education()
        {
            return View();
        }
        public IActionResult Contact()
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
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(_context.HttpContext.Session.GetString("UserId")); 
            if (userId != null)
            {
                var userdata = _unitOfWork.Repository.GetQueryable<InsightAcademy.Entities.User>().Where(m => m.Id == userId).FirstOrDefault();
                var TeacherProfile= _unitOfWork.Repository.GetQueryable<TeacherProfile>().Where(m => m.UserId == userId).FirstOrDefault();
                if (TeacherProfile != null && userdata != null)
                {
                    profileDto.id = TeacherProfile.Id ;
                    profileDto.UserId = TeacherProfile.UserId;
                    profileDto.Languages = TeacherProfile.Languages ?? "";
                    profileDto.HourlyRate = TeacherProfile.Rate ?? "";
                    profileDto.ZipCode = TeacherProfile.Zipcode;
                    profileDto.Introduction = TeacherProfile.Introduction ?? "";
                    profileDto.TagLine = TeacherProfile.TagLine ?? "";
                    profileDto.MyHome = TeacherProfile.MyHome;
                    profileDto.student = TeacherProfile.student ;
                    profileDto.online = TeacherProfile.online;
                    profileDto.PhoneNumber = TeacherProfile.PhoneNumber ?? "";
                    profileDto.Email = TeacherProfile.Email ?? "";
                    profileDto.Skype = TeacherProfile.Skype ?? "";
                    profileDto.Whatsapp = TeacherProfile.Whatsapp ?? "";
                    profileDto.firstName = userdata.FirstName ?? "";
                    profileDto.lastName = userdata.LastName ?? "";
                    profileDto.City = userdata.City ?? "";
                    profileDto.cites=  new SelectList(new List<SelectListItem>
{
    new SelectListItem { Text = "Select City from list" },
    new SelectListItem { Text = "Belize", Value = "Belize" },
    new SelectListItem { Text = "Benin", Value = "Benin" },
    new SelectListItem { Text = "Bermuda", Value = "Bermuda" },
    new SelectListItem { Text = "Bhutan", Value = "Bhutan" },
    new SelectListItem { Text = "Bolivia", Value = "Bolivia" },
    new SelectListItem { Text = "Bonaire", Value = "Bonaire" },
    new SelectListItem { Text = "Bosnia & Herzegovina", Value = "Bosnia & Herzegovina" },
    new SelectListItem { Text = "Botswana", Value = "Botswana" },
    new SelectListItem { Text = "Brazil", Value = "Brazil" },
    new SelectListItem { Text = "British Indian Ocean Ter", Value = "British Indian Ocean Ter" },
    new SelectListItem { Text = "Brunei", Value = "Brunei" },
    new SelectListItem { Text = "Bulgaria", Value = "Bulgaria" },
    new SelectListItem { Text = "Burkina Faso", Value = "Burkina Faso" },
    new SelectListItem { Text = "Burundi", Value = "Burundi" },
    new SelectListItem { Text = "Cambodia", Value = "Cambodia" },
    new SelectListItem { Text = "Cameroon", Value = "Cameroon" },
    new SelectListItem { Text = "Canada", Value = "Canada" },
    new SelectListItem { Text = "Canary Islands", Value = "Canary Islands" },
    new SelectListItem { Text = "Cape Verde", Value = "Cape Verde" },
    new SelectListItem { Text = "Cayman Islands", Value = "Cayman Islands" },
    new SelectListItem { Text = "Central African Republic", Value = "Central African Republic" },
    new SelectListItem { Text = "Chad", Value = "Chad" },
    new SelectListItem { Text = "Channel Islands", Value = "Channel Islands" },
    new SelectListItem { Text = "Chile", Value = "Chile" },
    new SelectListItem { Text = "China", Value = "China" },
    new SelectListItem { Text = "Christmas Island", Value = "Christmas Island" },
    new SelectListItem { Text = "Cocos Island", Value = "Cocos Island" }
}, "Value", "Text", userdata.City);
                    profileDto.Country = userdata.Country ?? "";
                    profileDto.Languageslist = new SelectList(new List<SelectListItem>
                    {
                        new SelectListItem{Text ="Select Languages"},
                        new SelectListItem{Text="Urdu",Value="Urdu"},
                        new SelectListItem{Text="English",Value="English"},
                        new SelectListItem{Text="Saraiki",Value="Saraiki"},
                        new SelectListItem{Text="Pushto",Value="Pushto"},
                        new SelectListItem{Text="Benin",Value="Benin"},
                        new SelectListItem{Text="Belize",Value="Belize"},
                    },"Value","Text",TeacherProfile.Languages);

                    profileDto.countrylist= new SelectList(new List<SelectListItem>
{
    new SelectListItem { Text = "Select City from list" },
    new SelectListItem { Text = "Belize", Value = "Belize" },
    new SelectListItem { Text = "Benin", Value = "Benin" },
    new SelectListItem { Text = "Bermuda", Value = "Bermuda" },
    new SelectListItem { Text = "Bhutan", Value = "Bhutan" },
    new SelectListItem { Text = "Bolivia", Value = "Bolivia" },
    new SelectListItem { Text = "Bonaire", Value = "Bonaire" },
    new SelectListItem { Text = "Bosnia & Herzegovina", Value = "Bosnia & Herzegovina" },
    new SelectListItem { Text = "Botswana", Value = "Botswana" },
    new SelectListItem { Text = "Brazil", Value = "Brazil" },
    new SelectListItem { Text = "British Indian Ocean Ter", Value = "British Indian Ocean Ter" },
    new SelectListItem { Text = "Brunei", Value = "Brunei" },
    new SelectListItem { Text = "Bulgaria", Value = "Bulgaria" },
    new SelectListItem { Text = "Burkina Faso", Value = "Burkina Faso" },
    new SelectListItem { Text = "Burundi", Value = "Burundi" },
    new SelectListItem { Text = "Cambodia", Value = "Cambodia" },
    new SelectListItem { Text = "Cameroon", Value = "Cameroon" },
    new SelectListItem { Text = "Canada", Value = "Canada" },
    new SelectListItem { Text = "Canary Islands", Value = "Canary Islands" },
    new SelectListItem { Text = "Cape Verde", Value = "Cape Verde" },
    new SelectListItem { Text = "Cayman Islands", Value = "Cayman Islands" },
    new SelectListItem { Text = "Central African Republic", Value = "Central African Republic" },
    new SelectListItem { Text = "Chad", Value = "Chad" },
    new SelectListItem { Text = "Channel Islands", Value = "Channel Islands" },
    new SelectListItem { Text = "Chile", Value = "Chile" },
    new SelectListItem { Text = "China", Value = "China" },
    new SelectListItem { Text = "Christmas Island", Value = "Christmas Island" },
    new SelectListItem { Text = "Cocos Island", Value = "Cocos Island" }
}, "Value", "Text", userdata.Country);
                    profileDto.Email = userdata.Email ?? "";
                    profileDto.Website = userdata.Website ?? "";
                    profileDto.Phone = userdata.Phone ?? "";

                }
                else if (TeacherProfile == null && userdata!=null)
                {
                    profileDto.id = userdata.Id;
                    profileDto.firstName = userdata.FirstName ?? "";
                    profileDto.lastName = userdata.LastName ?? "";
                    profileDto.Email = userdata.Email ?? "";
                    profileDto.Password = userdata.Password ?? "";
                    profileDto.Role = userdata.Role;
                    profileDto.ProfileImagepath = userdata.ProfileImage != null ? userdata.ProfileImage : new byte[0];
                    profileDto.Phone = userdata.Phone ?? "";
                    profileDto.Website = userdata.Website ?? "";
                    profileDto.StreetAddress = userdata.StreetAddress ?? "";
                    profileDto.City = userdata.City ?? "";

                    
                    profileDto.Country = userdata.Country ?? "";


                }
            }
            return View(profileDto);
        }

        [HttpPost]
        public IActionResult SaveORupdateProfile(TeacherProfileDto profileDto)
        {
            Entities.User user = _unitOfWork.Repository.GetById<Entities.User>(asNoTracking: false, id: profileDto.id);
            if (profileDto.formnumber == 1)
            {


                if (user != null)
                {
                    user.FirstName = profileDto.firstName;
                    user.LastName = profileDto.lastName;
                    user.City = profileDto.City;
                    user.Country = profileDto.Country;
                    _unitOfWork.Repository.Update<Entities.User, int>(user);
                    _unitOfWork.Repository.Complete();
                }
            teacherprofile = _unitOfWork.Repository.GetQueryable<TeacherProfile>().Where(m=>m.UserId==profileDto.UserId).FirstOrDefault();
                if (teacherprofile != null)
                {

                    teacherprofile.TagLine = profileDto.TagLine;
                    teacherprofile.Rate = profileDto.HourlyRate;
                    teacherprofile.Zipcode = profileDto.ZipCode;
                    teacherprofile.Languages = profileDto.Languages;
                    teacherprofile.online = profileDto.online;
                    teacherprofile.student = profileDto.student;
                    teacherprofile.MyHome = profileDto.MyHome;
                    teacherprofile.Introduction = profileDto.Introduction;

                    // teacherprofile.Email = profileDto.Email;
                    // teacherprofile.Skype = profileDto.Skype;
                    // teacherprofile.PhoneNumber = profileDto.PhoneNumber;
                    // teacherprofile.Whatsapp = profileDto.Whatsapp;
                    //  teacherprofile.WebSite = profileDto.Website;
                    _unitOfWork.Repository.Update<TeacherProfile, int>(teacherprofile);
                    _unitOfWork.Repository.Complete();
                }
                else if (teacherprofile == null)
                {
                    if (profileDto.formnumber == 1)
                    {
                        TeacherProfile profile = new TeacherProfile()
                        {
                            UserId = user.Id,
                            Languages = profileDto.Languages,
                            online = profileDto.online,
                            student = profileDto.student,
                            MyHome = profileDto.MyHome,
                            Zipcode = profileDto.ZipCode,
                            Introduction = profileDto.Introduction,
                            Rate = profileDto.HourlyRate,
                            TagLine = profileDto.TagLine,
                            Email = "",
                            PhoneNumber = "",
                            Skype = "",
                            WebSite = "",
                            Whatsapp = "",
                        };
                    _unitOfWork.Repository.Add<TeacherProfile>(profile);
                    _unitOfWork.Repository.Complete();
                    }
                }
            }
            else if (profileDto.formnumber == 2)
            {
                if (teacherprofile != null)
                {

                  //  teacherprofile.TagLine = profileDto.TagLine;
                  //  teacherprofile.Rate = profileDto.HourlyRate;
                  //  teacherprofile.Zipcode = profileDto.ZipCode;
                 //   teacherprofile.Languages = profileDto.Languages;
                  //  teacherprofile.online = profileDto.online;
                  //  teacherprofile.student = profileDto.student;
                 //   teacherprofile.MyHome = profileDto.MyHome;
                 //   teacherprofile.Introduction = profileDto.Introduction;

                     teacherprofile.Email = profileDto.Email;
                     teacherprofile.Skype = profileDto.Skype;
                     teacherprofile.PhoneNumber = profileDto.PhoneNumber;
                     teacherprofile.Whatsapp = profileDto.Whatsapp;
                     teacherprofile.WebSite = profileDto.Website;
                    _unitOfWork.Repository.Update<TeacherProfile, int>(teacherprofile);
                    _unitOfWork.Repository.CompleteAsync();
                }
                else if (teacherprofile == null)
                {
                    if (profileDto.formnumber == 2)
                    {
                        TeacherProfile profile = new TeacherProfile()
                        {
                            UserId = user.Id,
                            Languages = "",
                            online = false,
                            student = false,
                            MyHome = false,
                            Zipcode = 0,
                            Introduction = "",
                            Rate = "",
                            TagLine = "",

                            Email = profileDto.Email,
                            PhoneNumber = profileDto.PhoneNumber,
                            Skype = profileDto.Skype,
                            WebSite = profileDto.Website,
                            Whatsapp = profileDto.Whatsapp,
                        };
                        _unitOfWork.Repository.Add<TeacherProfile>(profile);
                        _unitOfWork.Repository.Complete();
                    }
                }


            }

                
            return RedirectToAction("Profile", "Home",profileDto);
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
