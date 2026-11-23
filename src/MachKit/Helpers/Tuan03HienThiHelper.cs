using System;

namespace MachKit.Helpers;

public static class Tuan03HienThiHelper
{
    // Chỉ dùng ảnh nội bộ đã upload hoặc nằm trong wwwroot/images.
    // Trả về đường dẫn ~/... để Url.Content xử lý đúng cả khi chạy dưới PathBase.
    public static string? AnhNoiBo(string? duongDan)
    {
        if (string.IsNullOrWhiteSpace(duongDan)) return null;
        string path = duongDan.Trim().Replace('\\', '/');
        if (path.StartsWith("~/", StringComparison.Ordinal)) path = path[2..];
        else if (path.StartsWith("/", StringComparison.Ordinal)
                 && !path.StartsWith("//", StringComparison.Ordinal)) path = path[1..];

        if (!(path.StartsWith("uploads/", StringComparison.OrdinalIgnoreCase)
              || path.StartsWith("images/", StringComparison.OrdinalIgnoreCase))) return null;
        if (path.Contains("..", StringComparison.Ordinal)
            || path.Contains(':') || path.Contains('%')) return null;
        return "~/" + path;
    }

    // Không cho phép javascript:, data:, file: hoặc URL thiếu giao thức.
    // Đây chỉ là kiểm tra định dạng URL; không xác nhận uy tín nội dung website đích.
    public static string? DatasheetHttp(string? value)
    {
        if (!Uri.TryCreate(value?.Trim(), UriKind.Absolute, out var uri)) return null;
        if ((uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp)
            || string.IsNullOrWhiteSpace(uri.Host)) return null;
        return uri.AbsoluteUri;
    }
}
