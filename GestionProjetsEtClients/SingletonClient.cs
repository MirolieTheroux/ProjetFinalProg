﻿using Microsoft.WindowsAppSDK.Runtime.Packages;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionProjetsEtClients
{
    internal class SingletonClient
    {
        static SingletonClient instance = null;
        MySqlConnection con;
        ObservableCollection<Client> listeClients;

        public SingletonClient()
        {
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=h2023_420214_gr02_1596189-fleurent-nicolas;Uid=1596189;Pwd=1596189;");
            listeClients = new ObservableCollection<Client>();
            getListeClients();
        }

        public static SingletonClient getInstance()
        {
            if (instance == null)
                instance = new SingletonClient();

            return instance;
        }

        public ObservableCollection<Client> Clients { get { return listeClients; } }

        public void getListeClients()
        {
            listeClients.Clear();
            try
            {
                MySqlCommand command = new MySqlCommand("p_get_clients");
                command.Connection = con;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                MySqlDataReader r = command.ExecuteReader();

                while (r.Read())
                {
                    int id = (int)r["id_client"];
                    string nom = (string)r["nom"];
                    string adresse = (string)r["adresse"];
                    string no_telephone = (string)r["no_telephone"];
                    string email = (string)r["email"];
                    Client client = new Client { Id = id, Nom = nom, Adresse = adresse, NoTelephone = no_telephone, Email = email };
                    listeClients.Add(client);

                }
                r.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }

        }
    }
}
