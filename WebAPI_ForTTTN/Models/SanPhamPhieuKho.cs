using System;
using System.Collections.Generic;

namespace WebAPI_ForTTTN.Models
{
    public partial class SanPhamPhieuKho
    {
        public string IdPhieuKho { get; set; } = null!;
        public string IdSanPham { get; set; } = null!;
        public int? SoLuong { get; set; }
        public int? SoLuongXuat { get; set; }
        public int? SoLuongNhap { get; set; }

        public virtual PhieuKho IdPhieuKhoNavigation { get; set; } = null!;
        public virtual SanPham IdSanPhamNavigation { get; set; } = null!;
    }
}
