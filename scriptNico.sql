-- Nicolas Fleurent
-- Script sql pour recréer la bd au bonne endroit quand elle sera faite

-- (Nicolas) Suppresion des tables au besoin
DROP TABLE IF EXISTS projet;
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

-- (Nicolas) Procédure pour valider si il reste des numéros de clients disponible
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

-- (Nicolas) Procédure pour ajouter un client
DROP PROCEDURE IF EXISTS p_ajout_client;
DELIMITER //
CREATE PROCEDURE p_ajout_client(IN _nom VARCHAR(100), IN _adresse VARCHAR(255), IN _no_telephone VARCHAR(20), IN _email VARCHAR(255))
BEGIN
    INSERT INTO client VALUES (null, _nom, _adresse, _no_telephone, _email);
END //
DELIMITER ;

-- (Nicolas) Déclencheur pour généré le nombre aléatoire avant l'ajout d'un client
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

-- (Nicolas) Procédure pour obtenir les clients
DROP PROCEDURE IF EXISTS p_get_clients;
DELIMITER //
CREATE PROCEDURE p_get_clients()
BEGIN
    SELECT * FROM client
    ORDER BY nom;
END //
DELIMITER ;

-- (Nicolas) Procédure pour modifier les clients
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

-- (Nicolas) Fonction pour valider si le numéro de projet est présent dans la table
DROP FUNCTION IF EXISTS f_valide_no_projet;
DELIMITER //
CREATE function f_valide_no_projet(new_id_client INT, nbr_aleatoire INT, new_date_debut DATE) RETURNS CHAR(11)
BEGIN
	DECLARE valide_no_projet CHAR(11);
	SELECT no_projet INTO valide_no_projet FROM projet WHERE no_projet = CONCAT(new_id_client, '-', LPAD(nbr_aleatoire, 2, '0'), '-',YEAR(new_date_debut));
	RETURN valide_no_projet;
END//
DELIMITER ;

-- (Nicolas) Déclencheur pour généré le numéro de projet avant l'ajout d'un projet
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

-- (Nicolas) Procédure pour ajouter un projet
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

-- (Nicolas) Fonction pour obetnir le nom du client
DROP FUNCTION IF EXISTS f_get_nom_client;
DELIMITER //
CREATE function f_get_nom_client(id_recherche INT) RETURNS VARCHAR(100)
BEGIN
	DECLARE nom_client VARCHAR(100);
	SELECT nom INTO nom_client FROM client WHERE id_client = id_recherche;
	RETURN nom_client;
END//
DELIMITER ;

-- (Nicolas) Requête pour voir les projets
DROP PROCEDURE IF EXISTS p_get_projets;
DELIMITER //
CREATE PROCEDURE p_get_projets()
BEGIN
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
END //
DELIMITER ;

-- (Nicolas) Procédure pour obtenir les projets d'un client
DROP PROCEDURE IF EXISTS p_get_projets_client;
DELIMITER //
CREATE PROCEDURE p_get_projets_client(IN _id_client INT)
BEGIN
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
    INNER JOIN client c on p.id_client = c.id_client
    WHERE p.id_client = _id_client
    ORDER BY FIELD(statut,'en cours','terminé'),
             date_debut;
END //
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