using Domain.ElectionAggregate.VoteLink;
using Domain.ElectionAggregate.VoteLink.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VotepucApp.Persistence.Configurations;

public class VoteLinkConfiguration : IEntityTypeConfiguration<VoteLink>
{
    public void Configure(EntityTypeBuilder<VoteLink> builder)
    {
        builder.ToTable("VoteLinks");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasOne(v => v.Election)
            .WithMany(e => e.VoteLinks)
            .HasForeignKey(v => v.ElectionId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.Property(x => x.Token)
            .IsRequired();
        
        builder.Property(x => x.ExpirationDate)
            .IsRequired();

        builder.Property(x => x.WasUtilized)
            .IsRequired();
        
        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.HasDiscriminator<VoteLinkStatusEnum>("VoteLinkStatus")
            .HasValue<InactiveVoteLink>(VoteLinkStatusEnum.Inactive)
            .HasValue<ActiveVoteLink>(VoteLinkStatusEnum.Active);
    }
}