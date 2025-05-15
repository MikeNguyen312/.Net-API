using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_ForTTTN.Models;
using WebAPI_ForTTTN.MyModels;

namespace WebAPI_ForTTTN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiController : ControllerBase
    {
        [HttpGet]
        public IActionResult getDSKM()
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                return Ok(
                    db.KhuyenMais.Select(t => new
                    {
                        Idkhuyenmai = t.IdKhuyenMai,
                        phanTram = t.PhanTramKm,
                    })
                 );
            }
            catch
            {
                return NotFound();
            }
        }
        [HttpGet("LayDSKM/{id}")]
        public IActionResult getDSSPKM(string id)
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                var khuyenMai = db.KhuyenMais
                    .Include(km => km.IdSanPhams)
                    .FirstOrDefault(km => km.IdKhuyenMai == id);

                if (khuyenMai == null)
                    return NotFound("Không tìm thấy khuyến mãi");

                var result = khuyenMai.IdSanPhams.Select(sp => new
                {
                    sp.IdSanPham,
                    sp.Ten,
                    sp.Size,
                    sp.Hang,
                    sp.Gia
                }).ToList();

                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("TaoKM")]
        public IActionResult TaoKM(CKhuyenMai x)
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                var a = CKhuyenMai.chuyendoi(x);
                db.KhuyenMais.Add(a);
                db.SaveChanges();
                return Ok(new { idkhuyenmai = a.IdKhuyenMai, phanTram = a.PhanTramKm });
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("XoaKMID")]
        public IActionResult xoaKM(string id)
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                var khuyenMai = db.KhuyenMais.Include(km => km.IdSanPhams).FirstOrDefault(km => km.IdKhuyenMai == id);
                if (khuyenMai == null)
                {
                    return NotFound("Khuyến mãi không tồn tại.");
                }

                if (khuyenMai.IdSanPhams.Any())
                {
                    return BadRequest("Khuyến mãi vẫn còn sản phẩm, không thể xoá.");
                }
                db.KhuyenMais.Remove(khuyenMai);
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("XoaSanPhamKM")]
        public IActionResult XoaSanPhamKhoiKhuyenMai(string idKhuyenMai, string idSanPham)
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                var khuyenMai = db.KhuyenMais.Include(km => km.IdSanPhams).FirstOrDefault(km => km.IdKhuyenMai == idKhuyenMai);
                if (khuyenMai == null)
                {
                    return NotFound("Khuyến mãi không tồn tại.");
                }
                var sanPham = db.SanPhams.Find(idSanPham);
                if (sanPham == null)
                {
                    return NotFound("Sản phẩm không tồn tại.");
                }
                if (!khuyenMai.IdSanPhams.Contains(sanPham))
                {
                    return BadRequest("Sản phẩm không có trong khuyến mãi này.");
                }
                khuyenMai.IdSanPhams.Remove(sanPham);
                // Khôi phục giá gốc
                if (khuyenMai.PhanTramKm.HasValue && khuyenMai.PhanTramKm.Value > 0)
                {
                    var phanTram = khuyenMai.PhanTramKm.Value;
                    sanPham.Gia = sanPham.Gia / (1 - (phanTram / 100.0));
                }
                db.SaveChanges();

                return Ok(new { message = "Sản phẩm đã được xoá khỏi khuyến mãi." });
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi xoá sản phẩm khỏi khuyến mãi: " + ex.Message);
            }
        }


        [HttpPost("ThemSanPhamKM")]
        public IActionResult ThemSanPhamVaoKhuyenMai(string idKhuyenMai, CSanPhamKhuyenMai x)
        {
            try
            {
                using var db = new DBThuctapContext();
                var khuyenMai = db.KhuyenMais.Find(idKhuyenMai);
                if (khuyenMai == null)
                {
                    return NotFound("Khuyến mãi không tồn tại.");
                }
                var sanPham = db.SanPhams.Find(x.IdSanPham);
                if (sanPham == null)
                {
                    return NotFound("Sản phẩm không tồn tại.");
                }
                if (khuyenMai.IdSanPhams.Contains(sanPham))
                {
                    return BadRequest("Sản phẩm đã có trong khuyến mãi này.");
                }
                // Kiểm tra sản phẩm đã thuộc khuyến mãi nào chưa
                var daCoTrongKhuyenMai = db.KhuyenMais.Any(km => km.IdSanPhams.Any(sp => sp.IdSanPham == x.IdSanPham));
                if (daCoTrongKhuyenMai)
                {
                    return BadRequest("Sản phẩm này đã thuộc một khuyến mãi khác.");
                }

                khuyenMai.IdSanPhams.Add(sanPham);
                // Cập nhật lại giá sản phẩm
                if (khuyenMai.PhanTramKm.HasValue)
                {
                    var giamGia = (sanPham.Gia ?? 0) * (khuyenMai.PhanTramKm.Value / 100.0);
                    sanPham.Gia = (sanPham.Gia ?? 0) - giamGia;
                }

                db.SaveChanges();

                return Ok("Sản phẩm đã được thêm vào khuyến mãi.");
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi thêm sản phẩm vào khuyến mãi: " + ex.Message);
            }
        }
        // Thêm phương thức này vào KhuyenMaiController
        [HttpGet("SanPhamKhuyenMai")]
        public IActionResult GetSanPhamKhuyenMai()
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();

                // Lấy tất cả các khuyến mãi và sản phẩm liên quan
                var khuyenMais = db.KhuyenMais
                    .Include(km => km.IdSanPhams)
                    .ToList();

                // Tạo danh sách sản phẩm khuyến mãi
                var sanPhamKhuyenMai = new List<object>();

                foreach (var km in khuyenMais)
                {
                    foreach (var sp in km.IdSanPhams)
                    {
                        sanPhamKhuyenMai.Add(new
                        {
                            sp.Anh,
                            sp.IdSanPham,
                            sp.Ten,
                            sp.Size,
                            sp.Hang,
                            sp.Gia,
                            KhuyenMai = new
                            {
                                IdKhuyenMai = km.IdKhuyenMai,
                                PhanTramKm = km.PhanTramKm
                            }
                        });
                    }
                }

                return Ok(sanPhamKhuyenMai);
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi lấy danh sách sản phẩm khuyến mãi: " + ex.Message);
            }
        }

    }
}
