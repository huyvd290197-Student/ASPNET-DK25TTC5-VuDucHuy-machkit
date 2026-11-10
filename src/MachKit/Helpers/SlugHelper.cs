using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MachKit.Helpers;

public static class SlugHelper
{
    public static string Generate(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return "";

        text = text.Trim().Replace('đ', 'd').Replace('Đ', 'D');
        string normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (char c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
                continue;

            if (char.IsLetterOrDigit(c)) sb.Append(char.ToLowerInvariant(c));
            else sb.Append('-');
        }

        string slug = Regex.Replace(sb.ToString(), "-+", "-").Trim('-');
        return slug;
    }
}
