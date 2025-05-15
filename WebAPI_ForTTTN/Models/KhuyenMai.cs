using System;
using System.Collections.Generic;

namespace WebAPI_ForTTTN.Models
{
    public partial class KhuyenMai
    {
        public KhuyenMai()
        {
            IdSanPhams = new HashSet<SanPham>();
        }

        public string IdKhuyenMai { get; set; } = null!;
        public double? PhanTramKm { get; set; }

        public virtual ICollection<SanPham> IdSanPhams { get; set; }
    }
}
