using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CapStone_AndreaGuarnieri.Models;
using CapStone_AndreaGuarnieri.Models.Services;
using CapStone_AndreaGuarnieri.Models.ViewModels;
using System.Linq;

namespace CapStone_AndreaGuarnieri.Controllers
{
    [Authorize]
    public class PrenotazioneController : Controller
    {
        private readonly PrenotazioneService _prenotazioneService;
        private readonly ClienteService _clienteService;
        private readonly CameraService _cameraService;
        private readonly ServizioAggiuntivoService _servizioAggiuntivoService;
        private readonly ServizioService _servizioService;

        public PrenotazioneController(
            PrenotazioneService prenotazioneService,
            ClienteService clienteService,
            CameraService cameraService,
            ServizioAggiuntivoService servizioAggiuntivoService,
            ServizioService servizioService)
        {
            _prenotazioneService = prenotazioneService;
            _clienteService = clienteService;
            _cameraService = cameraService;
            _servizioAggiuntivoService = servizioAggiuntivoService;
            _servizioService = servizioService;
        }

        // Metodo per visualizzare la lista di tutte le prenotazioni
        public IActionResult Index()
        {
            var prenotazioni = _prenotazioneService.GetAllPrenotazioni()
                .Select(p => new PrenotazioneViewModel
                {
                    ID = p.ID,
                    ClienteID = p.ClienteID,
                    CameraID = p.CameraID,
                    DataPrenotazione = p.DataPrenotazione,
                    NumeroProgressivo = p.NumeroProgressivo,
                    Anno = p.Anno,
                    DataInizio = p.DataInizio,
                    DataFine = p.DataFine,
                    Caparra = p.Caparra,
                    TipoSoggiorno = p.TipoSoggiorno
                }).ToList();
            return View(prenotazioni);
        }

        // Metodo GET per aggiungere una nuova prenotazione
        [HttpGet]
        public IActionResult AddClientePrenotazione()
        {
            var model = new ClientePrenotazioneViewModel
            {
                DataPrenotazione = DateTime.Today,
                DataInizio = DateTime.Today,
                DataFine = DateTime.Today,
                CamereDisponibili = _cameraService.GetCamereDisponibili().ToList()
            };
            return View(model);
        }

        // Metodo POST per aggiungere una nuova prenotazione
        [HttpPost]
        public IActionResult AddClientePrenotazione(ClientePrenotazioneViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.CamereDisponibili = _cameraService.GetCamereDisponibili().ToList();
                return View(model);
            }

            var cliente = new Cliente
            {
                CodiceFiscale = model.ClienteCodiceFiscale,
                Cognome = model.ClienteCognome,
                Nome = model.ClienteNome,
                Citta = model.ClienteCitta,
                Provincia = model.ClienteProvincia,
                Email = model.ClienteEmail,
                Telefono = model.ClienteTelefono,
                Cellulare = model.ClienteCellulare
            };

            var prenotazione = new Prenotazione
            {
                ClienteID = model.ClienteCodiceFiscale,
                CameraID = model.CameraID,
                DataPrenotazione = model.DataPrenotazione,
                NumeroProgressivo = _prenotazioneService.GetNextProgressiveNumber(),
                Anno = model.DataInizio.Year,
                DataInizio = model.DataInizio,
                DataFine = model.DataFine,
                Caparra = model.Caparra,
                TipoSoggiorno = model.TipoSoggiorno
            };

            _clienteService.AddCliente(cliente);
            _prenotazioneService.AddPrenotazione(prenotazione);

            return RedirectToAction("Index", "Home");
        }

        // Metodo per visualizzare i dettagli di una prenotazione
        public IActionResult Dettagli(int id)
        {
            var prenotazione = _prenotazioneService.GetPrenotazione(id);
            return View(prenotazione);
        }

        // Metodo GET per la ricerca delle prenotazioni
        public IActionResult Search()
        {
            return View(new SearchViewModel());
        }

        // Metodo POST per la ricerca delle prenotazioni per Codice Fiscale
        [HttpPost]
        public IActionResult Search(string codiceFiscale)
        {
            if (string.IsNullOrWhiteSpace(codiceFiscale))
            {
                return Json(new { success = false, message = "Codice Fiscale non valido." });
            }

            var result = _prenotazioneService.GetPrenotazioniByCodiceFiscale(codiceFiscale);
            if (result != null && result.Any())
            {
                return PartialView("SearchResult", result);
            }
            else
            {
                return Json(new { success = false, message = "Cliente non trovato." });
            }
        }

        // Metodo per visualizzare il conteggio delle tipologie di soggiorno
        public IActionResult TipologiaSoggiorno()
        {
            return View();
        }

        // Metodo POST per ottenere il conteggio delle tipologie di soggiorno
        [HttpPost]
        public IActionResult GetTipologiaSoggiornoCounts()
        {
            var counts = _prenotazioneService.GetTipologiaSoggiornoCounts();
            return Json(counts);
        }

        // Metodo per visualizzare la pagina di checkout
        public IActionResult Checkout(int id)
        {
            var prenotazione = _prenotazioneService.GetPrenotazione(id);
            if (prenotazione == null)
            {
                return NotFound();
            }

            var serviziAggiuntivi = _prenotazioneService.GetServiziAggiuntivi(id).ToList();

            var model = new CheckoutViewModel
            {
                NumeroCamera = prenotazione.CameraID,
                DataInizio = prenotazione.DataInizio,
                DataFine = prenotazione.DataFine,
                Tariffa = prenotazione.Camera.TariffaGiornaliera,
                ServiziAggiuntivi = serviziAggiuntivi,
                ImportoDaSaldare = (prenotazione.Camera.TariffaGiornaliera * (prenotazione.DataFine - prenotazione.DataInizio).Days) + serviziAggiuntivi.Sum(sa => sa.Servizio.Tariffa * sa.Quantita)
            };

            return View(model);
        }

        // Metodo GET per aggiungere un nuovo servizio aggiuntivo
        [HttpGet]
        public IActionResult AddServizioAggiuntivo(int id)
        {
            var model = new ServizioAggiuntivoViewModel
            {
                PrenotazioneID = id,
                ServiziDisponibili = _servizioService.GetAllServizi().ToList(),
                Data = DateTime.Now.Date
            };
            return View(model);
        }

        // Metodo POST per aggiungere un nuovo servizio aggiuntivo
        [HttpPost]
        public IActionResult AddServizioAggiuntivo(ServizioAggiuntivoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ServiziDisponibili = _servizioService.GetAllServizi().ToList();
                return View(model);
            }

            var servizioAggiuntivo = new ServizioAggiuntivo
            {
                PrenotazioneID = model.PrenotazioneID,
                ServizioID = model.ServizioID,
                Data = model.Data,
                Quantita = model.Quantita
            };

            _servizioAggiuntivoService.AddServizioAggiuntivo(servizioAggiuntivo);

            return RedirectToAction("Index", "Home");
        }
    }
}
