using DomainUser = ECMA.Domain.Entities.User;
using DomainPasswordResult = ECMA.Domain.Interfaces.PasswordVerificationResult;
using ECMA.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ECMA.Infrastructure.Services;

public sealed class PasswordHasherAdapter : IPasswordService
{
    private readonly PasswordHasher<DomainUser> _hasher = new();

    public string HashPassword(string password)
    {
        return _hasher.HashPassword(null!, password);
    }

    public DomainPasswordResult VerifyPassword(string password, string hashedPassword)
    {
        var result = _hasher.VerifyHashedPassword(null!, hashedPassword, password);
        return result switch
        {
            Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success => DomainPasswordResult.Verified,
            Microsoft.AspNetCore.Identity.PasswordVerificationResult.SuccessRehashNeeded => DomainPasswordResult.VerifiedNeedsRehash,
            _ => DomainPasswordResult.Failed
        };
    }
}