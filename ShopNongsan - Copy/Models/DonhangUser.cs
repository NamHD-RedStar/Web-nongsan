//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShopNongsan.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DonhangUser
    {
        public int id_donhang { get; set; }
        public string madonhang { get; set; }
        public Nullable<int> id_nguoidung { get; set; }
        public Nullable<int> id_sanpham { get; set; }
        public string ten_sanpham { get; set; }
        public Nullable<int> giatien { get; set; }
        public Nullable<int> soluong { get; set; }
        public Nullable<System.DateTime> ngaymua { get; set; }
        public Nullable<System.DateTime> ngaygiao { get; set; }
        public Nullable<int> trangthai { get; set; }
    
        public virtual Nguoidung Nguoidung { get; set; }
    }
}