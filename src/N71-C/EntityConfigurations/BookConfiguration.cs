using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N71_C.Models;

namespace N71_C.EntityConfigurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        // builder.HasData(new Book
        //     {
        //         Id = Guid.Parse("E9582B0D-A6C7-4FDB-B17B-7E9D64E5B784"),
        //         Title = "Asp.Net Core in Action (2018)",
        //         Genre = "Programming",
        //         Pages = 616,
        //         AuthorId = Guid.Parse("4941DEC3-36F6-412D-BABC-ABD92B79B9BB"),
        //     },
        //     new Book
        //     {
        //         Id = Guid.Parse("C50443A7-B9DA-4060-A827-E9360A7491CA"),
        //         Title = "Pro C# 7: With .NET and .NET Core (2017)",
        //         Genre = "Programming",
        //         Pages = 1372,
        //         AuthorId = Guid.Parse("4941DEC3-36F6-412D-BABC-ABD92B79B9BB"),
        //     });

        // One-to-One - HasOne WithOne
        // One-to-Many - HasMany WithOne yoki HasOne WithMany
        // Many-to-Many - HasMany WithMany

        builder.HasOne<Author>().WithMany(author => author.Books).HasForeignKey(book => book.AuthorId).OnDelete(DeleteBehavior.Restrict);
    }
}