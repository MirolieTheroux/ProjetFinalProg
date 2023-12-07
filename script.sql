-- Nicolas Fleurent - 1596189
-- Mirolie Théroux - 1468780
-- Script sql pour recréer la bd au bonne endroit quand elle sera faite


-- Modèle relationnel (Mirolie)
-- Employe : matricule (PK), nom, prenom, date_naissance, email, adresse, date_embauche, taux_horaire, lien_photo, statut
-- Client : id_client (PK), nom, adresse, no_telephone, email
-- Projet : no_projet (PK), titre, date_debut, description, budget, nbr_employe_requis, total_salaire, statut, id_client*,
-- Projet_employe : matricule*(PK), no_projet*(PK), nbr_heure_travail, salaire_employe_projet

-- (Nicolas/Mirolie) Suppresion des tables au besoin
DROP TABLE IF EXISTS projet_employe;
DROP TABLE IF EXISTS projet;
DROP TABLE IF EXISTS Employe;
DROP TABLE IF EXISTS client;
DROP TABLE IF EXISTS admin_user;

-- #######################################################################################
-- ################################### Table Client ######################################
-- #######################################################################################
-- (Nicolas) Création de la table Client
CREATE TABLE client (
    id_client INT PRIMARY KEY ,
    nom VARCHAR(100),
    adresse VARCHAR(255),
    no_telephone VARCHAR(20),
    email VARCHAR(255),
    CONSTRAINT ck_client_id CHECK ( id_client >= 100 AND id_client <= 999 )
);

-- (Nicolas) Procédure pour valider si il reste des numéros de clients disponible - Procccc
DROP PROCEDURE IF EXISTS p_valide_espace_client;
DELIMITER //
CREATE PROCEDURE p_valide_espace_client()
BEGIN
    DECLARE no_test INT;
    SET no_test = 100;
    loop_numero_valide: LOOP
        IF (SELECT id_client FROM client WHERE id_client=no_test) IS NULL THEN
            LEAVE loop_numero_valide;
        ELSE
            SET no_test = no_test + 1;
            IF no_test > 999 THEN
                SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'Il n\'y a pas d\'id disponible pour les clients';
                LEAVE loop_numero_valide;
            END IF;
        END IF ;

    END LOOP ;
END //
DELIMITER ;

-- (Nicolas) Déclencheur pour généré le nombre aléatoire avant l'ajout d'un client - trrrigger
DROP TRIGGER IF EXISTS trg_bi_client;
DELIMITER //
CREATE TRIGGER trg_bi_client
    BEFORE INSERT
    ON client
    FOR EACH ROW
    BEGIN
        CALL p_valide_espace_client();
        loop_numero_valide: LOOP
        SET NEW.id_client = FLOOR(RAND()*900+100);
        IF (SELECT id_client FROM client WHERE id_client=NEW.id_client) IS NULL THEN
            INSERT INTO client VALUES (no_id, _nom, _adresse, _no_telephone, _email);
            LEAVE loop_numero_valide;
        END IF;
    END LOOP ;
    end //
DELIMITER ;

-- (Nicolas) Procédure pour ajouter un client
DROP PROCEDURE IF EXISTS p_ajout_client;
DELIMITER //
CREATE PROCEDURE p_ajout_client(IN _nom VARCHAR(100), IN _adresse VARCHAR(255), IN _no_telephone VARCHAR(20), IN _email VARCHAR(255))
BEGIN
    INSERT INTO client VALUES (null, _nom, _adresse, _no_telephone, _email);
END //
DELIMITER ;

-- (Nicolas) Procédure pour obtenir les clients
DROP PROCEDURE IF EXISTS p_get_clients;
DELIMITER //
CREATE PROCEDURE p_get_clients()
BEGIN
    SELECT * FROM client
    ORDER BY id_client;
END //
DELIMITER ;

-- (Nicolas) Procédure pour modifier les clients - Procccc
DROP PROCEDURE IF EXISTS p_modifier_client;
DELIMITER //
CREATE PROCEDURE p_modifier_client(IN _id INT, IN _nom VARCHAR(100), IN _adresse VARCHAR(255), IN _no_telephone VARCHAR(20), IN _email VARCHAR(255))
BEGIN
    UPDATE client
    SET
        nom = _nom,
        adresse = _adresse,
        no_telephone = _no_telephone,
        email = _email
    WHERE id_client = _id;
END //
DELIMITER ;

