namespace ECMA.Domain.Interfaces;

public enum PasswordVerificationResult
{
    Failed,
    Verified,
    VerifiedNeedsRehash
}

public interface IPasswordService
{
    string HashPassword(string password);
    PasswordVerificationResult VerifyPassword(string password, string hashedPassword);
}