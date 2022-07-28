DROP DATABASE IF EXISTS VeloMax; 
CREATE DATABASE IF NOT EXISTS VeloMax; 
USE VeloMax;

DROP TABLE IF EXISTS Modele;
CREATE TABLE IF NOT EXISTS Modele (
    num_produit_modele INT NOT NULL,
    nom VARCHAR(50) NOT NULL,
    grandeur VARCHAR(50) NULL,
    prix_unitaire REAL NULL,
    ligne_produit VARCHAR(50) NULL,
    date_intro_m DATE NULL,
    date_discont_p DATE NULL,
    quantite INT,
    PRIMARY KEY (num_produit_modele)
);
  
DROP TABLE IF EXISTS Piece_detache;
CREATE TABLE IF NOT EXISTS Piece_detache (
    num_produit INT NOT NULL AUTO_INCREMENT,
    descriptionp VARCHAR(5) NOT NULL,
    nom_fournisseur VARCHAR(50) NULL,
    num_produit_cat_fournisseur INT NULL,
    prix_unitaire REAL NULL,
    date_intro_m DATE,
    date_discont_p DATE,
    delai INT,
    quantite INT,
    PRIMARY KEY (num_produit)
);
  
  
DROP TABLE IF EXISTS Modele_Pieces;
CREATE TABLE IF NOT EXISTS Modele_Pieces (
    num_produit_modele INT NOT NULL,
    num_produit_piece INT NOT NULL,
    PRIMARY KEY (num_produit_modele , num_produit_piece),
    FOREIGN KEY (num_produit_modele)
        REFERENCES Modele (num_produit_modele)
        ON DELETE CASCADE,
    FOREIGN KEY (num_produit_piece)
        REFERENCES Piece_detache (num_produit)
        ON DELETE CASCADE
);
  
  
DROP TABLE IF EXISTS Adresse;
CREATE TABLE IF NOT EXISTS Adresse (
    id_adresse INT NOT NULL AUTO_INCREMENT,
    ville VARCHAR(50) NOT NULL,
    rue VARCHAR(50) NULL,
    code_postal INT NULL,
    province VARCHAR(50) NULL,
    PRIMARY KEY (id_adresse)
); 


DROP TABLE IF EXISTS Fournisseur;
CREATE TABLE IF NOT EXISTS Fournisseur (
    siret VARCHAR(14) NOT NULL,
    id_adresse INT NOT NULL,
    nom_entreprise VARCHAR(50) NOT NULL,
    contact VARCHAR(50) NULL,
    libelle INT NULL,
    PRIMARY KEY (siret),
    FOREIGN KEY (id_adresse)
        REFERENCES Adresse (id_adresse)
        ON DELETE CASCADE
);
  
DROP TABLE IF EXISTS Fournissement;
CREATE TABLE IF NOT EXISTS Fournissement (
    num_produit INT NOT NULL,
    siret VARCHAR(14) NOT NULL,
    PRIMARY KEY (num_produit , siret),
    FOREIGN KEY (num_produit)
        REFERENCES Piece_detache (num_produit)
        ON DELETE CASCADE,
    FOREIGN KEY (siret)
        REFERENCES Fournisseur (siret)
        ON DELETE CASCADE
);

DROP TABLE IF EXISTS Programme_fidelio;
CREATE TABLE IF NOT EXISTS Programme_fidelio (
    num_programme INT NOT NULL AUTO_INCREMENT,
    description_programme VARCHAR(50) NOT NULL,
    cout_programme INT NOT NULL,
    duree_programme INT NOT NULL,
    rabais REAL NOT NULL,
    PRIMARY KEY (num_programme)
);

DROP TABLE IF EXISTS Individu;
CREATE TABLE IF NOT EXISTS Individu (
    id_individu INT NOT NULL AUTO_INCREMENT,
    id_adresse INT NOT NULL,
    nom_individu VARCHAR(50) NOT NULL,
    prenom_individu VARCHAR(50) NULL,
    telephone_individu VARCHAR(10) NULL,
    courriel_individu VARCHAR(50) NULL,
    id_programme INT NULL,
    date_adhesion DATE NULL,
    PRIMARY KEY (id_individu),
    FOREIGN KEY (id_programme)
        REFERENCES Programme_fidelio (num_programme)
        ON DELETE CASCADE,
    FOREIGN KEY (id_adresse)
        REFERENCES Adresse (id_adresse)
        ON DELETE CASCADE
);