-- (Nicolas) Ajout d'élément dans la table client
CALL p_ajout_client('Boisdaction inc.', '2165 rue Jules Paquette, Plessisville, QC, G6U 3R5', '819-621-2333','info@boisdaction.com');
CALL p_ajout_client('Cégep de Trois-Rivières', '3500 rue de Courval, Trois-Rivières, QC, G7T 5D4', '819-376-1721','info@cegeptr.qc.ca');
CALL p_ajout_client('Planit Canada inc.', '468 rue Main, Hudson, QC, H5T 4F5', '514-375-9676','info@planitcanada.ca');
CALL p_ajout_client('Progression', '356 rue Parent, Trois-Rivières, QC, G2T 5H6', '819-377-8456','info@progression.ca');
CALL p_ajout_client('Sonia Leblanc Agente immobilière', '245 des Érables, Plessisville, QC, G6W 3E4', '819-376-1721','sonial@hotmail.com');
CALL p_ajout_client('Cégep de Victoriaville', '432 boul. Bois-Franc, Victoriaville, QC, G5E 7T3', '819-758-7461','info@cegepvicto.qc.ca');
CALL p_ajout_client('Le Grand Constant', '30 rang St-Charles, Trois-Rivières, QC, G9B 3R5', '819-377-1552','constant@gmail.com');
CALL p_ajout_client('IGA Ronald Giguères', '260 chemin Ste-Margerite, Trois-Rivières, QC, G5R 6Y5', '819-377-8441','rgiguere@iga.ca');
CALL p_ajout_client('Desjardins Trois-Rivières', '450 boul. Jean XVIII, Trois-Rivières, QC, G2W 9G5', '819-687-8552','mtremblay@desjardins.ca');
CALL p_ajout_client('Tim Hortons', '3000 rue des Forges, Trois-Rivières, QC, G6T 9F4', '819-376-5579','info@timhortons.ca');
CALL p_ajout_client('Boutique Imaginaire', '2590 rue des Forges, Trois-Rivières, QC, G6T 3E4', '819-377-8876','info@imaginaire.ca');
CALL p_ajout_client('Patricia Tremblay Représentant Tupperware', '4503 rue de la forêt, Trois-Rivières, QC, G5T 2E0', '768-558-9842','ptremblay@gmail.com');
CALL p_ajout_client('Projet Interne', '123 à la maison, Trois-Rivières, QC, G5D 6Y8', '819-699-0432','info@NicoMiro.ca');
CALL p_ajout_client('Indigo', '1500 Av. McGill College, Montréal, QC, H3A 3J5', '(514) 281-5549','info@indigo.ca');


-- #######################################################################################
-- ################################### Table Employe #####################################
-- #######################################################################################

-- Création de la table employé (Mirolie)
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

-- Procédure pour récupérer tous les employés de la BD (Mirolie)
DROP procedure IF EXISTS p_get_employes;
CREATE procedure p_get_employes()
begin
    SELECT *
    from employe
    order by matricule;
end;

-- Déclencheur pour générer un matricule à l'employé avant l'ajout (Mirolie) - trrrigger
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

-- Procédure pour ajouter un employé dans la BD (Mirolie) - Procccc
DROP PROCEDURE IF EXISTS p_ajouter_employes;
CREATE procedure p_ajouter_employes(IN nomP varchar(50), IN prenomP varchar(50), IN date_naissanceP date,
                                    IN emailP varchar(255), IN adresseP varchar(255), IN date_embaucheP date,
                                    IN taux_horaireP double, IN lien_photoP varchar(255), IN statutP varchar(11))
BEGIN
    INSERT into employe 
    values (null,nomP, prenomP, date_naissanceP, emailP, adresseP, date_embaucheP, taux_horaireP, lien_photoP, statutP);
end;

-- Procédure pour modifier un employé (Mirolie)rêquete
DROP PROCEDURE IF EXISTS p_modifier_employe;
CREATE procedure p_modifier_enployes(IN mat char(10), IN nomP varchar(50), IN prenomP varchar(50),
                                                         IN emailP varchar(255), IN adresseP varchar(255),
                                                         IN taux_horaireP double, IN lien_photoP varchar(255),
                                                         IN statutP varchar(11))
begin
    UPDATE employe set nom=nomP, prenom=prenomP, email=emailP, adresse=adresseP,taux_horaire=taux_horaireP,lien_photo=lien_photoP, statut=statutP
    where matricule= mat;
end;

