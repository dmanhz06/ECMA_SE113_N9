using System.Text;
using ECMA.Domain.Entities;
using ECMA.Domain.Enums;
using ECMA.Domain.Interfaces;
using ECMA.Domain.Validation;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ECMA.Infrastructure.Data;

public sealed class AdminSeeder(
    EcmaDbContext context,
    IPasswordService passwords,
    ISecurityStampGenerator stamps)
{
    public async Task SeedAsync(string? configuredUsername, string? configuredFullName, string? password, CancellationToken cancellationToken = default)
    {
        var username = configuredUsername?.Normalize(NormalizationForm.FormC).Trim() ?? "";
        var fullName = configuredFullName?.Normalize(NormalizationForm.FormC).Trim() ?? "";

        if (!UsernameValidator.IsValid(username))
            throw new InvalidOperationException("Seed:AdminUsername không hợp lệ");
        if (!FullNameValidator.IsValid(fullName))
            throw new InvalidOperationException("Seed:AdminFullName không hợp lệ");
        if (!PasswordValidator.IsValid(password ?? ""))
            throw new InvalidOperationException("Seed:AdminPassword không hợp lệ");

        username = UsernameValidator.Normalize(username);
        await context.Database.MigrateAsync(cancellationToken);

        if (await context.Users.AnyAsync(user => user.Username == username, cancellationToken))
            return;

        context.Users.Add(new User(
            username,
            passwords.HashPassword(password!),
            stamps.GenerateStamp(),
            fullName,
            UserRole.Admin,
            UserStatus.Active));

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when
            (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation, ConstraintName: "uq_users_username" })
        {
            // Concurrent seed created same username. Existing account remains authoritative.
        }
    }
}
