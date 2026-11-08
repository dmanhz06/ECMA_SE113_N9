using System.Text;

namespace ECMA.Domain.Validation;

public static class FullNameValidator
{
    private const int MinLength = 2;
    private const int MaxLength = 100;

    public static bool IsValid(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return false;

        string normalized = fullName.Normalize(NormalizationForm.FormC);
        int length = normalized.EnumerateRunes().Count();
        return length >= MinLength && length <= MaxLength;
    }
}