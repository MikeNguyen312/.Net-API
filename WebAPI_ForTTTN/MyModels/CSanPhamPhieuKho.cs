using WebAPI_ForTTTN.Models;

namespace WebAPI_ForTTTN.MyModels
{
    public class CSanPhamPhieuKho
    {
        public string IdPhieuKho { get; set; } = null!;
        public string IdSanPham { get; set; } = null!;
        public int? SoLuong { get; set; }
        public int? SoLuongXuat { get; set; }
        public int? SoLuongNhap { get; set; }
        public static SanPhamPhieuKho chuyendoi(CSanPhamPhieuKho x)
        {
            return new SanPhamPhieuKho
            {
                IdPhieuKho = x.IdPhieuKho,
                IdSanPham = x.IdSanPham,
                SoLuong = x.SoLuong,
                SoLuongXuat = x.SoLuongXuat,
                SoLuongNhap = x.SoLuongNhap,
            };
        }
        public static CSanPhamPhieuKho chuyendoi(SanPhamPhieuKho x)
        {
            return new CSanPhamPhieuKho
            {
                IdPhieuKho = x.IdPhieuKho,
                IdSanPham = x.IdSanPham,
                SoLuong = x.SoLuong,
                SoLuongXuat = x.SoLuongXuat,
                SoLuongNhap = x.SoLuongNhap,
            };
        }
    }
}