DROP TABLE IF EXISTS Boutique;
CREATE TABLE IF NOT EXISTS Boutique (
    id_boutique INT NOT NULL AUTO_INCREMENT,
    id_adresse INT NOT NULL,
    nom_boutique VARCHAR(50) NOT NULL,
    telephone_boutique VARCHAR(10) NULL,
    courriel_boutique VARCHAR(50) NULL,
    contact VARCHAR(50) NULL,
    PRIMARY KEY (id_boutique),
    FOREIGN KEY (id_adresse)
        REFERENCES Adresse (id_adresse)
        ON DELETE CASCADE
); 

DROP TABLE IF EXISTS Commande;
CREATE TABLE IF NOT EXISTS Commande (
    num_commande INT NOT NULL AUTO_INCREMENT,
    date_commande DATE NULL,
    date_livraison DATE NULL,
    id_individu INT NULL,
    id_boutique INT NULL,
    PRIMARY KEY (num_commande),
    FOREIGN KEY (id_individu)
        REFERENCES Individu (id_individu)
        ON DELETE CASCADE,
    FOREIGN KEY (id_boutique)
        REFERENCES Boutique (id_boutique)
        ON DELETE CASCADE,
    CHECK (id_individu IS NULL
        OR id_boutique IS NULL)
);

DROP TABLE IF EXISTS Contient_Piece;
CREATE TABLE IF NOT EXISTS Contient_Piece (
    num_commande INT NOT NULL,
    num_produit INT NOT NULL,
    quantite_commande INT NOT NULL,
    PRIMARY KEY (num_commande , num_produit),
    FOREIGN KEY (num_commande)
        REFERENCES Commande (num_commande)
        ON DELETE CASCADE,
    FOREIGN KEY (num_produit)
        REFERENCES Piece_detache (num_produit)
        ON DELETE CASCADE
);


DROP TABLE IF EXISTS Contient_modele;
CREATE TABLE IF NOT EXISTS Contient_modele (
    num_commande INT NOT NULL,
    num_produit_modele INT NOT NULL,
    quantite_commande INT NOT NULL,
    PRIMARY KEY (num_commande , num_produit_modele),
    FOREIGN KEY (num_commande)
        REFERENCES Commande (num_commande)
        ON DELETE CASCADE,
    FOREIGN KEY (num_produit_modele)
        REFERENCES Modele (num_produit_modele)
        ON DELETE CASCADE
);




#création des modèles
INSERT INTO `VeloMax`.`Modele` (`num_produit_modele`, `nom`, `grandeur`, `prix_unitaire`, `ligne_produit`, `date_intro_m`, `date_discont_p`,`quantite`) VALUES (101,'Kilimandjaro', 'Adultes',569,'VTT','2222/05/10','2022/07/01',5);
INSERT INTO `VeloMax`.`Modele` (`num_produit_modele`, `nom`, `grandeur`, `prix_unitaire`, `ligne_produit`, `date_intro_m`, `date_discont_p`,`quantite`) VALUES (102,'NorthPole','Adultes',329,'VTT','2222/05/10','2222/05/10',6);
INSERT INTO `VeloMax`.`Modele` (`num_produit_modele`, `nom`, `grandeur`, `prix_unitaire`, `ligne_produit`, `date_intro_m`, `date_discont_p`,`quantite`) VALUES (103,'MontBlanc','Jeunes',399,'VTT','2222/05/10','2222/05/10',7);
INSERT INTO `VeloMax`.`Modele` (`num_produit_modele`, `nom`, `grandeur`, `prix_unitaire`, `ligne_produit`, `date_intro_m`, `date_discont_p`,`quantite`) VALUES (104,'Hooligan','Jeunes',199,'VTT','2222/05/10','2222/05/10',3);
INSERT INTO `VeloMax`.`Modele` (`num_produit_modele`, `nom`, `grandeur`, `prix_unitaire`, `ligne_produit`, `date_intro_m`, `date_discont_p`,`quantite`) VALUES (105,'Orléans','Hommes',229,'vélo de course','2222/05/10','2222/05/10',2);
INSERT INTO `VeloMax`.`Modele` (`num_produit_modele`, `nom`, `grandeur`, `prix_unitaire`, `ligne_produit`, `date_intro_m`, `date_discont_p`,`quantite`) VALUES (106,'Orléans','Dames',229,'vélo de course','2222/05/10','2222/05/10',1);
INSERT INTO `VeloMax`.`Modele` (`num_produit_modele`, `nom`, `grandeur`, `prix_unitaire`, `ligne_produit`, `date_intro_m`, `date_discont_p`,`quantite`) VALUES (107,'BlueJay','Hommes',349,'vélo de course','2222/05/10','2222/05/10',4);
INSERT INTO `VeloMax`.`Modele` (`num_produit_modele`, `nom`, `grandeur`, `prix_unitaire`, `ligne_produit`, `date_intro_m`, `date_discont_p`,`quantite`) VALUES (108,'BlueJay','Dames',349,'vélo de course','2222/05/10','2222/05/10',2);
INSERT INTO `VeloMax`.`Modele` (`num_produit_modele`, `nom`, `grandeur`, `prix_unitaire`, `ligne_produit`, `date_intro_m`, `date_discont_p`,`quantite`) VALUES (109,'Trail Explorer','Filles',129,'Classique','2222/05/10','2222/05/10',2);
INSERT INTO `VeloMax`.`Modele` (`num_produit_modele`, `nom`, `grandeur`, `prix_unitaire`, `ligne_produit`, `date_intro_m`, `date_discont_p`,`quantite`) VALUES (110,'Trail Explorer','Garçons',129,'Classique','2222/05/10','2222/05/10',2);
INSERT INTO `VeloMax`.`Modele` (`num_produit_modele`, `nom`, `grandeur`, `prix_unitaire`, `ligne_produit`, `date_intro_m`, `date_discont_p`,`quantite`) VALUES (111,'Night Hawk','Jeunes',189,'Classique','2222/05/10','2222/05/10',3);
INSERT INTO `VeloMax`.`Modele` (`num_produit_modele`, `nom`, `grandeur`, `prix_unitaire`, `ligne_produit`, `date_intro_m`, `date_discont_p`,`quantite`) VALUES (113,'Tierra Verde','Dames',199,'Classique','2222/05/10','2222/05/10',5);
INSERT INTO `VeloMax`.`Modele` (`num_produit_modele`, `nom`, `grandeur`, `prix_unitaire`, `ligne_produit`, `date_intro_m`, `date_discont_p`,`quantite`) VALUES (114,'Mud Zinger I','Jeunes',279,'BMX','2222/05/10','2222/05/10',3);
INSERT INTO `VeloMax`.`Modele` (`num_produit_modele`, `nom`, `grandeur`, `prix_unitaire`, `ligne_produit`, `date_intro_m`, `date_discont_p`,`quantite`) VALUES (115,'MudZinger II','Adultes',359,'BMX','2222/05/10','2222/05/10',4);

