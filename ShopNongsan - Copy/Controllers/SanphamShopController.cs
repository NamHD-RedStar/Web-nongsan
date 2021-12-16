using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopNongsan.Models;
using PagedList;
namespace ShopNongsan.Controllers
{
    public class SanphamShopController : Controller
    {
        private ShopNongsanEntities db = new ShopNongsanEntities();

        // GET: SanphamShop
        public ActionResult Index()
        {
            // Rau
            List<Sanpham> rau = db.Sanphams.Where(s => s.id_danhmuc == 1).Take(8).ToList();
            ViewBag.rau = rau;
            //Thịt
            List<Sanpham> thit = db.Sanphams.Where(s => s.id_danhmuc == 2).Take(8).ToList();
            ViewBag.thit = thit;
            //hoaqua
            List<Sanpham> hoaqua = db.Sanphams.Where(s => s.id_danhmuc == 3).Take(8).ToList();
            ViewBag.hoaqua = hoaqua;
            //Đồ khô
            List<Sanpham> dokho = db.Sanphams.Where(s => s.id_danhmuc == 4).Take(8).ToList();
            ViewBag.dokho = dokho;
            //Nông sản khác
            List<Sanpham> nongsankhac = db.Sanphams.Where(s => s.id_danhmuc == 5).Take(8).ToList();
            ViewBag.nongsankhac = nongsankhac;
            // Lấy số lượng sản phẩm giỏ hàng
            int tong = 0;
            if (Session["nguoidung"] != null)
            {
                var nguoi = Session["nguoidung"] as ShopNongsan.Models.Nguoidung;
                var soluong_giohang = db.GiahangUsers.Where(s => s.id_nguoidung == nguoi.id_nguoidung).ToList();
                foreach (var item in soluong_giohang)
                {
                    int tien = (int)(item.soluong * item.giatien);
                    tong += tien;
                }

                ViewBag.soluong_giohang = soluong_giohang.Count();
                ViewBag.tongtien = tong.ToString("n0");
                Session["soluong_giohang"] = soluong_giohang.Count();
                Session["tongtien"] = tong.ToString("n0");

                //Nội dung tin nhắn
                var tinnhan = db.Nhantins.Where(s => s.id_khachhang == nguoi.id_nguoidung && s.id_admin == 18).ToList();
                ViewBag.tinnhan = tinnhan;
            }
            else
            {
                ViewBag.soluong_giohang = 0;
                ViewBag.tongtien = 0;
                Session["soluong_giohang"] = 0;
                Session["tongtien"] = 0;
            }

            //var sanphams = db.Sanphams.Include(s => s.DanhmucSanpham);
            var sanphams = db.Sanphams.Include(s => s.DanhmucSanpham).Take(10);
            return View(sanphams.ToList());
        }
        // Xem sản phẩm theo danh mục
        public ActionResult all_sanpham_danhmuc(int? id, int? page)
        {
            int pagesize = 16;
            int pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            List<Sanpham> all_sanpham;
            if (id == 0 || id == null)
            {
                all_sanpham = db.Sanphams.Include(s => s.DanhmucSanpham).ToList();
                Session["id_all_sp"] = 0;
            }
            else
            {
                all_sanpham = db.Sanphams.Where(s => s.id_danhmuc == id).ToList();
                Session["name_danhmuc_sp"] = db.DanhmucSanphams.Where(s => s.id_danhmuc == id).FirstOrDefault().tendanhmuc;
                Session["id_all_sp"] = id;
            }
            ViewBag.all_sanpham = all_sanpham.Count();
            ViewBag.all1 = all_sanpham.ToPagedList(1, pagesize);
            ViewBag.all2 = all_sanpham.ToPagedList(2, pagesize);
            ViewBag.all3 = all_sanpham.ToPagedList(3, pagesize);
            return View();
        }
        public ActionResult all_sanpham_giaban(int? id, int? page)
        {
          
            int pagesize = 16;
            int pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            List<Sanpham> all_sanpham;

            if (Session["id_all_sp"] == null)
            {
                Session["id_all_sp"] = 0;
            }
            int id_danhmucspp = Convert.ToInt32(Session["id_all_sp"]);
            if (id_danhmucspp == 0)
            {
                if (id == 0 || id == null)
                {
                    all_sanpham = db.Sanphams.Include(s => s.DanhmucSanpham).ToList();
                }
                else if (id == 1)
                {
                    all_sanpham = db.Sanphams.Where(s => s.giatien >= 1000 && s.giatien <= 50000).ToList();
                }
                else if (id == 2)
                {
                    all_sanpham = db.Sanphams.Where(s => s.giatien > 50000 && s.giatien <= 100000).ToList();
                }
                else
                {
                    all_sanpham = db.Sanphams.Where(s => s.giatien > 100000).ToList();
                }
            }
            else
            {
                if (id == 0 || id == null)
                {
                    all_sanpham = db.Sanphams.Include(s => s.DanhmucSanpham).ToList();
                }
                else if (id == 1)
                {
                    all_sanpham = db.Sanphams.Where(s => s.giatien >= 1000 && s.giatien <= 50000 && s.id_danhmuc == id_danhmucspp).ToList();
                }
                else if (id == 2)
                {
                    all_sanpham = db.Sanphams.Where(s => s.giatien > 50000 && s.giatien <= 100000 && s.id_danhmuc == id_danhmucspp).ToList();
                }
                else
                {
                    all_sanpham = db.Sanphams.Where(s => s.giatien > 100000 && s.id_danhmuc == id_danhmucspp).ToList();
                }
            }
            ViewBag.all_sanpham = all_sanpham.Count();
            ViewBag.all1 = all_sanpham.ToPagedList(1, pagesize);
            ViewBag.all2 = all_sanpham.ToPagedList(2, pagesize);
            ViewBag.all3 = all_sanpham.ToPagedList(3, pagesize);
            return View();
        }
        // Danh mục sản phẩm/ không cần người dùng đăng nhập
        public ActionResult Timkiem_sanpham(Sanpham sanpham, string timkiem, int? page)
        {
            int pagesize = 16;
            int pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Sanpham> sp = null;
            var sanphams = db.Sanphams.Include(s => s.DanhmucSanpham).ToList();
            if (!String.IsNullOrEmpty(timkiem))
            {
                sanphams = db.Sanphams.Where(s => s.tensanpham.Contains(timkiem)).ToList();
                Session["id_all_sp"] = sanphams[0].id_danhmuc;
            }
            ViewBag.soluong_sp = sanphams.Count();
            sp = sanphams.ToPagedList(pageindex, pagesize);
            ViewBag.Timkiem_sanpham = sp;
            ViewBag.seach_1 = sanphams.ToPagedList(1, pagesize);
            ViewBag.seach_2 = sanphams.ToPagedList(2, pagesize);
            ViewBag.seach_3 = sanphams.ToPagedList(3, pagesize);

            return View(sp);
        }


