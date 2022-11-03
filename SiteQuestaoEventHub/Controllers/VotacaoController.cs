using Microsoft.AspNetCore.Mvc;
using SiteQuestaoEventHub.EventHubs;

namespace SiteQuestaoEventHub.Controllers;

public class VotacaoController : Controller
{
    private readonly ILogger<VotacaoController> _logger;
    private readonly VotacaoProducer _producer;

    public VotacaoController(ILogger<VotacaoController> logger,
        VotacaoProducer producer)
    {
        _logger = logger;
        _producer = producer;
    }

    public async Task<IActionResult> VotoEsportes()
    {
        return await ProcessarVoto("Esportes");
    }

    public async Task<IActionResult> VotoTecnologia()
    {
        return await ProcessarVoto("Tecnologia");
    }

    public async Task<IActionResult> VotoFilmes()
    {
        return await ProcessarVoto("Filmes");
    }

    public async Task<IActionResult> VotoSeriados()
    {
        return await ProcessarVoto("Seriados");
    }

    private async Task<IActionResult> ProcessarVoto(string interesse)
    {
        _logger.LogInformation($"Processando voto para o interesse: {interesse}");
        await _producer.Send(interesse);
        _logger.LogInformation($"Informações sobre o voto '{interesse}' enviadas para o Azure Event Hubs!");

        TempData["Voto"] = interesse;
        return RedirectToAction("Index", "Home", new { voto = interesse });
    }
}