using ECMA.Domain.Interfaces;

namespace ECMA.Infrastructure.Services;

public sealed class Clock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}