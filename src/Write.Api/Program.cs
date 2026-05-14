using Domain.Products.Repositories;
using FluentValidation;
using MediatR;
using System.Reflection;
using Write.Api.Application.Behaviors;
using Write.Api.Application.Products.Create;
using Write.Api.Infrastructure.ExceptionHandling;
using Write.Api.Infrastructure.Persistence;
using Write.Api.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WriteDbContext>();
builder.Services.AddScoped<IProductWriteRepository, ProductRepository>();

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();

app.MapPost("/api", async (
    IMediator mediator,
    Command command,
    CancellationToken cancellationToken) =>
{
    var id = await mediator.Send(command, cancellationToken);
    return Results.Created($"/api/{id}", new { id });
});

app.UseSwagger();
app.UseSwaggerUI();
app.Run();
