using Domain.Products;
using Domain.Products.Repositories;
using Write.Api.Infrastructure.Persistence;

namespace Write.Api.Infrastructure.Repositories;

public class ProductRepository(WriteDbContext context) : IProductWriteRepository
{
    public async Task<int> CreateAsync(Product product, CancellationToken cancellationToken)
    {
        context.Add(product);
        await context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
