using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_ForTTTN.Models;
using WebAPI_ForTTTN.MyModels;

namespace WebAPI_ForTTTN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhoController : ControllerBase
    {
        [HttpGet]
        public IActionResult getDSKho()
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                return Ok(
                    db.PhieuKhos.Select(t => new
                    {
                        IdPhieuKho = t.IdPhieuKho,
                        NgayLap = t.NgayLap,
                    }).ToList()
                    );
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetDSKhoID(string id)
        {
            try
            {
                using (DBThuctapContext db = new DBThuctapContext())
                {
                    var phieuKho = db.PhieuKhos.Find(id);
                    if (phieuKho == null)
                    {
                        return NotFound("Không tìm thấy phiếu kho.");
                    }

                    var danhSachSanPham = db.SanPhamPhieuKhos
                        .Where(sp => sp.IdPhieuKho == id)
                        .Select(sp => new CSanPhamPhieuKho
                        {
                            IdPhieuKho = sp.IdPhieuKho,
                            IdSanPham = sp.IdSanPham,
                            SoLuong = sp.SoLuong,
                            SoLuongNhap = sp.SoLuongNhap,
                            SoLuongXuat = sp.SoLuongXuat
                        }).ToList();
                    return Ok(danhSachSanPham);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi server: " + ex.Message);
            }
        }
        [HttpDelete]
        public IActionResult xoaDSKho(string id)
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                var pk = db.PhieuKhos.FirstOrDefault(p => p.IdPhieuKho == id);
                if (pk == null)
                {
                    return NotFound("Khong tim thay Phieu Kho");
                }

                var ktra = db.SanPhamPhieuKhos.Any(sp => sp.IdPhieuKho == id);
                if (ktra)
                {
                    return BadRequest("Con Item");
                }
                db.PhieuKhos.Remove(pk);
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("XoaID")]
        public IActionResult xoaItemtrongPK(string idPK, string idSanPham)
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                var a = db.SanPhamPhieuKhos.FirstOrDefault(sp => sp.IdPhieuKho == idPK && sp.IdSanPham == idSanPham);
                if (a == null)
                {
                    return NotFound("Khong tim thay san pham trong PK");
                }
                db.SanPhamPhieuKhos.Remove(a);
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("TaoPhieuKho")]
        public IActionResult TaoPhieuKho(CPhieuKho pk)
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                db.PhieuKhos.Add(CPhieuKho.chuyendoi(pk));
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("{idPhieuKho}/SanPham")]
        public IActionResult ThemItemvaoPK(CSanPhamPhieuKho x, string idPhieuKho)
        {
            try
            {
                using var db = new DBThuctapContext();

                // Tìm phiếu kho theo id
                var pk = db.PhieuKhos.Find(idPhieuKho);
                if (pk == null)
                {
                    return NotFound("Phiếu kho không tồn tại.");
                }

                // Tìm sản phẩm theo IdSanPham
                var sanPham = db.SanPhams.Find(x.IdSanPham);
                if (sanPham == null)
                {
                    return NotFound("Sản phẩm không tồn tại.");
                }

                // Cập nhật số lượng trong SanPhamPhieuKho
                var spPhieuKho = new CSanPhamPhieuKho
                {
                    IdPhieuKho = idPhieuKho,
                    IdSanPham = x.IdSanPham,
                    SoLuongNhap = x.SoLuongNhap,
                    SoLuongXuat = x.SoLuongXuat,
                    SoLuong = sanPham.SoLuong + (x.SoLuongNhap ?? 0) - (x.SoLuongXuat ?? 0),  // Tính lại SoLuong trong SanPhamPhieuKho
                };

                // Thêm sản phẩm vào phiếu kho
                db.SanPhamPhieuKhos.Add(CSanPhamPhieuKho.chuyendoi(spPhieuKho));

                // Cập nhật lại số lượng của sản phẩm trong bảng SanPham
                sanPham.SoLuong = sanPham.SoLuong + (x.SoLuongNhap ?? 0) - (x.SoLuongXuat ?? 0);

                db.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết (có thể log trong hệ thống hoặc ghi vào console)
                return BadRequest("Lỗi khi thêm sản phẩm vào phiếu kho: " + ex.Message);
            }
        }

        [HttpPost("CapNhatSoLuongMoiNhat")]
        public IActionResult CapNhatSoLuong()
        {
            using var db = new DBThuctapContext();

            var danhSachSanPham = db.SanPhams.ToList();

            foreach (var sanPham in danhSachSanPham)
            {
                var danhSachPhieuKho = db.SanPhamPhieuKhos
                    .Where(spk => spk.IdSanPham == sanPham.IdSanPham)
                    .ToList();

                int tongNhap = danhSachPhieuKho.Sum(spk => spk.SoLuongNhap ?? 0);
                int tongXuat = danhSachPhieuKho.Sum(spk => spk.SoLuongXuat ?? 0);
                int soLuongMoi = tongNhap - tongXuat;

                sanPham.SoLuong = soLuongMoi;
            }

            db.SaveChanges();

            return Ok("Đã cập nhật số lượng của tất cả sản phẩm theo nhập-xuất.");
        }
        [HttpGet("ThongKe")]
        public IActionResult ThongKeSanPham()
        {
            using var db = new DBThuctapContext();

            var result = db.SanPhamPhieuKhos
                .GroupBy(sp => sp.IdSanPham)
                .Select(g => new {
                    IdSanPham = g.Key,
                    SoLuongNhap = g.Sum(x => x.SoLuongNhap ?? 0),
                    SoLuongXuat = g.Sum(x => x.SoLuongXuat ?? 0),
                    TenSanPham = db.SanPhams.Where(sp => sp.IdSanPham == g.Key).Select(sp => sp.Ten).FirstOrDefault()
                }).ToList();

            return Ok(result);
        }

    }
}
