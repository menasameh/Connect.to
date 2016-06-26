using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using MySql.Data.MySqlClient;

namespace Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           if(Session["email"] != null){
               IEnumerable<post> timeline = db.getAllPosts((String)Session["email"]);
               foreach (post p in timeline){
                   p.PosterImagePath = db.getUserImage(p.PosterMail);
               }
               ViewBag.likes = db.getLikesOfUser((string)Session["email"]);
               return View(timeline);
           }
            return View();
        }
        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //POST :Home/SearchGeneral
        [HttpPost]
        [ActionName("SearchGeneral")]
        public ActionResult Search(int type, String text)
        {
            if (Session["email"] != null)
            {
                IEnumerable<user> searchRes = new LinkedList<user>();
                switch(type){
                    case 1:
                        searchRes = db.SearchMail(text);
                        break;
                    case 2:
                        searchRes = db.SearchPhone(text);
                        break;
                    case 3:
                        searchRes = db.SearchHome(text);
                        break;
                    case 4:
                        searchRes = db.SearchCaption(text);
                        break;
                }
                return View("Search", searchRes);
            }
            return RedirectToAction("Index", "Home");
        }

        //POST :Home/SearchName
        [HttpPost]
        [ActionName("SearchName")]
        public ActionResult Search(String first, String last)
        {
            if (Session["email"] != null)
            {
                IEnumerable<user> searchRes = db.SearchName(first, last);
                return View("Search", searchRes);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}