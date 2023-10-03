using Olx_New_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Olx_New_Project.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult login()
        {
            return View();

        }

        [HttpPost]
        public ActionResult login(UserModel loginModel)
        {
             DataAccess access = new DataAccess();
            bool result = access.authLogin(loginModel, out string msg);
            if (result)
            {


                if (access.IsAdmin(loginModel.userEmail))
                {
                    // return RedirectToAction("Index", "Home");
                    return Json(2);
                }
                else
                {

                    //return RedirectToAction("About", "Home");
                    return Json(1);
                }
                
            }
            
            else
            {
                ViewBag.m = msg;
                //return View("Index");
                return Json(3);
            }
           

        }
        
        public ActionResult Logout()
        {
            Session.Clear();
            return View("Index");
        }
    }
}