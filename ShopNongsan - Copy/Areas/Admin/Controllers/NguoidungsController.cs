using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopNongsan.Models;
using System.IO;
using ShopNongsan.Areas.Admin.Convert_image;

namespace ShopNongsan.Areas.Admin.Controllers
{
    public class NguoidungsController : Controller
    {
        private ShopNongsanEntities db = new ShopNongsanEntities();

        // GET: Admin/Nguoidungs
        public ActionResult Index(string timkiem)
        {
            var nguoi = Session["nguoidung"] as ShopNongsan.Models.Nguoidung;
            if (Session["nguoidung"] != null && nguoi.quyen == "Admin")
            {//Lấy thông tin người dùng
             //int id_nd = Convert.ToInt32(nguoi.id_nguoidung);
             // Nguoidung _nd = db.Nguoidungs.Where(s => s.id_nguoidung == nguoi.id_nguoidung).FirstOrDefault();
                TempData["nd_ten"] = nguoi.ho + " " + nguoi.ten;//_nd.ho+" "+ _nd.ten;
                TempData["nd_Img"] = nguoi.anh_daidien;//Session["User_Img"];
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
                // TempData["nd_Img"] = "< img src = '~/Images/null.jpg' class='img-profile rounded-circle' />";
            }

            var nguoidung = from l in db.Nguoidungs
                            select l;
            if (!String.IsNullOrEmpty(timkiem))
            {
                nguoidung = db.Nguoidungs.Where(s => s.ten.Contains(timkiem));
            }

            // Hiển thị thông báo có sản phẩm chốt đơn mới
            var donhang_chuachot = db.DonhangUsers.Where(s => s.trangthai == 0).ToList();
            var counts = (from c in db.DonhangUsers
                          where c.id_nguoidung != nguoi.id_nguoidung && c.trangthai == 0
                          group c by new { c.madonhang } into grouping
                          select new
                          {
                              _soluong = grouping.Count(),
                          });
            Session["donhang_chuachot"] = counts.Count();
          

            return View(nguoidung.ToList());
        }
        // Xủ lý người dùng quên mật khẩu
        public ActionResult Quenmatkhau(int?id)
        {
        
            if (Session["nguoidung"] != null)
            {
                var nguoi = db.Nguoidungs.Where(s => s.id_nguoidung == id).FirstOrDefault();
                nguoi.matkhau = "123456";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }

        }
        // Trả lời tin nhắn
        public ActionResult Hienthitinnhan_nguoidung(int id)
        {
            if (Session["nguoidung"] != null)
            {
                // Lấy toàn bộ tin nhắn với người dùng có id
                var tinnhan = db.Nhantins.Where(s => s.id_khachhang == id && s.id_admin == 18).ToList();
                ViewBag.tinnhan = tinnhan;
                ViewBag.id_nguoigui = id;
                return View(tinnhan);
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }
        } // Trả lời tin nhắn
        public ActionResult Traloitinnhan_nguoidung(string id, string noidungtin, Nhantin nhantin)
        {
            // Lấy toàn bộ tin nhắn với người dùng có id
            if (Session["nguoidung"] != null)
            {
              
                var idd = Convert.ToInt32(id);
                var tinnhan = db.Nhantins.Where(s => s.id_khachhang == idd  && s.id_admin == 18).ToList();
                if (noidungtin == null)
                {
                    return RedirectToAction("Hienthitinnhan_nguoidung/" + idd, "Nguoidungs");
                }
                else
                {
                    nhantin.id_khachhang = idd;
                    nhantin.id_admin = 18;
                    nhantin.noidungnhan = noidungtin;
                    nhantin.thoigiannhan = DateTime.Now;
                    nhantin.trangthai = 1;// Khách:0 - admin:1
                    db.Nhantins.Add(nhantin);
                    db.SaveChanges();
                    return RedirectToAction("Hienthitinnhan_nguoidung/" + idd, "Nguoidungs");
                }
              
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }
        }

        // GET: Admin/Nguoidungs/Details/5
        public ActionResult Details(int? id)
        {
            var nguoi = Session["nguoidung"] as Nguoidung;
            if (Session["nguoidung"] != null && nguoi.quyen == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Nguoidung nguoidung = db.Nguoidungs.Find(id);
                if (nguoidung == null)
                {
                    return HttpNotFound();
                }
                return View(nguoidung);
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }

          
        }

        // GET: Admin/Nguoidungs/Create
        public ActionResult Create()
        {
            var nguoi = Session["nguoidung"] as Nguoidung;
            if (Session["nguoidung"] != null && nguoi.quyen == "Admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }
         
        }

        // POST: Admin/Nguoidungs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_nguoidung,ho,ten,anh_daidien,diachi,sdt,gmail,matkhau,ngaydangky,quyen")] Nguoidung nguoidung, HttpPostedFileBase img)
        {
            if (img != null)
            {
                string physicalPath = Server.MapPath("~/Images/Nguoidung.jpg");
                img.SaveAs(physicalPath); // save image in folder
                nguoidung.anh_daidien = ImageToBase64.Convert_base64("Images/Nguoidung.jpg");
            }
            else
            {

                nguoidung.anh_daidien = ImageToBase64.Convert_base64("Images/null.jpg");// save database to base64
            }
            nguoidung.ngaydangky = DateTime.Now;
            nguoidung.quyen = "User";
            if (ModelState.IsValid)
            {
                db.Nguoidungs.Add(nguoidung);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nguoidung);
        }

        // GET: Admin/Nguoidungs/Edit/5
        public ActionResult Edit(int? id)
        {
            var nguoi = Session["nguoidung"] as Nguoidung;
            if (Session["nguoidung"] != null && nguoi.quyen == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Nguoidung nguoidung = db.Nguoidungs.Find(id);
                TempData["img_nguoidung"] = nguoidung.anh_daidien;
                TempData.Peek("img_nguoidung");
                TempData.Keep("img_nguoidung");
                if (nguoidung == null)
                {
                    return HttpNotFound();
                }
                return View(nguoidung);
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }

           
        }

        // POST: Admin/Nguoidungs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_nguoidung,ho,ten,anh_daidien,diachi,sdt,gmail,matkhau,ngaydangky,quyen")] Nguoidung nguoidung, HttpPostedFileBase img)
        {
            TempData.Peek("img_nguoidung");
            TempData.Keep("img_nguoidung");
            if (img != null)
            {
                string physicalPath = Server.MapPath("~/Images/Nguoidung.jpg");
                img.SaveAs(physicalPath); // save image in folder
                nguoidung.anh_daidien = ImageToBase64.Convert_base64("Images/Nguoidung.jpg");
            }
            else
            {
                nguoidung.anh_daidien = TempData["img_nguoidung"].ToString();
                db.Entry(nguoidung).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    TempData["msg"] = "Update thành công";
                    return RedirectToAction("index");
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(nguoidung).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nguoidung);
        }

        // GET: Admin/Nguoidungs/Delete/5
        public ActionResult Delete(int? id)
        {

            var nguoi = Session["nguoidung"] as Nguoidung;
            if (Session["nguoidung"] != null && nguoi.quyen == "Admin")
            {
                // Nội dung
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Nguoidung nguoidung = db.Nguoidungs.Find(id);
                if (nguoidung == null)
                {
                    return HttpNotFound();
                }
                return View(nguoidung);
                //Kết thúc
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }
          
        }

        // POST: Admin/Nguoidungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nguoidung nguoidung = db.Nguoidungs.Find(id);
            db.Nguoidungs.Remove(nguoidung);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
