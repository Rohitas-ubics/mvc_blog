﻿using Security.DAL;
using Security.DAL.Security;
using Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Script.Serialization;
using System.Web.Security;
using Newtonsoft.Json;
namespace Security.Controllers
{
    public class AccountController : Controller
    {
        DataContext Context = new DataContext();
        //
        mvc_blog_dbEntities db = new mvc_blog_dbEntities();
        //
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var user = Context.Users.Where(u => u.Username == model.Username && u.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                    var roles=user.Roles.Select(m => m.RoleName).ToArray();

                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.UserId = user.UserId;
                    serializeModel.FirstName = user.FirstName;
                    serializeModel.LastName = user.LastName;
                    serializeModel.roles = roles;

                   string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                            user.Email,
                             DateTime.Now,
                             DateTime.Now.AddMinutes(15),
                             false,
                             userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);

                    if(roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (roles.Contains("User"))
                    {
                        return RedirectToAction("Write_post", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Incorrect username and/or password");
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Account", null);
        }

        [AllowAnonymous]
        public ActionResult Default()
        {
            return View(db.Blogs.Where(i=>i.is_active == true).ToList());
        }

        [AllowAnonymous]
        public PartialViewResult All_blog_Partial()
        {
            return PartialView(db.Blogs.Where(i => i.is_active == true).ToList());
        }

        [AllowAnonymous]
        public ActionResult Details(Int32 id)
        {           
            return View(db.Blogs.Find(id));
        }
    }
}