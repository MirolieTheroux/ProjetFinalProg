﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GestionProjetsEtClients
{
    class SingletonProjetEmploye
    {
        static SingletonProjetEmploye instance = null;
        MySqlConnection connection;
        ObservableCollection<ProjetEmploye> listeProjetEmploye;
        int index;
        public SingletonProjetEmploye()
        {
            connection = new MySqlConnection("Server=cours.cegep3r.info;Database=a2023_420325ri_fabeq11;Uid=1468780;Pwd=1468780;");
            listeProjetEmploye = new ObservableCollection<ProjetEmploye>();
            //getListeProjetEmployeD();
        }

        public static SingletonProjetEmploye getInstance()
        {
            if (instance == null)
                instance = new SingletonProjetEmploye();

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

        public ObservableCollection<ProjetEmploye> ProjetsEmploye { get { return listeProjetEmploye; } }

        public void getListeProjetsEmploye(string sNo_projet)
        {
            listeProjetEmploye.Clear();
            try
            {
                MySqlCommand commande = new MySqlCommand("p_get_projets_employe");
                commande.Connection = connection;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                commande.Parameters.AddWithValue("num_projet", sNo_projet);

                connection.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    //string sMatricule = (string)reader["matricule"];
                    string sNom = (string)reader["nom"];
                    string sPrenom = (string)reader["prenom"];
                    //string sNo_projetBd = (string)reader["no_projet"];
                    double dNbHeures = (double)reader["nbr_heure_travail"];
                    double dSalaire = (double)reader["salaire_employe_projet"];

                    ProjetEmploye projetemploye = new ProjetEmploye
                    {
                        //Matricule = sMatricule,
                        Nom = sNom,
                        Prenom = sPrenom,
                        //NoProjet = sNo_projetBd,
                        NbHeuresTravaillees = dNbHeures,
                        SalaireEmploye = dSalaire
                    };
                    listeProjetEmploye.Add(projetemploye);
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

        public int ajouterProjetEmploye(string sMatricule, string sNo_projet, double dHeuresTravaillees)
        {
            int iValidation = 0;
            try
            {
                //appel de la procédure stockées
                MySqlCommand commande = new MySqlCommand("p_ajout_projets_employe");
                commande.Connection = connection;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                commande.Parameters.AddWithValue("matEmploye", sMatricule);
                commande.Parameters.AddWithValue("num_projet", sNo_projet);
                commande.Parameters.AddWithValue("nb_heures_travail", dHeuresTravaillees);
           
                connection.Open();
                commande.Prepare();
                iValidation = commande.ExecuteNonQuery();

                connection.Close();

                getListeProjetsEmploye(sNo_projet);
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