# création des pièces détachés: 
# Kilimandjaro
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('C32','Ridley',12,20,'2222/05/10','2222/05/10',0,5); #CADRE
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('G7','Ridley',13,40,'2222/05/10','2222/05/10',0,6); #GUIDON
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('F3','De Rosa',25,45,'2222/05/10','2222/05/10',0,3); #FREINS
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('S88','De Rosa',18,10,'2222/05/10','2222/05/10',0,2); #SEILLE
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('DV133','Decathlon',22,50,'2222/05/10','2222/05/10',0,1); #DERAILLEUR AVANT
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('DR56','Decathlon',16,56,'2222/05/10','2222/05/10',0,6);#DERAILLEUR A
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('R45','Peugeot',19,20,'2222/05/10','2222/05/10',0,5);#ROUE AVANT
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('R46','Peugeot',19,20,'2222/05/10','2222/05/10',0,4);#ROUE ARRIERE
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('P12','Cannondale',76,20,'2222/05/10','2222/05/10',0,6); # PEDALIER
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('02','Cannondale',47,300,'2222/05/10','2222/05/10',0,1);#ORDINATEUR

# lien avec les fournisseurs de ces pieces la 
#Kilimandjaro pieces - Modele
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('101',1); #Kilimandjaro pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('101',2); #Kilimandjaro pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('101',3); #Kilimandjaro pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('101',4); #Kilimandjaro pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('101',5); #Kilimandjaro pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('101',6); #Kilimandjaro pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('101',7); #Kilimandjaro pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('101',8); #Kilimandjaro pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('101',9); #Kilimandjaro pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('101',10); #Kilimandjaro pieces


