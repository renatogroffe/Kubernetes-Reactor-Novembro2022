using System.Text.Json;
using Azure.Messaging.EventHubs.Producer;

namespace SiteQuestaoEventHub.EventHubs;

public class VotacaoProducer
{
    private readonly ILogger<VotacaoProducer> _logger;
    private readonly IConfiguration _configuration;
    private readonly JsonSerializerOptions _serializerOptions;

    public VotacaoProducer(
        ILogger<VotacaoProducer> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    private string SerializeEventData<T>(T eventData)
    {
        string data = JsonSerializer.Serialize(eventData, _serializerOptions);
        _logger.LogInformation($"Evento: {data}");
        return data;
    }

    public async Task Send(string interesse)
    {
        EventHubProducerClient? producerClient = null;
        try
        {
            producerClient = new EventHubProducerClient(
                _configuration.GetConnectionString("AzureEventHubs"),
                _configuration["AzureEventHubs:EventHub"]);

            using var eventBatch = await producerClient.CreateBatchAsync();
            _logger.LogInformation("Gerando o Batch para envio dos eventos...");

            var idVoto = Guid.NewGuid().ToString();
            var horario = $"{DateTime.UtcNow.AddHours(-3):yyyy-MM-dd HH:mm:ss}";

            eventBatch.AddEvent(SerializeEventData<Voto>(new()
            {
                IdVoto = idVoto,
                Horario = horario,
                Producer = Environment.MachineName,
                Interesse = interesse
            }));

            await producerClient.SendAsync(eventBatch);
            _logger.LogInformation("Concluido o envio dos eventos!");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exceção: {ex.GetType().FullName} | " +
                             $"Mensagem: {ex.Message}");
        }
        finally
        {
            if (producerClient is not null)
            {
                await producerClient.DisposeAsync();
                _logger.LogInformation(
                    "Conexao com o Azure Event Hubs finalizada!");
            }
        }
    }
}