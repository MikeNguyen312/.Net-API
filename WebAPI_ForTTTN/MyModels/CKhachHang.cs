using WebAPI_ForTTTN.Models;

namespace WebAPI_ForTTTN.MyModels
{
    public class CKhachHang
    {
        public string IdKhachHang { get; set; } = null!;
        public string? HoTen { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? Passwords { get; set; }
        public static KhachHang chuyendoi(CKhachHang x)
        {
            return new KhachHang
            {
                IdKhachHang = x.IdKhachHang,
                HoTen = x.HoTen,
                SoDienThoai = x.SoDienThoai,
                Email = x.Email,
                Passwords = x.Passwords,
            };
        }
        public static CKhachHang chuyendoi(KhachHang x)
        {
            return new CKhachHang
            {
                IdKhachHang = x.IdKhachHang,
                HoTen = x.HoTen,
                SoDienThoai = x.SoDienThoai,
                Email = x.Email,
                Passwords = x.Passwords,
            };
        }
    }
}
