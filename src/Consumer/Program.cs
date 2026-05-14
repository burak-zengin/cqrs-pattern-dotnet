using Confluent.Kafka;
using Consumer.Infrastructure.Messaging;
using Consumer.Infrastructure.Persistence;
using Consumer.Infrastructure.Projections;
using Consumer.Infrastructure.Repositories;
using Domain.Products.IntegrationEvents;
using Domain.Products.Projections;
using Domain.Products.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

MongoConfiguration.RegisterClassMaps();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddSingleton<IProductProjector, ProductProjector>();
services.AddScoped<IProductProjectionRepository, ProductProjectionRepository>();
var serviceProvider = services.BuildServiceProvider();

var bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "broker:29092";
var groupId = configuration["Kafka:GroupId"] ?? "product-projection-consumer";
var topic = configuration["Kafka:Topic"] ?? "datatransfer.public.Products";

var cancellationSource = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cancellationSource.Cancel();
};

var consumerConfig = new ConsumerConfig
{
    GroupId = groupId,
    BootstrapServers = bootstrapServers,
    AutoOffsetReset = AutoOffsetReset.Earliest,
    EnableAutoCommit = false
};

using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
consumer.Subscribe(topic);

Console.WriteLine($"[Consumer] Listening topic '{topic}' on {bootstrapServers}...");

try
{
    while (!cancellationSource.IsCancellationRequested)
    {
        ConsumeResult<Ignore, string>? result;
        try
        {
            result = consumer.Consume(cancellationSource.Token);
        }
        catch (OperationCanceledException)
        {
            break;
        }
        catch (ConsumeException ex)
        {
            Console.Error.WriteLine($"[Consumer] Consume error: {ex.Error.Reason}");
            continue;
        }

        if (result?.Message?.Value is null)
        {
            continue;
        }

        try
        {
            await HandleAsync(result.Message.Value, cancellationSource.Token);
            consumer.Commit(result);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Consumer] Handler error (offset {result.Offset}): {ex.Message}");
        }
    }
}
finally
{
    consumer.Close();
}

async Task HandleAsync(string raw, CancellationToken cancellationToken)
{
    DebeziumEnvelope<DebeziumProduct>? envelope;
    try
    {
        envelope = JsonSerializer.Deserialize<DebeziumEnvelope<DebeziumProduct>>(raw);
    }
    catch (JsonException ex)
    {
        Console.Error.WriteLine($"[Consumer] Invalid JSON payload: {ex.Message}");
        return;
    }

    var integrationEvent = DebeziumEnvelopeMapper.Map(envelope);
    if (integrationEvent is null || integrationEvent.Operation == ProductOperation.Unknown)
    {
        return;
    }

    using var scope = serviceProvider.CreateScope();
    var projector = scope.ServiceProvider.GetRequiredService<IProductProjector>();
    var projection = scope.ServiceProvider.GetRequiredService<IProductProjectionRepository>();

    var currentVersion = await projection.GetCurrentVersionAsync(integrationEvent.ProductId, cancellationToken);
    if (currentVersion is { } v && integrationEvent.Version <= v)
    {
        return;
    }

    switch (integrationEvent.Operation)
    {
        case ProductOperation.Created:
        case ProductOperation.Updated:
        case ProductOperation.Snapshot:
            var readModel = projector.Project(integrationEvent);
            await projection.UpsertAsync(readModel, cancellationToken);
            break;

        case ProductOperation.Deleted:
            await projection.DeleteAsync(integrationEvent.ProductId, cancellationToken);
            break;
    }
}
