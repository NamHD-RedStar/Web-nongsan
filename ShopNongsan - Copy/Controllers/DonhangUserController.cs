using ShopNongsan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopNongsan.Controllers
{
    public class DonhangUserController : Controller
    {
        private ShopNongsanEntities db = new ShopNongsanEntities();
     
        // GET: DonhangUser
        public List<Donhang> donhangsss(int trangthai)
        {
                var nguoi = Session["nguoidung"] as ShopNongsan.Models.Nguoidung;
                var noofRecords = (from c in db.DonhangUsers
                                   where c.id_nguoidung == nguoi.id_nguoidung && c.trangthai == trangthai
                                   group c by new { c.madonhang } into grouping
                                   select new Donhang()
                                   {
                                       _madonhang = grouping.Key.madonhang,
                                       _soluong = grouping.Count(),
                                       _ngaymua = (DateTime)db.DonhangUsers.Where(s => s.id_nguoidung == nguoi.id_nguoidung && s.madonhang == grouping.Key.madonhang).Select(s => s.ngaymua).FirstOrDefault(),
                                       _ngaygiao = (DateTime)db.DonhangUsers.Where(s => s.id_nguoidung == nguoi.id_nguoidung && s.madonhang == grouping.Key.madonhang).Select(s => s.ngaygiao).FirstOrDefault(),
                                       _thanhtien = (int)db.DonhangUsers.Where(s => s.id_nguoidung == nguoi.id_nguoidung && s.madonhang == grouping.Key.madonhang).Sum(s => (s.giatien * s.soluong)),
                                       _trangthai = (int)db.DonhangUsers.Where(s => s.id_nguoidung == nguoi.id_nguoidung && s.madonhang == grouping.Key.madonhang).Select(s => s.trangthai).FirstOrDefault(),
                                       _tensp = db.DonhangUsers.Where(s => s.id_nguoidung == nguoi.id_nguoidung && s.madonhang == grouping.Key.madonhang).Select(s => s.ten_sanpham).FirstOrDefault(),
                                   });
            return noofRecords.ToList();
         
        }
        public List<Donhang> donhang_khac(int trangthai)
        {
            var nguoi = Session["nguoidung"] as ShopNongsan.Models.Nguoidung;
            var noofRecords = (from c in db.DonhangUsers
                               where c.id_nguoidung == nguoi.id_nguoidung && c.trangthai != trangthai
                               group c by new { c.madonhang } into grouping
                               select new Donhang()
                               {
                                   _madonhang = grouping.Key.madonhang,
                                   _soluong = grouping.Count(),
                                   _ngaymua = (DateTime)db.DonhangUsers.Where(s => s.id_nguoidung == nguoi.id_nguoidung && s.madonhang == grouping.Key.madonhang).Select(s => s.ngaymua).FirstOrDefault(),
                                   _ngaygiao = (DateTime)db.DonhangUsers.Where(s => s.id_nguoidung == nguoi.id_nguoidung && s.madonhang == grouping.Key.madonhang).Select(s => s.ngaygiao).FirstOrDefault(),
                                   _thanhtien = (int)db.DonhangUsers.Where(s => s.id_nguoidung == nguoi.id_nguoidung && s.madonhang == grouping.Key.madonhang).Sum(s => (s.giatien * s.soluong)),
                                   _trangthai = (int)db.DonhangUsers.Where(s => s.id_nguoidung == nguoi.id_nguoidung && s.madonhang == grouping.Key.madonhang).Select(s => s.trangthai).FirstOrDefault(),
                                   _tensp = db.DonhangUsers.Where(s => s.id_nguoidung == nguoi.id_nguoidung && s.madonhang == grouping.Key.madonhang).Select(s => s.ten_sanpham).FirstOrDefault(),
                               });
            return noofRecords.ToList();

        }
        public ActionResult Index()
        {
            if (Session["nguoidung"] != null)
            {
                ViewBag.donhangss = donhangsss(0);
                return View(donhang_khac(0));
            }
            else
            {
                return RedirectToAction("Index", "Nguoidungs");
            }
        }
        public ActionResult Chitietdonhang(string id)
        {
            if (Session["nguoidung"] != null)
            {
                var nguoi = Session["nguoidung"] as ShopNongsan.Models.Nguoidung;
                var donhang = db.DonhangUsers.ToList();
                List<DonhangUser> donhangUsers = db.DonhangUsers.Where(s => s.id_nguoidung == nguoi.id_nguoidung && s.madonhang == id).ToList();
                //var xuatdonhang = donhangUsers.Distinct().Select(s => s.madonhang);
                ViewBag.donhangUsers = donhangUsers;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Nguoidungs");
            }
        }
        public ActionResult Huydonhang(string id)
        {
            var donhang = db.DonhangUsers.Where(s => s.madonhang == id).ToList();
            foreach(var item in donhang)//Nếu hủy đơn hàng thì trả lại số lượng cho sp
            {
                var sp = db.Sanphams.Where(s => s.id_sanpham == item.id_sanpham).FirstOrDefault();
                sp.soluong = sp.soluong + item.soluong;
            }
            db.DonhangUsers.RemoveRange(donhang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Xacnhan_chuanhan(string id)//Nếu chưa nhận đc thì trạng thái sang 3
        {

            if (Session["nguoidung"] != null)
            {
                var nguoi = Session["nguoidung"] as Nguoidung;
                var trangthaidon = db.DonhangUsers.Where(s => s.madonhang == id).ToList();
                foreach (var item in trangthaidon)
                {
                    item.trangthai = 3;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Nguoidungs");
            }
        }
        public ActionResult Xacnhan_danhan(string id)//Nếu đã nhận đc thì trạng thái sang 4
        {

            if (Session["nguoidung"] != null)
            {
                var nguoi = Session["nguoidung"] as Nguoidung;
                var trangthaidon = db.DonhangUsers.Where(s => s.madonhang == id).ToList();
                foreach (var item in trangthaidon)
                {
                    item.trangthai = 4;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Nguoidungs");
            }
        }
      
    }
}