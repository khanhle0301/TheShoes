using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MyShop.Common;
using MyShop.Model.Models;
using MyShop.Web.App_Start;
using MyShop.Web.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyShop.Web.Controllers
{
    public class AccountController : Controller
    {      
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public AccountController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Login(string userName, string password)
        {
            ApplicationUser user = _userManager.Find(userName, password);
            if (user != null)
            {
                IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationProperties props = new AuthenticationProperties();
                authenticationManager.SignIn(props, identity);
                return Json(new
                {
                    status = true
                });
            }
            return Json(new
            {
                status = false,
                message = "Thông tin đăng nhập không hợp lệ."
            });
        }

        [HttpPost]
        public async Task<JsonResult> Register(string accountViewModel)
        {
            var userVm = new JavaScriptSerializer().Deserialize<RegisterViewModel>(accountViewModel);

            var userByEmail = await _userManager.FindByEmailAsync(userVm.Email);
            if (userByEmail != null)
            {
                return Json(new
                {
                    status = false,
                    message = "Email đã tồn tại."
                });
            }
            var userByUserName = await _userManager.FindByNameAsync(userVm.UserName);
            if (userByUserName != null)
            {
                return Json(new
                {
                    status = false,
                    message = "Tài khoản đã tồn tại."
                });
            }
            var user = new ApplicationUser()
            {
                UserName = userVm.UserName,
                Email = userVm.Email,
                EmailConfirmed = true,
                BirthDay = DateTime.Now,
                FullName = userVm.FullName,
                PhoneNumber = userVm.PhoneNumber,
                Address = userVm.Address
            };

            await _userManager.CreateAsync(user, userVm.Password);
            var adminUser = await _userManager.FindByEmailAsync(userVm.Email);
            if (adminUser != null)
                await _userManager.AddToRolesAsync(adminUser.Id, new string[] { "User" });
            string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/newuser.html"));
            content = content.Replace("{{UserName}}", adminUser.FullName);
            content = content.Replace("{{Link}}", ConfigHelper.GetByKey("CurrentLink") + "dang-nhap.html");
            MailHelper.SendMail(adminUser.Email, "Đăng ký thành công", content);

            return Json(new
            {
                status = true
            });
        }

        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return Redirect("/");
        }
    }
}