-- Ajout employés (Mirolie)
CALL p_ajouter_employes ('Théroux', 'Mirolie','1997-09-10','mirolie010@gmail.com','5 Hermina Plaza, New York', '2023-11-16',24.65,'https://yt3.googleusercontent.com/-CFTJHU7fEWb7BYEb6Jh9gm1EpetvVGQqtof0Rbh-VQRIznYYKJxCaqv_9HeBcmJmIsp2vOO9JU=s900-c-k-c0x00ffffff-no-rj', 'Journalier');
CALL p_ajouter_employes ('Paradis', 'Elisabeth','1997-09-02','ep@gmail.com','4860 rue principale, Gentilly', '2017-06-21',34.50, 'https://media.licdn.com/dms/image/C4D03AQEtHbakmwiVeQ/profile-displayphoto-shrink_200_200/0/1548023553374?e=1703116800&v=beta&t=1F8Mw184HJfYXnsFud10N8W7v1qlen9b9OXZyOjMofg','Temps plein');
CALL p_ajouter_employes ('Fleurent', 'Nicolas','1997-07-16','nicfleurent@gmail.com','55 rue Bob, Trois-Rivières', '2016-01-01',100,'https://media.licdn.com/dms/image/C5603AQFMpjGpeclJcw/profile-displayphoto-shrink_200_200/0/1517363963506?e=1703116800&v=beta&t=2jdNnOl1vAuQ5s-FLzjgo4DDn9X_dA9gvZVZ4Cx_l6w', 'Temps plein');
CALL p_ajouter_employes ('Turcotte', 'Kelly','1997-06-22','kellyturcotte@gmail.com','489 rang 2, Bécancour', '2019-03-16',26.95,'https://media.licdn.com/dms/image/C4E03AQEk5vNFzYmYHg/profile-displayphoto-shrink_800_800/0/1606762655282?e=2147483647&v=beta&t=SlbM9WlAIM0U_kGh3tairww8C2Z2CjpeHTZTvI-Iec8', 'Temps plein');
CALL p_ajouter_employes ('Auger-Beaudet', 'Marika','1997-07-01','mab@gmail.com','10 rue Bouvier, St-Grégoire', '2022-05-18',24.65,'https://secure.gravatar.com/avatar/5cf5c8770d52308d836b98b1f84f3712?s=200&d=identicon&r=g', 'Journalier');
CALL p_ajouter_employes ('Ronan', 'Saoirse','1994-10-10','sr@gmail.com','123 rue Desjardins, Nicolet', '2021-03-05',24.65,'https://hips.hearstapps.com/hmg-prod/images/gettyimages-914627104-copy.jpg?crop=0.9987135506003431xw:1xh;center,top&resize=640:*', 'Journalier');
CALL p_ajouter_employes ('Monaghan', 'Michelle','1976-02-22','michellem@gmail.com','569 rue des Forges, Trois-Rivières', '2011-11-16',45.35,'https://www.themoviedb.org/t/p/w500/jB47BoGdudHELszn9ZAZqnnUy8N.jpg', 'Temps plein');
CALL p_ajouter_employes ('Lépine', 'Nicolas','1990-01-26','nicolas.lepime@gmail.com','4870 route 218, Ste-Cécile', '2016-09-29',38.75,'https://wes.eletsonline.com/school/wp-content/uploads/sites/7/2021/03/Nicolas-Lepine.jpg', 'Temps plein');
CALL p_ajouter_employes ('Swift', 'Taylor','1989-12-13','taylorswift@gmail.com','5 Hermina Plaza, New York', '2023-05-04',24.65,'https://fr.web.img5.acsta.net/pictures/19/08/27/09/51/3618586.jpg', 'Journalier');
CALL p_ajouter_employes ('Lerman', 'Logan','1992-01-19','loganlerman@gmail.com','956 rue des Bouleaux, Bécancour', '2015-11-30',57.86,'https://www.themoviedb.org/t/p/w500/qWbN2toEEQgW9DFjgy3gT2VoVlQ.jpg', 'Temps plein');
CALL p_ajouter_employes ('OBrien', 'Dylan','1991-08-26','dylanobrien@gmail.com','526 rue des Oiseaux, St-Grégoire', '2014-04-16',60.25,'https://upload.wikimedia.org/wikipedia/commons/3/35/Dylan_O%27Brien_2014_Comic_Con_%28cropped%29.jpg', 'Temps plein');
CALL p_ajouter_employes ('Larson', 'Brie','1989-10-01','brielarson@gmail.com','444 rue Papineau, Trois-Rivières', '2022-11-16',25.65,'https://m.media-amazon.com/images/M/MV5BNDE4ZWY1ZTUtYjNhMy00MTQyLWFmMjktNTkyYTFjOGRlNDk0XkEyXkFqcGdeQXVyMTE1MTYxNDAw._V1_.jpg', 'Journalier');
CALL p_ajouter_employes ('Waltz', 'Christoph','1956-10-04','christophwaltz@gmail.com','89 rue Capricorne, Gentilly', '2022-03-17',26.32,'https://media.gq-magazine.co.uk/photos/5d139c2086dd7e5a22553b33/16:9/w_1280,c_limit/Christoph-Waltz_GQ_01Apr15_Matthew-Brookes_b.jpg', 'Journalier');
CALL p_ajouter_employes ('Shipka', 'Kiernan','1999-11-10','kiernanshipka@gmail.com','19 rue Étoile, St-Angèle', '2023-11-01',24.65,'https://www.instyle.com/thmb/z0l1fFbXOrCgD4FNKm4eh5qy8No=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/Kiernan-Shipka-HEd-Shot-Ramona-Rosales-x-Netflix-9453ca691f9a440894354ef9e7640928.jpg', 'Journalier');

