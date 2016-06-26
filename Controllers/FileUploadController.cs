using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;

namespace Controllers
{
    public class FileUploadController : Controller
    {

        //POST: FileUpload/upload
        public ActionResult upload(HttpPostedFileBase file)
        {
            String mail = (String)Session["email"];
            if (file != null && mail != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                int id = db.insertImage(pic);
                if (id == -1)
                {
                    return RedirectToAction("Index", "Home");
                }
                // file is uploaded
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/Content/images/"), id + "-" + pic);
                file.SaveAs(path);
                Session["ppsrc"] = id + "-" + pic;
                db.updateUserImage(id, mail);

                user u = db.getUser((String)Session["email"], false);
                post p = new post();
                p.PosterMail = (String)Session["email"];
                p.Caption = "User " + u.FirstName + " " + u.LastName + " changed their profile picture";
                p.IsPublic = false;
                p.ImagePath = id.ToString() ;
                db.insertPost(p);
                //save the user new image
                return RedirectToAction("EditProfile", "User");
            }

            return RedirectToAction("Index", "Home");
        }

        //POST: FileUpload/upload
        public ActionResult post(HttpPostedFileBase file, post p)
        {
            String mail = (String)Session["email"];
            if (mail != null)
            {
                if (file != null) { 
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    int id = db.insertImage(pic);
                    if (id == -1)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    // file is uploaded
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/Content/images/"), id + "-" + pic);
                    file.SaveAs(path);
                    p.ImagePath = id.ToString();
                }
                else
                {
                    p.ImagePath = "1";
                }
                p.PosterMail = mail;

                db.insertPost(p);
                return RedirectToAction("Profile", "User", new { mail = mail});
            }

            return RedirectToAction("Index", "Home");
        }
    }
}