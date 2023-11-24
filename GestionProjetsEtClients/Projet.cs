using Microsoft.WindowsAppSDK.Runtime.Packages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionProjetsEtClients
{
    class Projet
    {
        public string NoProjet { get; set; }
        public string Titre { get; set; }
        public string DateDebut { get; set; }
        public string Description { get; set; }
        public double Budget { get; set; }
        public string BudgetFormat
        {
            get { return Budget.ToString("C2", CultureInfo.CurrentCulture); }
        }
        public int NbrEmployeRequis { get; set; }
        public double TotalSalaire { get; set; }
        public string TotalSalaireFormat
        {
            get { return TotalSalaire.ToString("C2", CultureInfo.CurrentCulture); }
        }
        public string Statut { get; set; }
        public int IdClient { get; set; }
        public string NomClient { get; set; }

        public override string ToString()
        {
            return $"NoProjet = {NoProjet}, Titre = {Titre}, DateDebut = {DateDebut}, Budget = {BudgetFormat}, NbrEmployeRequis = {NbrEmployeRequis}, TotalSalaire = {TotalSalaireFormat}, Statut = {Statut}, IdClient = {IdClient}, NomClient = {NomClient}, Description = {Description}";
        }
    }
}
