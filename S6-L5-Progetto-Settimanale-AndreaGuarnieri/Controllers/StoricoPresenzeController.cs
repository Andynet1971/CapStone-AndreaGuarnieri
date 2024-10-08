﻿using CapStone_AndreaGuarnieri.Models.Services;
using CapStone_AndreaGuarnieri.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class StoricoPresenzeController : Controller
{
    private readonly StoricoService _storicoService;

    public StoricoPresenzeController(StoricoService storicoService)
    {
        _storicoService = storicoService;
    }
    public IActionResult Storico(string periodo)
    {
        var viewModel = new StoricoViewModel();  // Usa StoricoViewModel

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

        // Genera le date
        viewModel.Date = GeneraDate(dataInizio, dataFine);

        // Ottieni i dati per il grafico
        viewModel.TassoOccupazione = _storicoService.GetTassoOccupazione(viewModel.Date);
        viewModel.Incassi = _storicoService.GetIncassi(viewModel.Date);

        // Popola le nuove statistiche nel ViewModel
        viewModel.OccupazioneMedia = _storicoService.GetOccupazioneMedia(dataInizio, dataFine);
        viewModel.DurataMediaSoggiorni = _storicoService.GetDurataMediaSoggiorni(dataInizio, dataFine);
        viewModel.IncassoTotale = _storicoService.GetIncassoTotale(dataInizio, dataFine);
        viewModel.UtilizzoServiziAggiuntivi = _storicoService.GetUtilizzoServiziAggiuntivi(dataInizio, dataFine);
        viewModel.PercentualeClientiConServiziAggiuntivi = _storicoService.GetPercentualeClientiConServiziAggiuntivi(dataInizio, dataFine);

        // Imposta il periodo selezionato
        viewModel.PeriodoSelezionato = periodo;

        return View(viewModel);
    }


    private List<DateTime> GeneraDate(DateTime dataInizio, DateTime dataFine)
    {
        var date = new List<DateTime>();
        for (var dateCursor = dataInizio; dateCursor <= dataFine; dateCursor = dateCursor.AddDays(1))
        {
            date.Add(dateCursor);
        }
        return date;
    }
}
