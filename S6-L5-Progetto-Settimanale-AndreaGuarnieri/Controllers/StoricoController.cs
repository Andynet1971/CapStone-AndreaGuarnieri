using CapStone_AndreaGuarnieri.Models.Services;
using CapStone_AndreaGuarnieri.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

public class StoricoController : Controller
{
    private readonly StoricoService _storicoService;

    public StoricoController(StoricoService storicoService)
    {
        _storicoService = storicoService;
    }

    public IActionResult Storico(string periodo)
    {
        var viewModel = new StoricoViewModel();

        DateTime dataInizio, dataFine;
        switch (periodo)
        {
            case "ultimoMese":
                dataInizio = DateTime.Now.AddMonths(-1);
                dataFine = DateTime.Now;
                break;
            case "ultimi3Mesi":
                dataInizio = DateTime.Now.AddMonths(-3);
                dataFine = DateTime.Now;
                break;
            case "ultimi6Mesi":
                dataInizio = DateTime.Now.AddMonths(-6);
                dataFine = DateTime.Now;
                break;
            case "ultimoAnno":
                dataInizio = DateTime.Now.AddYears(-1);
                dataFine = DateTime.Now;
                break;
            default:
                dataInizio = DateTime.Now.AddMonths(-1);
                dataFine = DateTime.Now;
                break;
        }

        viewModel.OccupazioneMedia = _storicoService.GetOccupazioneMedia(dataInizio, dataFine);
        viewModel.DurataMediaSoggiorni = _storicoService.GetDurataMediaSoggiorni(dataInizio, dataFine);
        viewModel.IncassoTotale = _storicoService.GetIncassoTotale(dataInizio, dataFine);
        viewModel.UtilizzoServiziAggiuntivi = _storicoService.GetUtilizzoServiziAggiuntivi(dataInizio, dataFine);
        viewModel.PercentualeClientiConServiziAggiuntivi = _storicoService.GetPercentualeClientiConServiziAggiuntivi(dataInizio, dataFine);

        return View(viewModel);
    }

}