#NorthPole
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`)  VALUES ('C34','Decathlon',12,18,'2222/05/10','2222/05/10',0,3); #CADRE
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`)  VALUES ('G7','Decathlon',13,38,'2222/05/10','2222/05/10',0,2); #GUIDON
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`)  VALUES ('F3','De Rosa',25,43,'2222/05/10','2222/05/10',0,2); #FREINS
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`)  VALUES ('S88','De Rosa',18,12,'2222/05/10','2222/05/10',0,4); #SEILLE
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`)  VALUES ('DV17','Peugeot',22,51,'2222/05/10','2222/05/10',0,1); #DERAILLEUR AVANT
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`)  VALUES ('DR87','Peugeot',16,56,'2222/05/10','2222/05/10',0,3);#DERAILLEUR ARRIERE
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`)  VALUES ('R48','Cannondale',19,20,'2222/05/10','2222/05/10',0,2);#ROUE AVANT
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`)  VALUES ('R47','Cannondale',19,18,'2222/05/10','2222/05/10',0,1);#ROUE ARRIERE
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`)  VALUES ('P12','Pinarello',76,21,'2222/05/10','2222/05/10',0,2); # PEDALIER
#NorthPole Pieces - Modee
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('102',11); #NorthPole pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('102',12); #NorthPole pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('102',13); #NorthPole pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('102',14); #NorthPole pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('102',15); #NorthPole pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('102',16); #NorthPole pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('102',17); #NorthPole pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('102',18); #NorthPole pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('102',19); #NorthPole pieces

# MontBlanc
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('C76','Pinarello',12,23,'2222/05/10','2222/05/10',0,4); #CADRE
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('G7','Ridley',13,40,'2222/05/10','2222/05/10',0,2); #GUIDON
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('F3','De Rosa',25,45,'2222/05/10','2222/05/10',0,1); #FREINS
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('S88','De Rosa',18,10,'2222/05/10','2222/05/10',0,5); #SEILLE
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('DV133','Decathlon',22,50,'2222/05/10','2222/05/10',0,3); #DERAILLEUR AVANT
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('DR56','Decathlon',16,56,'2222/05/10','2222/05/10',0,2);#DERAILLEUR A
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('R45','Peugeot',19,20,'2222/05/10','2222/05/10',0,4);#ROUE AVANT
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('R46','Peugeot',19,20,'2222/05/10','2222/05/10',0,5);#ROUE ARRIERE
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('P12','Cannondale',76,20,'2222/05/10','2222/05/10',0,3); # PEDALIER
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('02','Cannondale',47,300,'2222/05/10','2222/05/10',0,2);#ORDINATEUR
#MontBlanc Pieces - Modele
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('103',20); #MontBlanc pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('103',21); #MontBlanc pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('103',22); #MontBlanc pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('103',23); #MontBlanc pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('103',24); #MontBlanc pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('103',25); #MontBlanc pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('103',26); #MontBlanc pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('103',27); #MontBlanc pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('103',28); #MontBlanc pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('103',29); #MontBlanc pieces


# Hooligan
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('C76','Pinarello',12,23,'2222/05/10','2222/05/10',0,3); #CADRE
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('G7','Ridley',13,40,'2222/05/10','2222/05/10',0,1); #GUIDON
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('F3','De Rosa',25,45,'2222/05/10','2222/05/10',0,2); #FREINS
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('S88','De Rosa',18,10,'2222/05/10','2222/05/10',0,2); #SEILLE
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('DV87','Cannondale',70,55,'2222/05/10','2222/05/10',0,4); #DERAILLEUR AVANT
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('DR86','Pinarello',16,58,'2222/05/10','2222/05/10',0,1);#DERAILLEUR A
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('R32','LOOK',10,16,'2222/05/10','2222/05/10',0,1);#ROUE AVANT
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('R46','LOOK',13,15,'2222/05/10','2222/05/10',0,4);#ROUE ARRIERE
INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`) VALUES ('P12','Cannondale',76,20,'2222/05/10','2222/05/10',0,3); # PEDALIER

# Hooligan - Pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('104',30); #Hooligan pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('104',31); #Hooligan pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('104',32); #Hooligan pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('104',33); #Hooligan pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('104',34); #Hooligan pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('104',35); #Hooligan pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('104',36); #Hooligan pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('104',37); #Hooligan pieces
INSERT INTO `VeloMax`.`Modele_pieces` (`num_produit_modele`, `num_produit_piece`) VALUES ('104',38); #Hooligan pieces

# ADRESSE des fournisseurs
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (1,'Villeneuve-d Ascq','3 rue du decathlon',59009,'France'); # decathlon
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (2,'ANTIBES','32 AV ROBERT SOLEAU',06600,'France'); # cannondale
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (3,'Paris','5 RUE ROBERT ESTIENNE ',75008,'France'); # pinarello
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (4,'NANTES','5 BD VINCENT GACHE',44200,'France'); # LOOK
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (5,'MARSEILLE','80 BD BOMPARD  ',13007 ,'France'); # De Rosa
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (6,'Beringen','26 rue du belge',35800,'Belgique'); # Ridley
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (7,'VELIZY-VILLACOUBLAY','RTE DE GIZY ',78140,'France'); # Peugeot

