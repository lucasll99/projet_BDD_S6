using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_VeloMax
{
    public class Velo_Max
    {
        #region attributs
        Dictionary<int, Piece_Detache> pieces_detaches = new Dictionary<int, Piece_Detache> { };
        Dictionary<int, Modele> modeles = new Dictionary<int, Modele> { };
        Dictionary<string, Fournisseur> fournisseurs = new Dictionary<string, Fournisseur> { };
        Dictionary<int, Adresse> adresses = new Dictionary<int, Adresse> { };
        Dictionary<int,Individu> individus = new Dictionary<int,Individu> { };
        List<Boutique> boutiques = new List<Boutique> { };
        Dictionary<int, Commande> commandes = new Dictionary<int, Commande> { };
        Dictionary<int, Programme_Fidelio> programmes_fidelio = new Dictionary<int, Programme_Fidelio> { };
        Dictionary<int,Client> clients = new Dictionary<int,Client> { };
        Dictionary<string, Fournisseur> pieces_fournisseurs = new Dictionary<string, Fournisseur> { };
        #endregion

        #region getter et setter
        public Dictionary<int, Piece_Detache> Pieces_detaches { get => pieces_detaches; set => pieces_detaches = value; }
        public Dictionary<int, Modele> Modeles { get => modeles; set => modeles = value; }
        public Dictionary<string, Fournisseur> Fournisseurs { get => fournisseurs; set => fournisseurs = value; }
        public Dictionary<int, Adresse> Adresses { get => adresses; set => adresses = value; }
        public Dictionary<int,Individu> Individus { get => individus; set => individus = value; }
        internal List<Boutique> Boutiques { get => boutiques; set => boutiques = value; }
        public Dictionary<int, Commande> Commandes { get => commandes; set => commandes = value; }
        public Dictionary<int, Programme_Fidelio> Programmes_fidelio { get => programmes_fidelio; set => programmes_fidelio = value; }
        public Dictionary<int, Client> Clients { get => clients; set => clients = value; }
        public Dictionary<string, Fournisseur> Pieces_fournisseurs { get => pieces_fournisseurs; set => pieces_fournisseurs = value; }

        #endregion

        #region Chargement de la BDD table par table
        static MySqlConnection ConnectionBDD()
        {
            MySqlConnection maConnexion = null;

            try
            {
                //a. indiquer les informations pour la connexion
                string connexionInfo = "SERVER=localhost;PORT=3306;" +
                    "DATABASE=VeloMax;UID=root;PASSWORD=Lucas070799$";
                //b. objet de connexion
                maConnexion = new MySqlConnection(connexionInfo);
                //c. ouvrir le canal de communication/connexion
                maConnexion.Open();

            }
            catch (MySqlException e)
            {
                Console.WriteLine("Erreur de connexion : " + e.ToString());
                //return;
            }
            return maConnexion;
        }

        public void Chargement_Pieces_Detaches()//charger table Piece_detache
        {
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = " SELECT DISTINCT *"
                                + " FROM Piece_detache;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            while (reader.Read())   // parcours ligne par ligne
            {
                Piece_Detache piece = new Piece_Detache(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetDouble(4), reader.GetDateTime(5), reader.GetDateTime(6), reader.GetInt32(7), reader.GetInt32(8));
                //Console.WriteLine(reader.GetDouble(0));
                //Console.WriteLine(piece);
                this.pieces_detaches.Add(reader.GetInt32(0), piece);
            }

            connection.Close();
        }

        public void Chargement_Modeles()//charger table Modele
        {

            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = " SELECT DISTINCT *"
                                + " FROM Modele;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            while (reader.Read())   // parcours ligne par ligne
            {
                Modele modele1 = new Modele(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetDouble(3), reader.GetString(4), reader.GetDateTime(5), reader.GetDateTime(6), reader.GetInt32(7));
                //Console.WriteLine(modele1);
                this.modeles.Add(reader.GetInt32(0), modele1);
            }
            Console.WriteLine("Nombres de modele: " + this.modeles.Count);
            connection.Close();

        }


        public void Chargement_Pieces_Modeles()
        {
            foreach(int num_modele in this.modeles.Keys)
            {
                MySqlConnection connection = ConnectionBDD();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = " SELECT DISTINCT *"
                                    + " FROM modele_pieces where num_produit_modele = @num_produit_modele;";
                command.Parameters.AddWithValue("@num_produit_modele", num_modele);
                MySqlDataReader reader;
                reader = command.ExecuteReader();

                while (reader.Read())   // parcours ligne par ligne
                {
                    this.modeles[num_modele].Pieces.Add(this.pieces_detaches[reader.GetInt32(1)]); 
                }
                connection.Close();
            }
        }

        public void Chargement_Adresse() //charger table adresse
        {

            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = " SELECT DISTINCT *"
                                + " FROM Adresse;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            while (reader.Read())   // parcours ligne par ligne
            {
                Adresse adresse1 = new Adresse(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetString(4));
                //Console.WriteLine(adresse1);
                this.adresses.Add(reader.GetInt32(0), adresse1);
            }
            Console.WriteLine("Nombre total de modèle : " + this.modeles.Count);
            connection.Close();

        }

        public void Chargement_Fournisseur() //charger table fournisseur auquel on ajoute l'ensemble des pieces qui sont fournis(liste pieces fournies dans classe fournisseur)
        {
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = " select f.siret,f.id_adresse,f.nom_entreprise,f.contact,f.libelle,m.num_produit"
                                + " from fournisseur f inner join fournissement m on f.siret = m.siret;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            while (reader.Read())   // parcours ligne par ligne
            {
                if (this.fournisseurs.ContainsKey(reader.GetString(0))) // si ajout d'une piece ou d'un modele dans la commande (commande deja présente dans la liste des commandes)
                {
                    if ((this.fournisseurs[reader.GetString(0)].Pieces_detache_fournies.ContainsKey(reader.GetInt32(5))) != true) // si fournisseur deja existant et que piece detache n'est pas deja dans la liste(dico) des piece detachées fournit par le fournisseur, alors on l'ajoute dedans
                    {
                        this.fournisseurs[reader.GetString(0)].Pieces_detache_fournies.Add(reader.GetInt32(5), this.pieces_detaches[reader.GetInt32(5)]); //on l'ajoute
                    }
                }
                else //si nouveux fournisseur(pas encore chargé dans la liste des fournisseurs): 
                {
                    Adresse adresse1 = adresses[reader.GetInt32(1)];//on recupère l'adresse
                    Fournisseur fournisseur1 = new Fournisseur(reader.GetString(0), adresse1, reader.GetString(2), reader.GetString(3), reader.GetInt32(4));

                    fournisseur1.Pieces_detache_fournies.Add(reader.GetInt32(5), this.pieces_detaches[reader.GetInt32(5)]);
                    this.fournisseurs.Add(reader.GetString(0), fournisseur1);
                }
                
            }
            Console.WriteLine("Nombre total de fournisseur : " + this.fournisseurs.Count);
            //foreach(Fournisseur i in this.fournisseurs.Values) { Console.WriteLine(i); }
            connection.Close();

        }

        public void Chargement_Programmes()
        {
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = " SELECT DISTINCT *"
                                + " FROM programme_fidelio;";
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            while (reader.Read())   // parcours ligne par ligne
            {
                Programme_Fidelio programme = new Programme_Fidelio(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetFloat(4));
                //Console.WriteLine(adresse1);
                this.programmes_fidelio.Add(reader.GetInt32(0), programme);
            }
            Console.WriteLine("Nombre total de programme : " + this.programmes_fidelio.Count);
            connection.Close();
        }

        public void Chargement_Individu() //charger table fournisseur
        {

            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = " SELECT DISTINCT *"
                                + " FROM Individu;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            while (reader.Read())   // parcours ligne par ligne
            {
                Individu individu1 = null; // création individu null pour les deux cas diff(programme et sans programme)
                Adresse adresse1 = adresses[reader.GetInt32(1)];//on recupère l'adresse
                if (!reader.IsDBNull(6)) // si adhesion à un programme 
                {
                    individu1 = new Individu(reader.GetInt32(0), adresse1, reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), this.programmes_fidelio[reader.GetInt32(6)], reader.GetDateTime(7));
                }
                else// si adhesion à aucun programme 
                {
                    individu1 = new Individu(reader.GetInt32(0), adresse1, reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
                }
                //Console.WriteLine(individu1);
                this.individus.Add(individu1.Id_client,individu1);
            }
            Console.WriteLine("Nombre total d'individu : " + this.individus.Count);
            connection.Close();

        }

        public void Chargement_Boutique() //charger table fournisseur
        {

            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = " SELECT DISTINCT *"
                                + " FROM Boutique;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            while (reader.Read())   // parcours ligne par ligne
            {
                Adresse adresse1 = adresses[reader.GetInt32(1)];//on recupère l'adresse
                Boutique boutique1 = new Boutique(reader.GetInt32(0), adresse1, reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
                //Console.WriteLine(boutique1);
                this.boutiques.Add(boutique1);
            }
            Console.WriteLine("Nombre total de boutique: " + this.boutiques.Count);
            connection.Close();

        }

        public void Chargement_Client() // ne sert à rien, juste pour faire une requete utilisant l'union
        {
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = " select id_individu, id_adresse, nom_individu as nom,telephone_individu as tel,courriel_individu as courriel from individu i" 
                                + " Union " 
                                +"select id_boutique,id_adresse,nom_boutique as nom,telephone_boutique as tel,courriel_boutique as courriel from boutique;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            while (reader.Read())   // parcours ligne par ligne
            {
                Adresse adresse1 = adresses[reader.GetInt32(1)];//on recupère l'adresse
                Client client1 = new Client(reader.GetInt32(0), adresse1, reader.GetString(2), reader.GetString(3), reader.GetString(4));
                //Console.WriteLine(boutique1);
                if (!this.clients.ContainsKey(reader.GetInt32(0))) { this.clients.Add(reader.GetInt32(0),client1); }
            }
            Console.WriteLine("Nombre total de clients: " + this.clients.Count);
            connection.Close();

        }

        public void Chargement_Commande()
        {

            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = " select distinct c.num_commande, c.date_commande, c.date_livraison, c.id_individu,c.id_boutique,m.num_produit_modele, m.quantite_commande as quantite_modele, p.num_produit, p.quantite_commande as quantite_pieces FROM commande c"
                                + " inner join" 
                                + " Contient_modele m on c.num_commande = m.num_commande" 
                                + " inner join"
                                + " Contient_piece p on c.num_commande = p.num_commande;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            int num_client = 0; // on recupère le numéro du client pour ensuite lui attribuer 
            while (reader.Read())   // parcours ligne par ligne
            {
                if (this.commandes.ContainsKey(reader.GetInt32(0))) // si ajout d'une piece ou d'un modele dans la commande (commande deja présente dans la liste des commandes)
                {
                    if ((this.commandes[reader.GetInt32(0)].Modele1.ContainsKey(this.modeles[reader.GetInt32(5)])) != true)
                    {
                        this.commandes[reader.GetInt32(0)].Modele1.Add(this.modeles[reader.GetInt32(5)], reader.GetInt32(6));
                    }
                    if (this.commandes[reader.GetInt32(0)].Liste_pieces_detachee.ContainsKey(this.pieces_detaches[reader.GetInt32(7)]) != true)
                    {
                        this.commandes[reader.GetInt32(0)].Liste_pieces_detachee.Add(this.pieces_detaches[reader.GetInt32(7)], reader.GetInt32(8));
                    }
                }
                else //si nouvelle commande: 
                {
                    Commande commande1 = null;
                    //Console.WriteLine(reader.GetString(1));
                    if (!reader.IsDBNull(4))
                    {
                        commande1 = new Commande(reader.GetInt32(0), reader.GetDateTime(1), reader.GetDateTime(2), this.clients[reader.GetInt32(4)]);//cas client est une boutique
                        this.clients[reader.GetInt32(4)].Commandes_client.Add(commande1);

                    }
                    else
                    {
                        commande1 = new Commande(reader.GetInt32(0), reader.GetDateTime(1), reader.GetDateTime(2), this.clients[reader.GetInt32(3)]); //cas client est un individu
                        this.clients[reader.GetInt32(3)].Commandes_client.Add(commande1);
                        this.individus[reader.GetInt32(3)].Commandes_client.Add(commande1);
                    }
                    commande1.Modele1.Add(this.modeles[reader.GetInt32(5)], reader.GetInt32(6));
                    commande1.Liste_pieces_detachee.Add(this.pieces_detaches[reader.GetInt32(7)], reader.GetInt32(8));
                    this.commandes.Add(commande1.Num_commande, commande1);
                }
            }
            //foreach(Commande c in this.commandes.Values) { Console.WriteLine(c); }
            foreach(Commande commande1 in this.commandes.Values) { commande1.Prix_Total(); }
            foreach(Client client1 in this.clients.Values) { client1.Total_Depenses(); }
            foreach (Individu individu1 in this.individus.Values) { individu1.Total_Depenses(); }
            Console.WriteLine("Nombre total de commandes: " + this.commandes.Count);
            //foreach (Commande commande1 in this.commandes.Values) { Console.WriteLine(commande1); }
            connection.Close();

        }
        


        #endregion


        #region MAJ (update) des tables : 

        public void Update_Piece_Detache(Piece_Detache piece_d=null)
        {
            if(piece_d == null)
            {

                Console.WriteLine("Veuillez rentrer les nouvelles informations de la piece:"); 

            }
            else
            {
                MySqlConnection connection = ConnectionBDD();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Piece_Detache SET descriptionp = @descriptionp, nom_fournisseur = @fournisseur, num_produit_cat_fournisseur = @num_prod_four,prix_unitaire = @prix, date_intro_m = @datei, date_discont_p = @dated,delai = @delai, quantite = @quantite Where num_produit = @num_prod";
                command.Parameters.AddWithValue("@num_prod", piece_d.Num_production);
                command.Parameters.AddWithValue("@descriptionp", piece_d.Description);
                command.Parameters.AddWithValue("@fournisseur", piece_d.Nom_fournisseur);
                command.Parameters.AddWithValue("@num_prod_four", piece_d.Num_produit_catalogue_fournisseur);
                command.Parameters.AddWithValue("@prix", piece_d.Prix_unitaire);
                command.Parameters.AddWithValue("@datei", piece_d.Date_introduction_marche);
                command.Parameters.AddWithValue("@dated", piece_d.Date_discontinuation);
                command.Parameters.AddWithValue("@delai", piece_d.Delai);
                command.Parameters.AddWithValue("@quantite", piece_d.Quantite);
                MySqlDataReader reader;
                reader = command.ExecuteReader();
                Console.WriteLine("modification effectué !");
                connection.Close();
            }
            
        }


        public void Update_Modele(Modele modele1)
        {
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Modele SET grandeur = @grandeur,nom = @nom, prix_unitaire = @prix, ligne_produit = @ligne_prod, date_intro_m = @datei, date_discont_p = @dated, quantite = @quantite Where num_produit_modele = @num_prod ";
            command.Parameters.AddWithValue("@num_prod", modele1.Num_modele);
            command.Parameters.AddWithValue("@nom", modele1.Nom);
            command.Parameters.AddWithValue("@grandeur", modele1.Grandeur);
            command.Parameters.AddWithValue("@prix", modele1.Prix_unitaire);
            command.Parameters.AddWithValue("@ligne_prod", modele1.Ligne_produit);
            command.Parameters.AddWithValue("@datei", modele1.Date_introduction);
            command.Parameters.AddWithValue("@dated", modele1.Date_discontinuation);
            command.Parameters.AddWithValue("@quantite", modele1.Quantite);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("modification effectué !");
            connection.Close();
        }


        public void Update_Fournisseur(Fournisseur fournisseur1) //Maj table Fournisseur
        {
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Fournisseur SET id_adresse = @adresse,nom_entreprise = @nom, contact = @contact, libelle = @libelle Where siret = @siret";
            command.Parameters.AddWithValue("@siret", fournisseur1.Siret);
            command.Parameters.AddWithValue("@adresse", fournisseur1.Siege.Id_adresse);
            command.Parameters.AddWithValue("@nom", fournisseur1.Nom_entreprise);
            command.Parameters.AddWithValue("@contact", fournisseur1.Contact);
            command.Parameters.AddWithValue("@libelle", fournisseur1.Libelle);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("modification Fournisseur effectué !");
            connection.Close();
        }

        public void Update_Adresse(Adresse adresse1) //Maj table Adresse
        {
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Adresse SET ville = @ville,rue = @rue, code_postal = @codeP, province = @province Where id_adresse = @id_adresse";
            command.Parameters.AddWithValue("@id_adresse", adresse1.Id_adresse);
            command.Parameters.AddWithValue("@ville", adresse1.Ville);
            command.Parameters.AddWithValue("@rue", adresse1.Rue);
            command.Parameters.AddWithValue("@codeP", adresse1.Code_postal);
            command.Parameters.AddWithValue("@province", adresse1.Province);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("Demenagement effectué !");
            connection.Close();
        }

        public void Update_Individu(Individu individu1) //Maj table Fournisseur
        {
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = null; 
            if(individu1.Abonnement!=null)
            {
                command.CommandText = "UPDATE Individu SET id_adresse = @adresse,nom_individu = @nom,prenom_individu = @prenom, telephone_individu = @tel, courriel_individu = @courriel, id_programme = @programme, date_adhesion = @date_adhe Where id_individu = @idI";
                command.Parameters.AddWithValue("@programme", individu1.Abonnement.Num_programme); 
                command.Parameters.AddWithValue("@date_adhe", individu1.Date_adhesion);
            }
            else
            {
                command.CommandText = "UPDATE Individu SET id_adresse = @adresse,nom_individu = @nom,prenom_individu = @prenom, telephone_individu = @tel, courriel_individu = @courriel Where id_individu = @idI";

            }
            command.Parameters.AddWithValue("@idI", individu1.Id_client);
            command.Parameters.AddWithValue("@adresse", individu1.Adresse_client.Id_adresse);
            command.Parameters.AddWithValue("@nom", individu1.Nom);
            command.Parameters.AddWithValue("@prenom", individu1.Prenom);
            command.Parameters.AddWithValue("@tel", individu1.Telephone);
            command.Parameters.AddWithValue("@courriel", individu1.Courriel_client);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("modification Individu effectué !");
            connection.Close();
        }

        public void Update_Boutique(Boutique boutique1) //Maj table Fournisseur
        {
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Boutique SET id_adresse = @adresse,nom_boutique = @nom, telephone_boutique = @tel, courriel_boutique = @courriel,contact = @contact Where id_Boutique = @idB";
            command.Parameters.AddWithValue("@idB", boutique1.Id_client);
            command.Parameters.AddWithValue("@adresse", boutique1.Adresse_client.Id_adresse);
            command.Parameters.AddWithValue("@nom", boutique1.Nom);
            command.Parameters.AddWithValue("@tel", boutique1.Telephone);
            command.Parameters.AddWithValue("@courriel", boutique1.Courriel_client);
            command.Parameters.AddWithValue("@contact", boutique1.Nom_contact);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("modification Boutique effectué !");
            connection.Close();
        }
        
        /*public void Update_Commande(Commande commande1) //Maj table Fournisseur
        {
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Commande SET id_adresse = @adresse,nom_boutique = @nom, telephone_boutique = @tel, courriel_boutique = @courriel,contact = @contact Where num_commande = @idC";
            command.Parameters.AddWithValue("@idB", boutique1.Id_boutique);
            command.Parameters.AddWithValue("@adresse", boutique1.Adresse_boutique.Id_adresse);
            command.Parameters.AddWithValue("@nom", boutique1.Nom);
            command.Parameters.AddWithValue("@tel", boutique1.Telephone);
            command.Parameters.AddWithValue("@courriel", boutique1.Courriel);
            command.Parameters.AddWithValue("@contact", boutique1.Nom_contact);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("modification Boutique effectué !");
            connection.Close();
        }*/




        #endregion


        #region Insertion nouvel élement : 

        public DateTime Inserer_Date()
        {
            Console.Write("Jour: \t");
            int jour = int.Parse(Console.ReadLine());
            Console.Write("Mois: \t");
            int mois = int.Parse(Console.ReadLine());
            Console.Write("Annee: \t");
            int annee = int.Parse(Console.ReadLine());
            return new DateTime(annee,mois,jour);
        }

        public void Insertion_Fournissement(Piece_Detache piece,Fournisseur fournisseur1)
        {
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO `VeloMax`.`Fournissement` (`num_produit`,`siret`)"
               + " VALUES (@num_produit,@siret);";
            command.Parameters.AddWithValue("@num_produit", piece.Num_production);
            command.Parameters.AddWithValue("@siret", fournisseur1.Siret);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("Insertion pièce - fournisseur effectuée !");
            connection.Close(); 
        }

        public void Insertion_Modele_pieces(Modele modele1,Piece_Detache piece)
        {
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO `VeloMax`.`Modele_pieces`(`num_produit_modele`, `num_produit_piece`)"
               + " VALUES (@num_produitm,@num_produitp);";
            command.Parameters.AddWithValue("@num_produitm", modele1.Num_modele);
            command.Parameters.AddWithValue("@num_produitp", piece.Num_production);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("Insertion pièce - modèle effectuée !");
            connection.Close();
        }

        public void Insertion_Piece_Detachee()
        {
            Console.WriteLine("Bienvenue dans la procédure pour ajouter une piece détaché: ");
            Console.WriteLine("Veuillez rentrer les informations suivantes: ");
            Console.Write("Nom Piece(Description): \t");
            string descp = Console.ReadLine();

            Console.WriteLine("Liste des siret et des noms des fournisseurs: ");
            foreach (Fournisseur fournisseur1 in this.fournisseurs.Values) Console.WriteLine("siret: " +  fournisseur1.Siret +" nom: " + fournisseur1.Nom_entreprise);
            Console.Write("entrez le siret du fournisseur de la pièce: \t"); // par simplicité le fournisseur doit deja exister, flemme de faire une grosse boucle foreach en plus 
            string siret = Console.ReadLine();
            bool ok = false;
            if (this.fournisseurs.ContainsKey(siret)) ok = true; 
            while(!ok)
            {
                Console.Write("siret inecxistant(on rappelle q'un siret est composé de 14 éléments, reessayez: ");
                siret = Console.ReadLine();
                if (this.fournisseurs.ContainsKey(siret)) ok = true;


            }
            Console.Write("numéro de la piece dans le catalogue fournisseur: \t"); // par simplicité le fournisseur doit deja exister
            int num_produit = int.Parse(Console.ReadLine());    
            Console.Write("Prix unitaire: \t");
            double prix = Convert.ToDouble(Console.ReadLine());
            Console.Write("Date de discontinuation de la production: \n");
            DateTime date_discont = Inserer_Date();
            Console.Write("Delai de livraison(en nombre de jours)\t");
            int delai = int.Parse(Console.ReadLine());
            Console.Write("Stock de départ: \t");
            int stock_depart = int.Parse(Console.ReadLine());

            //on va rechercher les pieces associés : petit bémol, il ne faut pas se t   romper sur les noms des fournisseurs:
            /*
            foreach(string s1 in pieces_fournisseurs) // recup duo piece fournisseur
            {
                string[] s2 = s1.Split(":");
                foreach (Fournisseur fournisseur1 in this.fournisseurs.Values)
                {
                    if (s2[1] == fournisseur1.Nom_entreprise)
                    {
                        foreach (Piece_Detache piece in fournisseur1.Pieces_detache_fournies.Values)
                        {
                            if (s2[0] == piece.Description)
                            {
                                Insertion_Fournissement(piece, fournisseur1);
                                Console.WriteLine("Insertion" + s2[0] + " : " + s2[1]);
                                break;
                            }

                        }
                        break;
                    }
                        
                }
            }*/
            Piece_Detache piece_d = new Piece_Detache(this.pieces_detaches.Keys.Last() + 1, descp, this.fournisseurs[siret].Nom_entreprise, num_produit, prix, DateTime.Now, date_discont, delai, stock_depart);
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO `VeloMax`.`Piece_detache` (`descriptionp`, `nom_fournisseur`, `num_produit_cat_fournisseur`, `prix_unitaire`, `date_intro_m`, `date_discont_p`,`delai`,`quantite`)"
               + " VALUES (@descriptionp,@fournisseur,@num_prod_four,@prix,@datei,@dated,@delai,@quantite);";
            command.Parameters.AddWithValue("@descriptionp", piece_d.Description);
            command.Parameters.AddWithValue("@fournisseur", piece_d.Nom_fournisseur);
            command.Parameters.AddWithValue("@num_prod_four", piece_d.Num_produit_catalogue_fournisseur);
            command.Parameters.AddWithValue("@prix", piece_d.Prix_unitaire);
            command.Parameters.AddWithValue("@datei", piece_d.Date_introduction_marche);
            command.Parameters.AddWithValue("@dated", piece_d.Date_discontinuation);
            command.Parameters.AddWithValue("@delai", piece_d.Delai);
            command.Parameters.AddWithValue("@quantite", piece_d.Quantite);

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            connection.Close();
            this.pieces_detaches.Add(piece_d.Num_production, piece_d);
            Insertion_Fournissement(piece_d, this.fournisseurs[siret]);
            Console.WriteLine("Insertion pièce effectuée !");



        }


        public void Insertion_Modele()
        {
            Console.WriteLine("Bienvenue dans la procédure pour ajouter un Modèle: ");
            Console.WriteLine("Veuillez rentrer les informations suivantes: ");
            Console.Write("Nom Modele: \t");
            string nom = Console.ReadLine();
            Console.Write("grandeur: \t"); // par simplicité le fournisseur doit deja exister, flemme de faire une grosse boucle foreach en plus 
            string grandeur = Console.ReadLine();
            Console.Write("Prix unitaire: \t");
            double prix = Convert.ToDouble(Console.ReadLine());
            Console.Write("Ligne de produit(VTT,Vélo de course, Classique,BMX): \t");
            string ligne_produit = Console.ReadLine();
            List<string> choix = new List<string> { "VTT", "Vélo de course", "Classique", "BMX" };
            while(!choix.Contains(ligne_produit))
            {
                Console.Write("le type de velo doit appartenir à la liste suivante:  [VTT, Vélo de course, Classique, BMX), réessayez: ");
                ligne_produit = Console.ReadLine();
            }
            Console.Write("Date de discontinuation de la production: \t");
            DateTime date_discont = Inserer_Date();
            Console.Write("Stock de départ: \t");
            int stock_depart = int.Parse(Console.ReadLine());
            Modele modele1 = new Modele(this.modeles.Keys.Last() + 1, nom, grandeur, prix, ligne_produit, DateTime.Now, date_discont, stock_depart);
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO `VeloMax`.`Modele` (`num_produit_modele`, `nom`, `grandeur`, `prix_unitaire`, `ligne_produit`, `date_intro_m`, `date_discont_p`,`quantite`)"
               + " VALUES (@num_prod,@nom, @grandeur,@prix,@ligne_prod,@datei,@dated,@quant);";
            command.Parameters.AddWithValue("@num_prod", modele1.Num_modele); // 
            command.Parameters.AddWithValue("@nom", modele1.Nom);
            command.Parameters.AddWithValue("@grandeur", modele1.Grandeur);
            command.Parameters.AddWithValue("@prix", modele1.Prix_unitaire);
            command.Parameters.AddWithValue("@ligne_prod", modele1.Ligne_produit);
            command.Parameters.AddWithValue("@datei", modele1.Date_introduction);
            command.Parameters.AddWithValue("@dated", modele1.Date_discontinuation);
            command.Parameters.AddWithValue("@quant", modele1.Quantite);

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            connection.Close();
            this.modeles.Add(modele1.Num_modele,modele1);
            Console.WriteLine("Insertion Modele effectuée !");
            Console.WriteLine("Veuillez rentrer la liste des pièces détachés associés à ce modèle suivie de son fournisseur, séparé par des points virgules : ");
            string[] pieces_modele = Console.ReadLine().Split(";");
            List<Piece_Detache> pieces = new List<Piece_Detache> { };
            foreach (string s1 in pieces_modele) // recup duo piece fournisseur
            {
                string[] s2 = s1.Split(":");
                foreach (Piece_Detache piece in this.pieces_detaches.Values)
                {
                    if (s2[0] == piece.Description)
                    {
                        Insertion_Modele_pieces(modele1,piece);
                        Console.WriteLine("Insertion " + s2[0] + " : " + s2[1]);
                        break;
                    }

                }
            }
        }

        public Adresse Insertion_Adresse()
        {
            Console.Write("Rue: \t");
            string rue = Console.ReadLine();
            Adresse adresse1 = null;
            bool adresse_existante = false;
            foreach (Adresse adresset in this.adresses.Values) // on parcours les adresses existantes pour voir si deja existante:
            {
                if (adresset.Rue == rue)
                {
                    adresse1 = adresset; 
                    adresse_existante = true;
                    break;
                }
            }
            if (!adresse_existante)
            {
                Console.Write("Ville: \t");
                string ville = Console.ReadLine();
                Console.Write("Code postal: \t");
                int code_postal = int.Parse(Console.ReadLine());
                Console.Write("Province: \t");
                string province = Console.ReadLine();
                adresse1 = new Adresse(this.adresses.Values.Last().Id_adresse + 1,ville, rue, code_postal, province);
                this.adresses.Add(adresse1.Id_adresse,adresse1);
                MySqlConnection connection = ConnectionBDD();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO `VeloMax`.`Adresse` (`ville`, `rue`, `code_postal`, `province`)"
                   + " VALUES (@ville,@rue, @codep,@province);";
                //command.Parameters.AddWithValue("@id_adresse", adresse1.Id_adresse); // inutile car auto_increment
                command.Parameters.AddWithValue("@ville", adresse1.Ville);
                command.Parameters.AddWithValue("@rue", adresse1.Rue);
                command.Parameters.AddWithValue("@codep", adresse1.Code_postal);
                command.Parameters.AddWithValue("@province", adresse1.Province);
                MySqlDataReader reader;
                reader = command.ExecuteReader();
                Console.WriteLine("Insertion de la nouvelle adresse effectuée !");
                connection.Close();
                
            }

            Console.WriteLine(adresse1);
            return adresse1;


        }
        public void Insertion_Fournisseur()
        {
            Console.WriteLine("Bienvenue dans la procédure pour ajouter un fournisseur: ");
            Console.WriteLine("Veuillez rentrer les informations suivantes: ");
            Console.Write("Numéro de siret: \t");
            string siret = Console.ReadLine();
            Console.WriteLine("Adresse: "); 
            Adresse adresset = Insertion_Adresse();
            Console.Write("Nom: \t");
            string nom_entreprise = Console.ReadLine();
            Console.Write("Contact: \t");
            string contact = Console.ReadLine();
            Fournisseur fournisseur1 = new Fournisseur(siret, adresset, nom_entreprise, contact);
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            Console.WriteLine(fournisseur1.Siege.Id_adresse);
            command.CommandText = "INSERT INTO `VeloMax`.`Fournisseur` (`siret`, `id_adresse`, `nom_entreprise`, `contact`, `libelle`)"
               + "VALUES (@siret,@adresse,@nom_entreprise,@contact,@libelle);";
            command.Parameters.AddWithValue("@siret", fournisseur1.Siret);
            command.Parameters.AddWithValue("@adresse", fournisseur1.Siege.Id_adresse);
            command.Parameters.AddWithValue("@nom_entreprise", fournisseur1.Nom_entreprise);
            command.Parameters.AddWithValue("@contact", fournisseur1.Contact);
            command.Parameters.AddWithValue("@libelle", fournisseur1.Libelle);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("Insertion fournisseur effectuée !");
            connection.Close();

        }
        public void Insertion_Individu() // insertion d'un nouvel individu dans la table individu
        {
            Individu individu1 = null;
            Console.WriteLine("Bienvenue dans la procédure pour ajouter un individu: ");
            Console.WriteLine("Veuillez rentrer les informations suivantes: ");
            Console.Write("Nom: \t");
            string nom = Console.ReadLine();
            Console.Write("Prenom: ");
            string prenom = Console.ReadLine();
            Console.WriteLine("Adresse: ");
            Adresse adresset = Insertion_Adresse();
            Console.Write("Numero de Telephone: \t");
            string tel = Console.ReadLine();
            Console.Write("Adresse mail: \t");
            string mail = Console.ReadLine();
            Console.WriteLine("Cet individu a t'il adhéré à un abonnement: ");
            Console.WriteLine("1 - Oui \t\t 2 - Non");
            int rep = int.Parse(Console.ReadLine());
            Programme_Fidelio programme = null;
            switch (rep)
            {
                case 1:
                    Console.WriteLine("Liste des programmes possibles: "); 
                    foreach (Programme_Fidelio prog in this.programmes_fidelio.Values)
                    {
                        Console.WriteLine(prog.Num_programme + " - " + prog.Description);
                    }
                    Console.WriteLine(" numéro programme choisi:");
                    int rep2 = int.Parse(Console.ReadLine());
                    programme = this.programmes_fidelio[rep2];
                    break;
                case 2:
                    break;
            }
            if (programme != null) { individu1 = new Individu(this.Individus.Last().Value.Id_client + 1, adresset, nom, prenom, tel, mail, programme, DateTime.Now); } // si programme choisi
            else { individu1 = new Individu(this.Individus.Last().Value.Id_client + 1, adresset, nom, prenom, tel, mail);}
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = null;
            if (individu1.Abonnement != null)
            {
                command.CommandText = "INSERT INTO `VeloMax`.`Individu` ( `id_adresse`, `nom_individu`, `prenom_individu`,`telephone_individu`,`courriel_individu`,`id_programme`,`date_adhesion`)"
                               + " VALUES (@id_adresse,@nom,@prenom,@tel,@courriel,@programme,@date_adhe);";
                command.Parameters.AddWithValue("@programme", individu1.Abonnement.Num_programme);
                command.Parameters.AddWithValue("@date_adhe", individu1.Date_adhesion);
            }
            else
            {
                command.CommandText = "INSERT INTO `VeloMax`.`Individu` ( `id_adresse`, `nom_individu`, `prenom_individu`,`telephone_individu`,`courriel_individu`)"
                               + " VALUES (@id_adresse,@nom,@prenom,@tel,@courriel);";
            }
            command.Parameters.AddWithValue("@id_adresse", individu1.Adresse_client.Id_adresse);
            command.Parameters.AddWithValue("@nom", individu1.Nom);
            command.Parameters.AddWithValue("@prenom", individu1.Prenom);
            command.Parameters.AddWithValue("@tel", individu1.Telephone);
            command.Parameters.AddWithValue("@courriel", individu1.Courriel_client);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("Insertion Individu effectuée !");
            connection.Close();

        }
        public void Insertion_Boutique() // insertion d'un nouvel individu dans la table boutique
        {
            Console.WriteLine("Bienvenue dans la procédure pour ajouter une boutique: ");
            Console.WriteLine("Veuillez rentrer les informations suivantes: ");
            Console.Write("Nom Boutique: \t");
            string nom = Console.ReadLine();
            Console.WriteLine("Adresse: ");
            Adresse adresset = Insertion_Adresse();
            Console.Write("Numero de Telephone: \t");
            string tel = Console.ReadLine();
            Console.Write("Adresse mail professionnel: \t");
            string mail = Console.ReadLine();
            Console.Write("Nom du Contact: \t");
            string contact = Console.ReadLine();
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            Boutique boutique1 = new Boutique(this.boutiques.Last().Id_client + 1, adresset, nom, tel, mail, contact);
            command.CommandText = "INSERT INTO `VeloMax`.`Boutique` ( `id_adresse`, `nom_boutique`, `telephone_boutique`,`courriel_boutique`,`contact`)"
               + " VALUES (@id_adresse,@nom,@tel,@courriel,@contact);";
            command.Parameters.AddWithValue("@id_adresse", boutique1.Adresse_client.Id_adresse); 
            command.Parameters.AddWithValue("@nom", boutique1.Nom);
            command.Parameters.AddWithValue("@tel", boutique1.Telephone);
            command.Parameters.AddWithValue("@courriel", boutique1.Courriel_client);
            command.Parameters.AddWithValue("@contact", boutique1.Nom_contact);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            connection.Close();
            this.boutiques.Add(boutique1);
            Console.WriteLine("Insertion Boutique effectuée !");
        }


        public void Insertion_Commande()
        {
            Console.WriteLine("Bienvenue dans la procédure d'insertion d'une nouvelle commande: ");
            Console.WriteLine("Veuillez rentrer les informations suivantes: ");
            Client client1 = null;
            Console.WriteLine("liste des clients et de leur id: ");
            foreach (Client client in this.Clients.Values) Console.WriteLine("id: " + client.Id_client + "\t Nom: " + client.Nom);
            Console.Write("Rentrez l'id du client: ");
            int id_client = int.Parse(Console.ReadLine());
            foreach (Boutique boutique1 in this.boutiques)
            {
                if(boutique1.Id_client == id_client)
                {
                    client1 = (Client)boutique1;
                }
            }
            foreach (Individu individu1 in this.individus.Values)
            {
                if (individu1.Id_client == id_client)
                {
                    client1 = (Client)individu1;
                }
            }
            Console.Write("date de livraison: \t");
            DateTime date_livraison = Inserer_Date();
            Commande commande1 = new Commande(this.commandes.Keys.Last() + 1, DateTime.Now, date_livraison, client1);
            // modele
            Console.WriteLine("Veuillez rentrer le/les modèles associés à cette commande: ");
            Console.WriteLine("Liste des modèles: ");
            foreach(Modele modele1 in this.modeles.Values)
            {
                Console.WriteLine("id: " + modele1.Num_modele +  "   nom du modele: "  +  modele1.Nom + " grandeur: " + modele1.Grandeur + " quantité: " + modele1.Quantite);  
            }
            bool stop_modele = false;
            do
            {
                Console.Write("veuillez entrer le numéro du modèle choisie: \t");
                int num_modele = int.Parse(Console.ReadLine());
                Console.Write("Veuillez rentrer la quantité commandée de ce modèle: \t");
                int quantitem = int.Parse(Console.ReadLine());
                while (this.modeles[num_modele].Quantite - quantitem <0)
                {
                    Console.WriteLine("La quantite disponible pour cette piece " + "(" + this.modeles[num_modele].Quantite + ") " + "est inférieur à celle commandée" + "(" + quantitem);
                    Console.Write("veuillez rentrer une nouvelle quantité: \t");
                    quantitem = int.Parse(Console.ReadLine());
                }
                commande1.Modele1.Add(this.modeles[num_modele], quantitem);
                this.Modeles[num_modele].Quantite -= quantitem;
                Update_Modele(this.modeles[num_modele]);
                Console.Write("Avez vous un modèle à ajouter ? Si oui tapez 1, sinon 2: \t");
                int rep = int.Parse(Console.ReadLine());
                if (rep == 2)
                {
                    stop_modele = true;
                }
            } while (!stop_modele);
            //piece détachées
            Console.WriteLine("Veuillez désormais rentrer la/les pièce/s associées à cette commande par modèle: ");
            foreach(Modele modele1 in commande1.Modele1.Keys)
            {
                if (modele1.Quantite > 0)
                {
                    Console.WriteLine("modele : " + modele1.Nom + " " + modele1.Grandeur);
                }
                    
                foreach(Piece_Detache piece in modele1.Pieces)
                {
                    Console.WriteLine("id " + piece.Num_production + "   nom piece: " + piece.Description + " fournisseur: " + piece.Nom_fournisseur + " quantite disponible: " + piece.Quantite);
                    
                }
                bool stop_piece = false;
                do
                {
                    Console.Write("veuillez entrer le numéro de la pièce choisie: \t");
                    int num_piece = int.Parse(Console.ReadLine());
                    Console.Write("Veuillez rentrer la quantité commandée de cette piece: \t");
                    int cmp = 0;
                    int quantitep = int.Parse(Console.ReadLine());
                    while (this.pieces_detaches[num_piece].Quantite - quantitep < 0)
                    {
                        Console.WriteLine("La quantite disponible pour cette piece " + "(" + this.pieces_detaches[num_piece].Quantite + ") " + "est inférieur à celle commandée" + "(" + quantitep);
                        Console.Write("veuillez rentrer une nouvelle quantité: \t");
                        quantitep = int.Parse(Console.ReadLine());
                    }
                    if(commande1.Liste_pieces_detachee.ContainsKey(this.pieces_detaches[num_piece]))
                    {
                        commande1.Liste_pieces_detachee[this.pieces_detaches[num_piece]] += quantitep;
                    }
                    commande1.Liste_pieces_detachee.Add(this.pieces_detaches[num_piece], quantitep);
                    this.pieces_detaches[num_piece].Quantite -= quantitep;
                    Update_Piece_Detache(this.pieces_detaches[num_piece]);
                    Console.Write("Avez vous une nouvelle pièce de ce modèle ( "+ modele1.Nom + ") " + modele1.Grandeur +" à ajouter ? Si oui tapez 1, sinon 2: \t");
                    int rep2 = int.Parse(Console.ReadLine());
                    if (rep2 == 2)
                    {
                        stop_piece = true;
                    }
                } while (!stop_piece);
            }
            
            // insertion dans la bdd:
            // si commande d'un individu:
            //
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            if (commande1.Client is Individu)
            {
                command.CommandText = "INSERT INTO `VeloMax`.`Commande`( `date_commande`, `date_livraison`,`id_individu`,`id_boutique`)"
                   + " VALUES(@date_commande, @date_livraison, @id_individu, null);";
                command.Parameters.AddWithValue("@date_commande", commande1.Date_com);
                command.Parameters.AddWithValue("@date_livraison", commande1.Date_livraison);
                command.Parameters.AddWithValue("@id_individu", commande1.Client.Id_client);
                MySqlDataReader reader;
                reader = command.ExecuteReader();
                connection.Close();
                this.clients[commande1.Client.Id_client].Commandes_client.Add(commande1);
                Console.WriteLine("insertion commande individu effectuée !");

            }
            else//si commande d'une boutique
            {
                command.CommandText = "INSERT INTO `VeloMax`.`Commande`( `date_commande`, `date_livraison`,`id_individu`,`id_boutique`)"
                    + " VALUES(@date_commande, @date_livraison,null, @id_boutique);";
                command.Parameters.AddWithValue("@date_commande", commande1.Date_com);
                command.Parameters.AddWithValue("@date_livraison", commande1.Date_livraison);
                command.Parameters.AddWithValue("@id_boutique", commande1.Client.Id_client);
                MySqlDataReader reader;
                reader = command.ExecuteReader();
                connection.Close();
                this.clients[commande1.Client.Id_client].Commandes_client.Add(commande1);
                Console.WriteLine("insertion commande  boutique effectuée !");

            }
            // insertion contient piece
            foreach(Modele modele1 in commande1.Modele1.Keys)
            {
                connection = ConnectionBDD();
                command = connection.CreateCommand();
                command.CommandText = " INSERT INTO `VeloMax`.`Contient_modele`( `num_commande`, `num_produit_modele`, `quantite_commande`)"
                   + " VALUES(@numc, @numm, @quantitem);";
                command.Parameters.AddWithValue("@numc", commande1.Num_commande);
                command.Parameters.AddWithValue("@numm", modele1.Num_modele);
                command.Parameters.AddWithValue("@quantitem", commande1.Modele1[modele1]);
                MySqlDataReader reader;
                reader = command.ExecuteReader();
                connection.Close();
            }
            Console.WriteLine("insertion modèle_commande effectuée!");

            foreach (Piece_Detache piece in commande1.Liste_pieces_detachee.Keys)
            {
                connection = ConnectionBDD();
                command = connection.CreateCommand();
                command.CommandText = " INSERT INTO `VeloMax`.`Contient_Piece`( `num_commande`, `num_produit`, `quantite_commande`)"
                   + " VALUES(@numc, @nump, @quantitec);";
                command.Parameters.AddWithValue("@numc", commande1.Num_commande);
                command.Parameters.AddWithValue("@nump", piece.Num_production);
                command.Parameters.AddWithValue("@quantitec", commande1.Liste_pieces_detachee[piece]);
                MySqlDataReader reader;
                reader = command.ExecuteReader();
                connection.Close();
            }
            Console.WriteLine("insertion modele_piece effectuée !");
            commande1.Prix_Total();
            Console.WriteLine("prix de la commande: " + commande1.Prix_commande);
            //insertion contient modele






            //commande Boutique 1



        }

        #endregion

        #region Suppression d'un élément: 

        public void Suppression_Piece_detachee()
        {
            Console.WriteLine("Bienvenue dans la procédure pour supprimer une piece détaché: ");
            Console.WriteLine("Veuillez rentrer les informations suivantes: ");
            Console.Write("Description de la piece( en 3 caractères): \t");
            string descp = Console.ReadLine();
            Console.Write("Nom du fournisseur: \t");
            string nom_fournisseur = Console.ReadLine();
            Piece_Detache piece_d = null;
            foreach (Piece_Detache piece1 in this.pieces_detaches.Values)
            {
                if(piece1.Description == descp && piece1.Nom_fournisseur == nom_fournisseur) { piece_d = piece1; }
            }
            //on supprime l'élément de notre dictionnaire de piece de la classe: 
            this.pieces_detaches.Remove(piece_d.Num_production);
            // on supprime dans la bdd
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Delete from piece_detache where num_produit = @num_prod ";
            command.Parameters.AddWithValue("@num_prod", piece_d.Num_production);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("Suppression piece effectuée !");
            // piece pas supprimé de la liste des pieces fournisseur(fournissement) 
            this.fournisseurs.Clear();//On clear
            this.Chargement_Fournisseur();//On recharge pour faire la recharge sans la piece supprimé
            connection.Close();
        }

        public void Suppression_Piece_detachee2(Piece_Detache piece_d)
        {
            //on supprime l'élément de notre dictionnaire de piece de la classe: 
            this.pieces_detaches.Remove(piece_d.Num_production);
            // on supprime dans la bdd
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Delete from piece_detache where num_produit = @num_prod ";
            command.Parameters.AddWithValue("@num_prod", piece_d.Num_production);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("Suppression piece effectuée !");
            connection.Close();
        }

        public void Suppression_Modele()
        {
            Console.WriteLine("Bienvenue dans la procédure pour supprimer un modèle: ");
            Console.WriteLine("Veuillez rentrer les informations suivantes: ");
            Console.Write("Nom du modèle: \t");
            string nom = Console.ReadLine();
            Console.Write("Grandeur: \t");
            string nom_fournisseur = Console.ReadLine();
            Modele modele1 = null;
            foreach (Modele modele2 in this.modeles.Values)
            {
                if (modele2.Nom == nom && modele2.Grandeur == nom_fournisseur) { modele1 = modele2; }
            }
            //on supprime l'élément de notre dictionnaire de piece de la classe: 
            this.modeles.Remove(modele1.Num_modele);
            // on supprime dans la bdd
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Delete from modele where num_produit_modele = @num_mod ";
            command.Parameters.AddWithValue("@num_mod", modele1.Num_modele);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("Suppression modele effectuée !");
            // piece pas supprimé de la liste des pieces fournisseur(fournissement) 
            connection.Close();
        }

        public void Suppression_Fournisseur()
        {
            Console.WriteLine("Bienvenue dans la procédure pour supprimer une piece détaché: ");
            Console.WriteLine("Veuillez rentrer les informations suivantes: ");
            Console.Write("Nom du Fournisseur: \t");
            string nom = Console.ReadLine();
            Fournisseur fournisseur1 = null;
            foreach (Fournisseur fournisseur2 in this.fournisseurs.Values)
            {
                if (fournisseur2.Nom_entreprise == nom ) { fournisseur1 = fournisseur2; }
            }

            foreach(int num_piece1 in fournisseur1.Pieces_detache_fournies.Keys)
            {
                Suppression_Piece_detachee2(this.pieces_detaches[num_piece1]); // suppression bdd
                this.pieces_detaches.Remove(num_piece1); // suppression dico
            }
            // on supprime dans la bdd
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Delete from fournisseur where siret = @num_mod ";
            command.Parameters.AddWithValue("@num_mod", fournisseur1.Siret);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            // piece pas supprimé de la liste des pieces fournisseur(fournissement) 
            connection.Close();
            //on supprime l'élément de notre dictionnaire de piece de la classe: 
            this.fournisseurs.Remove(fournisseur1.Siret);
            Console.WriteLine("Suppression fournisseur effectuée !");
        }

        public void Suppression_Individu()
        {
            Console.WriteLine("Bienvenue dans la procédure pour supprimer une piece détaché: ");
            Console.WriteLine("Veuillez rentrer les informations suivantes: ");
            Console.Write("Nom de l'individu : \t");
            string nom = Console.ReadLine();
            Console.Write("Prénom de l'individu : \t");
            string prenom = Console.ReadLine();
            Individu individu1 = null;
            foreach (Individu individu2 in this.individus.Values)
            {
                if (individu2.Nom == nom && individu2.Prenom == prenom) { individu1 = individu2; }
            }
            //on supprime l'élément de notre dictionnaire de piece de la classe: 
            // on supprime dans la bdd
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Delete from individu where id_individu = @num_mod ";
            command.Parameters.AddWithValue("@num_mod", individu1.Id_client);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            connection.Close();
            this.individus.Remove(individu1.Id_client);
            Console.WriteLine("Suppression individu effectuée !");
            // piece pas supprimé de la liste des pieces fournisseur(fournissement) 
        }


        public void Suppression_Boutique()
        {
            Console.WriteLine("Bienvenue dans la procédure pour supprimer une boutique: ");
            Console.WriteLine("Veuillez rentrer les informations suivantes: ");
            Console.Write("Nom de la boutique : \t");
            string nom = Console.ReadLine();
            Boutique boutique1 = null;
            foreach (Boutique boutique2 in this.boutiques)
            {
                if (boutique2.Nom == nom) { boutique1 = boutique2; }
            }
            //on supprime l'élément de notre dictionnaire de piece de la classe: 
            // on supprime dans la bdd
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Delete from boutique where id_boutique = @num_mod ";
            command.Parameters.AddWithValue("@num_mod", boutique1.Id_client);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            connection.Close();
            // piece pas supprimé de la liste des pieces fournisseur(fournissement) 
            this.boutiques.Remove(boutique1);
            Console.WriteLine("Suppression boutique effectuée !");
        }


        public void Suppression_Commande()
        {
            Console.WriteLine("Bienvenue dans la procédure pour supprimer une commande: ");
            Console.WriteLine("Veuillez rentrer les informations suivantes: ");
            Console.Write("Numéro de la commande : \t");
            // on supprime dans la bdd
            Commande commande1 = null;
            int id = int.Parse(Console.ReadLine());
            foreach (Commande commande2 in this.commandes.Values)
            {
                if (commande2.Num_commande == id) 
                { 
                    commande1 = commande2; 
                }
            }
            Console.WriteLine(commande1);
            // on supprimer les modeles ainsi que les quantités que l'on ajoute au niveau des pieces détachées et des commandes
            foreach(KeyValuePair<Piece_Detache,int> piece in commande1.Liste_pieces_detachee)
            {
                Console.WriteLine("avant: " + this.pieces_detaches[piece.Key.Num_production].Quantite);
                this.pieces_detaches[piece.Key.Num_production].Quantite += piece.Value;
                Console.WriteLine(this.pieces_detaches[piece.Key.Num_production].Quantite);
                Console.WriteLine("apres: " + this.pieces_detaches[piece.Key.Num_production].Quantite);
                Update_Piece_Detache(this.pieces_detaches[piece.Key.Num_production]);
            }

            foreach (KeyValuePair<Modele, int> modele1 in commande1.Modele1)
            {
                this.modeles[modele1.Key.Num_modele].Quantite += modele1.Value;
                Update_Modele(this.modeles[modele1.Key.Num_modele]);
            }
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Delete from commande where num_commande = @num_mod ";
            command.Parameters.AddWithValue("@num_mod", commande1.Num_commande);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            connection.Close();
            // piece pas supprimé de la liste des pieces fournisseur(fournissement) 
            //on supprime l'élément de notre dictionnaire de piece de la classe: 
            this.commandes.Remove(commande1.Num_commande);
            Console.WriteLine("Suppression commande effectuée !");



        }










        #endregion

        #region Statistiques 

        public void Rapport_Stat_Ventes1() // v1 complexe sans sql
        {
            Dictionary<string, int> piece_vente = new Dictionary<string, int> { }; // on classe par piece vendu
            Dictionary<Modele, int> modele_vente = new Dictionary<Modele, int> { }; // on classe par modele vendu; je met en paramètre modele et non pas string car pas le meme modele selon la grandeur
            foreach (Commande commande1 in this.commandes.Values)
            {
                foreach (Modele modele1 in commande1.Modele1.Keys) // je parcours pour chaque commande, les modèles commandés
                {
                    if (modele_vente.ContainsKey(modele1))
                    {
                        modele_vente[modele1] += commande1.Modele1[modele1];
                    }
                    else
                    {
                        modele_vente.Add(modele1, commande1.Modele1[modele1]);
                    }
                }

                foreach (Piece_Detache piece in commande1.Liste_pieces_detachee.Keys) // je parcours pour chaque commande, les modèles commandés
                {
                    if (piece_vente.ContainsKey(piece.Description))
                    {
                        piece_vente[piece.Description] += commande1.Liste_pieces_detachee[piece];
                    }
                    else
                    {
                        piece_vente.Add(piece.Description, commande1.Liste_pieces_detachee[piece]);
                    }
                }
            }

            Console.WriteLine("-------------------------------------------" + "Rapport quantités des modèles vendues".ToUpper() + "--------------------------------------------");
            foreach (KeyValuePair<Modele, int> modele in modele_vente)
            {
                Console.WriteLine("Modele: " + modele.Key.Nom + " " + modele.Key.Grandeur + "\t quantités vendues: " + modele.Value);
            }
            Console.WriteLine("--------------------------------------------" + "Rapport quantités des Pièces vendues".ToUpper() + "--------------------------------------------");
            Console.WriteLine("Rapport quantités des Pièces vendues: ");
            foreach (KeyValuePair<string, int> piece in piece_vente)
            {
                Console.WriteLine("Piece: " + piece.Key + "\t quantités vendues: " + piece.Value);
            }
        }

        public void Rapport_Stat_Ventes2Modele() // v2 simple avec sql
        {
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT num_produit_modele,sum(quantite_commande)"
                                + " FROM Contient_Modele GROUP BY num_produit_modele;";
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("Rapport quantités des modèles vendues: ");
            while (reader.Read())   // parcours ligne par ligne
            {
                Modele modele1 = this.modeles[reader.GetInt32(0)];
                Console.WriteLine("Modele: " + modele1.Nom + " " + modele1.Grandeur + "\t quantités vendues: " + reader.GetInt32(1));
            }
            connection.Close();
        }

        public void Rapport_Stat_Ventes2Piece() // v2 simple avec sql
        {
            MySqlConnection connection = ConnectionBDD();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT num_produit_modele,sum(quantite_commande)"
                                + " FROM Contient_Modele GROUP BY num_produit_modele;";
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("Rapport quantités des modèles vendues: ");
            Console.WriteLine("-------------------------------------------" + "Rapport quantités des modèles vendues".ToUpper() + "--------------------------------------------");
            while (reader.Read())   // parcours ligne par ligne
            {
                Modele modele1 = this.modeles[reader.GetInt32(0)];
                Console.WriteLine("Modele: " + modele1.Nom + " " + modele1.Grandeur + "\t quantités vendues: " + reader.GetInt32(1));
            }
            connection.Close();
        }

        public void Membres_Programme() //la liste des membres pour chaque programme d’adhésion. // faire avec sql via un in (select)*
        {
            Dictionary<Programme_Fidelio, List<Individu>> programme_individu = new Dictionary<Programme_Fidelio, List<Individu>> { };
            foreach (Programme_Fidelio programme in this.programmes_fidelio.Values)
            {
                foreach(Individu individu1 in this.individus.Values)
                {
                    if(individu1.Abonnement == programme)//si abonnement de l'individu correspond à l'instance du programme du premeir foreachabonnement 
                    {
                        if (programme_individu.ContainsKey(programme))// et que programme deja present dans le dictionnnaire
                        {
                            programme_individu[programme].Add(individu1);//on ajoute l'individu à la liste d'individu associé à ce programme
                        }
                        else { programme_individu.Add(programme, new List<Individu> { individu1 }); } // sinon on ajoute une nouvelle clé programme(si programme pas présent dans le dictionnaire) et auquel on ajoute la liste d'individu composé d'un seul individu pour le moment
                    }
                }                
            }
            Console.WriteLine(programme_individu.Count());
            // une fois dictionnaire remplie, afficher le résultat :
            Console.WriteLine("-------------------------------------------" + "Rapport membres par programme fidelio".ToUpper() + "--------------------------------------------");
            foreach (KeyValuePair<Programme_Fidelio, List<Individu>> prog_ind in programme_individu)
            {
                Console.WriteLine("Programme: " + prog_ind.Key.Description + ": ");
                foreach (Individu ind in prog_ind.Value)
                {
                    Console.Write("Nom: " + ind.Nom + " Prenom: " + ind.Prenom + " " + "date expiration: " + ind.Date_adhesion.AddYears(prog_ind.Key.Duree).ToShortDateString());
                    if((ind.Date_adhesion.AddYears(prog_ind.Key.Duree) - DateTime.Now).TotalDays<60) // si moins de deux mois avant la fin du contrat -> conseiller
                    {
                        Console.WriteLine("  <-- pensez à lui proposer un renouvellement de contrat");
                    }
                    else
                    {
                        Console.WriteLine("");
                    }
                }
                Console.WriteLine("----------------------------------------------------------------------------------------");
            }
        }

        public void Meilleurs_clients()// classes les clients du meilleur au pire
        {
            List<Client> clients = new List<Client> { };
            foreach (Client client in this.clients.Values) {client.Total_Depenses();clients.Add(client); }
            clients.Sort((x, y) => y.CompareTo(x));
            Console.WriteLine("------------------------------------" + "Classement des meilleurs clients en terme de dépense".ToUpper() + "------------------------------------");
            for (int i = 0; i<clients.Count;i++)
            {
                Console.WriteLine((i + 1) + ". " + clients[i].Nom + " : " + clients[i].Depense_total + " euros");
            }



        }


        public void Stats_Commandes()
        {
            // moyenne montant des commandes
            //moyenne nombre de pieces par commande 
            //moyenne nombre de velo par commande
            double prix_moyen = 0;
            double nbre_pieces_moyen = 0;
            double nbre_velos_moyen = 0;
            int cmp_piece = 0;
            int cmp_velo=0; 
            foreach(Commande commande1 in this.commandes.Values) // on fait la somme 
            {
                prix_moyen += commande1.Prix_commande;
                foreach(KeyValuePair<Modele,int> mod in commande1.Modele1) { nbre_velos_moyen += mod.Value; cmp_velo += mod.Value; } // je calcule le nombre de velo commandé selon les modeles et j'ajoute dans ma variable 
                foreach (KeyValuePair<Piece_Detache, int> piece in commande1.Liste_pieces_detachee) { nbre_pieces_moyen += piece.Value; cmp_piece += piece.Value; }// de meme pour les pieces détachées
                nbre_pieces_moyen += commande1.Liste_pieces_detachee.Count();
                nbre_velos_moyen += commande1.Modele1.Count();
            }
            // on calcule les moyennes : 
            prix_moyen /= this.commandes.Count();
            nbre_pieces_moyen /= this.commandes.Count();
            nbre_velos_moyen /= this.commandes.Count();
            Console.WriteLine("-----------------------------------------------" + "Statistiques sur les commandes".ToUpper() + "-----------------------------------------------");
            Console.WriteLine("prix moyen des commandes: " + prix_moyen);
            Console.WriteLine("nombre de piece commandées par commande en moyenne: " + nbre_pieces_moyen);
            Console.WriteLine("nombre de vélos commandés par commande en moyenne: " + nbre_velos_moyen);

        }


        #endregion

        #region gestion des stocks 

        public void Stock_fournisseurs()
        {
            foreach (Fournisseur fournisseur1 in this.fournisseurs.Values)
            {

                Console.WriteLine("Siret: " + fournisseur1.Siret + "\nNom du fournisseur: " + fournisseur1.Nom_entreprise + "\nPieces fournies et quantités restantes:");
                foreach (int id_piece in fournisseur1.Pieces_detache_fournies.Keys)
                {
                    Console.WriteLine(this.pieces_detaches[id_piece].Description + " : " + this.pieces_detaches[id_piece].Quantite);
                }
            }
        }

        public void Stock_Pieces()
        {
            // classer les pieces par description :
            Dictionary<string, int> piece_quantite = new Dictionary<string, int> { };
            foreach (Piece_Detache piece in this.pieces_detaches.Values)
            {
                if (piece_quantite.ContainsKey(piece.Description))
                {
                    piece_quantite[piece.Description] += piece.Quantite;
                }
                else
                {
                    piece_quantite.Add(piece.Description, piece.Quantite);
                }
            }
            foreach (KeyValuePair<string, int> piece in piece_quantite)
            {
                Console.WriteLine("Piece: " + piece.Key + "\t quantités restantes: " + piece.Value);
            }
        }

        public void Stock_Modeles()
        {
            foreach (Modele modele1 in this.modeles.Values)
            {
                Console.WriteLine("Modele: " + modele1.Nom + " " + modele1.Grandeur + "\t quantités restantes: " + modele1.Quantite);
            }
        }

        public void Stock_Modele_Categorie()
        {
            Dictionary<string, int> cate_quantite = new Dictionary<string, int> { };
            foreach (Modele modele1 in this.modeles.Values)
            {
                if (cate_quantite.ContainsKey(modele1.Ligne_produit))
                {
                    cate_quantite[modele1.Ligne_produit] += modele1.Quantite;
                }
                else
                {
                    cate_quantite.Add(modele1.Ligne_produit, modele1.Quantite);
                }
            }
            foreach (KeyValuePair<string, int> modele in cate_quantite)
            {
                Console.WriteLine("Catégorie: " + modele.Key + "\t quantités restantes: " + modele.Value);
            }
        }

        public void Stock_Modele_Marque()
        {
            Dictionary<string, int> marque_quantite = new Dictionary<string, int> { };
            foreach (Modele modele1 in this.modeles.Values)
            {
                if (marque_quantite.ContainsKey(modele1.Nom))
                {
                    marque_quantite[modele1.Nom] += modele1.Quantite;
                }
                else
                {
                    marque_quantite.Add(modele1.Nom, modele1.Quantite);
                }
            }
            foreach (KeyValuePair<string, int> modele in marque_quantite)
            {
                Console.WriteLine("Marque: " + modele.Key + "\t quantités restantes: " + modele.Value);
            }
        }
        public void Serialize_Client()
        {
            StreamWriter writer = new StreamWriter("C:/Users/admin/OneDrive/Bureau/Travail/Cours_A3_ESILV/Cours_S6/Informatique/BDD_Interoperabilite/TD6/Projet_VeloMax/Projet_VeloMax/clients.json");
            JsonTextWriter jwriter = new JsonTextWriter(writer);
            jwriter.WriteStartObject();
            jwriter.WritePropertyName("Client");
            jwriter.WriteStartArray();
            Console.WriteLine("test");
            foreach (Individu individu1 in this.individus.Values)
            {
                if (individu1.Abonnement != null && (individu1.Date_adhesion.AddYears(individu1.Abonnement.Duree) - DateTime.Now).TotalDays < 60)
                {
                    
                    List<int> list_id_commande = new List<int>();
                    foreach(Commande commande1 in individu1.Commandes_client)
                    {
                        list_id_commande.Add(commande1.Num_commande);
                    }
                    jwriter.WritePropertyName("Id client");
                    jwriter.WriteValue(individu1.Id_client);
                    jwriter.WritePropertyName("Nom");
                    jwriter.WriteValue(individu1.Nom);
                    jwriter.WritePropertyName("Prenom");
                    jwriter.WriteValue(individu1.Prenom);
                    jwriter.WriteStartArray();
                    jwriter.WritePropertyName("adresse");
                    jwriter.WriteValue(individu1.Adresse_client.Rue);
                    jwriter.WriteValue(individu1.Adresse_client.Ville);
                    jwriter.WriteValue(individu1.Adresse_client.Code_postal);
                    jwriter.WriteValue(individu1.Adresse_client.Province);
                    jwriter.WriteEndArray();
                    jwriter.WritePropertyName("Telephone");
                    jwriter.WriteValue(individu1.Telephone);
                    jwriter.WritePropertyName("Courriel");
                    jwriter.WriteValue(individu1.Courriel_client);
                    jwriter.WritePropertyName("Abonnement: ");
                    jwriter.WriteValue(individu1.Abonnement.Description);
                    jwriter.WriteStartArray();
                    jwriter.WriteValue("commandes");
                    foreach(int test in list_id_commande)
                    {
                        jwriter.WriteValue(test);
                    }
                    jwriter.WriteEndArray();
                    
                    
                }
            }
            jwriter.WriteEndArray();
            jwriter.WriteEnd();
            jwriter.WriteEndObject();

        }
}
    #endregion

    

}
