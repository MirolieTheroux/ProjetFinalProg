using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace GestionProjetsEtClients
{
    internal class Employe
    {
        public string Matricule { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string NomCompletFormat
        {
            get { return Prenom + " " + Nom; }
        }
        public string DateNaissance { get; set; }
        public string Email { get; set; }
        public string Adresse { get; set; }
        public string DateEmbauche { get; set; }
        public double TauxHoraire { get; set; }

        public string TauxHoraireFormat
        {
            get { return TauxHoraire.ToString("C2", CultureInfo.CurrentCulture); }
        }
        public string LienPhoto { get; set; }
        public string Statut { get; set; }

        public override string ToString()
        {
            return Matricule + " - " + NomCompletFormat;
        }
    }
}
