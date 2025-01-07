using Cassandra;

namespace ICanUseCassandra;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public string Genre { get; set; }

    public static async Task CreateTableAsync(ISession session)
    {
        await session.ExecuteAsync(new SimpleStatement(
            """
            CREATE TABLE IF NOT EXISTS book (
                id uuid PRIMARY KEY,
                title text,
                author text,
                year int,
                genre text
            )
            """));
    }
}