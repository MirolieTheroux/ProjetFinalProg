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

        }

        public static SingletonEmploye getInstance()
        {
            if (instance == null)
                instance = new SingletonEmploye();

            return instance;
        }

        public ObservableCollection<Employe> Employes { get { return listeEmployes; } }

        /// <summary>
        /// Permet d'aller chercher tous les employés qui sont dans la BD
        /// </summary>
        public void getListeEmployesBD()
        {
            listeEmployes.Clear();
            try
            {
                MySqlCommand commande = new MySqlCommand("p_get_employes");
                commande.Connection = connection;
                commande.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    string sMatricule = (string)reader["matricule"];
                    string sNom = (string)reader["nom"];
                    string sPrenom = (string)reader["prenom"];
                    string datNaissance = (string)reader["date_naissance"];
                    string sEmail = (string)reader["email"];
                    string sAdresse = (string)reader["adresse"];
                    string datEmbauche = (string)reader["date_embauche"];
                    double dTauxHoraire = (double)reader["taux_horaire"];
                    string sLienPhoto = (string)reader["lien_photo"];
                    string sStatut = (string)reader["statut"];

                    Employe employe = new Employe
                    {
                        Matricule = sMatricule,
                        Nom = sNom,
                        Prenom = sPrenom,
                        DateNaissance = datNaissance,
                        Email = sEmail,
                        Adresse = sAdresse,
                        DateEmbauche = datEmbauche,
                        TauxHoraire = dTauxHoraire,
                        LienPhoto = sLienPhoto,
                        Statut = sStatut
                    };
                    listeEmployes.Add(employe);
                }

                reader.Close();
                connection.Close();
            }
            catch (MySqlException ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }
        /// <summary>
        /// Permet d'ajouter un employé dans la BD
        /// </summary>
        /// <param name="nom">Nom de l'employé</param>
        /// <param name="prenom">Prénom de l'employé</param>
        /// <param name="date_naissance">Date de naissance de l'employé</param>
        /// <param name="email">Email de l'employé</param>
        /// <param name="adresse">Adresse de l'employé</param>
        /// <param name="date_embauche">Date d'embauche de l'employé</param>
        /// <param name="taux_horaire">Taux Horaire de l'employé</param>
        /// <param name="lien_photo">Photo de l'employé</param>
        /// <param name="statut">Statut de l'employé</param>
        public void ajouterEmployesBD(string nom, string prenom,DateOnly date_naissance, string email, string adresse, DateOnly date_embauche, double taux_horaire, string lien_photo, string statut)
        {
            try
            {
                //appel de la procédure stockées (plus de requête SQL)
                MySqlCommand commande = new MySqlCommand("p_ajouter_enployes");
                commande.Connection = connection;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                //commande.Parameters.AddWithValue("matricule", matricule);
                commande.Parameters.AddWithValue("nom", nom);
                commande.Parameters.AddWithValue("prenom", prenom);
                commande.Parameters.AddWithValue("date_naissance", date_naissance);
                commande.Parameters.AddWithValue("email", email);
                commande.Parameters.AddWithValue("adresse", adresse);
                commande.Parameters.AddWithValue("date_embauche", date_embauche);
                commande.Parameters.AddWithValue("taux_horaire", taux_horaire);
                commande.Parameters.AddWithValue("lien_photo", lien_photo);
                commande.Parameters.AddWithValue("statut", statut);

                connection.Open();
                commande.Prepare();
                commande.ExecuteNonQuery();

                connection.Close();

                getListeEmployesBD();
            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

    }
}
