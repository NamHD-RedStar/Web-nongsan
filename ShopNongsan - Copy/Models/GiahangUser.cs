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
    
    public partial class GiahangUser
    {
        public int id_giohang { get; set; }
        public Nullable<int> id_nguoidung { get; set; }
        public Nullable<int> id_sanpham { get; set; }
        public string img_sanpham { get; set; }
        public string tensanpham { get; set; }
        public Nullable<int> giatien { get; set; }
        public Nullable<int> soluong { get; set; }
    
        public virtual Nguoidung Nguoidung { get; set; }
    }
}