        //Tìm kiếm sản phẩm

        // GET: SanphamShop/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            // Kiểm tra trạng thái sản phẩm (Dừng bán - hết hàng)

            Sanpham sanpham = db.Sanphams.Where(s => s.id_sanpham == id).FirstOrDefault();
            Session["id_sp"] = sanpham.id_sanpham;
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            if (sanpham.soluong <= 0 || sanpham.trangthai == 0)//Nếu hết hàng || dừng bán
            {
                ViewBag.trangthai = 0;
            }
            else
            {
                ViewBag.trangthai = 1;
            }
            // Sản phẩm tương tự
            var Sanphamtuongtu = db.Sanphams.Where(s => s.id_danhmuc == sanpham.id_danhmuc).ToList();
            ViewBag.Sanphamtuongtu = Sanphamtuongtu;
            // Load đánh giá
            List<Danhgianguoidung> danhgia = db.Danhgianguoidungs.Where(s => s.id_sanpham == id).ToList();
            ViewBag.danhgia = danhgia.OrderByDescending(s=>s.id_danhgia);
          

            return View(sanpham);
        }
        // GET: SanphamShop/Create
        public ActionResult Create()
        {
            ViewBag.id_danhmuc = new SelectList(db.DanhmucSanphams, "id_danhmuc", "tendanhmuc");
            return View();
        }

        // POST: SanphamShop/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_sanpham,id_danhmuc,tensanpham,giatien,img_sanpham,list_img,mota,soluong,soluotmua,ngay_them,trangthai")] Sanpham sanpham)
        {

            if (ModelState.IsValid)
            {
                db.Sanphams.Add(sanpham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_danhmuc = new SelectList(db.DanhmucSanphams, "id_danhmuc", "tendanhmuc", sanpham.id_danhmuc);
            return View(sanpham);
        }

        // GET: SanphamShop/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanpham sanpham = db.Sanphams.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_danhmuc = new SelectList(db.DanhmucSanphams, "id_danhmuc", "tendanhmuc", sanpham.id_danhmuc);
            return View(sanpham);
        }

        // POST: SanphamShop/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_sanpham,id_danhmuc,tensanpham,giatien,img_sanpham,list_img,mota,soluong,soluotmua,ngay_them,trangthai")] Sanpham sanpham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanpham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_danhmuc = new SelectList(db.DanhmucSanphams, "id_danhmuc", "tendanhmuc", sanpham.id_danhmuc);
            return View(sanpham);
        }

        // GET: SanphamShop/Delete/5
        public ActionResult Delete(int? id)
        {
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
        }

        // POST: SanphamShop/Delete/5
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
