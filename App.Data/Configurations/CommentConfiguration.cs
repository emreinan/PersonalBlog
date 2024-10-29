using App.Data.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Content).IsRequired().HasMaxLength(1000);
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.IsApproved).IsRequired();

        //builder.HasOne(c => c.Post)
        //    .WithMany(bp => bp.Comments)
        //    .HasForeignKey(c => c.PostId)
        //    .OnDelete(DeleteBehavior.Cascade);

        new CommentSeed().Configure(builder);
    }
}
internal class CommentSeed : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasData(
             new Comment
             {
                 Id = 1,
                 Content = "Great post!",
                 CreatedAt = DateTime.Now,
                 IsApproved = true,
                 PostId = new Guid("24900544-58fc-4be7-9ab1-18f088510da4"),
                 UserId = new Guid("a8647b9a-fbf3-4867-ba8f-5fc8239bff12")
             },
            new Comment
            {
                Id = 2,
                Content = "Thanks for sharing!",
                CreatedAt = DateTime.Now,
                IsApproved = true,
                PostId = new Guid("7aa98865-285c-4c67-a96a-8fcd30855234"),
                UserId = new Guid("44063c52-807a-40bf-a65c-a06d5ac3ee26")
            },
            new Comment
            {
                Id = 3,
                Content = "Awesome!",
                CreatedAt = DateTime.Now,
                IsApproved = true,
                PostId = new Guid("b860336c-64af-497d-9bbd-ed95fd752e9f"),
                UserId = new Guid("a8647b9a-fbf3-4867-ba8f-5fc8239bff12")
            },
            new Comment
            {
                Id = 4,
                Content = "Nice work!",
                CreatedAt = DateTime.Now,
                IsApproved = true,
                PostId = new Guid("24900544-58fc-4be7-9ab1-18f088510da4"),
                UserId = new Guid("44063c52-807a-40bf-a65c-a06d5ac3ee26")
            },
            new Comment
            {
                Id = 5,
                Content = "Keep it up!",
                CreatedAt = DateTime.Now,
                IsApproved = true,
                PostId = new Guid("7aa98865-285c-4c67-a96a-8fcd30855234"),
                UserId = new Guid("a8647b9a-fbf3-4867-ba8f-5fc8239bff12")
            },
            new Comment
            {
                Id = 6,
                Content = "Well done!",
                CreatedAt = DateTime.Now,
                IsApproved = true,
                PostId = new Guid("b860336c-64af-497d-9bbd-ed95fd752e9f"),
                UserId = new Guid("7c117612-fb38-48c9-9908-97b433a3f92b")
            },
            new Comment
            {
                Id = 7,
                Content = "I love it!",
                CreatedAt = DateTime.Now,
                IsApproved = false,
                PostId = new Guid("24900544-58fc-4be7-9ab1-18f088510da4"),
                UserId = new Guid("7c117612-fb38-48c9-9908-97b433a3f92b")
            },
            new Comment
            {
                Id = 8,
                Content = "This is great!",
                CreatedAt = DateTime.Now,
                IsApproved = false,
                PostId = new Guid("7aa98865-285c-4c67-a96a-8fcd30855234"),
                UserId = new Guid("44063c52-807a-40bf-a65c-a06d5ac3ee26")
            },
            new Comment
            {
                Id = 9,
                Content = "I like it!",
                CreatedAt = DateTime.Now,
                IsApproved = false,
                PostId = new Guid("b860336c-64af-497d-9bbd-ed95fd752e9f"),
                UserId = new Guid("7c117612-fb38-48c9-9908-97b433a3f92b")
            }
        );
    }
}
