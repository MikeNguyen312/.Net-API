using WebAPI_ForTTTN.Models;

namespace WebAPI_ForTTTN.MyModels
{
    public class CPhieuKho
    {
        public string IdPhieuKho { get; set; } = null!;
        public DateTime? NgayLap { get; set; }
        public static PhieuKho chuyendoi(CPhieuKho x)
        {
            return new PhieuKho
            {
                IdPhieuKho = x.IdPhieuKho,
                NgayLap = x.NgayLap,
            };
        }
        public static CPhieuKho chuyendoi(PhieuKho x)
        {
            return new CPhieuKho
            {
                IdPhieuKho = x.IdPhieuKho,
                NgayLap = x.NgayLap,
            };
        }
    }
}