-- Fonction pour calculer le nombre d'années d'ancienneté (Mirolie) - rêquete/fonction
drop function if exists f_calcul_annee_exp;
DELIMITER //
CREATE function f_calcul_annee_exp(matEmploye char(10)) RETURNS INT
BEGIN
    DECLARE nbAnnee int;
    SELECT floor(datediff(current_date, date_embauche) / 365.25)
    into nbAnnee
    from employe
    where matricule = matEmploye;
    RETURN nbAnnee;
end
//
DELIMITER ;
-- Appel de la fonction
SELECT f_calcul_annee_exp('AU-1997-86');

-- Fonction qui retourne le nombre de projets faits par un employé (Mirolie) - rêquete/fonction
DROP FUNCTION IF EXISTS f_nb_projetsFaits_employe;
DELIMITER //
CREATE FUNCTION f_nb_projetsFaits_employe(matEmp char(10)) RETURNS INT
BEGIN
    DECLARE nbProjetsFaits int;
    SELECT COUNT(*)
    into nbProjetsFaits
    from projet p
             inner join projet_employe pe on p.no_projet = pe.no_projet
             inner join employe e on e.matricule = pe.matricule
    where p.statut = 'terminé' and e.matricule = matEMp;
    RETURN nbProjetsFaits;
end
//
DELIMITER ;
-- Appel de la fonction
SELECT f_nb_projetsFaits_employe('LA-1989-29') as 'nbProjetsFaitsEmploye';

-- (Nicolas) Vue qui affiche les projets sur lequel Nicolas Fleurent à travailler
CREATE VIEW v_get_projet_Nicolas_Fleurent AS
    SELECT
        p.no_projet,
        p.titre,
        p.statut
    FROM projet_employe pe
    INNER JOIN projet p on pe.no_projet = p.no_projet
    WHERE pe.matricule = (SELECT matricule FROM employe e WHERE e.nom = 'Fleurent' AND e.prenom = 'Nicolas');

-- Vue qui permet d'afficher les employés qui travaillent sur un projet en cours (Mirolie)
DROP VIEW IF EXISTS v_projetencours_employe;
CREATE VIEW v_projetencours_employe as
SELECT e.matricule, e.nom, e.prenom
from employe e
where matricule IN(SELECT matricule from projet_employe where no_projet IN(SELECT no_projet from projet where statut = 'en cours'));
SELECT * from v_projetencours_employe;


-- #######################################################################################
-- ################################### Table Projet ######################################
-- #######################################################################################
-- (Nicolas) Création de la table
CREATE TABLE projet(
    no_projet CHAR(11) PRIMARY KEY ,
    titre VARCHAR(50),
    date_debut DATE,
    description TEXT,
    budget DOUBLE,
    nbr_employe_requis INT,
    total_salaire DOUBLE,
    statut VARCHAR(10),
    id_client INT,
    CONSTRAINT fk_projet_idClient FOREIGN KEY (id_client) REFERENCES client(id_client)
);

-- (Nicolas) Fonction pour valider si le numéro de projet est présent dans la table - fonction
DROP FUNCTION IF EXISTS f_valide_no_projet;
DELIMITER //
CREATE function f_valide_no_projet(new_id_client INT, nbr_aleatoire INT, new_date_debut DATE) RETURNS CHAR(11)
BEGIN
	DECLARE valide_no_projet CHAR(11);
	SELECT no_projet INTO valide_no_projet FROM projet WHERE no_projet = CONCAT(new_id_client, '-', LPAD(nbr_aleatoire, 2, '0'), '-',YEAR(new_date_debut));
	RETURN valide_no_projet;
