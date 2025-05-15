using WebAPI_ForTTTN.Models;

namespace WebAPI_ForTTTN.MyModels
{
    public class CDonHang
    {

        public string IdDonHang { get; set; } = null!;
        public string? IdKhachHang { get; set; }
        public DateTime? NgayDatHang { get; set; }
        public decimal? TongTien { get; set; }
        public string? DiaChi { get; set; }
        public string? PhuongThuc { get; set; }
        public string? TrangthaiDh { get; set; }
        public static DonHang chuyendoi(CDonHang x)
        {
            return new DonHang
            {
                IdDonHang = x.IdDonHang,
                IdKhachHang = x.IdKhachHang,
                NgayDatHang = x.NgayDatHang,
                TongTien = x.TongTien,
                DiaChi = x.DiaChi,
                PhuongThuc = x.PhuongThuc,
                TrangthaiDh = x.TrangthaiDh,
            };
        }
        public static CDonHang chuyendoi(DonHang x)
        {
            return new CDonHang
            {
                IdDonHang = x.IdDonHang,
                IdKhachHang = x.IdKhachHang,
                NgayDatHang = x.NgayDatHang,
                TongTien = x.TongTien,
                DiaChi = x.DiaChi,
                PhuongThuc = x.PhuongThuc,
                TrangthaiDh = x.TrangthaiDh,
            };
        }
    }
}
