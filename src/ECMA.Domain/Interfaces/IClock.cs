namespace ECMA.Domain.Interfaces;

public interface IClock
{
    DateTime UtcNow { get; }
}