using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Microsoft.AspNet.SignalR;
using backOne.Hubs;

namespace Controllers
{
    public class FriendsController : Controller
    {
        // GET: Friends
        public ActionResult Index()
        {

            if (Session["email"] != null)
            {
                IEnumerable<user> friends = db.getFriends((String)Session["email"]);
                return View(friends);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Friends/addFriend
        [HttpPost]
        public ActionResult addFriend(String mail)
        {
            if( Session["email"] !=null){
                db.insertFriend((String)Session["email"], mail);
                user r = db.getUser((String)Session["email"], false);

                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
                hubContext.Clients.All.friendRequestNotification(r.FirstName + " " + r.LastName, mail);
                return Json(new { result = "success", url = Url.Action("Profile", "user", new { mail = mail }) });
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Friends/Requests
        public ActionResult Requests()
        {

            if (Session["email"] != null)
            {
                IEnumerable<user> requests = db.getRequests((String)Session["email"]);
                return View(requests);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Friends/AcceptRequest
        [HttpPost]
        public ActionResult AcceptRequest(String mail)
        {
            if (Session["email"] != null)
            {
                db.acceptRequest(mail, (String)Session["email"]);
                Session["requests"]=(int)Session["requests"]-1;
                user r = db.getUser((String)Session["email"], false);

                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
                hubContext.Clients.All.friendRequestAcceptedNotification(r.FirstName + " " + r.LastName, mail);
                return Json(new { result = "success", requests = (int)Session["requests"] });
            }
            return Json(new { result = "failure" });
        }

        // POST: Friends/RejectRequest
        [HttpPost]
        public ActionResult RejectRequest(String mail)
        {
            if (Session["email"] != null)
            {
                db.rejectRequest(mail, (String)Session["email"]);
                Session["requests"] = (int)Session["requests"] - 1;
                user r = db.getUser((String)Session["email"], false);

                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
                hubContext.Clients.All.friendRequestRejectedNotification(r.FirstName + " " + r.LastName, mail);
                return Json(new { result = "success", requests = (int)Session["requests"] });
            }
            return Json(new { result = "failure" });
        }
    }
}