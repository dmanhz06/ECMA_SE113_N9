using ECMA.Domain.Interfaces;

namespace ECMA.Infrastructure.Services;

public sealed class SecurityStampGenerator : ISecurityStampGenerator
{
    public string GenerateStamp()
    {
        return Guid.NewGuid().ToString("N");
    }
}