using ICanUseCassandra;

var dbContext = new CassandraDbContext();
await dbContext.CreateSchema();

var id = Guid.NewGuid();
await dbContext.Insert(new Book()
{
    Id = id,
    Author = "Test Author",
    Genre = "Fantasy",
    Title = "Test title",
    Year = 2005
});

var result = await dbContext.GetById(id);
Console.WriteLine($"Id = {result.Id}, Author = {result.Author}, Genre = {result.Genre}" +
                  $", Title = {result.Title}, Year = {result.Year}");

var all = await dbContext.GetAll();
Console.WriteLine(all.Count());
await dbContext.Delete(id);
all = await dbContext.GetAll();
Console.WriteLine(all.Count());