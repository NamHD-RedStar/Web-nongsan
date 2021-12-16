using ShopNongsan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopNongsan.Controllers
{
    public class ProfileController : Controller
    {
        private ShopNongsanEntities db = new ShopNongsanEntities();
        // GET: Profile
        public ActionResult Index()
        {
            if (Session["nguoidung"]!= null)
            {
                var nguoi = Session["nguoidung"] as Nguoidung;
                // Lấy lại thông tin người dùng
                var user = db.Nguoidungs.Where(s => s.id_nguoidung ==nguoi.id_nguoidung).FirstOrDefault();
                Session["nguoidung"] = user;
            }
            else
            {
                return RedirectToAction("Index", "Nguoidungs");
            }
            return View();
        }
    }
}