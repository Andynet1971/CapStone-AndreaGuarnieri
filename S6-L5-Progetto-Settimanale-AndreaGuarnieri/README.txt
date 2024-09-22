CAPSTONE FINALE DI ANDREA GUARNIERI

Per questo Capstone ho voluto realizzare una web-app per la gestione interna di un hotel. Ho implementato diverse funzionalità per l'insermiento,
visulizzazione, modifica dei dati e elaborazioni sui dati stassi. L'applicazione è stata sviluppata in ASP.NET Core(Model-View-Controller). 
Oltre a questa applicazione principale ho anche realizzato un App Windows Forms (.NET Framework) in C# che si collega al medesimo Database e permette la gestione
degli Utenti all'interno del DB stesso. Come ultima cosa ho anche realizzato un App, che ho compilato e quindi reso eseguibile, in Pyton. Questa app mi consente
di popolare il database con dati finti generati al momento. 

LINK PER L'APP IN PYTON PER POPOLARE IL DB
https://github.com/Andynet1971/Popola_db

LINK PER L'APP IN PYTON PER POPOLARE IL DB
https://github.com/Andynet1971/User-Managment

STRUMENTI UTILIZZATI

    Visual Studio
    Visual Studio Code
    SQL Server Managment Studio


LINGUAGGI DI PROGRAMMAZIONE  E FRAMEWORK USATI

    HTML
    CSS
    JavaScript
    C#
    Pyton
    ASP.NET CORE
    WINDOWS FORMS


Struttura del DATABASE

CREATE TABLE [dbo].[Camere] (
    [Numero]             INT             NOT NULL,
    [Descrizione]        NVARCHAR (100)  NOT NULL,
    [Tipologia]          NVARCHAR (20)   NOT NULL,
    [TariffaGiornaliera] DECIMAL (10, 2) NOT NULL,
    [Disponibile]        BIT             DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Numero] ASC),
    CHECK ([Tipologia]='doppia' OR [Tipologia]='singola' OR [Tipologia]='suite')
);

CREATE TABLE [dbo].[Clienti] (
    [CodiceFiscale] VARCHAR (16)   NOT NULL,
    [Cognome]       NVARCHAR (50)  NOT NULL,
    [Nome]          NVARCHAR (50)  NOT NULL,
    [Citta]         NVARCHAR (50)  NOT NULL,
    [Provincia]     NVARCHAR (50)  NOT NULL,
    [Email]         NVARCHAR (100) NOT NULL,
    [Telefono]      NVARCHAR (20)  NULL,
    [Cellulare]     NVARCHAR (20)  NOT NULL,
    PRIMARY KEY CLUSTERED ([CodiceFiscale] ASC)
);

CREATE TABLE [dbo].[OrariDipendenti] (
    [ID]                 INT            IDENTITY (1, 1) NOT NULL,
    [UtenteID]           INT            NOT NULL,
    [NumeroSettimana]    INT            NOT NULL,
    [LunediInizio1]      TIME (7)       NULL,
    [LunediFine1]        TIME (7)       NULL,
    [LunediInizio2]      TIME (7)       NULL,
    [LunediFine2]        TIME (7)       NULL,
    [MartediInizio1]     TIME (7)       NULL,
    [MartediFine1]       TIME (7)       NULL,
    [MartediInizio2]     TIME (7)       NULL,
    [MartediFine2]       TIME (7)       NULL,
    [MercolediInizio1]   TIME (7)       NULL,
    [MercolediFine1]     TIME (7)       NULL,
    [MercolediInizio2]   TIME (7)       NULL,
    [MercolediFine2]     TIME (7)       NULL,
    [GiovediInizio1]     TIME (7)       NULL,
    [GiovediFine1]       TIME (7)       NULL,
    [GiovediInizio2]     TIME (7)       NULL,
    [GiovediFine2]       TIME (7)       NULL,
    [VenerdiInizio1]     TIME (7)       NULL,
    [VenerdiFine1]       TIME (7)       NULL,
    [VenerdiInizio2]     TIME (7)       NULL,
    [VenerdiFine2]       TIME (7)       NULL,
    [SabatoInizio1]      TIME (7)       NULL,
    [SabatoFine1]        TIME (7)       NULL,
    [SabatoInizio2]      TIME (7)       NULL,
    [SabatoFine2]        TIME (7)       NULL,
    [DomenicaInizio1]    TIME (7)       NULL,
    [DomenicaFine1]      TIME (7)       NULL,
    [DomenicaInizio2]    TIME (7)       NULL,
    [DomenicaFine2]      TIME (7)       NULL,
    [GiornoLibero]       NVARCHAR (10)  NOT NULL,
    [OrePermessoResidue] DECIMAL (5, 2) DEFAULT ((0)) NULL,
    [OreFerieResidue]    DECIMAL (5, 2) DEFAULT ((0)) NULL,
    [MinutiRitardo]      INT            DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([UtenteID]) REFERENCES [dbo].[Utenti] ([ID])
);

