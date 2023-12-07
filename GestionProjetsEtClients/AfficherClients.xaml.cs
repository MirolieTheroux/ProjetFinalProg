using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.VisualBasic;
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
    public sealed partial class AfficherClients : Page
    {
        public AfficherClients()
        {
            this.InitializeComponent();
            SingletonFenetre.getInstance().NavView.Header = "Liste des clients";
            SingletonClient.getInstance().getListeClients();
            lvListeClients.ItemsSource = SingletonClient.getInstance().Clients;
            if (SingletonAdmin.getInstance().valideConnexion())
            {
                commandBar.Visibility = Visibility.Visible;
            }
            else
            {
                commandBar.Visibility= Visibility.Collapsed;
            }
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ModalAjoutClient ajoutClient = new ModalAjoutClient();
            ajoutClient.XamlRoot = grilleListeClient.XamlRoot;
            ajoutClient.Title = "Ajouter un client";
            ajoutClient.PrimaryButtonText = "Ajouter";
            ajoutClient.SecondaryButtonText = "Annuler";
            ajoutClient.DefaultButton = ContentDialogButton.Primary;

            var resultat = await ajoutClient.ShowAsync();

            if (SingletonMessageValidation.getInstance().AfficherSucces)
            {
                infoBar.IsOpen = true;
                infoBar.Severity = InfoBarSeverity.Success;
                infoBar.Title = SingletonMessageValidation.getInstance().Titre.ToString();
                infoBar.Message = SingletonMessageValidation.getInstance().Message.ToString();
            }
            else if(SingletonMessageValidation.getInstance().AfficherErreur)
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

        private void lvListeClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvListeClients.SelectedIndex >= 0)
            {
                SingletonMessageValidation.getInstance().annulerMessage();
                this.Frame.Navigate(typeof(ZoomClient), lvListeClients.SelectedIndex);
            }
        }

        private void txtBoxRechercheNom_SelectionChanged(object sender, RoutedEventArgs e)
        {
            SingletonClient.getInstance().getClientParNom(txtBoxRechercheNom.Text);
        }
    }
}
