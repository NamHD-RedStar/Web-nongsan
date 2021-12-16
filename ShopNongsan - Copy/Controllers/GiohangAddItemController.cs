using ShopNongsan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopNongsan.Controllers
{
    public class GiohangAddItemController : Controller
    {
        ShopNongsanEntities db = new ShopNongsanEntities();
        private const string CartSession = "CartSession";
        // GET: GiohangAddItem
        //public ActionResult Index()
        //{
        //    Session[CartSession] = new List<CartItem>();
        //    var cart = Session[CartSession];
        //    var list = new List<CartItem>();
        //    if(cart != null)
        //    {
        //        list = (List<CartItem>)cart;
        //    }
        //    return View();
        //}
        //public ActionResult AddItem(int idsp,int soluong=0)
        //{
        //    var cart = Session[CartSession];
        //    if(cart != null)
        //    {
        //        var list = (List<CartItem>)cart;
        //        if (list.Exists(x => x.id_sanpham == idsp))
        //        {
        //            foreach (var item in list)
        //            {
        //                if (item.id_sanpham == idsp)
        //                {
        //                    item.soluong += soluong;
        //                }
        //            }
               
        //        }
        //        else
        //        {
        //            // tao moi doi tuong cart item
        //            var item = new CartItem();
        //            item.id_sanpham = idsp;
        //            item.soluong = soluong;
        //            list.Add(item);
        //        }
        //        // Gan vao Session
        //        Session[CartSession] = list;
        //    }
           
        //    else
        //    {
        //        // Tao moi doi tuong cart item
        //        var item = new CartItem();
        //        item.id_sanpham = idsp;
        //        item.soluong = soluong;
        //        var list = new List<CartItem>();
        //        //Gan vao sesion
        //        Session[CartSession] = list;
        //    }
        //    return RedirectToAction("Index");
        //}
        public ActionResult Index()
        {
            if (TempData["cart"] != null)
            {
                float x = 0;
                List<CartItem> li2 = TempData["cart"] as List<CartItem>;
                foreach (var item in li2)
                {
                    x += item.giatien;

                }

                TempData["total"] = x;
            }
            TempData.Keep();
            return View(db.Sanphams.OrderByDescending(x => x.id_sanpham).ToList());

        }
        List<Sanpham> li = new List<Sanpham>();
        public ActionResult AddtoCart(int? id)
        {
            Sanpham p = db.Sanphams.Where(x => x.id_sanpham == id).SingleOrDefault();
            return View(p);
        }

       [HttpPost]
        public ActionResult AddtoCart()
        {
            //c
           // Sanpham p = db.Sanphams.Where(x => x.id_sanpham == id).SingleOrDefault();

            GiahangUser c = new GiahangUser();
            c.id_sanpham =3;
            c.giatien =1000;
            c.soluong = 5;
            c.tensanpham ="gai";
            //if (TempData["cart"] == null)
            //{
            //    li.Add(c);
            //    TempData["cart"] = li;
            //}
            //else
            //{
            //    List<Sanpham> li2 = TempData["cart"] as List<Sanpham>;
            //    li2.Add(c);
            //    TempData["cart"] = li2;
            //}
            if (ModelState.IsValid)
            {
                db.GiahangUsers.Add(c);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            TempData.Keep();
            //TempData.Peek("cart");
            return RedirectToAction("Index");
        }
        public ActionResult checkout()
        {
            TempData.Keep("cart");
            TempData.Peek("cart");
            return View();
        }
    }
}