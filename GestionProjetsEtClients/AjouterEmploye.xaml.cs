using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestionProjetsEtClients
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AjouterEmploye : Page
    {
        public AjouterEmploye()
        {
            this.InitializeComponent();
        }

        private void btnAjouterEmploye_Click(object sender, RoutedEventArgs e)
        {
            enleverMessagesErreurs();
            bool bErreur = false;

            if (!SingletonVerificationEmploye.getInstance().isTexteNonVideEtNonNum(txtBoxNom.Text))
            {
                txtBlErreurNom.Text = "Veuillez entrer le nom d'employ�";
                bErreur = true;
            }

            if (!SingletonVerificationEmploye.getInstance().isTexteNonVideEtNonNum(txtBoxPrenom.Text))
            {
                txtBlErreurPrenom.Text = "Veuillez entrer le pr�nom de l'employ�";
                bErreur = true;
            }

            if (!SingletonVerificationEmploye.getInstance().isDateValide(DateOnly.FromDateTime(calDateNaissance.Date)))
            {

            }
        }

        /// <summary>
        /// Permet d'enlever les messages d'erreurs
        /// </summary>
        private void enleverMessagesErreurs()
        {
            txtBlErreurNom.Text = string.Empty;
            txtBlErreurPrenom.Text = string.Empty;
            txtBlErreurDdn.Text = string.Empty;
            txtBlErreurCourriel.Text = string.Empty;
            txtBlErreurAdresse.Text = string.Empty;
            txtBlErreurDateEmbauche.Text = string.Empty;
            txtBlErreurTauxHoraire.Text = string.Empty;
            txtBlErreurPhoto.Text = string.Empty;
            txtBlErreurStatut.Text = string.Empty;        
        }
    }
}
