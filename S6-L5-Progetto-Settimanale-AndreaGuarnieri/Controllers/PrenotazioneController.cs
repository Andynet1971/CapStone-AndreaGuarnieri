using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CapStone_AndreaGuarnieri.Models;
using CapStone_AndreaGuarnieri.Models.Services;
using CapStone_AndreaGuarnieri.Models.ViewModels;
using System.Data.SqlClient;

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

            // Popolamento dei servizi aggiuntivi con mapping al ViewModel
            var serviziAggiuntivi = _prenotazioneService.GetServiziAggiuntivi(id)
                .Select(sa => new ServizioAggiuntivoViewModel
                {
                    ServizioID = sa.ServizioID,
                    Quantita = sa.Quantita,
                    NomeServizio = sa.Servizio.Nome, // Nome del servizio
                    TariffaServizio = sa.Servizio.Tariffa // Tariffa del servizio
                })
                .ToList();

            // Ottenere le tariffe per il periodo in base al TipoCamera
            var tariffePerPeriodo = _prenotazioneService.GetTariffePerPeriodo(prenotazione.DataInizio, prenotazione.DataFine, prenotazione.CameraID)
                .Select(t => new TariffaPeriodoViewModel
                {
                    TipoStagione = t.TipoStagione,
                    TariffaGiornaliera = t.TariffaGiornaliera,
                    DataInizio = t.DataInizio,
                    DataFine = t.DataFine
                })
                .ToList();

            // Calcolo del totale importo da saldare (tariffe camera + servizi aggiuntivi)
            var importoDaSaldare = tariffePerPeriodo.Sum(t => t.TariffaGiornaliera * (prenotazione.DataFine - prenotazione.DataInizio).Days)
                                    + serviziAggiuntivi.Sum(sa => sa.TariffaServizio * sa.Quantita);

            // Popolamento del ViewModel di Checkout
            var model = new CheckoutViewModel
            {
                NumeroCamera = prenotazione.CameraID,
                DataInizio = prenotazione.DataInizio,
                DataFine = prenotazione.DataFine,
                Caparra = prenotazione.Caparra,
                ServiziAggiuntivi = serviziAggiuntivi,
                TariffePerPeriodo = tariffePerPeriodo,  // Aggiungi qui le tariffe per periodo
                ImportoDaSaldare = importoDaSaldare
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
        public List<TariffaPeriodoViewModel> GetTariffePerPeriodo(DateTime dataInizioPrenotazione, DateTime dataFinePrenotazione, int cameraId)
        {
            var tariffePerPeriodo = new List<TariffaPeriodoViewModel>();

            // Recupera tutte le tariffe per la camera specificata
            var tariffe = new List<Tariffa>();

            using (SqlConnection conn = new SqlConnection("your_connection_string_here"))
            {
                conn.Open();
                string query = @"
            SELECT TipoStagione, TariffaGiornaliera, DataInizio, DataFine, TipoCamera
            FROM Tariffe
            WHERE TipoCamera = (SELECT Tipologia FROM Camere WHERE Numero = @CameraID)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CameraID", cameraId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tariffe.Add(new Tariffa
                            {
                                TipoStagione = reader.GetString(0),
                                TariffaGiornaliera = reader.GetDecimal(1),
                                DataInizio = reader.GetDateTime(2),
                                DataFine = reader.GetDateTime(3)
                            });
                        }
                    }
                }
            }

            // Ora iteriamo sui giorni di prenotazione
            var currentDate = dataInizioPrenotazione;

            while (currentDate <= dataFinePrenotazione)
            {
                // Trova la tariffa valida per il giorno corrente
                var tariffaValida = tariffe.FirstOrDefault(t => t.DataInizio <= currentDate && t.DataFine >= currentDate);

                if (tariffaValida != null)
                {
                    // Determina l'inizio del periodo per la prenotazione e la tariffa
                    var dataInizioPeriodo = currentDate;
                    var dataFinePeriodo = dataFinePrenotazione;

                    // Se la fine della tariffa è prima della fine della prenotazione, chiudi il periodo tariffario
                    if (tariffaValida.DataFine < dataFinePrenotazione)
                    {
                        dataFinePeriodo = tariffaValida.DataFine;
                    }

                    // Aggiungi la tariffa valida con il periodo sovrapposto
                    tariffePerPeriodo.Add(new TariffaPeriodoViewModel
                    {
                        TipoStagione = tariffaValida.TipoStagione,
                        TariffaGiornaliera = tariffaValida.TariffaGiornaliera,
                        DataInizio = dataInizioPeriodo,
                        DataFine = dataFinePeriodo
                    });

                    // Avanza al giorno dopo la fine del periodo corrente
                    currentDate = dataFinePeriodo.AddDays(1);
                }
                else
                {
                    // Se non troviamo nessuna tariffa per un giorno, avanza semplicemente di un giorno
                    currentDate = currentDate.AddDays(1);
                }
            }

            return tariffePerPeriodo;
        }

    }
}
