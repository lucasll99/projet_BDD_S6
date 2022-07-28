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
    class VeloMax_Manager
    {
        Velo_Max entreprise_velo;
        string user_name;

        public string User_name { get => user_name; set => user_name = value; }
        public Velo_Max Entreprise_velo { get => entreprise_velo; set => entreprise_velo = value; }

        public VeloMax_Manager(string user_name)
        {
            this.user_name = user_name;
            this.entreprise_velo = new Velo_Max();
        }

        #region instanciation de la bdd 
        /*static MySqlConnection ConnectionBDD(string user_name)
        {
            MySqlConnection maConnexion = null;
            string password = "";
            if (user_name == "root") password = "Lucas070799$";
            else if (user_name == "bozo") password = "user";

            try
            {
                //a. indiquer les informations pour la connexion
                string connexionInfo = "SERVER=localhost;PORT=3306;" +
                    "DATABASE=VeloMax;UID="+ user_name + ";PASSWORD=" + password ;
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
        }*/
        public void Chargement_bdd()
        {
            entreprise_velo.Chargement_Pieces_Detaches();
            entreprise_velo.Chargement_Modeles();
            entreprise_velo.Chargement_Pieces_Modeles();// pour chaque modele, charge à l'interieur les pieces qui lui sont associés:
            entreprise_velo.Chargement_Adresse();
            entreprise_velo.Chargement_Fournisseur();
            entreprise_velo.Chargement_Programmes();
            entreprise_velo.Chargement_Individu();
            entreprise_velo.Chargement_Boutique();
            entreprise_velo.Chargement_Client();
            entreprise_velo.Chargement_Commande();
        }//chargement de la bdd
        #endregion
        public void Insertion_element()
        {
            bool quit = false;
            Console.Clear();
            do
            {
                Console.Clear();
                Console.WriteLine("Quelle élement souhaitez vous insérer dans la base: ");
                Console.WriteLine("1. Un fournisseur" + "\t2. une piece détachée" + "\t3. Un modèle" + "\t4. Un client" + "\t5. Une commande" + "\t6. Quitter ce menu");
                Console.Write("Numéro action choisie: ");
                int choix = int.Parse(Console.ReadLine());
                Console.WriteLine();
                switch (choix)
                {
                    case 1: this.entreprise_velo.Insertion_Fournisseur();
                        Console.ReadKey();
                        break;
                    case 2:
                        this.entreprise_velo.Insertion_Piece_Detachee();
                        Console.ReadKey();
                        break;
                    case 3:
                        this.entreprise_velo.Insertion_Modele(); 
                        Console.ReadKey();
                        break;
                    case 4:
                        Console.WriteLine("Tapez 1 si le client à insérer est un individu et 2 si le client est une boutique: \t");
                        int choix2 = int.Parse(Console.ReadLine());
                        while (choix2 !=1 && choix2 !=2)
                        {
                            Console.WriteLine("Mauvais choix ! Tapez 1 si le client est un individu et 2 si le client est une boutique, réessayez: \t");
                            choix2 = int.Parse(Console.ReadLine());
                        }
                        if(choix2 ==1) { this.entreprise_velo.Insertion_Individu(); Console.ReadKey(); break; }
                        else { this.entreprise_velo.Insertion_Boutique(); Console.ReadKey(); break; }

                    case 5:
                        entreprise_velo.Insertion_Commande();
                        Console.ReadKey();
                        break;


                    case 6: quit = true; Console.Clear(); break;
                }
            } while (quit != true);
        }//insetion des éléments
        

        public void Supprimer_element()
        {
            bool quit = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Quelle élement souhaitez vous supprimer dans la base: ");
                Console.WriteLine("1. Un fournisseur" + "\t2. Une piece détachée" + "\t3. Un Modele" + "\t4. Un client" + "\t5. Une commande" + "\t6. Quitter ce menu");
                Console.Write("Numéro action choisie: ");
                int choix = int.Parse(Console.ReadLine());
                Console.WriteLine();
                switch (choix)
                {
                    case 1: this.entreprise_velo.Suppression_Fournisseur();
                        Console.ReadKey();
                        break;
                    case 2:
                        this.entreprise_velo.Suppression_Piece_detachee();
                        Console.ReadKey();
                        break;
                    case 3:
                        this.entreprise_velo.Suppression_Modele();
                        Console.ReadKey(); 
                        break;
                    case 4:
                        Console.WriteLine("Tapez 1 si le client à supprimer est un individu et 2 si le client est une boutique: \t");
                        int choix2 = int.Parse(Console.ReadLine());
                        while (choix2 != 1 && choix2 != 2)
                        {
                            Console.WriteLine("Mauvais choix ! Tapez 1 si le client est un individu et 2 si le client est une boutique, réessayez: \t");
                            choix2 = int.Parse(Console.ReadLine());
                        }
                        if (choix2 == 1) { this.entreprise_velo.Suppression_Individu(); Console.ReadKey(); break; }
                        else { this.entreprise_velo.Suppression_Boutique(); Console.ReadKey(); break; }

                    case 5:
                        entreprise_velo.Suppression_Commande();
                        Console.ReadKey();
                        break;


                    case 6: quit = true;Console.Clear(); break;
                }
            } while (quit != true);
        }//suppression d'un élément


        public void Mise_A_Jour()
        {
            bool quit = false;
            Console.Clear();
            do
            {
                Console.WriteLine("Bienvenue dans le menu des mises à jour! Quelle élément souhaitez vous mettre à jour?");
                Console.WriteLine("1. Un fournisseur" + " \t2. une piece détachée" + " \t3. Un modele" + " \t4. Quitter ce menu");
                Console.Write("Numéro action choisie: ");
                int choix = int.Parse(Console.ReadLine());
                Console.WriteLine();
                switch (choix)
                {
                    case 1:
                        Console.Write("Veuillez rentrer le siret du fournisseur à mettre à jour: \t");
                        string choix2 = Console.ReadLine();
                        Fournisseur fournisseur1 = this.entreprise_velo.Fournisseurs[choix2];
                        Console.WriteLine("Que souhaitez vous modifier ? 1-adresse, 2 - nom_entreprise, 3- contact");
                        int choix3 = int.Parse(Console.ReadLine());
                        switch (choix3)
                        {
                            case 1:
                                Console.WriteLine("Rentrez la nouvelle adresse du siege du fournisseur: \t");
                                Adresse adressen = this.entreprise_velo.Insertion_Adresse();
                                fournisseur1.Siege = adressen;
                                this.entreprise_velo.Update_Fournisseur(fournisseur1); break;
                            case 2:
                                Console.WriteLine("Veuillez entrer le nouveau nom de ce fournisseur: \t");
                                string nomf = Console.ReadLine();
                                fournisseur1.Nom_entreprise = nomf;
                                this.entreprise_velo.Update_Fournisseur(fournisseur1);
                                break;
                            case 3:
                                Console.WriteLine("Veuillez entrer le nouveau mail de contact de ce fournisseur: \t");
                                string mailc = Console.ReadLine();
                                fournisseur1.Contact = mailc;
                                this.entreprise_velo.Update_Fournisseur(fournisseur1);
                                break;
                        }
                        Console.ReadKey();
                        break;
                   case 2:
                        Console.Write("Veuillez rentrer l'id de la pièce détacher à mettre à jour: \t");
                        int id_piece = int.Parse(Console.ReadLine());
                        Piece_Detache piece = this.entreprise_velo.Pieces_detaches[id_piece];
                        Console.WriteLine("Que souhaitez vous modifier ? 1-prix unitaire, 2 - date de discontinuation, 3- quantite en stock");
                        int choix4 = int.Parse(Console.ReadLine());
                        switch (choix4)
                        {
                            case 1:
                                Console.WriteLine("Rentrez le nouveau prix unitaire de la piece: \t");
                                int prixu = int.Parse(Console.ReadLine());
                                piece.Prix_unitaire = prixu;
                                this.entreprise_velo.Update_Piece_Detache(piece); break;
                            case 2:
                                Console.WriteLine("Veuillez la nouvelle date de discontinuation de la piece: \t");
                                DateTime dated = this.entreprise_velo.Inserer_Date();
                                piece.Date_discontinuation = dated;
                                this.entreprise_velo.Update_Piece_Detache(piece);
                                break;
                            case 3:
                                Console.WriteLine("Veuillez entrer la nouvelle quantite en stock: \t");
                                int quantites = int.Parse(Console.ReadLine());
                                piece.Quantite = quantites;
                                this.entreprise_velo.Update_Piece_Detache(piece);
                                break;
                        }
                        Console.ReadKey();
                        break;
                    case 3:
                        Console.Write("Veuillez rentrer l'id du modèle à mettre à jour: \t");
                        int num_modele = int.Parse(Console.ReadLine());
                        Modele modele1 = this.entreprise_velo.Modeles[num_modele];
                        Console.WriteLine("Que souhaitez vous modifier ? 1-prix unitaire, 2 - date de discontinuation, 3- quantite en stock");
                        int choix5 = int.Parse(Console.ReadLine());
                        switch (choix5)
                        {
                            case 1:
                                Console.WriteLine("Rentrez le nouveau prix unitaire de la piece: \t");
                                int prixu = int.Parse(Console.ReadLine());
                                modele1.Prix_unitaire = prixu;
                                this.entreprise_velo.Update_Modele(modele1); break;
                            case 2:
                                Console.WriteLine("Veuillez la nouvelle date de discontinuation de la piece: \t");
                                DateTime dated = this.entreprise_velo.Inserer_Date();
                                modele1.Date_discontinuation = dated;
                                this.entreprise_velo.Update_Modele(modele1);
                                break;
                            case 3:
                                Console.WriteLine("Veuillez entrer la nouvelle quantite en stock: \t");
                                int quantites = int.Parse(Console.ReadLine());
                                modele1.Quantite = quantites;
                                this.entreprise_velo.Update_Modele(modele1);
                                break;
                        }
                        Console.ReadKey();
                        break;

                    /*case 4:
                        Console.Write("Veuillez rentrer l'id de la commande à mettre à jour: \t");
                        int id_commande = int.Parse(Console.ReadLine());
                        Console.WriteLine("Que souhaitez vous modifier ? 1- , 2 - date de discontinuation, 3- quantite en stock");*/
                    case 4: quit = true;Console.Clear(); break;


                }


            } while (quit != true);
        }//les mises à jours
        public void Statistiques()
        {
            bool quit = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Bienvenue dans le menu des statistiques! Quelle statistique souhaitez vous voir?");
                Console.WriteLine("1. les ventes (modele et piece)" + "\t2. Liste des membres pour chaque programme fidelio" + "\t3. classement des clients (dépenses)" + "\t4. Les commandes" + "\t5. Quitter ce menu");
                Console.Write("Numéro action choisie: ");
                int choix = int.Parse(Console.ReadLine());
                Console.WriteLine();
                switch (choix)
                {
                    case 1:
                        this.entreprise_velo.Rapport_Stat_Ventes1();
                        Console.ReadKey();
                        break;
                    case 2:
                        this.entreprise_velo.Membres_Programme();
                        Console.ReadKey();
                        break;
                    case 3:
                        this.entreprise_velo.Meilleurs_clients();
                        Console.ReadKey(); 
                        break;
                    case 4:
                        entreprise_velo.Stats_Commandes();
                        Console.ReadKey();
                        break;
                    case 5: quit = true;Console.Clear(); break;
                }
            } while (quit != true);
        }//module statistiques
        public void Gestion_Stock()
        {
            bool quit = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Bienvenue dans le menu de la gestion des stocks! Quelle stocks souhaitez vous voir?");
                Console.WriteLine("1. Stock pieces" + " \t2.Stock fournisseurs" + " \t3. Stock velo" + " \t4. Stock marque - catégorie velo" + " \t5. Quitter ce menu");
                Console.Write("Numéro action choisie: ");
                int choix = int.Parse(Console.ReadLine());
                Console.WriteLine();
                switch (choix)
                {
                    case 1:
                        this.entreprise_velo.Stock_Pieces();
                        Console.ReadKey();
                        break;
                    case 2:
                        this.entreprise_velo.Stock_fournisseurs();
                        Console.ReadKey();
                        break;
                    case 3:
                        this.entreprise_velo.Stock_Modeles(); 
                        Console.ReadKey();
                        break;
                    case 4:
                        Console.WriteLine("Stock des vélos par marque ");
                        entreprise_velo.Stock_Modele_Marque();
                        Console.WriteLine("Stock des vélos par catégorie ");
                        entreprise_velo.Stock_Modele_Categorie();
                        Console.ReadKey();
                        break;
                    case 5: quit = true; Console.Clear(); break;
                }
            } while (quit != true);
        }//module gestion des stocks
        public void Demo()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------" + "Table des pièces détachées".ToUpper() + "--------------------------------------------");
            Console.WriteLine();
            foreach (Piece_Detache piece in this.entreprise_velo.Pieces_detaches.Values)
            {
                Console.WriteLine(piece);
                Console.WriteLine();
            }
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("-------------------------------------------" + "Table des modèles".ToUpper() + "--------------------------------------------");
            Console.WriteLine();

            foreach (Modele modele in this.entreprise_velo.Modeles.Values)
            {
                Console.WriteLine(modele);
                Console.WriteLine();
            }
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("-------------------------------------------" + "Table des fournisseurs".ToUpper() + "--------------------------------------------");
            Console.WriteLine();

            foreach (Fournisseur fournisseur in this.entreprise_velo.Fournisseurs.Values)
            {
                Console.WriteLine(fournisseur);
                Console.WriteLine();
            }
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("-------------------------------------------" + "Table des individus".ToUpper() + "--------------------------------------------");
            Console.WriteLine();
            foreach (Individu individu1 in this.entreprise_velo.Individus.Values)
            {
                Console.WriteLine(individu1);
                Console.WriteLine();
            }
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("-------------------------------------------" + "Table des boutiques".ToUpper() + "--------------------------------------------");
            Console.WriteLine();
            foreach (Boutique boutique1 in this.entreprise_velo.Boutiques)
            {
                Console.WriteLine(boutique1);
                Console.WriteLine();
            }
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("-------------------------------------------" + "Table des programmes".ToUpper() + "--------------------------------------------");
            Console.WriteLine();
            foreach (Programme_Fidelio programme1 in this.entreprise_velo.Programmes_fidelio.Values)
            {
                Console.WriteLine(programme1);
                Console.WriteLine();
            }
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("-------------------------------------------" + "Table des commandes".ToUpper() + "--------------------------------------------");
            Console.WriteLine();
            foreach (Commande commande1 in this.entreprise_velo.Commandes.Values)
            {
                Console.WriteLine(commande1);
                Console.WriteLine();
            }
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("-------------------------------------------" + "Statistiques sur les clients".ToUpper() + "--------------------------------------------");
            Console.WriteLine("Nombre de client: " + this.entreprise_velo.Clients.Count + " (" + this.entreprise_velo.Individus.Count + " individus et " + this.entreprise_velo.Boutiques.Count + " boutiques )");
            Console.ReadKey();
            Console.WriteLine("Liste des clients et de leurs dépenses totales: ");
            foreach (Client client1 in this.entreprise_velo.Clients.Values)
            {
                Console.WriteLine("Nom: " + client1.Nom + "\ttotal dépensé: " + client1.Depense_total + " euros");
            }
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("-------------------------------------------" + "Statistiques sur les produits et les fournisseurs".ToUpper() + "--------------------------------------------");
            Console.WriteLine("Liste des produits avec un stock faible: "); 
            foreach (Modele modele1 in this.entreprise_velo.Modeles.Values)
            {
                if(modele1.Quantite<=2)
                {
                    Console.WriteLine("Nom du produit: " + modele1.Nom + " " + modele1.Grandeur + "\t Quantité: " + modele1.Quantite); 
                }
            }
            foreach (Piece_Detache piece in this.entreprise_velo.Pieces_detaches.Values)
            {
                if (piece.Quantite <= 2)
                {
                    Console.WriteLine("Nom du produit: " + piece.Description + "\t Fournisseur: " + piece.Nom_fournisseur + "\t Quantité: " + piece.Quantite);
                }
            }
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("Liste des pièces/vélo fournis par un fournisseur: ");
            foreach(Fournisseur fournisseur1 in this.entreprise_velo.Fournisseurs.Values)
            {
                Console.WriteLine("Siret " + fournisseur1.Siret + " Nom: " + fournisseur1.Nom_entreprise + " Nombre de pièces/vélos fournis: " + fournisseur1.Pieces_detache_fournies.Count);
            }
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("Transormation element de la table fournisseur en un fichier json: ");
            List<Fournisseur> _data = new List<Fournisseur>();
            foreach (Fournisseur fournisseur1 in this.entreprise_velo.Fournisseurs.Values)
            {
                fournisseur1.Calcul_delai_moyen();
                _data.Add(fournisseur1);
            }
            string json = JsonConvert.SerializeObject(_data);
            File.WriteAllText(@"C:/Users/admin/OneDrive/Bureau/Travail/Cours_A3_ESILV/Cours_S6/Informatique/BDD_Interoperabilite/TD6/Projet_VeloMax/Projet_VeloMax/.json", json);
            Console.WriteLine("Effectué!");
        }//démonstration présentation du projet


    }

    

}
