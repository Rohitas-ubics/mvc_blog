using Security.DAL.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Security.Controllers
{
    // [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
   // [CustomAuthorize(UsersConfigKey = "UsersConfigKey")]
     [CustomAuthorize(Roles= "User")]
    // [CustomAuthorize(Users = "1,2")]
    public class UserController : BaseController
    {
         mvc_blog_dbEntities db = new mvc_blog_dbEntities();
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Blog_list()
        {             
            return View(db.Blogs.Where(x=>x.Author_id.Equals(User.UserId)).ToList());
        }

        public ActionResult Edit(Int32 id)
        {
            return View(db.Blogs.Find(id));
        }
    }
}