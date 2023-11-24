using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAppSDK.Runtime.Packages;
using MySql.Data.MySqlClient;

namespace GestionProjetsEtClients
{
    internal class SingletonEmploye
    {
        static SingletonEmploye instance = null;
        MySqlConnection connection;
        ObservableCollection<Employe> listeEmployes;
        int index;
        public SingletonEmploye()
        {
            connection = new MySqlConnection("Server=cours.cegep3r.info;Database=a2023_420325ri_fabeq11;Uid=1468780;Pwd=1468780;");
            listeEmployes = new ObservableCollection<Employe>();
            getListeEmployesBD();
        }

        public static SingletonEmploye getInstance()
        {
            if (instance == null)
                instance = new SingletonEmploye();

            return instance;
        }

        public void setIndex(int iIndex)
        {
            index = iIndex;
        }

        public int getIndex()
        {
            return index;
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
                    DateTime datNaissance = ((DateTime)reader["date_naissance"]).Date;
                    string sEmail = (string)reader["email"];
                    string sAdresse = (string)reader["adresse"];
                    DateTime datEmbauche = ((DateTime)reader["date_embauche"]).Date;
                    double dTauxHoraire = (double)reader["taux_horaire"];
                    string sLienPhoto = (string)reader["lien_photo"];
                    string sStatut = (string)reader["statut"];

                    Employe employe = new Employe
                    {
                        Matricule = sMatricule,
                        Nom = sNom,
                        Prenom = sPrenom,
                        DateNaissance = datNaissance.ToString("yyyy-MM-dd"),
                        Email = sEmail,
                        Adresse = sAdresse,
                        DateEmbauche = datEmbauche.ToString("yyyy-MM-dd"),
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
        public int ajouterEmployesBD(string nom, string prenom, string date_naissance, string email, string adresse, string date_embauche, double taux_horaire, string lien_photo, string statut)
        {
            int iValidation = 0;
            try
            {
                //appel de la procédure stockées
                MySqlCommand commande = new MySqlCommand("p_ajouter_employes");
                commande.Connection = connection;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                commande.Parameters.AddWithValue("nomP", nom);
                commande.Parameters.AddWithValue("prenomP", prenom);
                commande.Parameters.AddWithValue("date_naissanceP", date_naissance);
                commande.Parameters.AddWithValue("emailP", email);
                commande.Parameters.AddWithValue("adresseP", adresse);
                commande.Parameters.AddWithValue("date_embaucheP", date_embauche);
                commande.Parameters.AddWithValue("taux_horaireP", taux_horaire);
                commande.Parameters.AddWithValue("lien_photoP", lien_photo);
                commande.Parameters.AddWithValue("statutP", statut);

                connection.Open();
                commande.Prepare();
                iValidation = commande.ExecuteNonQuery();

                connection.Close();

                getListeEmployesBD();
                return iValidation;
            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    return iValidation;
                }
                return iValidation;
            }
        }

        /// <summary>
        /// Permet de modifier un employé
        /// </summary>
        /// <param name="matricule">Matricule de l'employé</param>
        /// <param name="nom">Nom de l'employé</param>
        /// <param name="prenom">Prénom de l'employé</param>
        /// <param name="email">Email de l'employé</param>
        /// <param name="adresse">Adresse de l'employé</param>
        /// <param name="taux_horaire">Taux Horaire de l'employé</param>
        /// <param name="lien_photo">Photo de l'employé</param>
        /// <param name="statut">Statut de l'employé</param>
        public int modifierEmployesBD(string matricule, string nom, string prenom, string email, string adresse, double taux_horaire, string lien_photo, string statut)
        {
            int iValidation = 0;
            try
            {
                //appel de la procédure stockées 
                MySqlCommand commande = new MySqlCommand("p_modifier_employe");
                commande.Connection = connection;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                commande.Parameters.AddWithValue("mat", matricule);
                commande.Parameters.AddWithValue("nomP", nom);
                commande.Parameters.AddWithValue("prenomP", prenom);
                commande.Parameters.AddWithValue("emailP", email);
                commande.Parameters.AddWithValue("adresseP", adresse);
                commande.Parameters.AddWithValue("taux_horaireP", taux_horaire);
                commande.Parameters.AddWithValue("lien_photoP", lien_photo);
                commande.Parameters.AddWithValue("statutP", statut);

                connection.Open();
                commande.Prepare();
                iValidation = commande.ExecuteNonQuery();

                connection.Close();

                getListeEmployesBD();
                return iValidation;
            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    return iValidation;
                }
                return iValidation;
            }
        }
    }
}

