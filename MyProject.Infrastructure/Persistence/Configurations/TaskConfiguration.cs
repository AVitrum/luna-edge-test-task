using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyProject.Domain.Entities;
using Task = MyProject.Domain.Entities.Task;

namespace MyProject.Infrastructure.Persistence.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.Property(t => t.Status)
            .HasConversion<string>()
            .IsRequired();
        
        builder.Property(t => t.Priority)
            .HasConversion<string>()
            .IsRequired();
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(t => t.UpdatedAt)
            .IsRequired()
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}