using Domain.Shared.AppError.Constants;
using Domain.Shared.Constants;
using Domain.UserAggregate.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VotepucApp.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.Property(x => x.UserName)
            .HasMaxLength(LengthProperties.PersonNameMaxLength)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(LengthProperties.PersonEmailMaxLength)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.PasswordHash)
            .IsRequired();
        
    }
}