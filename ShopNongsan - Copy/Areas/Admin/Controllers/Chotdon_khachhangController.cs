using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopNongsan.Models;
namespace ShopNongsan.Areas.Admin.Controllers
{
    public class Chotdon_khachhangController : Controller
    {
        private ShopNongsanEntities db = new ShopNongsanEntities();
        // GET: Admin/Chotdon_khachhang
        public List<Donhang> donhangss(int trangthai)
        {
            var nguoi = Session["nguoidung"] as ShopNongsan.Models.Nguoidung;
            var noofRecords = (from c in db.DonhangUsers
                               where c.id_nguoidung != nguoi.id_nguoidung && c.trangthai ==trangthai
                               group c by new { c.madonhang } into grouping
                               select new Donhang()
                               {
                                   _madonhang = grouping.Key.madonhang,
                                   _soluong = grouping.Count(),
                                   _ngaymua = (DateTime)db.DonhangUsers.Where(s => s.madonhang == grouping.Key.madonhang).Select(s => s.ngaymua).FirstOrDefault(),
                                   _ngaygiao = (DateTime)db.DonhangUsers.Where(s => s.madonhang == grouping.Key.madonhang).Select(s => s.ngaygiao).FirstOrDefault(),
                                   _thanhtien = (int)db.DonhangUsers.Where(s => s.madonhang == grouping.Key.madonhang).Sum(s => (s.giatien * s.soluong)),
                                   _trangthai = (int)db.DonhangUsers.Where(s => s.madonhang == grouping.Key.madonhang).Select(s => s.trangthai).FirstOrDefault(),
                                   _tensp = db.DonhangUsers.Where(s => s.madonhang == grouping.Key.madonhang).Select(s => s.ten_sanpham).FirstOrDefault(),
                               });
          
            Session["donhang_chuachot"] = noofRecords.Count();
            return noofRecords.ToList();
        }
        public ActionResult Index()
        {
            var nguoi = Session["nguoidung"] as Nguoidung;
            if (Session["nguoidung"] != null && nguoi.quyen == "Admin")
            {
                ViewBag.dondachot = donhangss(1);
                ViewBag.giaothangcong = donhangss(4);
                ViewBag.giaothatbai = donhangss(3);
                return View(donhangss(0));
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }
        }
        // Chi tiết đơn hàng
        public ActionResult Chitietdonhang_khach(string id)
        {
            if (Session["nguoidung"] != null)
            {
                var donhang = db.DonhangUsers.ToList();
                List<DonhangUser> donhangUsers = db.DonhangUsers.Where(s => s.madonhang == id).ToList();

                // lấy id người dùng thông qua mã đơn hàng
                var id_nguoidungs = db.DonhangUsers.Where(s => s.madonhang == id).FirstOrDefault();
                // lấy toàn bộ thông tin người dùng
                var nguoi_mua = db.Nguoidungs.Where(s => s.id_nguoidung == id_nguoidungs.id_nguoidung).FirstOrDefault();
                ViewBag.nguoimua = nguoi_mua;
                //var xuatdonhang = donhangUsers.Distinct().Select(s => s.madonhang);
                ViewBag.donhangUsers = donhangUsers;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }
        }
        // Chốt đơn cho khách
        public ActionResult Chotdon(string id)
        {
            if (Session["nguoidung"] != null)
            {
                var nguoi = Session["nguoidung"] as Nguoidung;
                var trangthaidon = db.DonhangUsers.Where(s =>s.madonhang == id).ToList();
                foreach(var item in trangthaidon)
                {
                    item.trangthai = 1;
                }
              
                db.SaveChanges();
                return RedirectToAction("Index", "Chotdon_khachhang");
            }
            else
            {
                return RedirectToAction("Index", "SanphamShop", new { area = "" });
            }
        }

    }
}