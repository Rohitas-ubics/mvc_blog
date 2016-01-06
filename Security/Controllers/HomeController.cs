using RTE;
using Security.DAL.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Editor Editor1 = new Editor(System.Web.HttpContext.Current, "Editor1");
            Editor1.LoadFormData("");
            Editor1.MvcInit();
            Editor1.Toolbar = "minimal";
            Editor1.ID = "Editor_id";
            Editor1.Name = "Editor_name";
            ViewBag.Editor = Editor1.MvcGetString(); 
            return View();
        }


        [HttpPost , ValidateInput(false)]        
        public ActionResult Write_post(Blog current ,FormCollection form1)
        {             
            if (ModelState.IsValid)
            {

                var key = "Editor_name";
                var blog_text = form1[key].ToString();
                current.Contents = blog_text;
                db.Blogs.Add(current);
                db.SaveChanges();

                var Last_insert_blog_id = current.Id;

                key = "tags";
                var blog_tags =  form1[key].ToString().ToLower();
                string[] blog_tag_array = blog_tags.Split(',');
                foreach (string tag in blog_tag_array)
                {
                    Tag_master tag_master_item = new Tag_master();
                    tag_master_item = db.Tag_master.Where(x=>x.tag_name.Equals(tag)).FirstOrDefault();
                    if (tag_master_item != null)   //tag already present .. so driectly map to blog_tag
                    {
                        Blog_Tag blog_tag_item = new Blog_Tag();
                        blog_tag_item.blog_id = Last_insert_blog_id;
                        blog_tag_item.tag_id = tag_master_item.id;
                        db.Blog_Tag.Add(blog_tag_item);
                        db.SaveChanges();
                    }
                    else if (tag != "")    // new tag(not empty)  ... so add tag first and then save to blog_tag                    
                    {
                        Tag_master current_tm = new Tag_master();
                        current_tm.tag_name = tag;
                        db.Tag_master.Add(current_tm);
                        db.SaveChanges();

                        Blog_Tag blog_tag_item = new Blog_Tag();
                        blog_tag_item.blog_id = Last_insert_blog_id;
                        blog_tag_item.tag_id = current_tm.id;
                        db.Blog_Tag.Add(blog_tag_item);
                        db.SaveChanges();
                    } 
                } 
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
        [AllowAnonymous]
        public JsonResult get_all_tags()
        {
            List<Tag_master> all_tag_list = db.Tag_master.ToList();
            StringBuilder json_str = new StringBuilder();
            json_str.Append("[");
            int list_lenth = all_tag_list.Count;   //logic to find second last item
            int counter = 0;
            foreach (Tag_master item in all_tag_list)
            {                 
                json_str.Append("{\"Tag_Name\":\"" + item.tag_name + "\" ,\" id \":\"" + item.id + "");
                counter++;
                if (counter != (list_lenth))
                {
                    json_str.Append("\"},");
                }
                else
                {
                    json_str.Append("\"}");  // for last element,... no comma
                }
            }
            json_str.Append("]");

            var abc = json_str;
            return Json(json_str.ToString(),JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public PartialViewResult All_blog_Partial(string tag_content)
        {
            Tag_master this_tm = db.Tag_master.Where(i => i.tag_name.Equals(tag_content)).FirstOrDefault();  //get id of tag
            List<Blog_Tag> bt_list = db.Blog_Tag.Where(i => i.tag_id.Equals(this_tm.id)).ToList();   // get blog id from blog_tag

            List<Blog> blog_list = new List<Blog>();

            foreach (Blog_Tag item in bt_list)
            {
                Blog temp_bl = db.Blogs.Where(i=>i.Id.Equals(item.blog_id)).FirstOrDefault();
                blog_list.Add(temp_bl);
            }
            var pops = blog_list;
            return PartialView(blog_list.Where(i=>i.is_active == true).ToList());
        }          
    }
}