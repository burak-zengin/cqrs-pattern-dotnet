using Domain.Products.ReadModels;
using Domain.Products.Repositories;
using MediatR;

namespace Read.Api.Application.Products.GetAll;

public class Handler(IProductReadRepository repository) : IRequestHandler<Query, List<ProductReadModel>>
{
    public async Task<List<ProductReadModel>> Handle(Query request, CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync(cancellationToken);
    }
}
