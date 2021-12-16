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

namespace ShopNongsan.Controllers
{
    public class GiahangUsersController : Controller
    {
        //public string Convert_base64(string path_img)
        //{
        //    string Pic_Path = Path.Combine(HttpRuntime.AppDomainAppPath, path_img);
        //    using (System.Drawing.Image image = System.Drawing.Image.FromFile(Pic_Path))
        //    {
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            string base64String;
        //            image.Save(ms, image.RawFormat);
        //            byte[] imageBytes = ms.ToArray();
        //            base64String = Convert.ToBase64String(imageBytes);
        //            return base64String;
        //        }
        //    }
        //}
        private ShopNongsanEntities db = new ShopNongsanEntities();
         
        // GET: GiahangUsers
        public ActionResult Index()
        {
            if (Session["nguoidung"] != null)//Nếu người dùng đã đăng nhập
            {
                var nguoi = Session["nguoidung"] as ShopNongsan.Models.Nguoidung;
                // Lấy lại thông tin người dùng
                var user = db.Nguoidungs.Where(s => s.gmail == nguoi.gmail && s.matkhau == nguoi.matkhau).FirstOrDefault();
                Session["nguoidung"] = user;
                //  var giahangUsers = db.GiahangUsers.Include(g => g.id_nguoidung ==nguoi.id_nguoidung);
                // xuất SẢN PHẨM của người dùng đăng nhập
                List<GiahangUser> Giohang_user = db.GiahangUsers.Where(s => s.id_nguoidung == nguoi.id_nguoidung).ToList();
                ViewBag.Giohang_user = Giohang_user;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Nguoidungs");
            }
        }
        //Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public ActionResult Themgiohang(GiahangUser giahangUser, FormCollection giohang, int soluong)
        {
            if (Session["nguoidung"] != null)//Nếu người dùng đã đăng nhập
            {
                //Lấy thông tin người dùng
                var nguoi = Session["nguoidung"] as Nguoidung;
                //id_giohang,id_nguoidung,id_sanpham,img_sanpham,tensanpham,giatien,soluong
                //int id_sanpham = Convert.ToInt32(Request.Form["id_sanpham"]);
                int id_sanpham = Convert.ToInt32(Session["id_sp"]);
              //Lấy thông tin sản phẩm
                Sanpham sanpham = db.Sanphams.Find(id_sanpham);
              
                var giahang = db.GiahangUsers.Where(s => s.id_sanpham == id_sanpham && s.id_nguoidung == nguoi.id_nguoidung).FirstOrDefault();
                if (giahang == null)//Nếu sản phẩm chưa tồn tại thì thêm mới
                {
                    giahangUser.id_nguoidung = nguoi.id_nguoidung;
                    giahangUser.id_sanpham = id_sanpham;
                    giahangUser.tensanpham = sanpham.tensanpham;
                    giahangUser.giatien = sanpham.giatien;
                    giahangUser.soluong = soluong;
                    giahangUser.img_sanpham = sanpham.img_sanpham;
                    // trừ số lượng mua
                 
                    db.GiahangUsers.Add(giahangUser);
                    if (ModelState.IsValid)
                    {
                        db.SaveChanges();
                        return RedirectToAction("Index", "GiahangUsers");
                    }
                }
                else //Nếu đã tồn tại thì cộng thêm số lượng
                {
                    var kiemtra_soluong = giahang.soluong + soluong;// kiểm tra số lượng trong giỏ hàng
                    if(kiemtra_soluong <= sanpham.soluong)
                    {
                        giahang.soluong = kiemtra_soluong;
                    }
                    else
                    {
                        Console.WriteLine("Đã đầy giỏ");
                    }
                 
                    if (ModelState.IsValid)
                    {
                        db.SaveChanges();
                        return RedirectToAction("Index", "GiahangUsers");
                    }
                }
                return View(giahangUser);
            }
            else //Nếu chưa đăng nhập
            {
                return RedirectToAction("Index", "Nguoidungs");
            }

        }
        public ActionResult Update_soluong_giohang(int id,int soluong)
        {
            // Lấy sp trong giỏ hàng với id giỏ hàng
            var sp_giohang = db.GiahangUsers.Where(s => s.id_giohang == id).FirstOrDefault();
            // tìm sản phẩm để kiểm tra số lượng
            var sp = db.Sanphams.Where(s => s.id_sanpham == sp_giohang.id_sanpham).FirstOrDefault();
            // Nếu số lượng nhập vào < số lượng hiện có của sp
            if(sp_giohang.soluong <= sp.soluong)
            {
                sp_giohang.soluong = soluong;
                db.SaveChanges();
            }
            else
            {
                Console.WriteLine("Đã đầy giỏ");
            }
          
            return RedirectToAction("Index", "GiahangUsers");
           
        }
        // Xóa san phẩm trong giỏ hàng
        [HttpPost]
        public ActionResult Deletegiohang(int id)
        {
            var nguoi = Session["nguoidung"] as ShopNongsan.Models.Nguoidung;
            var giahangUser = db.GiahangUsers.Where(s=>s.id_nguoidung==nguoi.id_nguoidung && s.id_giohang ==id).First();
            db.GiahangUsers.Remove(giahangUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Xuly_magiamgia(string ma)
        {
            var timma = db.Magiamgias.Where(s=>s.magiamgia1 == ma).FirstOrDefault();
            if(timma!= null)
            {
                if(timma.soluong == 0)
                {
                    Session["magiamgia"] = 0;
                }
                else
                {
                    Session["magiamgia"] = timma.dongia;
                    timma.soluong -= 1;
                    db.SaveChanges();
                }
            }
            // Giảm số lượng đơn hàng

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
