using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopNongsan.Models;

namespace ShopNongsan.Areas.Admin.Controllers
{
    public class DonhangUsersController : Controller
    {
        private ShopNongsanEntities db = new ShopNongsanEntities();

        // GET: Admin/DonhangUsers
        public ActionResult Index()
        {
            var nguoi = Session["nguoidung"] as ShopNongsan.Models.Nguoidung;
            if (Session["nguoidung"] != null && nguoi.quyen == "Admin")
            {
                var donhangUsers = db.DonhangUsers.Include(d => d.Nguoidung);
                return View(donhangUsers.ToList());
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }
          
        }
      

        // GET: Admin/DonhangUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonhangUser donhangUser = db.DonhangUsers.Find(id);
            if (donhangUser == null)
            {
                return HttpNotFound();
            }
            return View(donhangUser);
        }

        // GET: Admin/DonhangUsers/Create
        public ActionResult Create()
        {
            ViewBag.id_nguoidung = new SelectList(db.Nguoidungs, "id_nguoidung", "ho");
            return View();
        }

        // POST: Admin/DonhangUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_donhang,madonhang,id_nguoidung,id_sanpham,ten_sanpham,giatien,soluong,ngaymua,ngaygiao,trangthai")] DonhangUser donhangUser)
        {
            if (ModelState.IsValid)
            {
                db.DonhangUsers.Add(donhangUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_nguoidung = new SelectList(db.Nguoidungs, "id_nguoidung", "ho", donhangUser.id_nguoidung);
            return View(donhangUser);
        }

        // GET: Admin/DonhangUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonhangUser donhangUser = db.DonhangUsers.Find(id);
            if (donhangUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_nguoidung = new SelectList(db.Nguoidungs, "id_nguoidung", "ho", donhangUser.id_nguoidung);
            return View(donhangUser);
        }

        // POST: Admin/DonhangUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_donhang,madonhang,id_nguoidung,id_sanpham,ten_sanpham,giatien,soluong,ngaymua,ngaygiao,trangthai")] DonhangUser donhangUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donhangUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_nguoidung = new SelectList(db.Nguoidungs, "id_nguoidung", "ho", donhangUser.id_nguoidung);
            return View(donhangUser);
        }

        // GET: Admin/DonhangUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonhangUser donhangUser = db.DonhangUsers.Find(id);
            if (donhangUser == null)
            {
                return HttpNotFound();
            }
            return View(donhangUser);
        }

        // POST: Admin/DonhangUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DonhangUser donhangUser = db.DonhangUsers.Find(id);
            db.DonhangUsers.Remove(donhangUser);
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
