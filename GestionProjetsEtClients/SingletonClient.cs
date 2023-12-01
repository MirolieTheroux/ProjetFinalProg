using Microsoft.WindowsAppSDK.Runtime.Packages;
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
        int index;

        public SingletonClient()
        {
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2023_420325ri_fabeq11;Uid=1468780;Pwd=1468780;");
            listeClients = new ObservableCollection<Client>();
            index = -1;
            getListeClients();
        }

        public static SingletonClient getInstance()
        {
            if (instance == null)
                instance = new SingletonClient();

            return instance;
        }

        public void setIndex(int _index)
        {
            index = _index;
        }

        public int getIndex()
        {
            return index;
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

        public int ajouter(string nom, string adresse, string no_telephone, string email)
        {
            int validation = 0;
            try
            {
                MySqlCommand command = new MySqlCommand("p_ajout_client");
                command.Connection = con;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("_nom", nom);
                command.Parameters.AddWithValue("_adresse", adresse);
                command.Parameters.AddWithValue("_no_telephone", no_telephone);
                command.Parameters.AddWithValue("_email", email);

                con.Open();
                command.Prepare();
                validation = command.ExecuteNonQuery();
                con.Close();

                SingletonClient.getInstance().getListeClients();
                return validation;
            }
            catch (MySqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                    return validation;
                }
                return validation;
            }
        }

        public int modifier(int id, string nom, string adresse, string no_telephone, string email)
        {
            int validation = 0;
            try
            {
                MySqlCommand command = new MySqlCommand("p_modifier_client");
                command.Connection = con;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("_id", id);
                command.Parameters.AddWithValue("_nom", nom);
                command.Parameters.AddWithValue("_adresse", adresse);
                command.Parameters.AddWithValue("_no_telephone", no_telephone);
                command.Parameters.AddWithValue("_email", email);

                con.Open();
                command.Prepare();
                validation = command.ExecuteNonQuery();
                con.Close();

                SingletonClient.getInstance().getListeClients();
                return validation;
            }
            catch (MySqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                    return validation;
                }
                return validation;
            }
        }

        public int getIndexNom(string nom)
        {
            int index = -1;
            List<string> listeNom = new List<string>();

            foreach(Client client in listeClients)
            {
                listeNom.Add(client.Nom);
            }

            index = listeNom.IndexOf(nom);

            return index;
        }

        public ObservableCollection<Client> GetClientParNom(string nomRecherche)
        {
            listeClients.Clear();
            try
            {
                MySqlCommand command = new MySqlCommand("p_get_clients_par_nom");
                command.Connection = con;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("nomP", nomRecherche);
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
            return listeClients;
        }

    }
}
