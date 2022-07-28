using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_VeloMax
{
    public class Adresse
    {
        int id_adresse;
        string ville;
        string rue;
        int code_postal;
        string province;

        public Adresse(int id_adresse, string ville, string rue, int code_postal, string province)//lorsque chargement adresse
        {
            this.Id_adresse = id_adresse;
            this.ville = ville;
            this.rue = rue;
            this.code_postal = code_postal;
            this.province = province;
        }

        public Adresse(string ville, string rue, int code_postal, string province) // pour création d'adresse, pas besoin de donner id
        {
            this.Id_adresse = 0;
            this.ville = ville;
            this.rue = rue;
            this.code_postal = code_postal;
            this.province = province;
        }

        public int Id_adresse { get => id_adresse; set => id_adresse = value; }
        public string Ville { get => ville; set => ville = value; }
        public string Rue { get => rue; set => rue = value; }
        public int Code_postal { get => code_postal; set => code_postal = value; }
        public string Province { get => province; set => province = value; }

        public override string ToString()
        {
            string ret = "  " + this.rue + "  " + this.code_postal + "  " + this.ville + "  " + this.province;
            return ret;

        }
    }
}
