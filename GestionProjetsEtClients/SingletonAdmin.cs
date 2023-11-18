using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionProjetsEtClients
{
    internal class SingletonAdmin
    {
        static SingletonAdmin instance = null;
        static public bool connexion = false;
        MySqlConnection con;

        public SingletonAdmin()
        {
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2023_420325ri_fabeq11;Uid=1468780;Pwd=1468780;");
        }

        public static SingletonAdmin getInstance()
        {
            if (instance == null)
                instance = new SingletonAdmin();

            return instance;
        }

        public bool adminExiste()
        {
            int nbr_compte = 0;
            try
            {
                MySqlCommand command = new MySqlCommand("p_existe_admin");
                command.Connection = con;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                MySqlDataReader r = command.ExecuteReader();

                while (r.Read())
                {
                    nbr_compte = Convert.ToInt32(r["nbr_compte"]);
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

            if(nbr_compte > 0)
                return true;
            else
                return false;
        }

        public int ajouter(string user, string password)
        {
            int validation = 0;
            try
            {
                MySqlCommand command = new MySqlCommand("p_ajout_admin");
                command.Connection = con;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("_user", user);
                command.Parameters.AddWithValue("_password", convertionHash(password));

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

        public void connexionAdmin(string user, string password)
        {
            int nbr_compte = 0;
            try
            {
                MySqlCommand command = new MySqlCommand("p_valid_admin");
                command.Connection = con;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_user", user);
                command.Parameters.AddWithValue("_password", convertionHash(password));

                con.Open();
                command.Prepare();
                MySqlDataReader r = command.ExecuteReader();

                while (r.Read())
                {
                    nbr_compte = Convert.ToInt32(r["nbr_compte"]);
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

            if (nbr_compte > 0)
                connexion = true;
            else
                connexion = false;
        }

        private string convertionHash(string password)
        {
            SHA256 sha256 = SHA256.Create();
            Byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            Byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
            string cleanHashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "");
            return cleanHashedPassword;
        }
    }
}
