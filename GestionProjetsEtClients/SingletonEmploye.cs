using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GestionProjetsEtClients
{
    internal class SingletonEmploye
    {
        static SingletonEmploye instance = null;
        MySqlConnection connection;
        ObservableCollection<Employe> listeEmployes;

        public SingletonEmploye()
        {
            connection = new MySqlConnection("Server=cours.cegep3r.info;Database=1468780-mirolie-théroux;Uid=1468780;Pwd=1468780;");
            listeEmployes = new ObservableCollection<Employe>();
           // charger();
        }

        public static SingletonEmploye getInstance()
        {
            if (instance == null)
                instance = new SingletonEmploye();

            return instance;
        }

        public ObservableCollection<Employe> getListeEmployes()
        {
            return listeEmployes;
        }

    }
}
