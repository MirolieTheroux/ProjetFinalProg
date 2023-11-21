create table Employe
(
    matricule      char(8) PRIMARY KEY,
    nom            varchar(50),
    prenom         varchar(50),
    date_naissance date,
    email          varchar(255),
    adresse        varchar(255),
    date_embauche  date,
    taux_horaire   double,
    lien_photo     varchar(255),
    statut         varchar(11)
);

Insert into employep (nom,prenom,date_naissance,email,adresse,date_embauche,taux_horaire,lien_photo,statut) VALUES ('Mirolie','Théroux',1997-09-10, 'mirolie010@gmail.com',2023-11-16,24.65,'https://resizing.flixster.com/-XZAfHZM39UwaGJIFWKAE8fS0ak=/v3/t/assets/p8655066_b_v8_aa.jpg', 'Journalier');

CREATE procedure p_ajouter_enployes(IN nomP varchar(50), IN prenomP varchar(50), IN date_naissanceP date,
                                    IN emailP varchar(255), IN adresseP varchar(255), IN date_embaucheP date,
                                    IN taux_horaireP double, IN lien_photoP varchar(255), IN statutP varchar(11))
BEGIN
    INSERT into employep (nom, prenom, date_naissance, email, adresse, date_embauche, taux_horaire, lien_photo, statut)
    values (nomP, prenomP, date_naissanceP, emailP, adresseP, date_embaucheP, taux_horaireP, lien_photoP, statutP);
end;


delimiter //
CREATE TRIGGER genererMatricule
    before insert
    on employep
    for each row
begin
    set new.matricule = concat(substr(nom, 1, 2), '-', date_naissance, '-', round(RAND() * (99 - 10 + 1) + 10));
end;
//
delimiter ;