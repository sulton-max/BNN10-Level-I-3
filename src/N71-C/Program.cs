using Microsoft.EntityFrameworkCore;
using N71_C.DataContexts;

var options = new DbContextOptionsBuilder<LibraryDbContext>();
options.UseNpgsql("Host=localhost;Port=5432;Database=CrudExample;Username=postgres;Password=postgres");

// add simple logger 
options.LogTo(Console.WriteLine);

var dbContext = new LibraryDbContext(options.Options);

// var author = await dbContext.Authors.FirstOrDefaultAsync();

// Change tracker - entitylarni o'zgarishlarini saqlab va kuzatib boradi

// Entity entry - entity va u haqida ma'lumotlar ( ex. entity state )

// Entity state - bu entityni holatini EF core qanday tasavvur qilishi

// Detached - EF Core bu entityni track qilmayapti, ex - as no tracking, agar o'zgarish bilan save changes chaqirsak - hech nima bo'lmaydi, hali EF Core ga qo'shilmagan entity bo'lsa
// Added - EF Core da entity qo'shilganini bildiradi
// Unchanged - EF core entity ni track qilyapti, faqat entity da o'zgarishlar yo'q
// Modified - EF core entity update bo'lgan deb hisoblaydi
// Deleted - EF core entity ni delete bo'lgan deb hisoblaydi

// author.Name = "Jonibek";
//
// Console.WriteLine("before change - " + dbContext.Entry(author).State);
//
// dbContext.Update(author);
//
// Console.WriteLine("before change - " + dbContext.Entry(author).State);
//
// dbContext.SaveChanges();
//
// Console.WriteLine("before change - " + dbContext.Entry(author).State);
//
// Console.WriteLine(JsonSerializer.Serialize(author));


// Create - entitni EF corega qo'shish, bu entity ni Added state qiladi, SaveChanges dan keyin - Unchanged
// var newAuthor = new Author
// {
//     Name = "J.J.Gewax"
// };
//
// Console.WriteLine("before change - " + dbContext.Entry(newAuthor).State);
//
// dbContext.Add(newAuthor);
//
// Console.WriteLine("before change - " + dbContext.Entry(newAuthor).State);
//
// dbContext.SaveChanges();
//
// Console.WriteLine("before change - " + dbContext.Entry(newAuthor).State);
//
// newAuthor.Name = "Test";
//
// Console.WriteLine("before change - " + dbContext.Entry(newAuthor).State);
//
// Console.WriteLine(JsonSerializer.Serialize(newAuthor));

// Read - entitylarni EF core orqali o'qib olish, filter, search, order, include, pagination qilishimiz mumkin, default statei - Unchanged qachonki AsNoTracking qilmagunimizcha ( unda Detached state bo'ladi )
// var existingAuthor = dbContext.Authors.AsNoTracking()
//     .Include(author => author.Books.Where(book => book.Reviews.Any(review => review.Rating > 3)))
//     .ThenInclude(book => book.Reviews)
//     .Where(author => author.Books.Count > 1)
//     .OrderBy(author => author.Name)
//     .Skip(0)
//     .Take(10);
//
// Console.WriteLine(JsonSerializer.Serialize(existingAuthor));

// Update - entityni update qilish, EF Core da Modified state bilan saqlanadi

// appraoch - connected update and disconnected update

// connected update - bitta entity db context o'zi query qilsa va shu context orqali update bo'lsa
// var authorToUpdate = await dbContext.Authors.FirstOrDefaultAsync();
//
// authorToUpdate.Name = "Michael";
// dbContext.Update(authorToUpdate);
//
// await dbContext.SaveChangesAsync();
//
// // disconnected update - bitta entity ni db context update qiladi lekin query qilmadi
// dbContext.Dispose();
// dbContext = null;
//
// var anotherDbContext = new LibraryDbContext(options.Options);
//
// authorToUpdate.Name = "Jordan";
// anotherDbContext.Update(authorToUpdate);
//
// await anotherDbContext.SaveChangesAsync();
//
// anotherDbContext.Dispose();
// anotherDbContext = null;

// var yetAnotherDbContext = new LibraryDbContext(options.Options);
//
// var otherAuthor = new Author
// {
//     Id = Guid.Parse("559f7e9d-9d9f-4e04-b92a-edfa4543654d"),
//     Name = "G'ishtmat",
// };
//
// yetAnotherDbContext.Update(otherAuthor);
//
// await yetAnotherDbContext.SaveChangesAsync();


// Update with change tracker
// var lastUpdateExample = dbContext.Authors.OrderBy(author => author.Id).FirstOrDefault() ?? throw new InvalidOperationException();
//
// lastUpdateExample.Age = 25;
//
// await dbContext.SaveChangesAsync();

// Delete - entity ni EF Core o'chiradi, entity state - Deleted
var authorToDelete = dbContext.Authors.OrderBy(author => author.Id).FirstOrDefault() ?? throw new InvalidOperationException();

Console.WriteLine("test");

var yetauthorToDelete = dbContext.Authors.OrderBy(author => author.Id).FirstOrDefault() ?? throw new InvalidOperationException();

Console.WriteLine(yetauthorToDelete.Name);

// Delete behavior - entity o'chirilganda shu entityga bog'liq bo'lgan entitylarni nima qilish kerakligini belgilaydi

// Cascade delete - entity bog'liq relationlarni ham o'chiradi - default behavior

dbContext.Remove(authorToDelete);

// Restrict - entityga bo'g;liq relationlarda entity bo'ladigan asosiy o'chayotgan entityni o'chishiga xalaqit beradi

await dbContext.SaveChangesAsync();