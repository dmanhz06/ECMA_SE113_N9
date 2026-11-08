using ECMA.Domain.Entities;
using ECMA.Domain.Interfaces;
using ECMA.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECMA.Infrastructure.Repositories;

public sealed class UserRepository(EcmaDbContext context) : IUserRepository
{
    public async Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users.FindAsync([id], cancellationToken);
    }

    public async Task<User?> FindByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await context.Database.ExecuteSqlInterpolatedAsync($"SELECT id FROM users WHERE id = {id} FOR UPDATE", cancellationToken);
        return await context.Users.FindAsync([id], cancellationToken);
    }

    public async Task<User?> FindByUsernameAsync(string normalizedUsername, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Username == normalizedUsername, cancellationToken);
    }

    public async Task<bool> UsernameExistsAsync(string normalizedUsername, CancellationToken cancellationToken = default)
    {
        return await context.Users.AnyAsync(u => u.Username == normalizedUsername, cancellationToken);
    }

    public void Add(User user)
    {
        context.Users.Add(user);
    }

    public async Task<bool> TryUpdatePasswordAsync(
        Guid userId,
        string expectedHash,
        string expectedSecurityStamp,
        string newHash,
        string newSecurityStamp,
        CancellationToken cancellationToken = default)
    {
        var affected = await context.Users
            .Where(u => u.Id == userId && u.PasswordHash == expectedHash && u.SecurityStamp == expectedSecurityStamp)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.PasswordHash, newHash)
                .SetProperty(u => u.SecurityStamp, newSecurityStamp), cancellationToken);
        return affected == 1;
    }
}