END//
DELIMITER ;

-- (Nicolas) Déclencheur pour généré le numéro de projet avant l'ajout d'un projet - trrrigger
DROP TRIGGER IF EXISTS trg_bi_projet;
DELIMITER //
CREATE TRIGGER trg_bi_projet
    BEFORE INSERT
    ON projet
    FOR EACH ROW
    BEGIN
        DECLARE nbr_aleatoire INT;
        loop_numero_valide: LOOP
        SET nbr_aleatoire = FLOOR(RAND()*99+1);
        IF (SELECT f_valide_no_projet(NEW.id_client, nbr_aleatoire, NEW.date_debut)) IS NULL THEN
            SET NEW.no_projet = CONCAT(NEW.id_client, '-', LPAD(nbr_aleatoire, 2, '0'), '-',YEAR(NEW.date_debut));
            LEAVE loop_numero_valide;
        END IF;
    END LOOP ;
    end //
DELIMITER ;

-- (Nicolas) Procédure pour ajouter un projet - Procccc
DROP PROCEDURE IF EXISTS p_ajout_projet;
DELIMITER //
CREATE PROCEDURE p_ajout_projet(IN _titre VARCHAR(50), IN _date_debut DATE, IN _description TEXT, IN _budget DOUBLE,
                                IN _nbr_employe_requis INT, IN _id_client INT)
BEGIN
    DECLARE EXIT HANDLER FOR 3819
    BEGIN
        SELECT "Le nombre d'employés requis pour un projet ne peut pas excéder 5";
    END;
    DECLARE EXIT HANDLER FOR 1452
    BEGIN
        SELECT "L'identifiant du client n'est pas valide";
    END;

    INSERT INTO projet VALUES (null, _titre, _date_debut, _description, _budget, _nbr_employe_requis, DEFAULT, DEFAULT, _id_client);
END //
DELIMITER ;

-- (Nicolas) Fonction pour obtenir le nom du client - fonction
DROP FUNCTION IF EXISTS f_get_nom_client;
DELIMITER //
CREATE function f_get_nom_client(id_recherche INT) RETURNS VARCHAR(100)
BEGIN
	DECLARE nom_client VARCHAR(100);
	SELECT nom INTO nom_client FROM client WHERE id_client = id_recherche;
	RETURN nom_client;
END//
DELIMITER ;

-- (Nicolas) Vue / Requête pour voir les projets - rêquete
DROP VIEW IF EXISTS v_get_projets;
CREATE VIEW v_get_projets AS
    SELECT
        no_projet,
        titre,
        date_debut,
        description,
        budget,
        nbr_employe_requis,
        total_salaire,
        statut,
        p.id_client,
        c.nom as nom_client
    FROM projet p
    INNER JOIN client c on p.id_client = c.id_client;

-- (Nicolas) Procédure pour voir les projets
DROP PROCEDURE IF EXISTS p_get_projets;
DELIMITER //
CREATE PROCEDURE p_get_projets()
BEGIN
    SELECT * FROM v_get_projets;
END //
DELIMITER ;

-- (Nicolas) Procédure pour obtenir les projets d'un client
DROP PROCEDURE IF EXISTS p_get_projets_client;
DELIMITER //
CREATE PROCEDURE p_get_projets_client(IN _id_client INT)
BEGIN
    SELECT *
    FROM v_get_projets
    WHERE id_client = _id_client
    ORDER BY FIELD(statut,'en cours','terminé'),
             date_debut;
END //
DELIMITER ;

-- (Nicolas) Procédure pour modifier les projets
DROP PROCEDURE IF EXISTS p_modifier_projet;
DELIMITER //
CREATE PROCEDURE p_modifier_projet(IN _no_projet char(11), IN _titre varchar(50), IN _description TEXT, IN _budget DOUBLE)
BEGIN
    UPDATE projet
    SET
        titre = _titre,
        description = _description,
        budget = _budget
    WHERE no_projet = _no_projet;
END //
DELIMITER ;

-- (Nicolas) Procédure pour changer le statut à terminé
DROP PROCEDURE IF EXISTS p_terminer_projet;
DELIMITER //
CREATE PROCEDURE p_terminer_projet(IN _no_projet char(11))
BEGIN
    UPDATE projet
    SET statut = 'terminé'
    WHERE no_projet = _no_projet;
