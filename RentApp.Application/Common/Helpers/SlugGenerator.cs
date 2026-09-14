using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using RentApp.Application.Interfaces.Helpers;

namespace RentApp.Application.Common.Helpers;

public partial class SlugGenerator : ISlugGenerator
{
    public string Generate(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Guid.NewGuid().ToString("n")[..8];

        // Normalize string
        var normalizedString = text.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        var cleaned = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

        // Replace invalid characters with hyphens
        cleaned = Regex.Replace(cleaned, @"[^a-z0-9\s-]", "");
        // Convert multiple spaces/hyphens into single hyphen
        cleaned = Regex.Replace(cleaned, @"[\s-]+", "-").Trim('-');

        if (string.IsNullOrWhiteSpace(cleaned))
        {
            cleaned = Guid.NewGuid().ToString("n")[..8];
        }

        return cleaned;
    }
}
