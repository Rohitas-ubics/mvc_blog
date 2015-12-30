using Security.DAL.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Security.Controllers
{
    
    public class HomeController : BaseController
    {
        //
        // GET: /constructor/             
        public ActionResult Index()
        {
            string FullName = User.FirstName + " " + User.LastName;
            return View();
        }

        [CustomAuthorize(Roles = "User")]
        public ActionResult Write_post()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Search()
        {
            return View();
        }
    }
}