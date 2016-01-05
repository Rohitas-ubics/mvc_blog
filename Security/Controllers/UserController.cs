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
            return View();
        }
        public PartialViewResult partial_Blog_list()
        {             
            return  PartialView(db.Blogs.Where(x=>x.Author_id.Equals(User.UserId)).ToList());
        }

        public ActionResult Edit(Int32 id)
        {
            return View(db.Blogs.Find(id));
        }

        public string Delete_blog(Int32 Blog_id)
        {
            List<Blog_Tag> list_bt = db.Blog_Tag.Where(x=>x.blog_id.Equals(Blog_id)).ToList();
            foreach(var blog_tag_item in list_bt)
            { 
                db.Blog_Tag.Remove(blog_tag_item);
                db.SaveChanges();                      // removing child enitity
            }

            Blog current_blog = db.Blogs.Where(x=>x.Id.Equals(Blog_id)).First();
            db.Blogs.Remove(current_blog);
            db.SaveChanges();
            return "abc";
        }
    }
}