END //
DELIMITER ;

-- (Mirolie/Nicolas) Procédure pour avoir seulement les projets titre en cours
DROP procedure if exists p_get_projets_par_titre;
DELIMITER //
CREATE procedure p_get_projets_par_titre(IN titreP varchar(100))
BEGIN
    SELECT * FROM v_get_projets        
    where titre LIKE CONCAT('%', titreP, '%') AND statut = 'en cours';
end;
//
DELIMITER ;

-- (Nicolas) Ajout d'élément dans la table projet
-- Attention, modifier les id_client selon le réel si l'on éxécute se script pour la première fois (à cause des nombre aléatoire)
CALL p_ajout_projet("Retructuration de l'architecture informatique", '2022-10-10', "Recréer une nouvelle archiecture de l'infrastructure informatique du client en lui proposant un système de gestion des dossiers, un système de cybersécurité et un support technique.", 200000, 4, 972);
CALL p_ajout_projet("Audit sur le réseau inter-pavillons", CURDATE(), "Déterminer les enjeux lier à la passation d'un cable réseau entre le pavillon des humanités et le pavillons des sciences pour simplifier le transfert d'information entre ceux-ci. Valider le gain en cybersécurité, le cout et le temps nécessaire à la réalisation de cet éventuel projet.", 15800, 1, 915);
CALL p_ajout_projet("Patou Tremble en folie", '2020-09-09', "Réalisation de la plateforme web pour vendre des plats tupperwares.", 500, 1, 111);
CALL p_ajout_projet("Restructuration", CURDATE(), "Restructurer la façon de travailler en équipe pour être plus éficace dans les comunication.", 1000, 1, 124);
CALL p_ajout_projet("Mise à jours de soniaremax.com", '2023-10-03', "Enjolivement du site web de la cliente.", 15000, 2, 167);
CALL p_ajout_projet("Réparation Application Indigo", CURDATE(), "Réparation de leur application 'mal faite en tabarouette'. Mettre à jours la wishlist pour éviter les bug.", 1000000, 5, 186);
CALL p_ajout_projet("Mise à jour système informatique", '2002-04-05', "Le client désire mettre à jours l'infrastructure de son système informatique dans la surcursale.", 75750, 3, 284);
CALL p_ajout_projet("Système de communication", '2020-07-17', "Il faut mettre en place un système de communication inter-surcursale pour facilité le transfère des commandes de leur client.", 999999, 5, 410);
CALL p_ajout_projet("Paiement par tablette", '2023-08-05', "Installation d'un système de commende et paiement avec une tablette laisser au table.", 5000, 2, 455);
CALL p_ajout_projet("Sous-traitance programmation", CURDATE(), "Le client nous à contacter pour qu'on l'aide dans la programmation de son application.", 80000, 4, 534);
CALL p_ajout_projet("Bornes de commande", '2015-09-10', "Nous devons créer des bornes de commande tactile et les programmer pour qu'elles soient fonctionnel.", 2000000, 5, 614);
CALL p_ajout_projet("Recréation de planitcanada.ca", '2006-04-10', "Recréation du site web pour le rendre plus conviviale et symptathique pour les visiteurs.", 20000, 3, 802);
CALL p_ajout_projet("Erreur calcul guichets automatiques", '2022-03-30', "Il faut mettre à jour l'application des guichets automatiques pour corriger les erreurs de calcul des dépots.", 150000, 3, 853);
CALL p_ajout_projet("Local de cybersécurité", '2010-12-12', "Créer un local d'informatique avec des failles de sécurité isolé du reste du cégep pour que les élèves d'informatique effectue leur test.", 50000, 2, 931);

-- (Nicolas) Vue qui affiche les employé qui ont travailler sur des projets du client Boisdaction inc.
CREATE VIEW v_get_employe_Boisdaction AS
    SELECT
        matricule,
        prenom,
        nom
    FROM employe
    WHERE matricule IN (SELECT matricule FROM projet_employe WHERE no_projet = 972);

-- #######################################################################################
-- ################################# Table projet_employe ################################
-- #######################################################################################

-- Création de la table projet_employe (Mirolie)
DROP TABLE IF EXISTS projet_employe;
CREATE TABLE projet_employe
(
    matricule              char(10),
    no_projet              char(11),
    nbr_heure_travail      double,
    salaire_employe_projet double,
    PRIMARY KEY (matricule,no_projet),
    FOREIGN KEY (matricule) REFERENCES employe (matricule),
    FOREIGN KEY (no_projet) REFERENCES projet (no_projet)
);

