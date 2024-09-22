-- Cancella prima i dati della tabella ServiziAggiuntivi
DELETE FROM ServiziAggiuntivi;
DBCC CHECKIDENT ('ServiziAggiuntivi', RESEED, 0);

-- Cancella i dati della tabella Prenotazioni
DELETE FROM Prenotazioni;
DBCC CHECKIDENT ('Prenotazioni', RESEED, 0);

-- Cancella i dati della tabella Clienti
DELETE FROM Clienti;
-- Per la tabella Clienti, non serve RESEED perché non ha una colonna IDENTITY
