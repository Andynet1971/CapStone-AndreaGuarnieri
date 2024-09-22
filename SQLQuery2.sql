-- Popolamento della settimana dal 16 al 22 settembre 2024
INSERT INTO OrariDipendenti (UtenteID, NumeroSettimana, LunediInizio1, LunediFine1, LunediInizio2, LunediFine2,
                             MartediInizio1, MartediFine1, MartediInizio2, MartediFine2, 
                             MercolediInizio1, MercolediFine1, MercolediInizio2, MercolediFine2,
                             GiovediInizio1, GiovediFine1, GiovediInizio2, GiovediFine2,
                             VenerdiInizio1, VenerdiFine1, VenerdiInizio2, VenerdiFine2,
                             SabatoInizio1, SabatoFine1, SabatoInizio2, SabatoFine2,
                             DomenicaInizio1, DomenicaFine1, DomenicaInizio2, DomenicaFine2, 
                             GiornoLibero, OrePermessoResidue, OreFerieResidue, MinutiRitardo)
VALUES
(21, 38, -- Utente ID 1, settimana 38 (dal 16 al 22 settembre)
 '08:30', '11:30', '12:30', '16:30', -- Lunedi
 '09:00', '12:00', '13:00', '17:00', -- Martedi
 '10:00', '13:00', '14:00', '18:00', -- Mercoledi
 '09:30', '12:30', '13:30', '17:30', -- Giovedi
 '08:00', '11:00', '12:00', '16:00', -- Venerdi
 '09:00', '12:00', '13:00', '17:00', -- Sabato
 NULL, NULL, NULL, NULL,              -- Domenica (giorno libero)
 'Domenica', -- Giorno libero
 5.0, -- Ore di permesso residue (popolate una sola volta)
 12.0, -- Ore di ferie residue (popolate una sola volta)
 0 -- Minuti di ritardo
);

-- Popolamento della settimana dal 23 al 29 settembre 2024
INSERT INTO OrariDipendenti (UtenteID, NumeroSettimana, LunediInizio1, LunediFine1, LunediInizio2, LunediFine2,
                             MartediInizio1, MartediFine1, MartediInizio2, MartediFine2, 
                             MercolediInizio1, MercolediFine1, MercolediInizio2, MercolediFine2,
                             GiovediInizio1, GiovediFine1, GiovediInizio2, GiovediFine2,
                             VenerdiInizio1, VenerdiFine1, VenerdiInizio2, VenerdiFine2,
                             SabatoInizio1, SabatoFine1, SabatoInizio2, SabatoFine2,
                             DomenicaInizio1, DomenicaFine1, DomenicaInizio2, DomenicaFine2, 
                             GiornoLibero, OrePermessoResidue, OreFerieResidue, MinutiRitardo)
VALUES
(21, 39, -- Utente ID 1, settimana 39 (dal 23 al 29 settembre)
 NULL, NULL, NULL, NULL, -- Lunedi (giorno libero)
 '09:00', '12:00', '13:00', '17:00', -- Martedi
 '10:00', '13:00', '14:00', '18:00', -- Mercoledi
 '08:00', '11:00', '12:00', '16:00', -- Giovedi
 '09:00', '12:00', '13:00', '17:00', -- Venerdi
 '08:30', '11:30', '12:30', '16:30', -- Sabato
 '09:00', '12:00', '13:00', '17:00', -- Domenica
 'Lunedi', -- Giorno libero
 NULL, -- Ore di permesso non popolate di nuovo
 NULL, -- Ore di ferie non popolate di nuovo
 5 -- Minuti di ritardo accumulati
);
