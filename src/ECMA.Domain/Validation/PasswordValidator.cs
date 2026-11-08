using System.Text;

namespace ECMA.Domain.Validation;

public static class PasswordValidator
{
    private const int MinLength = 8;
    private const int MaxLength = 32;

    public static bool IsValid(string password)
    {
        if (string.IsNullOrEmpty(password) || password.Any(char.IsWhiteSpace))
            return false;

        int length = 0;
        bool hasLetter = false;
        bool hasDigit = false;

        foreach (Rune rune in password.EnumerateRunes())
        {
            length++;
            int value = rune.Value;
            hasLetter |= value is >= 'A' and <= 'Z' or >= 'a' and <= 'z';
            hasDigit |= value is >= '0' and <= '9';
        }

        return length is >= MinLength and <= MaxLength && hasLetter && hasDigit;
    }
}