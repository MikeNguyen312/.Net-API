using WebAPI_ForTTTN.Models;

namespace WebAPI_ForTTTN.MyModels
{
    public class CCTDH
    {
        public string IdDonHang { get; set; } = null!;
        public string IdSanPham { get; set; } = null!;
        public int? SoLuong { get; set; }
        public double? Gia { get; set; }
        public string IdKhachHang { get; set; } = null!;
        public static ChiTietDonHang chuyendoi(CCTDH x)
        {
            return new ChiTietDonHang
            {
                IdDonHang = x.IdDonHang,
                IdSanPham = x.IdSanPham,
                SoLuong = x.SoLuong,
                Gia = x.Gia,
            };
        }
        public static CCTDH chuyendoi(ChiTietDonHang x)
        {
            return new CCTDH
            {
                IdDonHang = x.IdDonHang,
                IdSanPham = x.IdSanPham,
                SoLuong = x.SoLuong,
                Gia = x.Gia,
                IdKhachHang = x.IdDonHangNavigation?.IdKhachHang,
            };
        }
    }
}
