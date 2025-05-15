using System;
using System.Collections.Generic;

namespace WebAPI_ForTTTN.Models
{
    public partial class ChiTietDonHang
    {
        public string IdDonHang { get; set; } = null!;
        public string IdSanPham { get; set; } = null!;
        public int? SoLuong { get; set; }
        public double? Gia { get; set; }

        public virtual DonHang IdDonHangNavigation { get; set; } = null!;
        public virtual SanPham IdSanPhamNavigation { get; set; } = null!;
    }
}
