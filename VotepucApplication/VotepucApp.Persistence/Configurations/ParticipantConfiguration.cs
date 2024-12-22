using Domain.ElectionAggregate.Participant;
using Domain.ElectionAggregate.Participant.Enumerations;
using Domain.Shared.AppError.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VotepucApp.Persistence.Configurations;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable("Participants");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasOne(p => p.Election)
            .WithMany(e => e.Participants)
            .HasForeignKey(p => p.ElectionId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(ConstantsMaxLength.PersonNameMaxLength)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(ConstantsMaxLength.PersonEmailMaxLength)
            .IsRequired();
        
        builder.Property(x => x.TypeOfParticipant)
            .HasConversion<string>()
            .IsRequired();

        builder.HasDiscriminator<TypeOfParticipantEnum>("TypeOfParticipant")
            .HasValue<Candidate>(TypeOfParticipantEnum.Candidate)
            .HasValue<Voter>(TypeOfParticipantEnum.Voter);
    }
}