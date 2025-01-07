using Cassandra;
using Cassandra.Mapping;

namespace ICanUseCassandra;

public class CassandraDbContext : IDisposable
{
    private readonly ISession _session;
    private readonly ICluster _cluster;
    private readonly IMapper _mapper;

    public CassandraDbContext(string host = "localhost", int port = 9042)
    {
        _cluster = Cluster.Builder()
            .AddContactPoint(host)
            .WithPort(port)
            .Build();
        
        _session = _cluster.Connect();
        _mapper = new Mapper(_session);
    }
    
    public async Task CreateSchema()
    {
        await _session.ExecuteAsync(new SimpleStatement(
            """
            CREATE KEYSPACE IF NOT EXISTS library 
            WITH replication = {'class':'SimpleStrategy', 'replication_factor':1}
            """));

        await _session.ExecuteAsync(new SimpleStatement(
            "USE library"));

        await Book.CreateTableAsync(_session);
    }
    
    public async Task Insert(Book book)
    {
        await _mapper.InsertAsync(book);
    }
    
    public async Task<Book> GetById(Guid id)
    {
        return await _mapper.FirstOrDefaultAsync<Book>("WHERE id = ?", id);
    }

    public async Task<IEnumerable<Book>> GetAll()
    {
        return await _mapper.FetchAsync<Book>();
    }
    
    public async Task Update(Book book)
    {
        await _mapper.UpdateAsync(book);
    }
    
    public async Task Delete(Guid id)
    {
        await _mapper.DeleteAsync<Book>("WHERE id = ?", id);
    }

    public void Dispose()
    {
        _session.Dispose();
        _cluster.Dispose();
    }
}