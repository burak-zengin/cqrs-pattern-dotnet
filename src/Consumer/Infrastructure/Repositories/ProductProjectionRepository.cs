using Domain.Products.ReadModels;
using Domain.Products.Repositories;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Consumer.Infrastructure.Repositories;

public sealed class ProductProjectionRepository : IProductProjectionRepository
{
    private readonly IMongoCollection<ProductReadModel> _collection;

    public ProductProjectionRepository(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
        var database = client.GetDatabase("Products");
        _collection = database.GetCollection<ProductReadModel>("ProductCollection");
    }

    public async Task UpsertAsync(ProductReadModel readModel, CancellationToken cancellationToken)
    {
        var filter = Builders<ProductReadModel>.Filter.Eq(p => p.Id, readModel.Id);
        var options = new ReplaceOptions { IsUpsert = true };
        await _collection.ReplaceOneAsync(filter, readModel, options, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var filter = Builders<ProductReadModel>.Filter.Eq(p => p.Id, id);
        await _collection.DeleteOneAsync(filter, cancellationToken);
    }

    public async Task<long?> GetCurrentVersionAsync(int id, CancellationToken cancellationToken)
    {
        var projection = Builders<ProductReadModel>.Projection.Include(p => p.Version);
        var doc = await _collection
            .Find(Builders<ProductReadModel>.Filter.Eq(p => p.Id, id))
            .Project<ProductReadModel>(projection)
            .FirstOrDefaultAsync(cancellationToken);

        return doc?.Version;
    }
}
