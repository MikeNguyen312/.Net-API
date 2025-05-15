using WebAPI_ForTTTN.Models;

namespace WebAPI_ForTTTN.MyModels
{
    public class CKhuyenMai
    {
        public string IdKhuyenMai { get; set; } = null!;
        public double? PhanTramKm { get; set; }
        public static KhuyenMai chuyendoi(CKhuyenMai x)
        {
            return new KhuyenMai
            {
                IdKhuyenMai = x.IdKhuyenMai,
                PhanTramKm = x.PhanTramKm,
            };
        }
        public static CKhuyenMai chuyendoi(KhuyenMai x)
        {
            return new CKhuyenMai
            {
                IdKhuyenMai = x.IdKhuyenMai,
                PhanTramKm = x.PhanTramKm,
            };
        }
    }
}
