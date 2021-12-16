using ShopNongsan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopNongsan.Controllers
{
    public class GiaodichController : Controller
    {
        // GET: Giaodich
        private ShopNongsanEntities db = new ShopNongsanEntities();
        public ActionResult Index()
        {
            if (Session["nguoidung"] != null)//Nếu người dùng đã đăng nhập
            {
                var nguoi = Session["nguoidung"] as ShopNongsan.Models.Nguoidung;
                //  var giahangUsers = db.GiahangUsers.Include(g => g.id_nguoidung ==nguoi.id_nguoidung);
                List<GiahangUser> Giohang_user = db.GiahangUsers.Where(s => s.id_nguoidung == nguoi.id_nguoidung).ToList();
                ViewBag.Giohang_user = Giohang_user;
                // Phí vận chuyển
               
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Nguoidungs");
            }
        }
        public ActionResult Muahang(DonhangUser donhangUser,Add_magiamgia_donhang add_Magiamgia_Donhang)
        {
            if (Session["nguoidung"] != null)//Nếu người dùng đã đăng nhập
            {
                var nguoi = Session["nguoidung"] as ShopNongsan.Models.Nguoidung;
                var giohang = db.GiahangUsers.Where(s => s.id_nguoidung == nguoi.id_nguoidung).ToList();
                int giamgia = 0;

                //Kiểm tra người dùng có tồn tại trong giỏ không
                if (giohang != null) // Nếu có, thì lấy hết sản phẩm giỏ hàng
                {
                    string _madonhang = "DH" + DateTime.Now.Minute.ToString() + DateTime.Now.Millisecond.ToString() + nguoi.id_nguoidung.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                    foreach (var item in giohang)
                    {

                        Sanpham sanpham = db.Sanphams.Find(item.id_sanpham);
                        donhangUser.madonhang = _madonhang;
                        donhangUser.id_nguoidung = item.id_nguoidung;
                        donhangUser.id_sanpham = item.id_sanpham;
                        donhangUser.ten_sanpham = item.tensanpham;
                        donhangUser.soluong = item.soluong;
                        donhangUser.giatien = item.giatien;
                        donhangUser.ngaymua = DateTime.Now;
                        donhangUser.ngaygiao = DateTime.Now.AddDays(+10);
                        donhangUser.trangthai =0;
                        sanpham.soluong = sanpham.soluong - donhangUser.soluong;
                        db.DonhangUsers.Add(donhangUser);//lưu đơn hàng

                        // Thêm mã giảm giá vào đon hàng
                        if (Session["magiamgia"] != null)
                        {
                            giamgia = Convert.ToInt32(Session["magiamgia"]);
                        }
                        add_Magiamgia_Donhang.madonhang = _madonhang;
                        add_Magiamgia_Donhang.giamgia = giamgia;
                        db.Add_magiamgia_donhang.Add(add_Magiamgia_Donhang);
                        Session["magiamgia"] = null;

                        if (ModelState.IsValid)
                        {
                            db.GiahangUsers.Remove(item);//Xóa sản phẩm trong giỏ sau khi mua
                            db.SaveChanges();
                        }
                      
                    }
                    return RedirectToAction("Index", "DonhangUser");

                }
             
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Nguoidungs");
            }
        }
    }
}