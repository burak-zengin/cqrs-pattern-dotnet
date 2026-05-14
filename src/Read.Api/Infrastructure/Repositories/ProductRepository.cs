using Domain.Products.ReadModels;
using Domain.Products.Repositories;
using MongoDB.Driver;

namespace Read.Api.Infrastructure.Repositories;

public class ProductRepository : IProductReadRepository
{
    private readonly IMongoCollection<ProductReadModel> _collection;

    public ProductRepository(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
        var database = client.GetDatabase("Products");
        _collection = database.GetCollection<ProductReadModel>("ProductCollection");
    }

    public async Task<List<ProductReadModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _collection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<ProductReadModel?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync(cancellationToken);
    }
}
