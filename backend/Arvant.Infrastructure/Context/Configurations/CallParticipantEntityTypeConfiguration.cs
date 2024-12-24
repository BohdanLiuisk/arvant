using Arvant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arvant.Infrastructure.Context.Configurations;

public class CallParticipantEntityTypeConfiguration : IEntityTypeConfiguration<CallParticipant>
{
    public void Configure(EntityTypeBuilder<CallParticipant> builder)
    {
        builder.HasKey(p => p.Id);
        builder
            .HasOne(p => p.Participant)
            .WithMany()
            .HasForeignKey(p => p.ParticipantId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
