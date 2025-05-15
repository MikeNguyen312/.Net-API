using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using WebAPI_ForTTTN.Models;
using WebAPI_ForTTTN.MyModels;

namespace WebAPI_ForTTTN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        [HttpGet]
        public IActionResult getDSSanPham()
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                return Ok(
                    db.SanPhams.Select(t => new
                    {
                        IdSanPham = t.IdSanPham,
                        Loai = t.Loai,
                        Ten = t.Ten,
                        Gia = t.Gia,
                        Hang = t.Hang,
                        Size = t.Size,
                        Thongtin =t.ThongTin,
                        SoLuong = t.SoLuong,
                        Anh = t.Anh != null ? Convert.ToBase64String(t.Anh) : null
                    }).ToList()
                    );
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetSanPhamById(string id)
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                var sanPham = db.SanPhams.FirstOrDefault(sp => sp.IdSanPham == id);

                if (sanPham == null)
                {
                    return NotFound();
                }

                var soLuongTon = db.SanPhamPhieuKhos
                    .Where(spk => spk.IdSanPham == id)
                    .Sum(spk => spk.SoLuong) ?? 0;

                return Ok(new
                {
                    IdSanPham = sanPham.IdSanPham,
                    Ten = sanPham.Ten,
                    Gia = sanPham.Gia,
                    Hang = sanPham.Hang,
                    Size = sanPham.Size,
                    SoLuong = sanPham.SoLuong,
                    ThongTin = sanPham.ThongTin,
                    Loai = sanPham.Loai,
                    Anh = sanPham.Anh != null ? Convert.ToBase64String(sanPham.Anh) : null,
                    SoLuongTon = soLuongTon 
                });
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("TaoSanPham")]
        public IActionResult themSanPham(CSanPham x)
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                var a = CSanPham.chuyendoi(x);
                db.SanPhams.Add(a);
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("XoaSanPham")]
        public IActionResult xoaSanPham(string id)
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                var a = db.SanPhams.Find(id);
                if (a == null)
                {
                    return NotFound("Sản Phẩm không tồn tại");
                }
                db.SanPhams.Remove(a);
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("SuaSanPham/{id}")]
        public IActionResult SuaSanPham(string id, [FromBody] CSanPham spMoi)
        {
            try
            {
                using var db = new DBThuctapContext();

                var sp = db.SanPhams.Find(id);
                if (sp == null)
                {
                    return NotFound("Không tìm thấy sản phẩm.");
                }

                // Cập nhật thông tin sản phẩm
                sp.Ten = spMoi.Ten;
                sp.Gia = spMoi.Gia;
                sp.Hang = spMoi.Hang;
                sp.Size = spMoi.Size;
                sp.ThongTin = spMoi.ThongTin;
                sp.Loai = spMoi.Loai;
                sp.SoLuong = spMoi.SoLuong;

                // Nếu ảnh có giá trị thì cập nhật (tránh ghi đè null lên ảnh hiện tại)
                if (spMoi.Anh != null && spMoi.Anh.Length > 0)
                {
                    sp.Anh = spMoi.Anh;
                }

                db.SaveChanges();
                return Ok("");
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi cập nhật sản phẩm: " + ex.Message);
            }
        }

    }
}
