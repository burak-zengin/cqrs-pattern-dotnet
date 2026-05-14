using Domain.Products.ReadModels;
using MediatR;

namespace Read.Api.Application.Products.Get;

public record Query(int Id) : IRequest<ProductReadModel?>;
