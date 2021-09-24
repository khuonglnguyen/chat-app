using app_chat_realtime_db.Models;
using app_chat_realtime_db.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace app_chat_realtime_db.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            return View(new LoginData());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginData loginData)
        {
            if (ModelState.IsValid)
            {
                int userId;
                if (new AppService().Login(loginData, out userId))
                {
                    FormsAuthentication.RedirectFromLoginPage(userId.ToString(), loginData.RememberMe);
                    return RedirectToAction("Index");
                }
                ViewBag.LoginError = "Username or Password is incorrect";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Logout(LoginData loginData)
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}