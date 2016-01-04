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

        mvc_blog_dbEntities db = new mvc_blog_dbEntities();
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Write_post(Blog current)
        {             
            if (ModelState.IsValid)
            {
                Blog test_blog = new Blog();
                test_blog.Name = current.Name;
                test_blog.Id = current.Id;
                test_blog.is_active = current.is_active;
                test_blog.Author_id = current.Author_id;
                test_blog.Blog_Tag = current.Blog_Tag;
                test_blog.Contents = current.Contents;                 
 
                db.Blogs.Add(current);
                db.SaveChanges();

                return RedirectToAction("Write_post");
            }
            else
            {                              
                return View(current);
            }
        }

        [AllowAnonymous]
        public ActionResult Search()
        {
            return View();
        }
    }
}