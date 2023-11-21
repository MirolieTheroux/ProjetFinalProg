using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionProjetsEtClients
{
    internal class SingletonMessageValidation
    {
        static SingletonMessageValidation instance;

        public bool AfficherSucces { get; set; }
        public bool AfficherErreur { get; set; }
        public string Titre { get; set; }
        public string Message { get; set; }
        public SingletonMessageValidation()
        {
            AfficherSucces = false;
            AfficherErreur = false;
            Titre = string.Empty;
            Message = string.Empty;
        }

        public static SingletonMessageValidation getInstance()
        {
            if (instance == null)
                instance = new SingletonMessageValidation();

            return instance;
        }

        public void annulerMessage()
        {
            AfficherSucces = false;
            AfficherErreur = false;
        }
    }
}
