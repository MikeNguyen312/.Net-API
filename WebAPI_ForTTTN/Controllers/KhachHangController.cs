using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAPI_ForTTTN.Models;
using WebAPI_ForTTTN.MyModels;

namespace WebAPI_ForTTTN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        [HttpGet]
        public IActionResult getDSKhachHang()
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                return Ok(
                    db.KhachHangs.Select(t => new
                    {
                        IdKhachHang = t.IdKhachHang,
                        HoTen = t.HoTen,
                        SoDienThoai = t.SoDienThoai,
                        Email = t.Email,
                        Passwords = t.Passwords,
                        trangthai = t.Trangthai,
                    }).ToList()
                    );
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("ThemKhachHang")]
        public IActionResult postDSKhachHang(CKhachHang x)
        {

            try
            {
                DBThuctapContext db = new DBThuctapContext();
                var kh = CKhachHang.chuyendoi(x);
                kh.Trangthai = "Chờ xác nhận"; // bắt buộc gán
                kh.RoleWeb = "user";
                db.KhachHangs.Add(kh);
                db.SaveChanges();
                return Ok();

            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("XoaKhachHang")]
        public IActionResult deleteKhachHang(string id)
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                KhachHang kh = db.KhachHangs.Find(id);
                if(kh == null)
                {
                    return NotFound();
                }
                else
                {
                    db.KhachHangs.Remove(kh);
                    db.SaveChanges();
                    return Ok();
                }   
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("SuaKhachHang")]
        public IActionResult EditKhachHang(CKhachHang x)
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                KhachHang mh = db.KhachHangs.Find(x.IdKhachHang);
                if (mh == null) return NotFound();
                mh.HoTen = x.HoTen;
                mh.SoDienThoai = x.SoDienThoai;
                mh.Passwords =x.Passwords;
                mh.Email = x.Email;
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("/api/login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var db = new DBThuctapContext();

                // Tìm đúng khách hàng theo email + password
                var user = db.KhachHangs
                    .Where(kh => kh.Email == request.Email && kh.Passwords == request.Password )
                    .Select(kh => new
                    {
                        ID_KhachHang = kh.IdKhachHang,
                        HoTen = kh.HoTen,
                        Email = kh.Email,
                        SoDienThoai = kh.SoDienThoai,
                        Trangthai = kh.Trangthai
                    })
                    .FirstOrDefault();

                if (user == null)
                {
                    return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }
        [HttpPut("CapNhatTrangThai")]
        public IActionResult CapNhatTrangThai(string id, string trangThai)
        {
            try
            {
                var db = new DBThuctapContext();
                var kh = db.KhachHangs.Find(id);
                if (kh == null) return NotFound();
                kh.Trangthai = trangThai;
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetKhachHangById(string id)
        {
            try
            {
                var db = new DBThuctapContext();
                var kh = db.KhachHangs
                    .Where(k => k.IdKhachHang == id)
                    .Select(k => new
                    {
                        IdKhachHang = k.IdKhachHang,
                        HoTen = k.HoTen,
                        SoDienThoai = k.SoDienThoai,
                        Email = k.Email,
                        Passwords = k.Passwords,
                        Trangthai = k.Trangthai,
                    })
                    .FirstOrDefault();

                if (kh == null)
                {
                    return NotFound(); // Không tìm thấy khách hàng với id này
                }

                return Ok(kh); // Trả về khách hàng
            }
            catch
            {
                return BadRequest(); // Lỗi server
            }
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        [HttpGet("donhang")]
        public IActionResult GetDonHangByKhachHang(string idKhachHang)
        {
            try
            {
                DBThuctapContext db = new DBThuctapContext();
                var donHangs = db.DonHangs
                    .Where(d => d.IdKhachHang == idKhachHang)
                    .Select(d => new
                    {
                        d.IdDonHang,
                        d.NgayDatHang,
                        d.TongTien,
                        d.DiaChi,
                        d.PhuongThuc,
                        d.TrangthaiDh
                    }).ToList();

                return Ok(donHangs);
            }
            catch
            {
                return BadRequest("Lỗi khi truy vấn đơn hàng");
            }
        }

        [HttpGet("don-hang-chi-tiet/{idKhachHang}")]
        public IActionResult GetDonHangVaChiTiet(string idKhachHang)
        {
            try
            {
                var db = new DBThuctapContext();

                var donHangs = db.DonHangs
                    .Include(dh => dh.ChiTietDonHangs)
                        .ThenInclude(ct => ct.IdSanPhamNavigation)
                    .Where(dh => dh.IdKhachHang == idKhachHang)
                    .Select(dh => new KhachHang.DonHangVaChiTietModelKH
                    {
                        IdDonHang = dh.IdDonHang,
                        IdKhachHang = dh.IdKhachHang,
                        NgayDatHang = dh.NgayDatHang ?? DateTime.MinValue,
                        TongTien = dh.TongTien ?? 0,
                        DiaChi = dh.DiaChi,
                        PhuongThuc = dh.PhuongThuc,
                        TrangthaiDh = dh.TrangthaiDh,
                        ChiTiet = dh.ChiTietDonHangs.Select(ct => new KhachHang.ChiTietModelKH
                        {
                            IdSanPham = ct.IdSanPham,
                            TenSanPham = ct.IdSanPhamNavigation != null ? ct.IdSanPhamNavigation.Ten : "",
                            SoLuong = ct.SoLuong ?? 0,
                            Gia = ct.Gia ?? 0
                        }).ToList()
                    })
                    .ToList();

                return Ok(donHangs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi lấy đơn hàng chi tiết", error = ex.Message });
            }
        }
    }
}
