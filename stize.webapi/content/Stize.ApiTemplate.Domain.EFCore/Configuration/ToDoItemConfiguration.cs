using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stize.ApiTemplate.Domain.Entities;

namespace Stize.ApiTemplate.Domain.EFCore.Configuration
{
    public class ToDoItemConfiguration : IEntityTypeConfiguration<ToDoItem>
    {
        public void Configure(EntityTypeBuilder<ToDoItem> builder)
        {
            builder.HasOne(x => x.ToDoList)
                    .WithMany(x => x.TodoItem)
                    .HasForeignKey(x => x.ToDoListId)
                    .IsRequired();
        }
    }
}
