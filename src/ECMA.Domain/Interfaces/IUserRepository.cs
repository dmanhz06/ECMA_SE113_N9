using ECMA.Domain.Entities;

namespace ECMA.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> FindByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> FindByUsernameAsync(string normalizedUsername, CancellationToken cancellationToken = default);
    Task<bool> UsernameExistsAsync(string normalizedUsername, CancellationToken cancellationToken = default);
    void Add(User user);

    Task<bool> TryUpdatePasswordAsync(
        Guid userId,
        string expectedHash,
        string expectedSecurityStamp,
        string newHash,
        string newSecurityStamp,
        CancellationToken cancellationToken = default);
}