using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_VeloMax
{
    public class Client : IComparable
    {
        int id_client;
        Adresse adresse_client;
        string nom;
        string telephone;
        string courriel_client;
        List<Commande> commandes_client;
        double depense_total;

        public Client(int id_client, Adresse adresse_client, string nom, string telephone, string courriel_client)
        {
            this.id_client = id_client;
            this.Adresse_client = adresse_client;
            this.nom = nom;
            this.telephone = telephone;
            this.courriel_client = courriel_client;
            this.commandes_client = new List<Commande> { };
            this.depense_total = 0;
        }

        public int Id_client { get => id_client; set => id_client = value; }
        public Adresse Adresse_client { get => adresse_client; set => adresse_client = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Telephone { get => telephone; set => telephone = value; }
        public string Courriel_client { get => courriel_client; set => courriel_client = value; }
        public List<Commande> Commandes_client { get => commandes_client; set => commandes_client = value; }
        public double Depense_total { get => depense_total; set => depense_total = value; }
        public override string ToString()
        {
            string ret = "id Client: " + this.Id_client + "\nNom " + this.Nom + "\nAdresse: " + this.Adresse_client + "\nTelephone: " + this.Telephone + "\nCourriel: " + this.Courriel_client + "\nCommande: ";
            foreach(Commande commande in this.commandes_client) { ret += commande.Num_commande + ", "; }
            ret += "\nBonjour! ";
            return ret;

        }
        public void Total_Depenses() // pour calculer le total des dépenses éffectuées par les clients(somme des prix des commandes) 
        {
            double depenses = 0; 
            foreach(Commande commande1 in this.commandes_client)
            {
                depenses += commande1.Prix_commande;
            }
            this.depense_total = depenses;
        }
        public int CompareTo(object val) // comparer les clients entre eux
        {
            Client valA = (Client)val;
            return this.depense_total.CompareTo(valA.Depense_total);
        }
    }
}
