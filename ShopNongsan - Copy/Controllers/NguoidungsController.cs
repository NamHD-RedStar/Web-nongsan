using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopNongsan.Areas.Admin.Convert_image;
using ShopNongsan.Models;

namespace ShopNongsan.Controllers
{
    public class NguoidungsController : Controller
    {
        private ShopNongsanEntities db = new ShopNongsanEntities();

        // GET: Nguoidungs
        public ActionResult Index()
        {
            return View(db.Nguoidungs.ToList());
        }
        // Đâng nhập
        public ActionResult Login(string gmail, string matkhau)
        {
            var user = db.Nguoidungs.Where(s => s.gmail == gmail && s.matkhau == matkhau).FirstOrDefault();
            if (user != null)
            {
                //Session["User_Gmail"] = user.gmail;
                Session["nguoidung"] = user;
                Session["User_Id"] = user.id_nguoidung;
                Session["User_Ten"] = user.ho + " " + user.ten + " ";
                Session["User_Img"] = user.anh_daidien;
                //return RedirectToAction("Index", "Nguoidungs", new { area = "Admin" });
                return RedirectToAction("Index", "SanphamShop");
            }
            else
            {
                TempData["Login_thatbai"] = "Tài khoản hoặc mật khẩu không chính xác";
                return RedirectToAction("Index");
            }

        }
        //Đăng xuất
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "SanphamShop");
        }
        //Đăng ký
        public ActionResult SignUp(Nguoidung nguoidung, string matkhau, string ho, string ten, string sdt, string gmail)
        {
            var kiemtra = db.Nguoidungs.Where(s => s.gmail == gmail).FirstOrDefault();
            //Nếu gmail chưa tồn tại thì thêm người dùng
            if (kiemtra == null)
            {
                nguoidung.ho = ho;
                nguoidung.ten = ten;
                nguoidung.sdt = sdt;
                nguoidung.gmail = gmail;
                nguoidung.matkhau = matkhau;
                nguoidung.quyen = "User";
                nguoidung.ngaydangky = DateTime.Now;
                nguoidung.anh_daidien = ImageToBase64.Convert_base64("Images/null.jpg");
                db.Nguoidungs.Add(nguoidung);
                if (ModelState.IsValid)
                {
                    db.SaveChanges();
                    TempData["LoginSuccess"] = "Đăng ký thành công vui lòng đăng nhập!";
                    return RedirectToAction("Index");
                }
                return ViewBag["ten"] = ten;
            }
            //Nếu gmail đã tồn tại
            else
            {

                TempData["LoginSuccess"] = "Đăng ký thất bại, Gmail đã được đăng ký";
                return RedirectToAction("Index");

            }
        }
        // Thêm đánh giá
        public ActionResult Danhgia(int id, string text_danhgia, Danhgianguoidung danhgianguoidung)
        {
            if (Session["nguoidung"] != null)
            {
                var nguoi = Session["nguoidung"] as Nguoidung;
                danhgianguoidung.id_nguoidung = nguoi.id_nguoidung;
                danhgianguoidung.id_sanpham = id;
                danhgianguoidung.noidung = text_danhgia;
                danhgianguoidung.sao_danhgia = 4;
                db.Danhgianguoidungs.Add(danhgianguoidung);
                db.SaveChanges();
                return RedirectToAction("Details/" + id, "SanphamShop");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Suabinhluan( int? id,string noidung)
        {
            var binhluan = db.Danhgianguoidungs.Where(s => s.id_danhgia == id).FirstOrDefault();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                binhluan.noidung = noidung;
                db.SaveChanges();
                return RedirectToAction("Details/" + binhluan.id_sanpham, "SanphamShop");
            }
        }
        public ActionResult Thaydoithongtin_User(Nguoidung nguoidung, string ten, string ho, string gmail, string sdt, string diachi, HttpPostedFileBase avata)
        {
            if (Session["nguoidung"] != null)
            {
                var nguoi = Session["nguoidung"] as Nguoidung;
                var thaydoi = db.Nguoidungs.Where(s => s.id_nguoidung == nguoi.id_nguoidung).FirstOrDefault();
                TempData["img_nguoidung"] = thaydoi.anh_daidien;
                TempData.Peek("img_nguoidung");
                TempData.Keep("img_nguoidung");
                // Thay đổi ảnh
                if (avata != null)
                {
                    string physicalPath = Server.MapPath("~/Images/Nguoidung.jpg");
                    avata.SaveAs(physicalPath); // save image in folder
                    thaydoi.anh_daidien = ImageToBase64.Convert_base64("Images/Nguoidung.jpg");
                }
                else
                {
                    thaydoi.anh_daidien = TempData["img_nguoidung"].ToString();
                }

                thaydoi.ten = ten;
                thaydoi.ho = ho;
                thaydoi.gmail = gmail;
                thaydoi.sdt = sdt;
                thaydoi.diachi = diachi;
                db.SaveChanges();


                return RedirectToAction("Index", "Profile");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        // Thay đổi mật khẩu
        public ActionResult Thaydoimatkhau_User(Nguoidung nguoidung, string matkhau_cu, string matkhau_moi, string nhaplai_matkhau)
        {
            if (Session["nguoidung"] != null)
            {
                var nguoi = Session["nguoidung"] as Nguoidung;
                var thaydoi = db.Nguoidungs.Where(s => s.id_nguoidung == nguoi.id_nguoidung).FirstOrDefault();
                if (matkhau_moi == nhaplai_matkhau) //nếu mật khẩu mới và nhập lại giống nhau thì tiếp tục
                {
                    if (matkhau_cu == thaydoi.matkhau)
                    {
                        thaydoi.matkhau = matkhau_moi;
                        TempData["thongbao"] = "Thay đổi thành công";
                    }
                    else TempData["thongbao"] = "Sai mật khẩu";
                }
                else// nếu không giống thì thông báo
                {
                    TempData["thongbao"] = "Mật khẩu không khớp";
                }
                db.SaveChanges();


                return RedirectToAction("Index", "Profile", TempData["thongbao"]);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        // Sử lý khách hàng nhắn tin
        public ActionResult Nhantin_khachhang(string noidung, Nhantin nhantin)
        {
            if (Session["nguoidung"] != null)
            {

                var nguoi = Session["nguoidung"] as Nguoidung;
                nhantin.id_khachhang = nguoi.id_nguoidung;
                nhantin.id_admin = 18;
                nhantin.noidungnhan = noidung;
                nhantin.thoigiannhan = DateTime.Now;
                nhantin.trangthai = 0;// Khách:0 - admin:1
                db.Nhantins.Add(nhantin);
                db.SaveChanges();
                return RedirectToAction("Index", "SanphamShop");
            }
            else
            {
                return RedirectToAction("Index");
            }

        }










        // GET: Nguoidungs/Details/5
        public ActionResult Details(int? id)
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

        //// GET: Nguoidungs/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Nguoidungs/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id_nguoidung,ho,ten,anh_daidien,diachi,sdt,gmail,matkhau,ngaydangky,quyen")] Nguoidung nguoidung)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Nguoidungs.Add(nguoidung);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(nguoidung);
        //}
        // GET: Nguoidungs/Edit/5
        public ActionResult Edit(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Nguoidung nguoidung, string diachi)
        {
            if (Session["nguoidung"] != null)
            {
                var nguoi = Session["nguoidung"] as ShopNongsan.Models.Nguoidung;
                var cust = db.Nguoidungs.Where(s => s.id_nguoidung == nguoi.id_nguoidung).First();
                cust.diachi = diachi;

                if (ModelState.IsValid)
                {
                    //db.Entry(nguoidung).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "GiahangUsers");
                }
                return View(nguoidung);
            }
            else
            {
                return RedirectToAction("Index", "Nguoidungs");
            }

        }

        // GET: Nguoidungs/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Nguoidungs/Delete/5
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
