using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stize.ApiTemplate.Domain.Entities;

namespace Stize.ApiTemplate.Domain.EFCore.Configuration
{
    public class ToDoListConfiguration : IEntityTypeConfiguration<ToDoList>
    {
        public void Configure(EntityTypeBuilder<ToDoList> builder)
        {
            builder.HasMany(x => x.TodoItem)
                .WithOne(x => x.ToDoList)
                .HasForeignKey(x => x.ToDoListId)
                .IsRequired();
        }
    }
}