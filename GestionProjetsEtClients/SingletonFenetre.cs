using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionProjetsEtClients
{
    internal class SingletonFenetre
    {
        static SingletonFenetre instance;
        Window fenetre;
        NavigationView navView;
        public SingletonFenetre() { }
        
        public static SingletonFenetre getInstance()
        {
            if (instance == null)
                instance = new SingletonFenetre();

            return instance;
        }
        public Window Fenetre
        {
            get { return fenetre; }
            set { fenetre = value; }
        }

        public NavigationView NavView
        {
            get { return navView; }
            set { navView = value; }
        }
    }
}
