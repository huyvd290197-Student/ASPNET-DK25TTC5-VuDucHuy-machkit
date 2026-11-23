using System.Globalization;
using MachKit.Models;

namespace MachKit.Helpers;

public static class DonHang4DHelper
{
    public static IReadOnlyList<string> TatCa { get; } = Array.AsReadOnly(new[]
    {
        TrangThaiDonHang.ChoXacNhan, TrangThaiDonHang.DaXacNhan,
        TrangThaiDonHang.DangGiao, TrangThaiDonHang.HoanThanh, TrangThaiDonHang.DaHuy
    });

    public static bool LaTrangThai(string? value) =>
        value is not null && TatCa.Contains(value);

    public static string[] BuocTiepTheo(string? value) => value switch
    {
        TrangThaiDonHang.ChoXacNhan => new[]
            { TrangThaiDonHang.DaXacNhan, TrangThaiDonHang.DaHuy },
        TrangThaiDonHang.DaXacNhan => new[]
            { TrangThaiDonHang.DangGiao, TrangThaiDonHang.DaHuy },
        TrangThaiDonHang.DangGiao => new[] { TrangThaiDonHang.HoanThanh },
        _ => Array.Empty<string>()
    };

    public static bool DuocChuyen(string? cu, string? moi) =>
        moi is not null && BuocTiepTheo(cu).Contains(moi);

    public static string TenTrangThai(string? value) => value switch
    {
        TrangThaiDonHang.ChoXacNhan => "Chờ xác nhận",
        TrangThaiDonHang.DaXacNhan => "Đã xác nhận",
        TrangThaiDonHang.DangGiao => "Đang giao",
        TrangThaiDonHang.HoanThanh => "Hoàn thành",
        TrangThaiDonHang.DaHuy => "Đã hủy",
        _ => "Trạng thái không nhận diện"
    };

    public static string LopTrangThai(string? value) => value switch
    {
        TrangThaiDonHang.ChoXacNhan => "mk4d-wait",
        TrangThaiDonHang.DaXacNhan => "mk4d-confirm",
        TrangThaiDonHang.DangGiao => "mk4d-ship",
        TrangThaiDonHang.HoanThanh => "mk4d-done",
        TrangThaiDonHang.DaHuy => "mk4d-cancel",
        _ => "mk4d-wait"
    };

    public static string MaDon(int id) => $"MK-{id:D6}";
    public static string Tien(decimal value) =>
        value.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ";
    public static string GioVietNam(DateTime utc) =>
        new DateTimeOffset(DateTime.SpecifyKind(utc, DateTimeKind.Utc))
            .ToOffset(TimeSpan.FromHours(7)).ToString("dd/MM/yyyy HH:mm:ss");
}
