using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Election.Constants;
using Domain.ElectionAggregate.Election.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VotepucApp.Persistence.Configurations;

public class ElectionConfiguration : IEntityTypeConfiguration<Election>
{
    public void Configure(EntityTypeBuilder<Election> builder)
    {
        builder.ToTable("Elections");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasOne(e => e.Owner)
            .WithMany(u => u.Elections)
            .HasForeignKey(e => e.OwnerId)
            .IsRequired();
        
        builder.Property(x => x.Title)
            .HasMaxLength(ConstantsElectionValidations.TitleMaxLength)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(ConstantsElectionValidations.DescriptionMaxLength)
            .IsRequired();

        builder.Property(x => x.EmailInvitationText)
            .HasMaxLength(ConstantsElectionValidations.EmailInvitationTextMaxLength)
            .IsRequired();

        builder.Property(x => x.Progress)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();
        
        builder.HasDiscriminator<ElectionStatusEnum>("ElectionStatus")
            .HasValue<PendingElection>(ElectionStatusEnum.Pending)
            .HasValue<ElectionRejected>(ElectionStatusEnum.Rejected)
            .HasValue<ElectionApproved>(ElectionStatusEnum.Approved);
    }
}