using System;
using System.Collections.Generic;

namespace WebAPI_ForTTTN.Models
{
    public partial class DonHang
    {
        public DonHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }
        public class DonHangVaChiTietModel
        {
            public string IdDonHang { get; set; }
            public string? IdKhachHang { get; set; }
            public DateTime NgayDatHang { get; set; }
            public decimal? TongTien { get; set; }
            public string? DiaChi { get; set; }
            public string? PhuongThuc { get; set; }
            public string? TrangthaiDh { get; set; }
            public List<ChiTietModel> ChiTiet { get; set; } = new();
        }

        public class ChiTietModel
        {
            public string IdSanPham { get; set; }
            public int? SoLuong { get; set; }
            public double? Gia { get; set; }
        }

        public string IdDonHang { get; set; } = null!;
        public string? IdKhachHang { get; set; }
        public DateTime? NgayDatHang { get; set; }
        public decimal? TongTien { get; set; }
        public string? DiaChi { get; set; }
        public string? PhuongThuc { get; set; }
        public string? TrangthaiDh { get; set; }

        public virtual KhachHang? IdKhachHangNavigation { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}
