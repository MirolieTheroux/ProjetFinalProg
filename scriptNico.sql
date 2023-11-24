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
CALL p_ajout_client('Boisdaction inc.', '2165 rue Jules Paquette', '819-621-2333','info@boisdaction.com');
CALL p_ajout_client('Cégep de Trois-Rivières', '3500 rue de Courval', '819-376-1721','info@cegeptr.qc.ca');

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

-- (Nicolas) Procédure pour obtenir les projets
DROP PROCEDURE IF EXISTS p_get_projets;
DELIMITER //
CREATE PROCEDURE p_get_projets()
BEGIN
    SELECT * FROM projet;
END //
DELIMITER ;

-- (Nicolas) Ajout d'élément dans la table projet
-- Attention, modifier les id_client selon le réel si l'on éxécute se script pour la première fois (à cause des nombre aléatoire)
CALL p_ajout_projet("Retructuration de l'architecture informatique", '2022-10-10', "Recréer une nouvelle archiecture de l'infrastructure informatique du client en lui proposant un système de gestion des dossiers, un système de cybersécurité et un support technique.", 200000, 4, 972);
CALL p_ajout_projet("Audit sur le réseau inter-pavillons", CURDATE(), "Déterminer les enjeux lier à la passation d'un cable réseau entre le pavillon des humanités et le pavillons des sciences pour simplifier le transfert d'information entre ceux-ci. Valider le gain en cybersécurité, le cout et le temps nécessaire à la réalisation de cet éventuel projet.", 15800, 1, 915);











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