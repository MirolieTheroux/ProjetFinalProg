using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionProjetsEtClients
{
    class ProjetEmploye
    {
        public string Matricule { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }

        public string NoProjet { get; set; }
        public double NbHeuresTravaillees { get; set; }
        public string NbHeures
        {
            get { return NbHeuresTravaillees.ToString(NbHeuresTravaillees + "h"); }
        }
        public double SalaireEmploye{ get; set; }
        public string SalaireEmpFormat
        {
            get { return SalaireEmploye.ToString("C2", CultureInfo.CurrentCulture); }
        }

    }
}
