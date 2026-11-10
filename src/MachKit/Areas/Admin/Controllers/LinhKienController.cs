using MachKit.Data;
using MachKit.Helpers;
using MachKit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MachKit.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,NhanVien")]
public class LinhKienController : Controller
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public LinhKienController(
        AppDbContext db,
        IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    // =========================
    // DANH SÁCH LINH KIỆN
    // =========================
    public async Task<IActionResult> Index()
    {
        var danhSach = await _db.LinhKiens
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .OrderBy(x => x.MaLinhKien)
            .ToListAsync();

        return View(danhSach);
    }


    // =========================
    // MỞ FORM THÊM
    // =========================
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await NapSelect();

        return View(new LinhKien
        {
            MucTonToiThieu = 5,
            NgungBan = false
        });
    }


    // =========================
    // LƯU LINH KIỆN MỚI
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        LinhKien model,
        IFormFile? anh)
    {
        // Chuẩn hóa dữ liệu
        model.MaLinhKien =
            model.MaLinhKien?.Trim().ToUpperInvariant() ?? "";

        model.TenLinhKien =
            model.TenLinhKien?.Trim() ?? "";

        // Slug được sinh tự động
        model.Slug =
            SlugHelper.Generate(model.TenLinhKien);

        // Form không nhập Slug nên bỏ lỗi validation cũ
        ModelState.Remove(nameof(LinhKien.Slug));


        // Kiểm tra mã linh kiện
        if (string.IsNullOrWhiteSpace(model.MaLinhKien))
        {
            ModelState.AddModelError(
                nameof(LinhKien.MaLinhKien),
                "Vui lòng nhập mã linh kiện.");
        }
        else
        {
            bool trungMa = await _db.LinhKiens
                .AnyAsync(x =>
                    x.MaLinhKien == model.MaLinhKien);

            if (trungMa)
            {
                ModelState.AddModelError(
                    nameof(LinhKien.MaLinhKien),
                    "Mã linh kiện đã tồn tại.");
            }
        }


        // Kiểm tra tên
        if (string.IsNullOrWhiteSpace(model.TenLinhKien))
        {
            ModelState.AddModelError(
                nameof(LinhKien.TenLinhKien),
                "Vui lòng nhập tên linh kiện.");
        }


        // Kiểm tra giá
        if (model.Gia < 0)
        {
            ModelState.AddModelError(
                nameof(LinhKien.Gia),
                "Giá không được nhỏ hơn 0.");
        }


        // Kiểm tra tồn
        if (model.SoLuongTon < 0)
        {
            ModelState.AddModelError(
                nameof(LinhKien.SoLuongTon),
                "Số lượng tồn không được nhỏ hơn 0.");
        }


        if (model.MucTonToiThieu < 0)
        {
            ModelState.AddModelError(
                nameof(LinhKien.MucTonToiThieu),
                "Mức tồn tối thiểu không được nhỏ hơn 0.");
        }


        // Kiểm tra ảnh trước khi lưu
        if (anh is not null && anh.Length > 0)
        {
            var loiAnh = KiemTraAnh(anh);

            if (loiAnh is not null)
            {
                ModelState.AddModelError(
                    "anh",
                    loiAnh);
            }
        }


        // Nếu có lỗi thì quay lại form
        if (!ModelState.IsValid)
        {
            await NapSelect(
                model.DanhMucId,
                model.ThuongHieuId);

            return View(model);
        }


        // Lưu ảnh
        if (anh is not null && anh.Length > 0)
        {
            model.HinhAnh =
                await LuuAnhAsync(anh);
        }


        _db.LinhKiens.Add(model);

        await _db.SaveChangesAsync();


        TempData["Success"] =
            $"Đã thêm linh kiện \"{model.TenLinhKien}\" thành công.";


        return RedirectToAction(nameof(Index));
    }


    // =========================
    // MỞ FORM SỬA
    // =========================
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model =
            await _db.LinhKiens.FindAsync(id);

        if (model is null)
            return NotFound();


        await NapSelect(
            model.DanhMucId,
            model.ThuongHieuId);


        return View(model);
    }


    // =========================
    // LƯU LINH KIỆN ĐÃ SỬA
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        LinhKien model,
        IFormFile? anh)
    {
        if (id != model.Id)
            return NotFound();


        model.MaLinhKien =
            model.MaLinhKien?.Trim().ToUpperInvariant() ?? "";

        model.TenLinhKien =
            model.TenLinhKien?.Trim() ?? "";

        model.Slug =
            SlugHelper.Generate(model.TenLinhKien);


        // Slug không nhập từ form
        ModelState.Remove(nameof(LinhKien.Slug));


        bool trungMa = await _db.LinhKiens
            .AnyAsync(x =>
                x.MaLinhKien == model.MaLinhKien &&
                x.Id != id);

        if (trungMa)
        {
            ModelState.AddModelError(
                nameof(LinhKien.MaLinhKien),
                "Mã linh kiện đã tồn tại.");
        }


        if (string.IsNullOrWhiteSpace(model.TenLinhKien))
        {
            ModelState.AddModelError(
                nameof(LinhKien.TenLinhKien),
                "Vui lòng nhập tên linh kiện.");
        }


        if (model.Gia < 0)
        {
            ModelState.AddModelError(
                nameof(LinhKien.Gia),
                "Giá không được nhỏ hơn 0.");
        }


        if (model.SoLuongTon < 0)
        {
            ModelState.AddModelError(
                nameof(LinhKien.SoLuongTon),
                "Số lượng tồn không được nhỏ hơn 0.");
        }


        if (model.MucTonToiThieu < 0)
        {
            ModelState.AddModelError(
                nameof(LinhKien.MucTonToiThieu),
                "Mức tồn tối thiểu không được nhỏ hơn 0.");
        }


        if (anh is not null && anh.Length > 0)
        {
            var loiAnh = KiemTraAnh(anh);

            if (loiAnh is not null)
            {
                ModelState.AddModelError(
                    "anh",
                    loiAnh);
            }
        }


        if (!ModelState.IsValid)
        {
            await NapSelect(
                model.DanhMucId,
                model.ThuongHieuId);

            return View(model);
        }


        var current =
            await _db.LinhKiens.FindAsync(id);

        if (current is null)
            return NotFound();


        current.MaLinhKien =
            model.MaLinhKien;

        current.TenLinhKien =
            model.TenLinhKien;

        current.Slug =
            model.Slug;

        current.MoTa =
            model.MoTa;

        current.Gia =
            model.Gia;

        current.SoLuongTon =
            model.SoLuongTon;

        current.MucTonToiThieu =
            model.MucTonToiThieu;

        current.DanhMucId =
            model.DanhMucId;

        current.ThuongHieuId =
            model.ThuongHieuId;

        current.DatasheetUrl =
            model.DatasheetUrl;

        current.NgungBan =
            model.NgungBan;


        // Có chọn ảnh mới thì thay ảnh
        if (anh is not null && anh.Length > 0)
        {
            var anhCu = current.HinhAnh;

            current.HinhAnh =
                await LuuAnhAsync(anh);

            XoaAnhCu(anhCu);
        }


        await _db.SaveChangesAsync();


        TempData["Success"] =
            $"Đã cập nhật linh kiện \"{model.TenLinhKien}\".";


        return RedirectToAction(nameof(Index));
    }


    // =========================
    // ẨN / HIỆN LINH KIỆN
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int id)
    {
        var model =
            await _db.LinhKiens.FindAsync(id);

        if (model is null)
            return NotFound();


        model.NgungBan =
            !model.NgungBan;


        await _db.SaveChangesAsync();


        TempData["Success"] =
            model.NgungBan
                ? $"Đã ngừng bán \"{model.TenLinhKien}\"."
                : $"Đã mở bán lại \"{model.TenLinhKien}\".";


        return RedirectToAction(nameof(Index));
    }


    // =========================
    // NẠP DANH MỤC / THƯƠNG HIỆU
    // =========================
    private async Task NapSelect(
        int? danhMucId = null,
        int? thuongHieuId = null)
    {
        var danhMucs = await _db.DanhMucs
            .AsNoTracking()
            .OrderBy(x => x.ThuTu)
            .ThenBy(x => x.TenDanhMuc)
            .ToListAsync();


        var thuongHieus = await _db.ThuongHieus
            .AsNoTracking()
            .OrderBy(x => x.TenThuongHieu)
            .ToListAsync();


        ViewBag.DanhMucId =
            new SelectList(
                danhMucs,
                "Id",
                "TenDanhMuc",
                danhMucId);


        ViewBag.ThuongHieuId =
            new SelectList(
                thuongHieus,
                "Id",
                "TenThuongHieu",
                thuongHieuId);
    }


    // =========================
    // KIỂM TRA ẢNH
    // =========================
    private static string? KiemTraAnh(
        IFormFile file)
    {
        if (file.Length == 0)
        {
            return "File ảnh không có dữ liệu.";
        }


        string ext =
            Path.GetExtension(file.FileName)
                .ToLowerInvariant();


        string[] choPhep =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };


        if (!choPhep.Contains(ext))
        {
            return
                "Ảnh chỉ nhận JPG, JPEG, PNG hoặc WEBP.";
        }


        if (file.Length > 2 * 1024 * 1024)
        {
            return
                "Dung lượng ảnh tối đa là 2 MB.";
        }


        return null;
    }


    // =========================
    // LƯU ẢNH
    // =========================
    private async Task<string> LuuAnhAsync(
        IFormFile file)
    {
        string ext =
            Path.GetExtension(file.FileName)
                .ToLowerInvariant();


        string tenFile =
            $"{Guid.NewGuid():N}{ext}";


        string thuMuc =
            Path.Combine(
                _env.WebRootPath,
                "uploads");


        Directory.CreateDirectory(thuMuc);


        string duongDan =
            Path.Combine(
                thuMuc,
                tenFile);


        await using var fs =
            new FileStream(
                duongDan,
                FileMode.Create);


        await file.CopyToAsync(fs);


        return "/uploads/" + tenFile;
    }


    // =========================
    // XÓA ẢNH CŨ KHI ĐỔI ẢNH
    // =========================
    private void XoaAnhCu(string? hinhAnh)
    {
        if (string.IsNullOrWhiteSpace(hinhAnh))
            return;


        if (!hinhAnh.StartsWith("/uploads/"))
            return;


        string tenFile =
            Path.GetFileName(hinhAnh);


        string duongDan =
            Path.Combine(
                _env.WebRootPath,
                "uploads",
                tenFile);


        if (System.IO.File.Exists(duongDan))
        {
            System.IO.File.Delete(duongDan);
        }
    }
}