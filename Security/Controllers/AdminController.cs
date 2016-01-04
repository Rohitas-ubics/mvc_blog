using Security.DAL.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data;

namespace Security.Controllers
{
    //[CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    // [CustomAuthorize(UsersConfigKey = "UsersConfigKey")]  
    // [CustomAuthorize(Users = "1")]
    [CustomAuthorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
         mvc_blog_dbEntities db = new mvc_blog_dbEntities();
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Approve_post(int? page)
        {
            int pagesize = 3;
            int pagenumber = (page??1);
            return View(db.Blogs.Where(i=>i.is_active == false).OrderBy(i => i.Id).ToPagedList(page??1,pagesize));
        }

        public string Approve_post_id(string Blog_id)
        {
            Blog current_blog = db.Blogs.Find(Convert.ToInt32(Blog_id));
            if (current_blog != null)
            {
                current_blog.is_active = true;
                //chaging here
                db.Entry(current_blog).State = EntityState.Modified;
                db.SaveChanges();
            }
            return "Abc";
        }
    }
}