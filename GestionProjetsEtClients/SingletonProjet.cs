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
    class SingletonProjet
    {
        static SingletonProjet instance = null;
        MySqlConnection con;
        ObservableCollection<Projet> listeProjets;
        ObservableCollection<Projet> projetEnCours;
        ObservableCollection<Projet> listeProjetsTermines;

        int index;
        bool bProjetEnCours;

        public SingletonProjet()
        {
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2023_420325ri_fabeq11;Uid=1468780;Pwd=1468780;");
            listeProjets = new ObservableCollection<Projet>();
            projetEnCours = new ObservableCollection<Projet>();
            listeProjetsTermines = new ObservableCollection<Projet>();
            index = -1;
            getListeProjets();
        }

        public static SingletonProjet getInstance()
        {
            if (instance == null)
                instance = new SingletonProjet();

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

        public ObservableCollection<Projet> Projets { get { return listeProjets; } }
        public ObservableCollection<Projet> ProjetEC { get { return projetEnCours; } }
        public ObservableCollection<Projet> ProjetsT { get { return listeProjetsTermines; } }
      
        public void getListeProjets()
        {
            bProjetEnCours = false;
            listeProjets.Clear();
            try
            {
                MySqlCommand command = new MySqlCommand("p_get_projets");
                command.Connection = con;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                MySqlDataReader r = command.ExecuteReader();

                while (r.Read())
                {
                    string no_projet = (string)r["no_projet"];
                    string titre = (string)r["titre"];
                    DateTime date_debut = ((DateTime)r["date_debut"]).Date;
                    string description = (string)r["description"];
                    double budget = (double)r["budget"];
                    int nbr_employe_requis = (int)r["nbr_employe_requis"];
                    double total_salaire = (double)r["total_salaire"];
                    string statut = (string)r["statut"];
                    int id_client = (int)r["id_client"];
                    string nom_client = (string)r["nom_client"];

                    Projet projet = new Projet
                    {
                        NoProjet = no_projet,
                        Titre = titre,
                        DateDebut = date_debut.ToString("yyyy-MM-dd"),
                        Description = description,
                        Budget = budget,
                        NbrEmployeRequis = nbr_employe_requis,
                        TotalSalaire = total_salaire,
                        Statut = statut,
                        IdClient = id_client,
                        NomClient = nom_client
                    };
                    listeProjets.Add(projet);

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
        public void getListeProjetsClient(int id_client_zoom)
        {
            listeProjets.Clear();
            try
            {
                MySqlCommand command = new MySqlCommand("p_get_projets_client");
                command.Connection = con;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_id_client", id_client_zoom);
                con.Open();
                MySqlDataReader r = command.ExecuteReader();

                while (r.Read())
                {
                    string no_projet = (string)r["no_projet"];
                    string titre = (string)r["titre"];
                    DateTime date_debut = ((DateTime)r["date_debut"]).Date;
                    string description = (string)r["description"];
                    double budget = (double)r["budget"];
                    int nbr_employe_requis = (int)r["nbr_employe_requis"];
                    double total_salaire = (double)r["total_salaire"];
                    string statut = (string)r["statut"];
                    int id_client = (int)r["id_client"];
                    string nom_client = (string)r["nom_client"];

                    Projet projet = new Projet
                    {
                        NoProjet = no_projet,
                        Titre = titre,
                        DateDebut = date_debut.ToString("yyyy-MM-dd"),
                        Description = description,
                        Budget = budget,
                        NbrEmployeRequis = nbr_employe_requis,
                        TotalSalaire = total_salaire,
                        Statut = statut,
                        IdClient = id_client,
                        NomClient = nom_client
                    };
                    listeProjets.Add(projet);

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
        public void getProjetsEnCours()
        {
            bProjetEnCours = true;
            listeProjets.Clear();
            try
            {
                MySqlCommand command = new MySqlCommand("p_get_projets_encours");
                command.Connection = con;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                MySqlDataReader r = command.ExecuteReader();

                while (r.Read())
                {
                    string no_projet = (string)r["no_projet"];
                    string titre = (string)r["titre"];
                    DateTime date_debut = ((DateTime)r["date_debut"]).Date;
                    string description = (string)r["description"];
                    double budget = (double)r["budget"];
                    int nbr_employe_requis = (int)r["nbr_employe_requis"];
                    double total_salaire = (double)r["total_salaire"];
                    string statut = (string)r["statut"];
                    int id_client = (int)r["id_client"];
                    string nom_client = (string)r["nom_client"];

                    Projet projet = new Projet
                    {
                        NoProjet = no_projet,
                        Titre = titre,
                        DateDebut = date_debut.ToString("yyyy-MM-dd"),
                        Description = description,
                        Budget = budget,
                        NbrEmployeRequis = nbr_employe_requis,
                        TotalSalaire = total_salaire,
                        Statut = statut,
                        IdClient = id_client,
                        NomClient = nom_client
                    };
                    listeProjets.Add(projet);
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
        public bool valideProjetEncours() { return bProjetEnCours; }  
        public void getProjetEmployeEnCours(string sMatriculeEmp)
        {
            projetEnCours.Clear();
            try
            {
                MySqlCommand commande = new MySqlCommand("p_get_employe_projet_encours");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                commande.Parameters.AddWithValue("matEmp", sMatriculeEmp);
                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    string sNo_Projet = (string)reader["no_projet"];
                    string sTitreProjet = (string)reader["titre"];

                    Projet projetsEmp = new Projet
                    {
                        NoProjet = sNo_Projet,
                        Titre = sTitreProjet,
                    };
                    projetEnCours.Add(projetsEmp);
                }

                reader.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }
        public void getListeProjetsEmployeTermines(string sMatriculeEmp)
        {
            listeProjetsTermines.Clear();
            try
            {
                MySqlCommand commande = new MySqlCommand("p_get_employe_projets_termines");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                commande.Parameters.AddWithValue("matEmp", sMatriculeEmp);
                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    string sNo_Projet = (string)reader["no_projet"];
                    string sTitreProjet = (string)reader["titre"];

                    Projet projetsEmp = new Projet
                    {
                        NoProjet = sNo_Projet,
                        Titre = sTitreProjet,
                    };
                    listeProjetsTermines.Add(projetsEmp);
                }

                reader.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }
        public ObservableCollection<Projet> getProjetParTitre(string titreRecherche)
        {
            listeProjets.Clear();
            try
            {
                MySqlCommand command = new MySqlCommand("p_get_projets_par_titre");
                command.Connection = con;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("titreP", titreRecherche);
                con.Open();
                MySqlDataReader r = command.ExecuteReader();

                while (r.Read())
                {
                    string no_projet = (string)r["no_projet"];
                    string titre = (string)r["titre"];
                    DateTime date_debut = ((DateTime)r["date_debut"]).Date;
                    string description = (string)r["description"];
                    double budget = (double)r["budget"];
                    int nbr_employe_requis = (int)r["nbr_employe_requis"];
                    double total_salaire = (double)r["total_salaire"];
                    string statut = (string)r["statut"];
                    int id_client = (int)r["id_client"];
                    string nom_client = (string)r["nom_client"];

                    Projet projet = new Projet
                    {
                        NoProjet = no_projet,
                        Titre = titre,
                        DateDebut = date_debut.ToString("yyyy-MM-dd"),
                        Description = description,
                        Budget = budget,
                        NbrEmployeRequis = nbr_employe_requis,
                        TotalSalaire = total_salaire,
                        Statut = statut,
                        IdClient = id_client,
                        NomClient = nom_client
                    };
                    listeProjets.Add(projet);

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
            return listeProjets;
        }
        public int getIndexParNoProjet(string noProjet)
        {
            int index = -1;
            List<string> listeNoProjet = new List<string>();
            foreach (Projet projet in listeProjets)
            {
                listeNoProjet.Add(projet.NoProjet);
            }
            index = listeNoProjet.IndexOf(noProjet);
            return index;
        }
        public int ajouter(string titre, string date_debut, string description, double budget, int nbr_employe_requis, int id_client)
        {
            int validation = 0;
            try
            {
                MySqlCommand command = new MySqlCommand("p_ajout_projet");
                command.Connection = con;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("_titre", titre);
                command.Parameters.AddWithValue("_date_debut", date_debut);
                command.Parameters.AddWithValue("_description", description);
                command.Parameters.AddWithValue("_budget", budget);
                command.Parameters.AddWithValue("_nbr_employe_requis", nbr_employe_requis);
                command.Parameters.AddWithValue("_id_client", id_client);

                con.Open();
                command.Prepare();
                validation = command.ExecuteNonQuery();
                con.Close();

                if (bProjetEnCours)
                {
                    SingletonProjet.getInstance().getProjetsEnCours();
                }
                else
                {
                    SingletonProjet.getInstance().getListeProjets();
                }
                
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
        public int modifier(string no_projet, string titre, string description, double budget)
        {
            int validation = 0;
            try
            {
                MySqlCommand command = new MySqlCommand("p_modifier_projet");
                command.Connection = con;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("_no_projet", no_projet);
                command.Parameters.AddWithValue("_titre", titre);
                command.Parameters.AddWithValue("_description", description);
                command.Parameters.AddWithValue("_budget", budget);

                con.Open();
                command.Prepare();
                validation = command.ExecuteNonQuery();
                con.Close();

                if (bProjetEnCours)
                {
                    SingletonProjet.getInstance().getProjetsEnCours();
                }
                else
                {
                    SingletonProjet.getInstance().getListeProjets();
                }

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
        public int terminer(string no_projet)
        {
            int validation = 0;
            try
            {
                MySqlCommand command = new MySqlCommand("p_terminer_projet");
                command.Connection = con;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("_no_projet", no_projet);

                con.Open();
                command.Prepare();
                validation = command.ExecuteNonQuery();
                con.Close();

                if (bProjetEnCours)
                {
                    SingletonProjet.getInstance().getProjetsEnCours();
                }
                else
                {
                    SingletonProjet.getInstance().getListeProjets();
                }

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
    }
}
