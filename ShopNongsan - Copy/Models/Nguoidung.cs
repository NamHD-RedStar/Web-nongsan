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
    
    public partial class Nguoidung
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Nguoidung()
        {
            this.GiahangUsers = new HashSet<GiahangUser>();
            this.Danhgianguoidungs = new HashSet<Danhgianguoidung>();
            this.DonhangUsers = new HashSet<DonhangUser>();
        }
    
        public int id_nguoidung { get; set; }
        public string ho { get; set; }
        public string ten { get; set; }
        public string anh_daidien { get; set; }
        public string diachi { get; set; }
        public string sdt { get; set; }
        public string gmail { get; set; }
        public string matkhau { get; set; }
        public Nullable<System.DateTime> ngaydangky { get; set; }
        public string quyen { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiahangUser> GiahangUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Danhgianguoidung> Danhgianguoidungs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonhangUser> DonhangUsers { get; set; }
    }
}