-- Fonction pour vérifier le statut du projet + requête sous-requête(Mirolie) - rêquete/fonction
drop function if exists f_verif_employe_dispo;
DELIMITER //
CREATE function f_verif_employe_dispo(matEmploye CHAR(10)) RETURNS varchar(10)
BEGIN
    DECLARE sStatut varchar(10);
    SELECT statut into sStatut
    from projet
    where no_projet IN(SELECT no_projet from projet_employe where matricule = matEmploye) AND
          statut = 'en cours'
    LIMIT 1;
    RETURN sStatut;
END;
DELIMITER //

-- Déclencheur pour calculer le salaire_employe_projet avant l'insertion du projet_employe (Mirolie) - trrrigger/requête
drop trigger if exists avantAjoutProjetEmploye;
delimiter //
CREATE TRIGGER avantAjoutProjetEmploye
    before insert
    on projet_employe
    for each row
BEGIN
    DECLARE salaire double;
    IF (SELECT f_verif_employe_dispo(new.matricule) IS NULL)
    THEN
        SET new.salaire_employe_projet = round((SELECT taux_horaire
                                          from employe e
                                          where new.matricule = e.matricule) * new.nbr_heure_travail);
    ELSE
        SIGNAL SQLSTATE '45000' SET message_text = 'L''employé a déjà un projet en cours.';
    end if;
end;
//
DELIMITER ;

-- Trigger pour update le total_salaire de la table projet après l'ajout d'un projet à un employé dans la table projet_employe (Mirolie) - trrrigger/rêquete
DROP TRIGGER IF EXISTS updateCoutProjet;
DELIMITER //
CREATE TRIGGER updateCoutProjet
    after insert
    on projet_employe
    for each row
   BEGIN
       DECLARE coutTotal double;
       SELECT sum(salaire_employe_projet) into coutTotal from projet_employe where no_projet= new.no_projet;
       UPDATE projet set total_salaire = coutTotal where no_projet=new.no_projet;
   end ;
DELIMITER //

-- Procédure pour avoir les employés d'un projet (Mirolie) - rêquete
DROP procedure if exists p_get_projets_employe;
DELIMITER //
CREATE procedure p_get_projets_employe(IN num_projet char(11))
BEGIN
   SELECT e.matricule, e.prenom, e.nom,e.taux_horaire, nbr_heure_travail, salaire_employe_projet
    from employe e
             inner join projet_employe pe on e.matricule = pe.matricule
    where no_projet = num_projet;
    end;
//
DELIMITER ;

-- Procédure pour ajouter un employé à un projet avec Handler(Mirolie) - Procccc
drop procedure if exists p_ajout_projets_employe;
DELIMITER //
CREATE procedure p_ajout_projets_employe(IN matEmploye char(10), IN num_projet char(11), IN nb_heures_travail double)
BEGIN
       DECLARE CONTINUE HANDLER FOR 1062
        BEGIN
            SELECT 'Cet employé est déjà assigné à un projet.';
        END;

       DECLARE CONTINUE HANDLER FOR 1452
        BEGIN
            SELECT 'Le matricule ou le numéro de projet n''est pas dans la base de données.';
        END;
    INSERT INTO projet_employe VALUES (matEmploye, num_projet, nb_heures_travail, null);
end
//
DELIMITER ;

-- Ajout de projets aux employés (Mirolie)
CALL p_ajout_projets_employe('TH-1997-19','972-23-2001',6);
CALL p_ajout_projets_employe('TH-1997-19','931-04-2010',75.5);
CALL p_ajout_projets_employe('FL-1997-13','915-01-2023',40);
CALL p_ajout_projets_employe('AU-1997-86','284-53-2002',15.75);
CALL p_ajout_projets_employe('TU-1997-81','614-68-2015',55.25);
CALL p_ajout_projets_employe('SW-1989-86','614-68-2015',30);
CALL p_ajout_projets_employe('MO-1976-78','534-43-2023',24);
CALL p_ajout_projets_employe('PI-1980-60','614-68-2015',59);
CALL p_ajout_projets_employe('RO-1994-87','455-81-2023',61);
CALL p_ajout_projets_employe('PA-1997-79','972-82-2022',74.25);
CALL p_ajout_projets_employe('SH-1999-40','124-19-2023',20);
CALL p_ajout_projets_employe('WA-1956-45','167-69-2023',35);
CALL p_ajout_projets_employe('OB-1991-49','186-92-2023',60.5);
CALL p_ajout_projets_employe('LE-1992-46','410-88-2020',42);
CALL p_ajout_projets_employe('LA-1989-29','802-15-2006',16);
CALL p_ajout_projets_employe('DO-1965-34','853-70-2022',40.75);

