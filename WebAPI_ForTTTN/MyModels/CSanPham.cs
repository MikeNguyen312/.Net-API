using WebAPI_ForTTTN.Models;

namespace WebAPI_ForTTTN.MyModels
{
    public class CSanPham
    {
        public string IdSanPham { get; set; } = null!;
        public string? Ten { get; set; }
        public double? Gia { get; set; }
        public string? Hang { get; set; }
        public string? Size { get; set; }
        public string? ThongTin { get; set; }
        public string? Loai { get; set; }
        public byte[]? Anh { get; set; }
        public int? SoLuong { get; set; }
        public static SanPham chuyendoi(CSanPham x)
        {
            return new SanPham
            {
                IdSanPham = x.IdSanPham,
                Ten = x.Ten,
                Gia = x.Gia,
                Hang = x.Hang,
                Size = x.Size,
                ThongTin = x.ThongTin,
                Loai = x.Loai,
                SoLuong = x.SoLuong,
                Anh = x.Anh,
            };
        }
        public static CSanPham chuyendoi(SanPham x)
        {
            return new CSanPham
            {
                IdSanPham = x.IdSanPham,
                Ten = x.Ten,
                Gia = x.Gia,
                Hang = x.Hang,
                Size = x.Size,
                ThongTin = x.ThongTin,
                Loai = x.Loai,
                Anh = x.Anh,
                SoLuong = x.SoLuong,
            };
        }
    }
}
