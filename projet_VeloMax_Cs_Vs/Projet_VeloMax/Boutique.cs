using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_VeloMax
{
    public class Boutique : Client
    {
        string nom_contact;

        public Boutique(int id_boutique, Adresse adresse_boutique, string nom, string telephone, string courriel, string nom_contact) : base(id_boutique, adresse_boutique, nom, telephone, courriel)
        {
            this.nom_contact = nom_contact;
        }

        public string Nom_contact { get => nom_contact; set => nom_contact = value; }

        public override string ToString()
        {
            string ret = "id Boutique: " + base.Id_client + "\nNom " + base.Nom + "\nAdresse: " + base.Adresse_client + "\nTelephone: " + base.Telephone + "\nCourriel: " + this.Courriel_client + "\nContact: " + this.nom_contact;
            return ret;
        }
    }
}
