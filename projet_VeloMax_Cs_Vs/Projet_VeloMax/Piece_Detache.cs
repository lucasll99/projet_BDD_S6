using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_VeloMax
{
    public class Piece_Detache
    {
        int num_production;
        string description;
        string nom_fournisseur;
        int num_produit_catalogue_fournisseur;
        double prix_unitaire; 
        DateTime date_introduction_marche;
        DateTime date_discontinuation;
        int delai;
        int quantite;

        public Piece_Detache(int num_production, string description, string nom_fournisseur, int num_produit_catalogue_fournisseur, double prix_unitaire, DateTime date_introduction_marche, DateTime date_discontinuation, int delai, int quantite = 0)
        {
            this.num_production = num_production;
            this.description = description;
            this.nom_fournisseur = nom_fournisseur;
            this.num_produit_catalogue_fournisseur = num_produit_catalogue_fournisseur;
            this.prix_unitaire = prix_unitaire;
            this.date_introduction_marche = date_introduction_marche;
            this.date_discontinuation = date_discontinuation;
            this.delai = delai;
            this.quantite = quantite;
        }

        public int Num_production { get => num_production; set => num_production = value; }
        public string Description { get => description; set => description = value; }
        public string Nom_fournisseur { get => nom_fournisseur; set => nom_fournisseur = value; }
        public int Num_produit_catalogue_fournisseur { get => num_produit_catalogue_fournisseur; set => num_produit_catalogue_fournisseur = value; }
        public double Prix_unitaire { get => prix_unitaire; set => prix_unitaire = value; }
        public DateTime Date_introduction_marche { get => date_introduction_marche; set => date_introduction_marche = value; }
        public DateTime Date_discontinuation { get => date_discontinuation; set => date_discontinuation = value; }
        public int Delai { get => delai; set => delai = value; }
        public int Quantite { get => quantite; set => quantite = value; }

        public override string ToString()
        {
            string ret = "num_produit: " + this.num_production + "\ndescription: " + this.description + "\nnom_fournisseur: " + this.nom_fournisseur + "\nnum_catalogue_fournisseur:" + this.num_produit_catalogue_fournisseur + "\n prix: " + this.prix_unitaire
                + "\ndate intro marche: " + this.date_introduction_marche + "\ndate discontinuation: " + this.date_discontinuation + "\n delai: " + this.delai + " jours";
            return ret; 

        }


    }
}
