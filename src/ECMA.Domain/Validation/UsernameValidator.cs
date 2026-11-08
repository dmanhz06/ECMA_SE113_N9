using System.Text;
using System.Text.RegularExpressions;

namespace ECMA.Domain.Validation;

public static class UsernameValidator
{
    private static readonly Regex Pattern = new(@"^[A-Za-z][A-Za-z0-9_]{3,19}$", RegexOptions.Compiled);

    public static bool IsValid(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return false;

        string normalized = username.Normalize(NormalizationForm.FormC);
        return Pattern.IsMatch(normalized);
    }

    public static string Normalize(string username)
    {
        return username.ToLowerInvariant();
    }
}