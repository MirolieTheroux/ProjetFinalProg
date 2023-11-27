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
    public sealed partial class ZoomClient : Page
    {
        int index = -1;
        public ZoomClient()
        {
            this.InitializeComponent();

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is not null)
            {
                index = (int)e.Parameter;

                tblTitrePage.Text = $"{SingletonClient.getInstance().Clients[index].Nom.ToString()} - #{SingletonClient.getInstance().Clients[index].Id.ToString()}";
                tblAdresse.Text = $"Adresse : {SingletonClient.getInstance().Clients[index].Adresse.ToString()}";
                tblNoTel.Text = $"Numéro de téléphone : {SingletonClient.getInstance().Clients[index].NoTelephone.ToString()}";
                tblEmail.Text = $"Adresse courriel : {SingletonClient.getInstance().Clients[index].Email.ToString()}";

                SingletonProjet.getInstance().getListeProjetsClient(SingletonClient.getInstance().Clients[index].Id);
                lvListeProjetClient.ItemsSource = SingletonProjet.getInstance().Projets;
            }
        }

        private async void abModifier_Click(object sender, RoutedEventArgs e)
        {
            SingletonClient.getInstance().setIndex(index);
            ModalModificationClient modifClient = new ModalModificationClient();
            modifClient.XamlRoot = grilleClient.XamlRoot;
            modifClient.Title = "Modifier un client";
            modifClient.PrimaryButtonText = "Modifier";
            modifClient.SecondaryButtonText = "Annuler";
            modifClient.DefaultButton = ContentDialogButton.Primary;
            var resultat = await modifClient.ShowAsync();
            this.Frame.Navigate(typeof(ZoomClient), index);
        }

        private async void abCreerProjet_Click(object sender, RoutedEventArgs e)
        {
            SingletonClient.getInstance().setIndex(index);
            ModalAjoutProjet ajoutProjet = new ModalAjoutProjet();
            ajoutProjet.XamlRoot = grilleClient.XamlRoot;
            ajoutProjet.Title = "Ajouter un projet";
            ajoutProjet.PrimaryButtonText = "Ajouter";
            ajoutProjet.SecondaryButtonText = "Annuler";
            ajoutProjet.DefaultButton = ContentDialogButton.Primary;

            var resultat = await ajoutProjet.ShowAsync();

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

        private void lvListeProjetClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvListeProjetClient.SelectedIndex >= 0)
            {
                SingletonMessageValidation.getInstance().annulerMessage();
                this.Frame.Navigate(typeof(ZoomProjet), lvListeProjetClient.SelectedIndex);
            }
        }
    }
}
