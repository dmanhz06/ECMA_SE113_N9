using ECMA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECMA.Infrastructure.Data.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id).HasName("pk_users");
        builder.Property(u => u.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        builder.Property(u => u.Username).HasColumnName("username").IsRequired().HasMaxLength(20);
        builder.Property(u => u.PasswordHash).HasColumnName("password_hash").IsRequired().HasMaxLength(500);
        builder.Property(u => u.SecurityStamp).HasColumnName("security_stamp").IsRequired().HasMaxLength(100);
        builder.Property(u => u.FullName).HasColumnName("full_name").IsRequired().HasMaxLength(100);
        builder.Property(u => u.Role).HasColumnName("role").IsRequired();
        builder.Property(u => u.Status).HasColumnName("status").IsRequired();
        builder.Property(u => u.CreatedAt).HasColumnName("created_at").IsRequired();

        builder.HasIndex(u => u.Username).IsUnique().HasDatabaseName("uq_users_username");
    }
}