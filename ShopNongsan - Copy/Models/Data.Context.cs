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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ShopNongsanEntities : DbContext
    {
        public ShopNongsanEntities()
            : base("name=ShopNongsanEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DanhmucSanpham> DanhmucSanphams { get; set; }
        public virtual DbSet<Nguoidung> Nguoidungs { get; set; }
        public virtual DbSet<Sanpham> Sanphams { get; set; }
        public virtual DbSet<GiahangUser> GiahangUsers { get; set; }
        public virtual DbSet<Danhgianguoidung> Danhgianguoidungs { get; set; }
        public virtual DbSet<DonhangUser> DonhangUsers { get; set; }
        public virtual DbSet<Nhantin> Nhantins { get; set; }
        public virtual DbSet<Magiamgia> Magiamgias { get; set; }
        public virtual DbSet<Add_magiamgia_donhang> Add_magiamgia_donhang { get; set; }
    }
}
