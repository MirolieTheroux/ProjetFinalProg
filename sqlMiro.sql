-- Mirolie Théroux

-- Modèle relationnel (Mirolie)
-- Employe : matricule (PK), nom, prenom, date_naissance, email, adresse, date_embauche, taux_horaire, lien_photo, statut
-- Client : id_client (PK), nom, adresse, no_telephone, email
-- Projet : no_projet (PK), titre, date_debut, description, budget, nbr_employe_requis, total_salaire, statut, id_client*,
-- Projet_employe : matricule*(PK), no_projet*(PK), nbr_heure_travail, salaire_employe_projet


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
CREATE procedure p_get_employes()
begin
    SELECT *
    from employe
    order by matricule;
end;

-- Procédure pour ajouter un employé dans la BD (Mirolie)
DROP PROCEDURE IF EXISTS p_ajouter_employes;
CREATE procedure p_ajouter_employes(IN nomP varchar(50), IN prenomP varchar(50), IN date_naissanceP date,
                                    IN emailP varchar(255), IN adresseP varchar(255), IN date_embaucheP date,
                                    IN taux_horaireP double, IN lien_photoP varchar(255), IN statutP varchar(11))
BEGIN
    INSERT into employe 
    values (null,nomP, prenomP, date_naissanceP, emailP, adresseP, date_embaucheP, taux_horaireP, lien_photoP, statutP);
end;

-- Procédure pour modifier un employé (Mirolie)
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

-- Déclencheur pour générer un matricule à l'employé avant l'ajout (Mirolie)
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

-- Fonction pour calculer le nombre d'années d'ancienneté (Mirolie)
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

-- Procédure pour avoir les projets des employés (Mirolie)
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

-- Procédure pour ajouter un employé à un projet avec Handler(Mirolie)
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

-- Fonction pour vérifier le statut du projet + requête sous-requête(Mirolie)
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

-- Déclencheur pour calculer le salaire_employe_projet avant l'insertion du projet_employe (Mirolie)
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

-- Trigger pour update le total_salaire de la table projet après l'ajout d'un projet à un employé dans la table projet_employe (Mirolie)
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

-- Procédure pour rechercher par nom ou prénom un employé (Mirolie)
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
    SELECT no_projet,
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
    where p.statut = 'en cours';
end;
//
DELIMITER ;