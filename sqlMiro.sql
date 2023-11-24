-- Mirolie Théroux

-- Modèle relationnel
-- Employe : matricule (PK), nom, prenom, date_naissance, email, adresse, date_embauche, taux_horaire, lien_photo, statut
-- Client : id_client (PK), nom, adresse, no_telephone, email
-- Projet : no_projet (PK), titre, date_debut, description, budget, nbr_employe_requis, total_salaire, statut, id_client*,
-- Projet_employe : matricule*, no_projet*, nbr_heure_travail, salaire_employe_projet


-- #######################################################################################
-- ################################### Table Employe #####################################
-- #######################################################################################

-- Création de la table employé
create table Employe
(
    matricule      char(10) PRIMARY KEY,
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


-- Procédure pour récupérer tous les employés de la BD
CREATE procedure p_get_employes()
begin
    SELECT *
    from employe
    order by matricule;
end;

-- Procédure pour ajouter un employé dans la BD
DROP PROCEDURE IF EXISTS p_ajouter_employes;
CREATE procedure p_ajouter_employes(IN nomP varchar(50), IN prenomP varchar(50), IN date_naissanceP date,
                                    IN emailP varchar(255), IN adresseP varchar(255), IN date_embaucheP date,
                                    IN taux_horaireP double, IN lien_photoP varchar(255), IN statutP varchar(11))
BEGIN
    INSERT into employe (nom, prenom, date_naissance, email, adresse, date_embauche, taux_horaire, lien_photo, statut)
    values (nomP, prenomP, date_naissanceP, emailP, adresseP, date_embaucheP, taux_horaireP, lien_photoP, statutP);
end;

-- Procédure pour modifier un employé
DROP PROCEDURE IF EXISTS p_modifier_employe;
CREATE procedure p_modifier_enployes(IN nomP varchar(50), IN prenomP varchar(50), IN date_naissanceP date,
                                    IN emailP varchar(255), IN adresseP varchar(255), IN date_embaucheP date,
                                    IN taux_horaireP double, IN lien_photoP varchar(255), IN statutP varchar(11))
BEGIN
    INSERT into employep (nom, prenom, date_naissance, email, adresse, date_embauche, taux_horaire, lien_photo, statut)
    values (nomP, prenomP, date_naissanceP, emailP, adresseP, date_embaucheP, taux_horaireP, lien_photoP, statutP);
end;

-- Déclencheur pour générer un matricule à l'employé avant l'ajout
DROP TRIGGER IF EXISTS genererMatricule;
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

-- #######################################################################################
-- ################################# Table projet_employe ################################
-- #######################################################################################

--Création de la table projet_employe
CREATE TABLE projet_employe(
matricule char(10),
no_projet char(11),
nbr_heure_travail double,
salaire_employe_projet double,
FOREIGN KEY (matricule) REFERENCES employe (matricule),
FOREIGN KEY (no_projet) REFERENCES projet (no_projet)
);

