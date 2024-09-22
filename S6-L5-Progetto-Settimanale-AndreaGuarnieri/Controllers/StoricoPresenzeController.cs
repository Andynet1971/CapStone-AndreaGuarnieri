using CapStone_AndreaGuarnieri.Models.Services;
using CapStone_AndreaGuarnieri.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class StoricoPresenzeController : Controller
{
    private readonly PrenotazioneService _prenotazioneService;

    public StoricoPresenzeController(PrenotazioneService prenotazioneService)
    {
        _prenotazioneService = prenotazioneService;
    }

    public IActionResult Storico(string periodo)
    {
        var viewModel = new OccupazioneViewModel();

        // Imposta il periodo di tempo in base alla selezione
        switch (periodo)
        {
            case "ultimoMese":
                viewModel.Date = GeneraDate(DateTime.Now.AddMonths(-1), DateTime.Now);
                break;
            case "ultimi3Mesi":
                viewModel.Date = GeneraDate(DateTime.Now.AddMonths(-3), DateTime.Now);
                break;
            case "ultimi6Mesi":
                viewModel.Date = GeneraDate(DateTime.Now.AddMonths(-6), DateTime.Now);
                break;
            case "ultimoAnno":
                viewModel.Date = GeneraDate(DateTime.Now.AddYears(-1), DateTime.Now);
                break;
            default:
                viewModel.Date = GeneraDate(DateTime.Now.AddMonths(-1), DateTime.Now);
                break;
        }

        // Ottieni i tassi di occupazione per le date generate
        viewModel.TassoOccupazione = _prenotazioneService.GetTassoOccupazione(viewModel.Date);

        // Ottieni gli incassi per le date generate
        viewModel.Incassi = _prenotazioneService.GetIncassi(viewModel.Date); // Aggiungi questo metodo nel servizio

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
