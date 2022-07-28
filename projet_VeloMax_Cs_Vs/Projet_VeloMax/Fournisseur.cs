using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_VeloMax
{
    public class Fournisseur
    {
        string siret;
        string nom_entreprise;
        string contact;
        int libelle;
        Adresse siege;
        double delai_moyen; // permettra de lui donner une note (libelle)
        Dictionary<int, Piece_Detache> pieces_detache_fournies;

        public Fournisseur(string siret, Adresse siege, string nom_entreprise, string contact, int libelle)
        {
            this.siret = siret;
            this.nom_entreprise = nom_entreprise;
            this.contact = contact;
            this.libelle = libelle;
            this.siege = siege;
            this.pieces_detache_fournies = new Dictionary<int, Piece_Detache> { };
        }

        public Fournisseur(string siret, Adresse siege, string nom_entreprise, string contact)
        {
            this.siret = siret;
            this.nom_entreprise = nom_entreprise;
            this.contact = contact;
            this.libelle = 0;
            this.siege = siege;
            this.pieces_detache_fournies = new Dictionary<int, Piece_Detache> { };
        }

        public string Siret { get => siret; set => siret = value; }
        public string Nom_entreprise { get => nom_entreprise; set => nom_entreprise = value; }
        public string Contact { get => contact; set => contact = value; }
        public int Libelle { get => libelle; set => libelle = value; }
        public Adresse Siege { get => siege; set => siege = value; }
        public double Delai_moyen { get => delai_moyen; set => delai_moyen = value; }
        public Dictionary<int,Piece_Detache> Pieces_detache_fournies { get => pieces_detache_fournies; set => pieces_detache_fournies = value; }

        public override string ToString()
        {
            string ret = "Numéro fournisseur: " + this.siret + "\nNom fournisseur: " + this.nom_entreprise + "\nAdresse du siege: " + this.siege + "\nContact: " + this.contact + "\nlibelle: " + this.libelle + "\nListe des Pieces Détachées fournies: ";
            foreach(int i in this.pieces_detache_fournies.Keys)
            {
                ret += i + ", ";
            }
            return ret;

        }

        public double Calcul_delai_moyen() // on classera les fournisseurs selon les delais moyens pour ensuite faire les libelles avec une fonction libelle_fournisseur dans la classe VeloMax 
        {
            double ret = 0; 
            foreach(Piece_Detache i in this.pieces_detache_fournies.Values)
            {
                ret += i.Delai; 
            }
            ret = (double)(ret / pieces_detache_fournies.Count());
            return ret; 
        }
    }

    
}
