using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PicTick.Models;

namespace PicTick.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult AdminLogin()
        {
            return View();
        }

        public ActionResult StudioLogin()
        {
            return View();
        }

        // this code is for session management : kapil singh ; 05-02-2018
        public class MyObject
        {
            public string success { get; set; }
        }

        public static class Constants
        {
            public static string User = "user";
        }

        public static class SessionManagement
        {
            public static User user
            {
                get
                {
                    return
                        System.Web.HttpContext.Current.Session[Constants.User] != null ?
                        System.Web.HttpContext.Current.Session[Constants.User] as User : null;
                }
                set
                {
                    System.Web.HttpContext.Current.Session[Constants.User] = value;
                }
            }
        }

        [HttpPost]
        public JsonResult SetLoginSession(User user)
        {
            SessionManagement.user = user as User;
            return new JsonResult { Data = { } };
        }

        public JsonResult GetLoginSession()
        {
            return Json(new JsonResult { Data = SessionManagement.user }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DestoryLoginSession()
        {
            SessionManagement.user = null;
            return new JsonResult { Data = { } };
        }
    }
}