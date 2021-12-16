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
    public class MagiamgiasController : Controller
    {
        private ShopNongsanEntities db = new ShopNongsanEntities();

        // GET: Admin/Magiamgias
        public ActionResult Index()
        {
            var nguoi = Session["nguoidung"] as ShopNongsan.Models.Nguoidung;
            if (Session["nguoidung"] != null && nguoi.quyen == "Admin")
            {
                return View(db.Magiamgias.ToList());
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }
          
        }

        // GET: Admin/Magiamgias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Magiamgia magiamgia = db.Magiamgias.Find(id);
            if (magiamgia == null)
            {
                return HttpNotFound();
            }
            return View(magiamgia);
        }

        // GET: Admin/Magiamgias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Magiamgias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_magiamgia,magiamgia1,dongia,tenmagiamgia,soluong,trangthai")] Magiamgia magiamgia)
        {
            if (ModelState.IsValid)
            {
                db.Magiamgias.Add(magiamgia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(magiamgia);
        }

        // GET: Admin/Magiamgias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Magiamgia magiamgia = db.Magiamgias.Find(id);
            if (magiamgia == null)
            {
                return HttpNotFound();
            }
            return View(magiamgia);
        }

        // POST: Admin/Magiamgias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_magiamgia,magiamgia1,dongia,tenmagiamgia,soluong,trangthai")] Magiamgia magiamgia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(magiamgia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(magiamgia);
        }

        // GET: Admin/Magiamgias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Magiamgia magiamgia = db.Magiamgias.Find(id);
            if (magiamgia == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.Magiamgias.Remove(magiamgia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
         
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
