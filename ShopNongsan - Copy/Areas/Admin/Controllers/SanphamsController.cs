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
using PagedList;
namespace ShopNongsan.Areas.Admin.Controllers
{
    public class SanphamsController : Controller
    {
        private ShopNongsanEntities db = new ShopNongsanEntities();

        // GET: Admin/Sanphams

        public ActionResult Index(string timkiem,int? page)//Tìm kiếm
        {
            int pagesize = 6;
            int pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Sanpham> sp = null;

            var nguoi = Session["nguoidung"] as Nguoidung;
            if (Session["nguoidung"] != null && nguoi.quyen == "Admin")
            {
                // Nội dung
                var sanphams = db.Sanphams.Include(s => s.DanhmucSanpham).ToList();
                if (!String.IsNullOrEmpty(timkiem))
                {
                    sanphams = db.Sanphams.Where(s => s.tensanpham.Contains(timkiem)).ToList();
                }
                sp = sanphams.ToPagedList(pageindex, pagesize);
                return View(sp);
              //  return View(sanphams.ToList());
                //Kết thúc
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }
        }
        public ActionResult Color_bg( string id)
        {
            Session["Color"] = id;
            return RedirectToAction("Index", "Sanphams");
        }
     
        // GET: Admin/Sanphams/Details/5
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
                Sanpham sanpham = db.Sanphams.Find(id);
                if (sanpham == null)
                {
                    return HttpNotFound();
                }
                return View(sanpham);
                //Kết thúc
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }
        }

        // GET: Admin/Sanphams/Create
        public ActionResult Create()
        {
            var nguoi = Session["nguoidung"] as Nguoidung;
            if (Session["nguoidung"] != null && nguoi.quyen == "Admin")
            {
                // Nội dung
                ViewBag.id_danhmuc = new SelectList(db.DanhmucSanphams, "id_danhmuc", "tendanhmuc");
                return View();
                //Kết thúc
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }
        }
        // POST: Admin/Sanphams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_sanpham,id_danhmuc,tensanpham,giatien,img_sanpham,list_img,mota,soluong,soluotmua,ngay_them,trangthai")] Sanpham sanpham,HttpPostedFileBase img)
        {
            if (img != null)
            {
                string physicalPath = Server.MapPath("~/Images/Sanpham.jpg");
                img.SaveAs(physicalPath); // save image in folder
               sanpham.img_sanpham = ImageToBase64.Convert_base64("Images/Sanpham.jpg");
            }
            else
            {
                sanpham.img_sanpham = ImageToBase64.Convert_base64("Images/null.jpg");
              
              
            }
            sanpham.ngay_them = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Sanphams.Add(sanpham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_danhmuc = new SelectList(db.DanhmucSanphams, "id_danhmuc", "tendanhmuc", sanpham.id_danhmuc);
            return View(sanpham);
        }

        // GET: Admin/Sanphams/Edit/5
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
                Sanpham sanpham = db.Sanphams.Find(id);
                TempData["img_sanpham"] = sanpham.img_sanpham;
                TempData.Peek("img_sanpham");
                TempData.Keep("img_sanpham");

                if (sanpham == null)
                {
                    return HttpNotFound();
                }
                ViewBag.id_danhmuc = new SelectList(db.DanhmucSanphams, "id_danhmuc", "tendanhmuc", sanpham.id_danhmuc);
                return View(sanpham);
                //Kết thúc
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }
          
        }

        // POST: Admin/Sanphams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_sanpham,id_danhmuc,tensanpham,giatien,img_sanpham,list_img,mota,soluong,soluotmua,ngay_them,trangthai")] Sanpham sanpham,HttpPostedFileBase img)
        {
            TempData.Peek("img_sanpham");
            TempData.Keep("img_sanpham");
            if (img != null)
            {
                string physicalPath = Server.MapPath("~/Images/Sanpham.jpg");
                img.SaveAs(physicalPath); // save image in folder
                sanpham.img_sanpham = ImageToBase64.Convert_base64("Images/Sanpham.jpg");
            }
            else
            {
                sanpham.img_sanpham = TempData["img_sanpham"].ToString();
                db.Entry(sanpham).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    TempData["msg"] = "Update thành công";
                    return RedirectToAction("index");
                }
            }
            sanpham.ngay_them = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(sanpham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_danhmuc = new SelectList(db.DanhmucSanphams, "id_danhmuc", "tendanhmuc", sanpham.id_danhmuc);
            return View(sanpham);
        }

        // GET: Admin/Sanphams/Delete/5
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
                Sanpham sanpham = db.Sanphams.Find(id);
                if (sanpham == null)
                {
                    return HttpNotFound();
                }
                return View(sanpham);
                //Kết thúc
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }
          
        }

        // POST: Admin/Sanphams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sanpham sanpham = db.Sanphams.Find(id);
            db.Sanphams.Remove(sanpham);
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
