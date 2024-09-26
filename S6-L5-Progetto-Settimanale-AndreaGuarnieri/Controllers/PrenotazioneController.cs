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
        private readonly ILogger<PrenotazioneController> _logger;

        public PrenotazioneController(
            PrenotazioneService prenotazioneService,
            ClienteService clienteService,
            CameraService cameraService,
            ServizioAggiuntivoService servizioAggiuntivoService,
            ServizioService servizioService,
            ILogger<PrenotazioneController> logger) // Iniezione del logger
        {
            _prenotazioneService = prenotazioneService;
            _clienteService = clienteService;
            _cameraService = cameraService;
            _servizioAggiuntivoService = servizioAggiuntivoService;
            _servizioService = servizioService;
            _logger = logger; // Assegna il logger al campo privato
        }

        // Metodo per visualizzare la lista di tutte le prenotazioni
        public IActionResult Index()
        {
            var prenotazioni = _prenotazioneService.GetAllPrenotazioni()
                .Where(p => p.Confermata == false)  // Filtro per prenotazioni non confermate
                .Select(p => new PrenotazioneViewModel
                {
                    ID = p.ID,
                    ClienteID = p.ClienteID,
                    Cognome = p.Cognome, // Assicurati che questi campi siano popolati correttamente
                    Nome = p.Nome,
                    CameraID = p.CameraID,
                    DataPrenotazione = p.DataPrenotazione,
                    NumeroProgressivo = p.NumeroProgressivo,
                    Anno = p.Anno,
                    DataInizio = p.DataInizio,
                    DataFine = p.DataFine,
                    Caparra = p.Caparra,
                    TipoSoggiorno = p.TipoSoggiorno,
                    PrezzoTotale = p.PrezzoTotale,
                    Confermata = p.Confermata
                }).ToList();

            return View(prenotazioni);
        }



        // Metodo per visualizzare la lista di tutte le prenotazioni
        public IActionResult Dettagli(int id)
        {
            var prenotazione = _prenotazioneService.GetPrenotazioneById(id);

            if (prenotazione == null)
            {
                return NotFound();
            }

            // Recupera le camere disponibili
            var camereDisponibili = _cameraService.GetCamereDisponibili()
                .Select(c => new
                {
                    Numero = c.Numero,
                    CameraDisplay = $"{c.Numero} - {c.Tipologia}" // Combina Numero e Tipologia
                })
                .ToList();

            // Aggiungi la camera attuale alla lista delle camere disponibili, anche se non è disponibile
            var cameraAttuale = _cameraService.GetCameraById(prenotazione.CameraID);
            if (cameraAttuale != null && !camereDisponibili.Any(c => c.Numero == cameraAttuale.Numero))
            {
                camereDisponibili.Add(new { Numero = cameraAttuale.Numero, CameraDisplay = $"{cameraAttuale.Numero} - {cameraAttuale.Tipologia}" });
            }

            var serviziAggiuntivi = _servizioAggiuntivoService.GetServiziAggiuntiviByPrenotazioneId(id)
                .Select(sa => new ServizioAggiuntivoViewModel
                {
                    PrenotazioneID = sa.PrenotazioneID,
                    ServizioID = sa.ServizioID,
                    NomeServizio = sa.Servizio.Nome,
                    TariffaServizio = sa.Servizio.Tariffa,
                    Quantita = sa.Quantita,
                    Data = sa.Data,
                    ServiziDisponibili = _servizioService.GetAllServizi().ToList()
                })
                .ToList();

            var model = new DettaglioPrenotazioneViewModel
            {
                ID = prenotazione.ID,
                ClienteID = prenotazione.ClienteID,
                CameraID = prenotazione.CameraID,
                CamereDisponibili = camereDisponibili.Select(c => new Camera { Numero = c.Numero, Descrizione = c.CameraDisplay }).ToList(),
                DataPrenotazione = prenotazione.DataPrenotazione,
                NumeroProgressivo = prenotazione.NumeroProgressivo,
                Anno = prenotazione.Anno,
                DataInizio = prenotazione.DataInizio,
                DataFine = prenotazione.DataFine,
                Caparra = prenotazione.Caparra,
                TipoSoggiorno = prenotazione.TipoSoggiorno,
                PrezzoTotale = prenotazione.PrezzoTotale,
                Confermata = prenotazione.Confermata,

                // Dettagli Cliente
                Cognome = prenotazione.Cliente.Cognome,
                Nome = prenotazione.Cliente.Nome,
                Citta = prenotazione.Cliente.Citta,
                Provincia = prenotazione.Cliente.Provincia,
                Email = prenotazione.Cliente.Email,
                Telefono = prenotazione.Cliente.Telefono,
                Cellulare = prenotazione.Cliente.Cellulare,

                // Aggiungi i servizi aggiuntivi
                ServiziAggiuntivi = serviziAggiuntivi
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult SalvaDettagliPrenotazione(DettaglioPrenotazioneViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Se il modello non è valido, ricarica solo i campi necessari senza la camera
                model.CamereDisponibili = _cameraService.GetCamereDisponibili().ToList(); // Se necessario
                return View("Dettagli", model);
            }

            var prenotazione = _prenotazioneService.GetPrenotazioneById(model.ID);

            if (prenotazione == null)
            {
                return NotFound();
            }

            // Rimuovi il codice che gestisce la logica di modifica delle camere
            // Aggiorna la prenotazione e il cliente (senza camera)
            _prenotazioneService.UpdatePrenotazione(model);
            _clienteService.UpdateCliente(model);

            // Rimane nella vista dei dettagli
            return View("Dettagli", model);
        }

        public IActionResult EliminaServizioAggiuntivo(int id, int prenotazioneId)
        {
            _servizioAggiuntivoService.DeleteServizioAggiuntivo(id);
            return RedirectToAction("Dettagli", new { id = prenotazioneId });
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

        // Metodo GET per la ricerca delle prenotazioni
        [HttpGet]
        public IActionResult Search()
        {
            return View(new SearchViewModel());
        }

        [HttpPost]
        public IActionResult Search(string query)
        {
            Console.WriteLine("Metodo Search chiamato con query: " + query);

            if (string.IsNullOrWhiteSpace(query))
            {
                Console.WriteLine("Query vuota o non valida.");
                ViewBag.ErrorMessage = "Inserisci un codice fiscale o cognome valido.";
                return PartialView("_NoResults");
            }

            var cliente = _clienteService.GetClienteByCodiceFiscaleOrCognome(query);

            if (cliente == null)
            {
                Console.WriteLine("Cliente non trovato.");
                ViewBag.ErrorMessage = "Cliente non trovato.";
                return PartialView("_NoResults");
            }

            Console.WriteLine("Cliente trovato: " + cliente.Cognome + " " + cliente.Nome);

            var prenotazioni = _prenotazioneService.GetPrenotazioniByClienteId(cliente.CodiceFiscale);

            if (prenotazioni == null || !prenotazioni.Any())
            {
                Console.WriteLine("Nessuna prenotazione trovata per il cliente.");
                ViewBag.ErrorMessage = "Nessuna prenotazione trovata.";
                return PartialView("_NoResults");
            }

            Console.WriteLine($"Trovate {prenotazioni.Count()} prenotazioni per il cliente.");

            var resultViewModel = prenotazioni.Select(p => new SearchResultViewModel
            {
                Cognome = cliente.Cognome,
                Nome = cliente.Nome,
                CodiceFiscale = cliente.CodiceFiscale,
                DataInizio = p.DataInizio,
                DataFine = p.DataFine,
                NumeroProgressivo = p.NumeroProgressivo

            }).ToList();

            return PartialView("SearchResult", resultViewModel);
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
            // Recupera tutti i servizi disponibili tramite il service
            var serviziDisponibili = _servizioService.GetAllServizi().ToList();

            // Se nessun servizio è disponibile, ritorna un messaggio di errore
            if (!serviziDisponibili.Any())
            {
                return NotFound("Nessun servizio disponibile.");
            }

            // Crea un nuovo ViewModel e passa i servizi disponibili
            var model = new ServizioAggiuntivoViewModel
            {
                PrenotazioneID = id,
                ServiziDisponibili = serviziDisponibili,  // Popola il dropdown con i servizi disponibili
                TariffaServizio = serviziDisponibili.First().Tariffa,  // Imposta la tariffa del primo servizio disponibile
                NomeServizio = serviziDisponibili.First().Nome,        // Imposta il nome del primo servizio
                Data = DateTime.Now,  // Imposta la data odierna
                Quantita = 1          // Imposta la quantità predefinita
            };

            return View(model);
        }

        // Metodo POST per aggiungere un nuovo servizio aggiuntivo
        [HttpPost]
        public IActionResult AddServizioAggiuntivo(ServizioAggiuntivoInputModel model)
        {
            _logger.LogInformation("Inizio metodo POST AddServizioAggiuntivo");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState non valido");

                foreach (var modelStateKey in ModelState.Keys)
                {
                    var value = ModelState[modelStateKey];
                    foreach (var error in value.Errors)
                    {
                        _logger.LogError($"Errore per chiave {modelStateKey}: {error.ErrorMessage}");
                    }
                }

                var serviziDisponibili = _servizioService.GetAllServizi().ToList();
                var viewModel = new ServizioAggiuntivoViewModel
                {
                    PrenotazioneID = model.PrenotazioneID,
                    ServizioID = model.ServizioID,
                    Data = model.Data,
                    Quantita = model.Quantita,
                    ServiziDisponibili = serviziDisponibili
                };

                return View(viewModel); // Ritorna il ViewModel con i dati disponibili
            }

            try
            {
                var servizioAggiuntivo = new ServizioAggiuntivo
                {
                    PrenotazioneID = model.PrenotazioneID,
                    ServizioID = model.ServizioID,
                    Data = model.Data,
                    Quantita = model.Quantita
                };

                _logger.LogInformation($"Aggiunta servizio aggiuntivo: PrenotazioneID = {model.PrenotazioneID}, ServizioID = {model.ServizioID}");

                _servizioAggiuntivoService.AddServizioAggiuntivo(servizioAggiuntivo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'inserimento del servizio aggiuntivo");
                ModelState.AddModelError(string.Empty, "Errore durante l'inserimento del servizio aggiuntivo.");
                return View(model);
            }

            _logger.LogInformation("Fine metodo POST AddServizioAggiuntivo");

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
        public IActionResult CamerePerTipologia(string tipologia)
        {
            var camere = _cameraService.GetCamereByTipologia(tipologia);

            if (camere == null || !camere.Any())
            {
                return NotFound($"Nessuna camera trovata per la tipologia: {tipologia}");
            }

            return View(camere); // Assicurati di avere una vista che visualizzi le camere
        }
        public IActionResult GetPrenotazioneByCameraId(int cameraID)
        {
            var prenotazione = _prenotazioneService.GetPrenotazioneNonConfermataByCameraId(cameraID);

            if (prenotazione == null)
            {
                return NotFound();
            }

            // Reindirizza alla pagina dei dettagli usando il numero progressivo
            return RedirectToAction("Dettagli", new { id = prenotazione.NumeroProgressivo });
        }

    }
}
