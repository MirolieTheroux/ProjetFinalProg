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
        public string NomCompletFormat
        {
            get { return Prenom + " " + Nom; }
        }
        public string NoProjet { get; set; }
        public double TauxHoraire { get; set; }
        public string TauxHoraireFormat
        {
            get { return TauxHoraire.ToString("C2", CultureInfo.CurrentCulture); }
        }
        public double NbHeuresTravaillees { get; set; }
        public string NbHeures
        {
            get { return NbHeuresTravaillees.ToString() + "h"; }
        }
        public double SalaireEmploye { get; set; }
        public string SalaireEmpFormat
        {
            get { return SalaireEmploye.ToString("C2", CultureInfo.CurrentCulture); }
        }

    }
}
