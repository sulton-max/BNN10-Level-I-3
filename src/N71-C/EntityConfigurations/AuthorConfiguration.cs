using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N71_C.Models;

namespace N71_C.EntityConfigurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasData(
            new Author
            {
                Id = Guid.Parse("4941DEC3-36F6-412D-BABC-ABD92B79B9BB"),
                Name = "Andrew Troelsen"
            }
        );
    }
}