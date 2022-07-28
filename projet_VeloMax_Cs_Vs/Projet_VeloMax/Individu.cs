using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_VeloMax
{

    public class Individu : Client // classe individu 
    {
        string prenom;
        Programme_Fidelio abonnement; // l'abonnement de l'individu
        DateTime date_adhesion;

        public Individu(int id_individu, Adresse adresse_individu, string nom, string prenom, string num_telephone, string courriel, Programme_Fidelio abonnement, DateTime date_adhesion) : base(id_individu, adresse_individu,nom,num_telephone,courriel)
        {
            this.prenom = prenom;
            this.abonnement = abonnement;
            this.date_adhesion = date_adhesion;
        }

        public Individu(int id_individu, Adresse adresse_individu, string nom, string prenom, string num_telephone, string courriel) : base(id_individu, adresse_individu, nom, num_telephone, courriel)
        {
            this.prenom = prenom;
            this.abonnement = null;
        }


        public string Prenom { get => prenom; set => prenom = value; }
        public Programme_Fidelio Abonnement { get => abonnement; set => abonnement = value; }
        public DateTime Date_adhesion { get => date_adhesion; set => date_adhesion = value; }

        public override string ToString()
        {
            string ret = "id Individu: " + this.Id_client + "\nNom " + this.Nom + "\tPrenom: " + this.prenom +  "\nAdresse: " + this.Adresse_client + "\nTelephone: " + this.Telephone + "\nCourriel: " + this.Courriel_client + "\nCommande: ";
            foreach(Commande commande in this.Commandes_client) { ret += commande.Num_commande + ", "; }
            if (this.abonnement != null) ret += "\nAbonnée programme: " + this.abonnement.Description;
            return ret;

        }
    }
}
