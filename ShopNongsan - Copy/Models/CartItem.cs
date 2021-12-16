using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopNongsan.Models
{
    [Serializable]
    public class CartItem
    {
        public int id_giohang { get; set; }
        public int id_nguoidung { get; set; }
        public int id_sanpham { get; set; }
        public string img_sanpham { get; set; }
        public string tensanpham { get; set; }
        public int giatien { get; set; }
        public int soluong { get; set; }

    }
}