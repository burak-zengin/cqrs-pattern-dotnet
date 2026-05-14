using Domain.Products.ReadModels;
using Domain.Products.Repositories;
using MediatR;

namespace Read.Api.Application.Products.Get;

public class Handler(IProductReadRepository repository) : IRequestHandler<Query, ProductReadModel?>
{
    public async Task<ProductReadModel?> Handle(Query request, CancellationToken cancellationToken)
    {
        return await repository.GetAsync(request.Id, cancellationToken);
    }
}
