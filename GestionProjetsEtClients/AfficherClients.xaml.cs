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
    public sealed partial class AfficherClients : Page
    {
        public AfficherClients()
        {
            this.InitializeComponent();
            lvListeClients.ItemsSource = SingletonClient.getInstance().Clients;
        }

        private void lvListeClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void btModalAjoutClient_Click(object sender, RoutedEventArgs e)
        {
            ModalAjoutClient ajoutClient = new ModalAjoutClient();
            ajoutClient.XamlRoot = grilleListeClient.XamlRoot;
            ajoutClient.Title = "Ajouter un client";
            ajoutClient.PrimaryButtonText = "Ajouter";
            ajoutClient.SecondaryButtonText = "Annuler";
            ajoutClient.DefaultButton = ContentDialogButton.Primary;

            var resultat = await ajoutClient.ShowAsync();

            /*if(resultat == ContentDialogResult.Primary)
            {
                tblTest.Text = "Nom : " + ajoutClient.Nom + "Adresse : " + ajoutClient.Adresse + "NoTelephone : " + ajoutClient.NoTelephone + "Email : " + ajoutClient.Email;
            }*/
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
        }
    }
}
