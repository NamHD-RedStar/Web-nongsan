using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopNongsan.Models;
using ShopNongsan.Areas.Admin.Convert_image;
namespace ShopNongsan.Areas.Admin.Controllers
{
    public class DanhmucSanphamsController : Controller
    {
      
        private ShopNongsanEntities db = new ShopNongsanEntities();

        // GET: Admin/DanhmucSanphams
        public ActionResult Index()
        {
            var nguoi = Session["nguoidung"] as Nguoidung;
            if (Session["nguoidung"] != null && nguoi.quyen == "Admin")
            {
                // Nội dung
                return View(db.DanhmucSanphams.ToList());
                //Kết thúc
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }  
        }

        // GET: Admin/DanhmucSanphams/Details/5
        public ActionResult Details(int? id)
        {
            var nguoi = Session["nguoidung"] as Nguoidung;
            if (Session["nguoidung"] != null && nguoi.quyen == "Admin")
            {
                // Nội dung
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DanhmucSanpham danhmucSanpham = db.DanhmucSanphams.Find(id);
                if (danhmucSanpham == null)
                {
                    return HttpNotFound();
                }
                return View(danhmucSanpham);
                //Kết thúc
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }



        }

        // GET: Admin/DanhmucSanphams/Create
        public ActionResult Create()
        {
            var nguoi = Session["nguoidung"] as Nguoidung;
            if (Session["nguoidung"] != null && nguoi.quyen == "Admin")
            {
                // Nội dung
                return View();
                //Kết thúc
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }
          
        }

        // POST: Admin/DanhmucSanphams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_danhmuc,tendanhmuc,img_danhmuc")] DanhmucSanpham danhmucSanpham,HttpPostedFileBase img)
        {
            if (img != null)
            {
                string physicalPath = Server.MapPath("~/Images/Danhmuc.jpg");
                img.SaveAs(physicalPath); // save image in folder
              danhmucSanpham.img_danhmuc = ImageToBase64.Convert_base64("Images/Danhmuc.jpg");
            }
            else
            {
                danhmucSanpham.img_danhmuc = ImageToBase64.Convert_base64("Images/null.jpg");// save database to base64
            }
            if (ModelState.IsValid)
            {
                db.DanhmucSanphams.Add(danhmucSanpham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(danhmucSanpham);
        }

        // GET: Admin/DanhmucSanphams/Edit/5
        public ActionResult Edit(int? id)
        {
            var nguoi = Session["nguoidung"] as Nguoidung;
            if (Session["nguoidung"] != null && nguoi.quyen == "Admin")
            {
                // Nội dung
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DanhmucSanpham danhmucSanpham = db.DanhmucSanphams.Find(id);
                TempData["img_danhmuc"] = danhmucSanpham.img_danhmuc;
                TempData.Peek("img_danhmuc");
                TempData.Keep("img_danhmuc");
                if (danhmucSanpham == null)
                {
                    return HttpNotFound();
                }
                return View(danhmucSanpham);
                //Kết thúc
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }


           
        }

        // POST: Admin/DanhmucSanphams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_danhmuc,tendanhmuc,img_danhmuc")] DanhmucSanpham danhmucSanpham,HttpPostedFileBase img)
        {
            TempData.Peek("img_danhmuc");
            TempData.Keep("img_danhmuc");
            if (img != null)
            {
                string physicalPath = Server.MapPath("~/Images/Danhmuc.jpg");
                img.SaveAs(physicalPath); // save image in folder
                danhmucSanpham.img_danhmuc = ImageToBase64.Convert_base64("Images/Danhmuc.jpg");
            }
            else
            {
             
                danhmucSanpham.img_danhmuc = TempData["img_danhmuc"].ToString();
                db.Entry(danhmucSanpham).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    TempData["msg"] = "Update thành công";
                    return RedirectToAction("index");
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(danhmucSanpham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danhmucSanpham);
        }

        // GET: Admin/DanhmucSanphams/Delete/5
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
                DanhmucSanpham danhmucSanpham = db.DanhmucSanphams.Find(id);
                if (danhmucSanpham == null)
                {
                    return HttpNotFound();
                }
                return View(danhmucSanpham);
                //Kết thúc
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }

           
        }

        // POST: Admin/DanhmucSanphams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DanhmucSanpham danhmucSanpham = db.DanhmucSanphams.Find(id);
            db.DanhmucSanphams.Remove(danhmucSanpham);
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
