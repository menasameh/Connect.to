using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Microsoft.Owin.Security;
using Microsoft.AspNet.SignalR;
using backOne.Hubs;


namespace Controllers
{
    public class UserController : Controller
    {
        // GET: User/Login
        public ActionResult Login()
        {
            if (Session["email"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        
        // POST: User/Login
        [HttpPost]
        public ActionResult Login(user u)
        {
            if (db.validate_user(u))
            {
                Session.Timeout = 60*24*30; //month in minute
                Session["email"] = u.Email;
                Session["ppsrc"] = db.getUserImage(u.Email);
                Session["requests"] = db.getRequestsCount(u.Email);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.msg = "login error";
            return View(u);
        }

        // POST: User/calcRequests
        [HttpPost]
        public ActionResult calcRequests()
        {
            if (Session["email"] != null)
            {
                Session["requests"] = db.getRequestsCount((String)Session["email"]);
                return Json(new { result = "success", count = Session["requests"] });
            }
            return null;
        }

        // POST: User/Logout
        [HttpPost]
        public ActionResult Logout()
        {
            Session.Abandon();
            //Session["email"] = null;
            //Session["ppsrc"] = null;
            return Json(new { result = "Redirect", url = Url.Action("Index", "Home") });
        }


        // GET: User/Register
        public ActionResult Register()
        {
            //check same email registered before
            if (Session["email"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        // POST: User/Register
        [HttpPost]
        public ActionResult Register(user u)
        {
            if (db.check_mail(u.Email))
            {
                if (db.insertUser(u))
                {
                    return RedirectToAction("Login", "User");
                }
            }
            else
            {
                ViewBag.msg = "this email is already registered";
            }
           
            return View(u);
        }


        // GET: User/Profile/{mail}
        [ActionName("Profile")]
        public ActionResult User_Profile(string mail)
        {
            if (Session["email"] != null)
            {
                bool friends = db.isFriend((string)Session["email"], mail);
                ViewBag.owner = (string)Session["email"] ==  mail;
                user u = db.getUser(mail, friends || (bool)ViewBag.owner);
                if(u == null){
                    ViewBag.founded = false;
                }
                else
                {
                    ViewBag.friend = friends;
                    ViewBag.friendRequestSent = db.isWaiting((string)Session["email"], mail);
                    u.posts = db.getProfilePosts(mail, friends || ViewBag.owner);
                    foreach (post p in u.posts)
                    {
                        p.PosterImagePath = db.getUserImage(p.PosterMail);
                    }
                    u.likes = db.getLikesOfUser((string)Session["email"]);
                    ViewBag.founded = true;
                }
                return View(u);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: User/EditProfile?mail={mail}
        [ActionName("EditProfile")]
        public ActionResult Edit_User_Profile()
        {
            if (Session["email"] != null)
            {
                return View(db.getUser((String)Session["email"], true));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: User/EditProfile
        [ActionName("EditProfile")]
        [HttpPost]
        public ActionResult Edited_User_Profile(user u)
        {
            if (Session["email"] != null)
            {
                if (db.check_mail(u.Email))
                {
                    db.UpdateUser(u, (String)Session["email"]);
                    return View(u);
                }
                else
                {
                    ViewBag.msg = "this email is already registered";
                }
                
                return View(u);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: User/ChangePassword
        [ActionName("ChangePassword")]
        [HttpPost]
        public ActionResult changeUserPassword(String oldPass, String newPass)
        {
            if (Session["email"] != null)
            {
                db.ChangePassword((String)Session["email"], oldPass, newPass);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }






        // POST: User/Resetpp
        [ActionName("Resetpp")]
        [HttpPost]
        public ActionResult RestUserProfilePicture()
        {
            if (Session["email"] != null)
            {
                db.ResetProfilePicture((String)Session["email"]);
                Session["ppsrc"] = db.getUserImage((String)Session["email"]);
                user u = db.getUser((String)Session["email"], false);
                post p = new post();
                p.PosterMail = (String)Session["email"];
                p.Caption = "User " + u.FirstName + " " + u.LastName + " changed their profile picture";
                p.IsPublic = false;
                p.ImagePath = ((String)Session["ppsrc"]).Substring(0, 1);
                db.insertPost(p);
                return Json(new { result = "Redirect", url = Url.Action("Index", "Home") });
                //return RedirectToAction("Profile", "User");
            }
            return RedirectToAction("Index", "Home");
        }


        // POST: User/Like
        [ActionName("Like")]
        [HttpPost]
        public ActionResult like(int postId)
        {
            if (Session["email"] != null)
            {
                db.insertLike((String)Session["email"], postId);
                user r = db.getUser((String)Session["email"],false);
                String mail = db.getUserMail(postId);

                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
                hubContext.Clients.All.likeNotifaication(r.FirstName+" "+r.LastName, mail);
                return Json(new { result = "success" });
                //return RedirectToAction("Profile", "User");
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: User/Likes
        [ActionName("Likes")]
        [HttpPost]
        public ActionResult likes(int postId)
        {
            if (Session["email"] != null)
            {
                LinkedList<user> res = db.getPostLikes(postId);

                return Json(new { result = "success", likes = res });
                //return RedirectToAction("Profile", "User");
            }
            return RedirectToAction("Index", "Home");
        }



    }
}