-- Inserimento dei Clienti
INSERT INTO Clienti (CodiceFiscale, Cognome, Nome, Citta, Provincia, Email, Telefono, Cellulare) 
VALUES ('RSSMRA85M01H501Z', 'Rossi', 'Mario', 'Roma', 'RM', 'mario.rossi@example.com', '0645212345', '3331234567'),
       ('BNCGPP80A01F205X', 'Bianchi', 'Giuseppe', 'Milano', 'MI', 'giuseppe.bianchi@example.com', '0245678901', '3338765432'),
       ('VRDLGI70T20L219R', 'Verdi', 'Luigi', 'Napoli', 'NA', 'luigi.verdi@example.com', '0819876543', '3342345678'),
       ('FRNLRA85E01H501R', 'Ferrari', 'Luca', 'Torino', 'TO', 'luca.ferrari@example.com', '0113456789', '3381239876'),
       ('BRTGLL85C01Z404T', 'Bartoli', 'Giulia', 'Firenze', 'FI', 'giulia.bartoli@example.com', '0556543210', '3391234567'),
       ('CLMLGI95A12B963Q', 'Colombo', 'Lorenzo', 'Bologna', 'BO', 'lorenzo.colombo@example.com', '0512345678', '3379876543'),
       ('MNZGPP90T20F205Z', 'Monzani', 'Giuseppe', 'Milano', 'MI', 'giuseppe.monzani@example.com', '0245632190', '3318765432'),
       ('MRNMRN85A01A123Y', 'Marin', 'Marco', 'Venezia', 'VE', 'marco.marin@example.com', '0412345678', '3386543210'),
       ('PNTFNC80L01H501G', 'Ponti', 'Francesca', 'Roma', 'RM', 'francesca.ponti@example.com', '0645213456', '3341239876'),
       ('TRTMCH75M11F205X', 'Torti', 'Michele', 'Milano', 'MI', 'michele.torti@example.com', '0287654321', '3316549870'),
       ('LNGGPP85D20A662U', 'Longhi', 'Giuseppe', 'Genova', 'GE', 'giuseppe.longhi@example.com', '0103456789', '3335678901'),
       ('STLLRA90M20L219X', 'Stella', 'Laura', 'Napoli', 'NA', 'laura.stella@example.com', '0811234567', '3312345678'),
       ('GRGLGU85P01Z404T', 'Gargiulo', 'Luigi', 'Firenze', 'FI', 'luigi.gargiulo@example.com', '0552345678', '3399876543'),
       ('BRSNDR85L20F205X', 'Bruno', 'Sandro', 'Milano', 'MI', 'sandro.bruno@example.com', '0245678902', '3381236549'),
       ('RNDVNC85A01H501X', 'Rinaldi', 'Vincenzo', 'Roma', 'RM', 'vincenzo.rinaldi@example.com', '0665432109', '3341234569');

-- Inserimento delle Prenotazioni con aggiornamento della disponibilità delle Camere
-- Prenotazione 1
INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno, Confermata)
VALUES ('RSSMRA85M01H501Z', 101, GETDATE(), 1, 2024, '2024-09-20', '2024-09-27', 150.50, 'pernottamento con prima colazione', 0);
UPDATE Camere SET Disponibile = 0 WHERE Numero = 101;

-- Prenotazione 2
INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno, Confermata)
VALUES ('BNCGPP80A01F205X', 102, GETDATE(), 2, 2024, '2024-09-20', '2024-09-29', 200.75, 'pensione completa', 0);
UPDATE Camere SET Disponibile = 0 WHERE Numero = 102;

-- Prenotazione 3
INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno, Confermata)
VALUES ('VRDLGI70T20L219R', 103, GETDATE(), 3, 2024, '2024-09-20', '2024-09-30', 120.00, 'mezza pensione', 0);
UPDATE Camere SET Disponibile = 0 WHERE Numero = 103;

-- Prenotazione 4
INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno, Confermata)
VALUES ('FRNLRA85E01H501R', 104, GETDATE(), 4, 2024, '2024-09-20', '2024-09-28', 100.00, 'pernottamento con prima colazione', 0);
UPDATE Camere SET Disponibile = 0 WHERE Numero = 104;

-- Prenotazione 5
INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno, Confermata)
VALUES ('BRTGLL85C01Z404T', 105, GETDATE(), 5, 2024, '2024-09-21', '2024-09-29', 75.00, 'mezza pensione', 0);
UPDATE Camere SET Disponibile = 0 WHERE Numero = 105;

-- Prenotazione 6
INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno, Confermata)
VALUES ('CLMLGI95A12B963Q', 106, GETDATE(), 6, 2024, '2024-09-21', '2024-09-30', 180.50, 'pensione completa', 0);
UPDATE Camere SET Disponibile = 0 WHERE Numero = 106;

-- Prenotazione 7
INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno, Confermata)
VALUES ('MNZGPP90T20F205Z', 107, GETDATE(), 7, 2024, '2024-09-22', '2024-09-30', 220.75, 'pernottamento con prima colazione', 0);
UPDATE Camere SET Disponibile = 0 WHERE Numero = 107;

-- Prenotazione 8
INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno, Confermata)
VALUES ('MRNMRN85A01A123Y', 108, GETDATE(), 8, 2024, '2024-09-23', '2024-10-01', 95.00, 'mezza pensione', 0);
UPDATE Camere SET Disponibile = 0 WHERE Numero = 108;

-- Prenotazione 9
INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno, Confermata)
VALUES ('PNTFNC80L01H501G', 109, GETDATE(), 9, 2024, '2024-09-24', '2024-10-02', 130.00, 'pernottamento con prima colazione', 0);
UPDATE Camere SET Disponibile = 0 WHERE Numero = 109;

-- Prenotazione 10
INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno, Confermata)
VALUES ('TRTMCH75M11F205X', 111, GETDATE(), 10, 2024, '2024-09-25', '2024-10-03', 150.00, 'pensione completa', 0);
UPDATE Camere SET Disponibile = 0 WHERE Numero = 111;

-- Prenotazione 11
INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno, Confermata)
VALUES ('LNGGPP85D20A662U', 112, GETDATE(), 11, 2024, '2024-09-26', '2024-10-04', 175.25, 'mezza pensione', 0);
UPDATE Camere SET Disponibile = 0 WHERE Numero = 112;

-- Prenotazione 12
INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno, Confermata)
VALUES ('STLLRA90M20L219X', 113, GETDATE(), 12, 2024, '2024-09-27', '2024-10-05', 140.50, 'pensione completa', 0);
UPDATE Camere SET Disponibile = 0 WHERE Numero = 113;

-- Prenotazione 13
INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno, Confermata)
VALUES ('GRGLGU85P01Z404T', 114, GETDATE(), 13, 2024, '2024-09-28', '2024-10-06', 185.75, 'mezza pensione', 0);
UPDATE Camere SET Disponibile = 0 WHERE Numero = 114;

-- Prenotazione 14
INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno, Confermata)
VALUES ('BRSNDR85L20F205X', 115, GETDATE(), 14, 2024, '2024-09-29', '2024-10-07', 160.00, 'pernottamento con prima colazione', 0);
UPDATE Camere SET Disponibile = 0 WHERE Numero = 115;

-- Prenotazione 15
INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno, Confermata)
VALUES ('RNDVNC85A01H501X', 116, GETDATE(), 15, 2024, '2024-09-30', '2024-10-08', 155.00, 'mezza pensione', 0);
UPDATE Camere SET Disponibile = 0 WHERE Numero = 116;