#Fournisseur
-- SELECT * from fournisseur f,adresse a  where f.id_adresse = a.id_adresse;
INSERT INTO `VeloMax`.`Fournisseur` (`siret`, `id_adresse`, `nom_entreprise`, `contact`, `libelle`) VALUES ('30613890000001',1,'Decathlon','loic@decathlon.fr',2); # decathlon
INSERT INTO `VeloMax`.`Fournisseur` (`siret`, `id_adresse`, `nom_entreprise`, `contact`, `libelle`) VALUES ('43464578400019',2,'Cannondale','stephane@cannondale.fr',3); # Cannondale
INSERT INTO `VeloMax`.`Fournisseur` (`siret`, `id_adresse`, `nom_entreprise`, `contact`, `libelle`) VALUES ('50164187200038',3,'Pinarello','lucas@pinarello.fr',4); # Pinarello
INSERT INTO `VeloMax`.`Fournisseur` (`siret`, `id_adresse`, `nom_entreprise`, `contact`, `libelle`) VALUES ('90780154200016',4,'LOOK','andreas.look.fr',2); # LOOK
INSERT INTO `VeloMax`.`Fournisseur` (`siret`, `id_adresse`, `nom_entreprise`, `contact`, `libelle`) VALUES ('35024705200011',5,'De Rosa','faniel@de_rosa.fr',1); # De Rosa
INSERT INTO `VeloMax`.`Fournisseur` (`siret`, `id_adresse`, `nom_entreprise`, `contact`, `libelle`) VALUES ('26376394200017',6,'Ridley','George@ridley.fr',2); # Ridley 
INSERT INTO `VeloMax`.`Fournisseur` (`siret`, `id_adresse`, `nom_entreprise`, `contact`, `libelle`) VALUES ('55210055400054',7,'Peugeot','Thierry@peugeot.fr',3); # Peugeot

#Fournissement 
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (1,'26376394200017'); # Ridley
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (2,'26376394200017'); # Ridley
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (3,'35024705200011'); # De Rosa
INSERT INTO `VeloMax`.`Fournissement` (`num_produit`, `siret`) VALUES (4,'35024705200011'); # De Rosa
INSERT INTO `VeloMax`.`Fournissement` (`num_produit`, `siret`) VALUES (5,'30613890000001'); # Decathlon
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (6,'30613890000001'); # Decathlon
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (7,'55210055400054'); # Peugeot
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (8,'55210055400054'); # Peugeot
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (9,'43464578400019'); # Cannondale
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (10,'43464578400019'); # Cannondale
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (11,'30613890000001'); # decathlon
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (12,'30613890000001'); # decathlon
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (13,'35024705200011'); # De Rosa
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (14,'35024705200011'); # De Rosa
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (15,'55210055400054'); # Peugeot
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (16,'55210055400054'); # Peugeot
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (17,'43464578400019'); # Cannondale
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (18,'43464578400019'); # Cannondale
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (19,'50164187200038'); # Pinarello
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (20,'50164187200038'); # Pinarello
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (21,'26376394200017'); # Ridley
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (22,'35024705200011'); # De Rosa
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (23,'35024705200011'); # De Rosa
INSERT INTO `VeloMax`.`Fournissement` (`num_produit` ,`siret`) VALUES (24,'30613890000001'); # decathlon
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (25,'30613890000001'); # decathlon
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (26,'55210055400054'); # Peugeot
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (27,'55210055400054'); # Peugeot
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (28,'43464578400019'); # Cannondale
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (29,'43464578400019'); # Cannondale
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (30,'50164187200038'); # Pinarello
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (31,'26376394200017'); # Ridley
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (32,'35024705200011'); # De Rosa
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (33,'35024705200011'); # De Rosa
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (34,'43464578400019'); # Cannondale
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (35,'50164187200038'); # Pinarello
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (36,'90780154200016'); # LOOK
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (37,'90780154200016'); # LOOK
INSERT INTO `VeloMax`.`Fournissement` ( `num_produit`,`siret`) VALUES (38,'43464578400019'); # Cannondale

# Programmes fidelio ( il y en a 4) :
INSERT INTO `VeloMax`.`Programme_fidelio` ( `num_programme`,`description_programme`,`cout_programme`,`duree_programme`,`rabais`) VALUES (1,'Fidelio',15,1,0.05); # Fidélio
INSERT INTO `VeloMax`.`Programme_fidelio` ( `num_programme`,`description_programme`,`cout_programme`,`duree_programme`,`rabais`) VALUES (2,'Fidelio Or',25,2,0.08); # Fidélio Or
INSERT INTO `VeloMax`.`Programme_fidelio` ( `num_programme`,`description_programme`,`cout_programme`,`duree_programme`,`rabais`) VALUES (3,'Fidelio Platine',60,2,0.10); # Fidélio Platine
INSERT INTO `VeloMax`.`Programme_fidelio` ( `num_programme`,`description_programme`,`cout_programme`,`duree_programme`,`rabais`) VALUES (4,'Fidelio Max',100,3,0.20); # Fidélio Platine

