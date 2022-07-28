using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_VeloMax
{
    public class Commande
    {
        int num_commande;
        DateTime date_com;
        DateTime date_livraison;
        Dictionary<Modele,int> modele1; //les modele et les quantiteés
        Dictionary<Piece_Detache,int> liste_pieces_detachee;//les pieces et les quantiteés
        Client client;
        double prix_commande;


        public Commande(int num_commande,DateTime date_com, DateTime date_livraison, Client client = null)
        {
            this.num_commande = num_commande;
            this.date_com = date_com;
            this.date_livraison = date_livraison;
            this.modele1 = new Dictionary<Modele, int> { };
            this.liste_pieces_detachee = new Dictionary<Piece_Detache, int> { };
            this.client = client;
            this.prix_commande = 0;
        }

        public Commande()
        {
            this.modele1 = new Dictionary<Modele, int> { };
            this.liste_pieces_detachee = new Dictionary<Piece_Detache, int> { };
        }

        public int Num_commande { get => num_commande; set => num_commande = value; }
        public DateTime Date_com { get => date_com; set => date_com = value; }
        public DateTime Date_livraison { get => date_livraison; set => date_livraison = value; }
        public Dictionary<Modele, int> Modele1 { get => modele1; set => modele1 = value; }
        public Dictionary<Piece_Detache, int> Liste_pieces_detachee { get => liste_pieces_detachee; set => liste_pieces_detachee = value; }
        public Client Client { get => client; set => client = value; }
        public double Prix_commande { get => prix_commande; set => prix_commande = value; }

        public override string ToString()
        {
            string ret = "num_commande: " + this.num_commande + "\nAdresse_livraison " + this.client.Adresse_client + "\nDate de livraison: " + this.date_livraison + "\nListe des modèles : Quantité: ";
            foreach(KeyValuePair<Modele,int> i in this.modele1)
            {
                ret += i.Key.Num_modele + " : " + i.Value + " ;";
            }
            ret += "\nListe Pieces_detachées : quantité : ";
            foreach (KeyValuePair <Piece_Detache,int> j in this.liste_pieces_detachee)
            {
                ret += j.Key.Num_production + " : " + j.Value + "; ";
            }
            ret+="\nNumero_client: " + this.client.Id_client;
            return ret;
        }

        public void Prix_Total()
        {
            double prix_total = 0;
            foreach(KeyValuePair<Piece_Detache,int> piece in this.liste_pieces_detachee)
            {
                prix_total += piece.Key.Prix_unitaire * piece.Value;
            }
            foreach (KeyValuePair<Modele, int> modele1 in this.Modele1)
            {
                prix_total += modele1.Key.Prix_unitaire * modele1.Value;
            }
            this.prix_commande = prix_total;
        }
    }


    
}
