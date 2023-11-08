using Microsoft.EntityFrameworkCore;
using N71_C.DataContexts;

var options = new DbContextOptionsBuilder<LibraryDbContext>();
options.UseNpgsql("Host=localhost;Port=5432;Database=CrudExample;Username=postgres;Password=postgres");

// add simple logger 
options.LogTo(Console.WriteLine);

var dbContext = new LibraryDbContext(options.Options);


// Console.WriteLine(dbContext.Books.Count());