# Individus
#on commence par insérer les adresses : 
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (8,'Paris','3 rue du mbappé',75008,'France'); # mbappé
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (9,'Lyon','5 rue du Benzema',69002,'France'); # Benzema 
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (10,'Marseille','7 route de Payet',13005,'France'); # Payet
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (11,'Lille','28 rue du corchia',59006,'France'); # corchia
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (12,'Rennes','8 impasse d Andre',35004,'France'); # Andre
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (13,'Toulouse','9 rue du Dupont',31004,'France'); # Dupont
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (14,'Lens','3 route de Clauss',62218,'France'); # Clauss
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (15,'Monaco','3 rue du Golovin',98000,'Monaco'); # Golovin


INSERT INTO `VeloMax`.`Individu` ( `id_adresse`, `nom_individu`, `prenom_individu`,`telephone_individu`,`courriel_individu`,`id_programme`,`date_adhesion`) VALUES (8,'Mbappé','Kylian','0785960594','KMappe@psg.fr',4,'2020/06/20'); # mbappé
INSERT INTO `VeloMax`.`Individu` ( `id_adresse`, `nom_individu`, `prenom_individu`,`telephone_individu`,`courriel_individu`,`id_programme`,`date_adhesion`) VALUES (9,'Benzema','Karim','0788360553','KBenzema@ol.fr',2,'2020/05/25'); # Benzema
INSERT INTO `VeloMax`.`Individu` ( `id_adresse`, `nom_individu`, `prenom_individu`,`telephone_individu`,`courriel_individu`) VALUES (10,'Payet','Dimitry','0703360453','DPayet@om.fr'); # Payet
INSERT INTO `VeloMax`.`Individu` ( `id_adresse`, `nom_individu`, `prenom_individu`,`telephone_individu`,`courriel_individu`,`id_programme`,`date_adhesion`) VALUES (11,'Corchia','Sebastien','0738490573','SCorchia@losc.fr',1,'2021/05/20'); # Corchia
INSERT INTO `VeloMax`.`Individu` ( `id_adresse`, `nom_individu`, `prenom_individu`,`telephone_individu`,`courriel_individu`,`id_programme`,`date_adhesion`) VALUES (12,'Andre','Benjamin','0773849678','BAndre@rennes.fr',2,'2020/05/30'); # Andre
INSERT INTO `VeloMax`.`Individu` ( `id_adresse`, `nom_individu`, `prenom_individu`,`telephone_individu`,`courriel_individu`,`id_programme`,`date_adhesion`) VALUES (14,'Clauss','Jonathan','0712635263','JClauss@lens.fr',3,'2020/05/28'); # Clauss
INSERT INTO `VeloMax`.`Individu` ( `id_adresse`, `nom_individu`, `prenom_individu`,`telephone_individu`,`courriel_individu`) VALUES (15,'Golovin','Aleksandr','0772839403','AGolovin@monaco.fr'); # Golovin

#Boutique
#on commence par insérer les adresses : 
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (16,'Giberville','121 rue du cimetiere',14730,'France'); # boutique 1
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (17,'Caen','147 rue pierre camus',14000,'France'); # boutique 2 
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (18,'Saint-Quentin','35 rle des quize setiers',02100,'France'); # boutique 3
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (19,'Lille','68 rue de guise',59006,'France'); # boutique 4
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (20,'Ham','3 route de la chaussee romaine',80400,'France'); # boutique 5
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (21,'Toulouse','9 gauche de gauchy',31004,'France'); # boutique 6
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (22,'Paris','33 rue de bourgogne',75007,'France'); # boutique 7
INSERT INTO `VeloMax`.`Adresse` (`id_adresse`, `ville`, `rue`, `code_postal`, `province`) VALUES (23,'Monaco','7 rue antoine parmentier',98000,'Monaco'); # boutique 8

INSERT INTO `VeloMax`.`Boutique` ( `id_boutique`,`id_adresse`, `nom_boutique`, `telephone_boutique`,`courriel_boutique`,`contact`) VALUES (101,16,'boutique_1','0185960594','boutique_1@yahoo.fr','Franck'); # boutique 1
INSERT INTO `VeloMax`.`Boutique` ( `id_adresse`, `nom_boutique`, `telephone_boutique`,`courriel_boutique`,`contact`)  VALUES (17,'boutique_2','0188960653','boutique_2@yahoo.fr','Lea'); # boutique 2
INSERT INTO `VeloMax`.`Boutique` ( `id_adresse`, `nom_boutique`, `telephone_boutique`,`courriel_boutique`,`contact`) VALUES (18,'boutique_3','0103463453','boutique_3@yahoo.fr','Andreas'); # boutique 3
INSERT INTO `VeloMax`.`Boutique` ( `id_adresse`, `nom_boutique`, `telephone_boutique`,`courriel_boutique`,`contact`)  VALUES (19,'boutique_4','0138390573','boutique_4@yahoo.fr','Camille'); # boutique 4
INSERT INTO `VeloMax`.`Boutique` ( `id_adresse`, `nom_boutique`, `telephone_boutique`,`courriel_boutique`,`contact`)  VALUES (20,'boutique_5','0173890678','boutique_5@yahoo.fr','Leo'); # boutique 5
INSERT INTO `VeloMax`.`Boutique` ( `id_adresse`, `nom_boutique`, `telephone_boutique`,`courriel_boutique`,`contact`)  VALUES (21,'boutique_6','0145635263','boutique_6@yahoo.fr','Ambre'); # boutique 6
INSERT INTO `VeloMax`.`Boutique` ( `id_adresse`, `nom_boutique`, `telephone_boutique`,`courriel_boutique`,`contact`)  VALUES (22,'boutique_7','0170849403','boutique_7@yahoo.fr','Brice'); # boutique 7