CREATE TABLE [dbo].[Prenotazioni] (
    [ID]                INT             IDENTITY (1, 1) NOT NULL,
    [ClienteID]         VARCHAR (16)    NOT NULL,
    [CameraID]          INT             NOT NULL,
    [DataPrenotazione]  DATE            DEFAULT (getdate()) NOT NULL,
    [NumeroProgressivo] INT             NOT NULL,
    [Anno]              INT             NOT NULL,
    [DataInizio]        DATE            NOT NULL,
    [DataFine]          DATE            NOT NULL,
    [Caparra]           DECIMAL (10, 2) NOT NULL,
    [TipoSoggiorno]     NVARCHAR (40)   NOT NULL,
    [PrezzoTotale]      DECIMAL (10, 2) DEFAULT ((0)) NOT NULL,
    [Confermata]        BIT             DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ClienteID]) REFERENCES [dbo].[Clienti] ([CodiceFiscale]),
    FOREIGN KEY ([CameraID]) REFERENCES [dbo].[Camere] ([Numero]),
    CHECK ([TipoSoggiorno]='pernottamento con prima colazione' OR [TipoSoggiorno]='pensione completa' OR [TipoSoggiorno]='mezza pensione')
);

CREATE TABLE [dbo].[Servizi] (
    [ID]      INT             IDENTITY (1, 1) NOT NULL,
    [Nome]    NVARCHAR (100)  NOT NULL,
    [Tariffa] DECIMAL (10, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

CREATE TABLE [dbo].[ServiziAggiuntivi] (
    [ID]             INT  IDENTITY (1, 1) NOT NULL,
    [PrenotazioneID] INT  NOT NULL,
    [ServizioID]     INT  NOT NULL,
    [Data]           DATE DEFAULT (getdate()) NOT NULL,
    [Quantita]       INT  DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ServizioID]) REFERENCES [dbo].[Servizi] ([ID])
);

CREATE TABLE [dbo].[Tariffe] (
    [ID]                 INT             IDENTITY (1, 1) NOT NULL,
    [TipoStagione]       NVARCHAR (50)   NOT NULL,
    [TipoCamera]         NVARCHAR (50)   NOT NULL,
    [TariffaGiornaliera] DECIMAL (18, 2) NOT NULL,
    [DataInizio]         DATE            NOT NULL,
    [DataFine]           DATE            NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

CREATE TABLE [dbo].[Utenti] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [Username]     NVARCHAR (50)  NOT NULL,
    [PasswordHash] NVARCHAR (255) NOT NULL,
    [Salt]         NVARCHAR (255) NOT NULL,
    [Nome]         NVARCHAR (50)  NOT NULL,
    [Cognome]      NVARCHAR (50)  NOT NULL,
    [Ruolo]        NVARCHAR (50)  NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    UNIQUE NONCLUSTERED ([Username] ASC)
);

STRUTTURA DELL' APPLICAZIONE

CapStone-AndreaGuarnieri
├── wwwroot
│   ├── css
│   │   └── site.css
│   ├── Images
│   ├── js
│   └── favicon.ico
├── Controllers
│   ├── AccountController.cs
│   ├── HomeController.cs
│   ├── PrenotazioneController.cs
│   ├── StoricoPresenzeController.cs
│   └── UtenteController.cs
├── DataAccess
│   ├── CameraDataAccess.cs
│   ├── ClienteDataAccess.cs
│   ├── PrenotazioneDataAccess.cs
│   ├── ServizioAggiuntivoDataAccess.cs
│   └── UtenteDataAccess.cs
├── Interfaces
│   ├── ICamera.cs
│   ├── ICliente.cs
│   ├── IPrenotazione.cs
│   ├── IServizio.cs
│   └── IServizioAggiuntivo.cs
├── Models
│   ├── Camera.cs
│   ├── Cliente.cs
│   ├── Prenotazione.cs
│   ├── Servizio.cs
│   ├── ServizioAggiuntivo.cs
│   ├── Tariffa.cs
│   └── Utente.cs
│   └── ViewModels
│       ├── CheckoutViewModel.cs
│       ├── ClientePrenotazioneViewModel.cs
│       ├── LoginViewModel.cs
│       ├── OccupazioneViewModel.cs
│       ├── PrenotazioneViewModel.cs
│       ├── SearchResultViewModel.cs
│       ├── SearchViewModel.cs
│       ├── ServizioAggiuntivoViewModel.cs
│       └── UtenteViewModel.cs
├── Services
│   ├── CameraService.cs
│   ├── ClienteService.cs
│   ├── PrenotazioneService.cs
│   ├── ServizioAggiuntivoService.cs
│   └── UtenteService.cs
├── Views
│   ├── Account
│   │   ├── GestisciUtenti.cshtml
│   │   ├── Login.cshtml
│   │   ├── LoginFailed.cshtml
│   │   └── ModificaUtente.cshtml
│   ├── Home
│   │   ├── Index.cshtml
│   │   └── Privacy.cshtml
│   ├── Prenotazione
│   │   ├── AddClientePrenotazione.cshtml
│   │   ├── AddServizioAggiuntivo.cshtml
│   │   ├── Checkout.cshtml
│   │   ├── Index.cshtml
│   │   ├── Search.cshtml
│   │   ├── SearchResult.cshtml
│   │   └── TipologiaSoggiorno.cshtml
│   ├── Shared
│   │   ├── _Layout.cshtml
│   │   ├── _ValidationScriptsPartial.cshtml
│   │   └── Error.cshtml
│   └── StoricoPresenze
│       ├── Storico.cshtml
│       └── _ViewImports.cshtml
│       └── _ViewStart.cshtml
├── appsettings.json
├── Program.cs
└── README.txt

