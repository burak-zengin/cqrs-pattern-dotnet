namespace Domain.Products.Repositories;

public interface IProductWriteRepository
{
    Task<int> CreateAsync(Product product, CancellationToken cancellationToken);
}
