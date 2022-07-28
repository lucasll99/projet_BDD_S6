using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_VeloMax
{
    public class Programme_Fidelio
    {
        int num_programme;
        string description;
        int cout;
        int duree;
        float rabais;

        public Programme_Fidelio(int num_programme, string description, int cout, int duree, float rabais)
        {
            this.Num_programme = num_programme;
            this.Description = description;
            this.Cout = cout;
            this.Duree = duree;
            this.Rabais = rabais;
        }

        public int Num_programme { get => num_programme; set => num_programme = value; }
        public string Description { get => description; set => description = value; }
        public int Cout { get => cout; set => cout = value; }
        public int Duree { get => duree; set => duree = value; }
        public float Rabais { get => rabais; set => rabais = value; }
        public override string ToString()
        {
            string rep = "Nom:" + this.description + "\nCout: " + this.cout + "\nDuree: " + this.duree + "ans \nRabais: " + this.rabais + " %";
            return rep;
        }
    }

    
}
