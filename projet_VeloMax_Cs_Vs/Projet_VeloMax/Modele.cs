using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_VeloMax
{
    public class Modele
    {
        int num_modele;
        string nom;
        string grandeur;
        double prix_unitaire;
        string ligne_produit;
        DateTime date_introduction;
        DateTime date_discontinuation;
        List<Piece_Detache> pieces;
        int quantite; 

        public int Num_modele { get => num_modele; set => num_modele = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Grandeur { get => grandeur; set => grandeur = value; }
        public double Prix_unitaire { get => prix_unitaire; set => prix_unitaire = value; }
        public string Ligne_produit { get => ligne_produit; set => ligne_produit = value; }
        public DateTime Date_introduction { get => date_introduction; set => date_introduction = value; }
        public DateTime Date_discontinuation { get => date_discontinuation; set => date_discontinuation = value; }
        public int Quantite { get => quantite; set => quantite = value; }
        public List<Piece_Detache> Pieces { get => pieces; set => pieces = value; }

        public Modele(int num_modele, string nom, string grandeur, double prix_unitaire, string ligne_produit, DateTime date_introduction, DateTime date_discontinuation, int quantite = 0)
        {
            this.Num_modele = num_modele;
            this.Nom = nom;
            this.Grandeur = grandeur;
            this.Prix_unitaire = prix_unitaire;
            this.ligne_produit = ligne_produit;
            this.Date_introduction = date_introduction;
            this.Date_discontinuation = date_discontinuation;
            this.quantite = quantite;
            this.pieces = new List<Piece_Detache>();
        }

        public override string ToString()
        {
            string ret = "num modele: " + this.num_modele + "\ngrandeur: " + this.grandeur + "\nprix_unitaire: " + this.prix_unitaire + "\ndate intro marche: " + this.Date_introduction + "\ndate discontinuation: " + this.date_discontinuation;
            return ret;

        }
    }
}