-- Procédure pour rechercher par nom ou prénom un employé (Mirolie) - rêquete
drop procedure if exists p_recherche_nom_prenom_employe;
DELIMITER //
CREATE procedure p_recherche_nom_prenom_employe(IN nomOuPrenom varchar(100))
BEGIN
   SELECT * FROM employe WHERE nom LIKE CONCAT('%', nomOuPrenom, '%') OR prenom LIKE CONCAT('%', nomOuPrenom, '%');
end;
//
DELIMITER ;

-- Procédure pour avoir seulement les projets en cours (Mirolie)
DROP procedure if exists p_get_projets_encours;
DELIMITER //
CREATE procedure p_get_projets_encours()
BEGIN
   SELECT * FROM v_get_projets where statut = 'en cours';
end;
//
DELIMITER ;

-- Procédure pour faire une recherche par nom de client (Mirolie) - rêquete
drop procedure if exists p_get_clients_par_nom;
DELIMITER //
CREATE procedure p_get_clients_par_nom(IN nomP varchar(100))
BEGIN
   SELECT * FROM client WHERE nom LIKE CONCAT('%', nomP, '%');
end;
//
DELIMITER ;

-- Procédure pour avoir seulement les projets titre (Mirolie)
DROP procedure if exists p_get_projets_par_titre;
DELIMITER //
CREATE procedure p_get_projets_par_titre(IN titreP varchar(100))
BEGIN
    SELECT * FROM v_get_projets        
    where titre LIKE CONCAT('%', titreP, '%');
end;
//
DELIMITER ;

-- Vue/Requête pour avoir les projets d'un employé (Mirolie) - rêquete
DROP VIEW IF EXISTS v_get_projets_employe;
CREATE VIEW v_get_projets_employe AS
    SELECT p.no_projet, p.titre,e.matricule,p.statut
    from projet p
             inner join projet_employe pe on p.no_projet = pe.no_projet
             inner join employe e on pe.matricule = e.matricule;

-- Procédure pour avoir le projet en cours de l'employé (Mirolie)
DROP procedure if exists  p_get_employe_projet_encours;
DELIMITER //
CREATE procedure p_get_employe_projet_encours(IN matEmp varchar(10))
BEGIN
  SELECT no_projet,titre from v_get_projets_employe
    where matricule = matEmp AND statut = 'en cours';
    end;
//
DELIMITER ;

-- Procédure pour avoir les projets terminés de l'employé (Mirolie)
DROP procedure if exists p_get_employe_projets_termines;
DELIMITER //
CREATE procedure p_get_employe_projets_termines(IN matEmp varchar(10))
BEGIN
    SELECT no_projet,titre from v_get_projets_employe
    where matricule = matEmp AND statut = 'terminé';
    end;
//
DELIMITER ;



-- #######################################################################################
-- ################################# Table Admin_User ####################################
-- #######################################################################################
-- (Nicolas) Création de la table Admin_User
CREATE TABLE admin_user
(
    user VARCHAR(50) PRIMARY KEY,
    password varchar(64) NOT NULL
);

-- (Nicolas) Procédure pour valider si un compte existe déjà
DROP PROCEDURE IF EXISTS p_existe_admin;
DELIMITER //
CREATE PROCEDURE p_existe_admin()
BEGIN
    SELECT COUNT(*) AS nbr_compte FROM admin_user;
END //
DELIMITER ;

-- (Nicolas) Procédure pour ajouter un compte admin
DROP PROCEDURE IF EXISTS p_ajout_admin;
DELIMITER //
CREATE PROCEDURE p_ajout_admin(IN _user varchar(50), IN _password varchar(64))
BEGIN
    INSERT INTO admin_user VALUES (_user, _password);
END //
DELIMITER ;

-- (Nicolas) Procédure pour valider si le compte de connexion est valide
DROP PROCEDURE IF EXISTS p_valid_admin;
DELIMITER //
CREATE PROCEDURE p_valid_admin(IN _user varchar(50), IN _password varchar(64))
BEGIN
    SELECT COUNT(*) AS nbr_compte FROM admin_user WHERE user = _user AND password = _password;
END //
DELIMITER ;