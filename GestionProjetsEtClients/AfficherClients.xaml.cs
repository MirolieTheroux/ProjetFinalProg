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

        private void autoSuggestBoxNom_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
       {
            List<String> suggestion = new List<String>();

            foreach(Client client in SingletonClient.getInstance().Clients)
            {
                if (client.Nom.Contains(autoSuggestBoxNom.Text))
                {
                    suggestion.Add(client.Nom);
                }

            }

            autoSuggestBoxNom.ItemsSource = suggestion;
        }

        private void autoSuggestBoxNom_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            int index = -1;
            bool quitterPage = false;

            try
            {
                index = SingletonClient.getInstance().getIndexNom(args.ChosenSuggestion.ToString());
                this.Frame.Navigate(typeof(ZoomClient), index);
            }
            catch (NullReferenceException nre)
            {
                foreach (Client client in SingletonClient.getInstance().Clients)
                {
                    if (client.Nom.Contains(sender.Text))
                    {
                        {
                            index = SingletonClient.getInstance().getIndexNom(client.Nom);
                            quitterPage = true;
                            break;
                        }
                    }
                }
                if(quitterPage)
                {
                    this.Frame.Navigate(typeof(ZoomClient), index);
                }
            }

        }
    }
}
