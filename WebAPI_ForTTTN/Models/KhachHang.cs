using System;
using System.Collections.Generic;

namespace WebAPI_ForTTTN.Models
{
    public partial class KhachHang
    {
        public KhachHang()
        {
            DonHangs = new HashSet<DonHang>();
        }

        public string IdKhachHang { get; set; } = null!;
        public string? HoTen { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? Passwords { get; set; }
        public string? RoleWeb { get; set; }
        public string? Trangthai { get; set; }
        public class DonHangVaChiTietModelKH
        {
            public string IdDonHang { get; set; }
            public string? IdKhachHang { get; set; }
            public DateTime NgayDatHang { get; set; }
            public decimal? TongTien { get; set; }
            public string? DiaChi { get; set; }
            public string? PhuongThuc { get; set; }
            public string? TrangthaiDh { get; set; }
            public List<ChiTietModelKH> ChiTiet { get; set; } = new();
        }

        public class ChiTietModelKH
        {
            public string IdSanPham { get; set; }
            public string TenSanPham { get; set; }
            public int SoLuong { get; set; }
            public double? Gia { get; set; }
        }
        public virtual ICollection<DonHang> DonHangs { get; set; }
    }
}