#Insertion des commandes : commandes individus
INSERT INTO `VeloMax`.`Commande`( `date_commande`, `date_livraison`,`id_individu`,`id_boutique`) VALUES ('2022/04/30','2022/05/15',1,null); # commande mbappé
INSERT INTO `VeloMax`.`Commande`( `date_commande`, `date_livraison`,`id_individu`,`id_boutique`) VALUES ('2022/04/20','2022/05/05',2,null); # commande benzema
INSERT INTO `VeloMax`.`Commande`( `date_commande`, `date_livraison`,`id_individu`,`id_boutique`) VALUES ('2022/04/15','2022/05/04',3,null); # commande payet
INSERT INTO `VeloMax`.`Commande`( `date_commande`, `date_livraison`,`id_individu`,`id_boutique`) VALUES ('2022/04/14','2022/05/03',4,null); # commande corchia
INSERT INTO `VeloMax`.`Commande`( `date_commande`, `date_livraison`,`id_individu`,`id_boutique`) VALUES ('2022/04/17','2022/05/12',5,null); # commande andre
INSERT INTO `VeloMax`.`Commande`( `date_commande`, `date_livraison`,`id_individu`,`id_boutique`) VALUES ('2022/04/19','2022/05/10',6,null); # commande clauss
INSERT INTO `VeloMax`.`Commande`( `date_commande`, `date_livraison`,`id_individu`,`id_boutique`) VALUES ('2022/04/28','2022/05/19',7,null); # commande golovin
# contenu des commandes :
#modeles:  
INSERT INTO `VeloMax`.`Contient_modele`( `num_commande`, `num_produit_modele`, `quantite_commande`) VALUES (1,101,1); # commande mbappé - Kilimandjaro
INSERT INTO `VeloMax`.`Contient_modele`( `num_commande`, `num_produit_modele`, `quantite_commande`) VALUES (2,101,2); # commande benzema - Kilimandjaro
INSERT INTO `VeloMax`.`Contient_modele`( `num_commande`, `num_produit_modele`, `quantite_commande`) VALUES (3,102,2); # commande payet - NorthPole
INSERT INTO `VeloMax`.`Contient_modele`( `num_commande`, `num_produit_modele`, `quantite_commande`) VALUES (4,102,3); # commande corchia - NorthPole
INSERT INTO `VeloMax`.`Contient_modele`( `num_commande`, `num_produit_modele`, `quantite_commande`) VALUES (5,103,1); # commande andre - MontBlanc
INSERT INTO `VeloMax`.`Contient_modele`( `num_commande`, `num_produit_modele`, `quantite_commande`) VALUES (6,103,3); # commande clauss - MontBlanc
INSERT INTO `VeloMax`.`Contient_modele`( `num_commande`, `num_produit_modele`, `quantite_commande`) VALUES (7,104,2); # commande golovin - Hooligan

# pieces
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (1,1,1); # commande mbappé - C32 -- peut etre pour les pieces ajouter attributs modele et liste piece de rechange dans la classe commande
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (1,2,2); # commande mbappé - G7
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (1,3,2); # commande mbappé - F3
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (1,7,2); # commande mbappé - R45
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (1,8,1); # commande mbappé - R46

INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (2,1,1); # commande benzema - C32
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (2,7,4); # commande benzema - R45
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (2,8,4); # commande benzema - R46

INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (3,11,2); # commande Payet - C34
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (3,12,2); # commande Payet - G7
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (3,17,4); # commande Payet - R48
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (3,18,4); # commande Payet - R47

INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (4,17,6); # commande corchia - R48
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (4,18,6); # commande corchia - R47

INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (5,24,1); # commande andre - DV133
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (5,25,1); # commande andre - DR56
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (5,29,1); # commande andre - 02

INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (6,24,2); # commande Clauss - DV133
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (6,25,2); # commande Clauss - DR56
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (6,26,3); # commande Clauss - R45
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (6,27,3); # commande Clauss - R46

INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (7,38,2); # commande Golovin - P12


#Insertion des commandes : commandes Boutique
INSERT INTO `VeloMax`.`Commande`( `date_commande`, `date_livraison`,`id_individu`,`id_boutique`) VALUES ('2022/04/10','2022/05/12',null,101); # commande Boutique 1
INSERT INTO `VeloMax`.`Commande`( `date_commande`, `date_livraison`,`id_individu`,`id_boutique`) VALUES ('2022/04/26','2022/06/02',null,102); # commande Boutique 2
INSERT INTO `VeloMax`.`Commande`( `date_commande`, `date_livraison`,`id_individu`,`id_boutique`) VALUES ('2022/04/20','2022/05/14',null,103); # commande Boutique 3
INSERT INTO `VeloMax`.`Commande`( `date_commande`, `date_livraison`,`id_individu`,`id_boutique`) VALUES ('2022/04/29','2022/05/08',null,104); # commande Boutique 4
INSERT INTO `VeloMax`.`Commande`( `date_commande`, `date_livraison`,`id_individu`,`id_boutique`) VALUES ('2022/04/10','2022/05/10',null,105); # commande Boutique 5
INSERT INTO `VeloMax`.`Commande`( `date_commande`, `date_livraison`,`id_individu`,`id_boutique`) VALUES ('2022/04/13','2022/05/01',null,106); # commande Boutique 6
INSERT INTO `VeloMax`.`Commande`( `date_commande`, `date_livraison`,`id_individu`,`id_boutique`) VALUES ('2022/04/08','2022/05/09',null,107); # commande Boutique 7

# contenu des commandes : 
#modeles
INSERT INTO `VeloMax`.`Contient_modele`( `num_commande`, `num_produit_modele`, `quantite_commande`) VALUES (8,101,8); # commande Boutique 1 - Kilimandjaro
INSERT INTO `VeloMax`.`Contient_modele`( `num_commande`, `num_produit_modele`, `quantite_commande`) VALUES (9,101,10); # commande Boutique 2 - Kilimandjaro
INSERT INTO `VeloMax`.`Contient_modele`( `num_commande`, `num_produit_modele`, `quantite_commande`) VALUES (10,102,7); # commande Boutique 3 - NorthPole
INSERT INTO `VeloMax`.`Contient_modele`( `num_commande`, `num_produit_modele`, `quantite_commande`) VALUES (11,102,9); # commande Boutique 4 - NorthPole
INSERT INTO `VeloMax`.`Contient_modele`( `num_commande`, `num_produit_modele`, `quantite_commande`) VALUES (12,103,5); # commande Boutique 5 - MontBlanc
INSERT INTO `VeloMax`.`Contient_modele`( `num_commande`, `num_produit_modele`, `quantite_commande`) VALUES (13,103,12); # commande Boutique 6 - MontBlanc
INSERT INTO `VeloMax`.`Contient_modele`( `num_commande`, `num_produit_modele`, `quantite_commande`) VALUES (14,104,17);

#pieces
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (8,1,8); # commande Boutique 1 - C32 -- peut etre pour les pieces ajouter attributs modele et liste piece de rechange dans la classe commande
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (8,2,16); # commande Boutique 1 - G7
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (8,3,10); # commande Boutique 1 - F3
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (8,7,18); # commande Boutique 1 - R45
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (8,10,8); # commande Boutique 1 - O2

INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (9,1,10); # commande Boutique 2 - C32
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (9,7,20); # commande Boutique 2 - R45
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (9,8,20); # commande Boutique 2 - R46

INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (10,11,14); # commande Boutique 3 - C34
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (10,12,14); # commande Boutique 3 - G7
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (10,17,10); # commande Boutique 3 - R48
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (10,18,10); # commande Boutique 3 - R47

INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (11,17,20); # commande Boutique 4 - R48
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (11,18,20); # commande Boutique 4 - R47

INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (12,24,6); # commande Boutique 5 - DV133
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (12,25,6); # commande Boutique 5 - DR56
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (12,29,8); # commande Boutique 5 - 02

INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (13,24,20); # commande Boutique 6 - DV133
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (13,25,24); # commande Boutique 6 - DR56
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (13,26,15); # commande Boutique 6 - R45
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (13,27,15); # commande Boutique 6 - R46
INSERT INTO `VeloMax`.`Contient_piece`( `num_commande`, `num_produit`, `quantite_commande`) VALUES (14,38,20); # commande Boutique 7 - P12
