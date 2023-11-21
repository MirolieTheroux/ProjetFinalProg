-- Nicolas Fleurent
-- Script sql pour recréer la bd au bonne endroit quand elle sera faite

-- (Nico) Suppresion des tables au besoin
DROP TABLE IF EXISTS client;
DROP TABLE IF EXISTS admin_user;

-- #######################################################################################
-- ################################### Table Client ######################################
-- #######################################################################################
-- (Nico) Création de la table Client
CREATE TABLE client (
    id_client INT PRIMARY KEY ,
    nom VARCHAR(100),
    adresse VARCHAR(255),
    no_telephone VARCHAR(20),
    email VARCHAR(255),
    CONSTRAINT ck_client_id CHECK ( id_client >= 100 AND id_client <= 999 )
);

-- (Nico) Procédure pour valider si il reste des numéros de clients disponible
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

-- (Nico) Procédure pour ajouter un client
DROP PROCEDURE IF EXISTS p_ajout_client;
DELIMITER //
CREATE PROCEDURE p_ajout_client(IN _nom VARCHAR(100), IN _adresse VARCHAR(255), IN _no_telephone VARCHAR(20), IN _email VARCHAR(255))
BEGIN
    INSERT INTO client VALUES (null, _nom, _adresse, _no_telephone, _email);
END //
DELIMITER ;

-- (Nico) Trigger pour généré le nombre aléatoire avant l'ajout d'un client
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

-- (Nico) Procédure pour obtenir les clients
DROP PROCEDURE IF EXISTS p_get_clients;
DELIMITER //
CREATE PROCEDURE p_get_clients()
BEGIN
    SELECT * FROM client
    ORDER BY nom;
END //
DELIMITER ;

-- (Nico) Procédure pour modifier les clients
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

-- #######################################################################################
-- ################################# Table Admin_User ####################################
-- #######################################################################################
-- (Nico) Création de la table Admin_User
CREATE TABLE admin_user
(
    user VARCHAR(50) PRIMARY KEY,
    password varchar(64) NOT NULL
);

-- (Nico) Procédure pour valider si un compte existe déjà
DROP PROCEDURE IF EXISTS p_existe_admin;
DELIMITER //
CREATE PROCEDURE p_existe_admin()
BEGIN
    SELECT COUNT(*) AS nbr_compte FROM admin_user;
END //
DELIMITER ;

-- (Nico) Procédure pour ajouter un compte admin
DROP PROCEDURE IF EXISTS p_ajout_admin;
DELIMITER //
CREATE PROCEDURE p_ajout_admin(IN _user varchar(50), IN _password varchar(64))
BEGIN
    INSERT INTO admin_user VALUES (_user, _password);
END //
DELIMITER ;

-- (Nico) Procédure pour valider si le compte de connexion est valide
DROP PROCEDURE IF EXISTS p_valid_admin;
DELIMITER //
CREATE PROCEDURE p_valid_admin(IN _user varchar(50), IN _password varchar(64))
BEGIN
    SELECT COUNT(*) AS nbr_compte FROM admin_user WHERE user = _user AND password = _password;
END //
DELIMITER ;

-- #######################################################################################
-- ################################### Table Projet ######################################
-- #######################################################################################
-- (Nico) Création de la table
