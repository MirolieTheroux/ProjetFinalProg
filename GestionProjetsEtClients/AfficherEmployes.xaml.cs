using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestionProjetsEtClients
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AfficherEmployes : Page
    {
        public AfficherEmployes()
        {
            this.InitializeComponent();
            SingletonFenetre.getInstance().NavView.Header = "Liste des employ�s";
            SingletonEmploye.getInstance().getListeEmployesBD();
            lvEmployes.ItemsSource = SingletonEmploye.getInstance().Employes;
            if (SingletonAdmin.getInstance().valideConnexion())
            {
                comAjout.Visibility = Visibility.Visible;
            }
            else
            {
                comAjout.Visibility = Visibility.Collapsed;
            }

        }

        private async void  AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ModalAjouterEmploye ajouterEmploye = new ModalAjouterEmploye();
            ajouterEmploye.XamlRoot = grilleEmployes.XamlRoot;
            ajouterEmploye.Title = "Ajouter un employ�";
            ajouterEmploye.PrimaryButtonText = "Ajouter";
            ajouterEmploye.SecondaryButtonText = "Annuler";
            ajouterEmploye.DefaultButton = ContentDialogButton.Primary;

            var resultat = await ajouterEmploye.ShowAsync();

            if (SingletonMessageValidation.getInstance().AfficherSucces)
            {
                infoBar.IsOpen = true;
                infoBar.Severity = InfoBarSeverity.Success;
                infoBar.Title = SingletonMessageValidation.getInstance().Titre.ToString();
                infoBar.Message = SingletonMessageValidation.getInstance().Message.ToString();
            }
            else if (SingletonMessageValidation.getInstance().AfficherErreur)
            {
                infoBar.IsOpen = true;
                infoBar.Severity = InfoBarSeverity.Error;
                infoBar.Title = SingletonMessageValidation.getInstance().Titre.ToString();
                infoBar.Message = SingletonMessageValidation.getInstance().Message.ToString();
            }
            else
            {
                infoBar.IsOpen = false;
            }
        }

        private void lvEmployes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvEmployes.SelectedIndex >= 0)
            {
                InfosNavigation infos = new InfosNavigation()
                {
                    NomPage = "AfficherEmployes",
                    IndexEmploye = lvEmployes.SelectedIndex
                };
                this.Frame.Navigate(typeof(ZoomEmploye), infos);
            }
        }

        private void txtBoxRechercheVille_SelectionChanged(object sender, RoutedEventArgs e)
        {
            SingletonEmploye.getInstance().getEmployeParNom(txtBoxRechercheNomOuPrenom.Text);

        }
    }
}
