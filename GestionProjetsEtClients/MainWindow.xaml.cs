using Google.Protobuf.Collections;
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
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            SingletonFenetre.getInstance().Fenetre = this;
            SingletonFenetre.getInstance().NavView = navView;
            mainFrame.Navigate(typeof(AfficherProjets));
            afficherBoutonConnexion();
        }
        private void navView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(SingletonAdmin.getInstance().adminExiste()))
            {
                afficherCreationAdmin();
            }
        }
        private void navView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var item = (NavigationViewItem)args.InvokedItemContainer;

            switch (item.Name)
            {
                case "iProjets":
                    SingletonMessageValidation.getInstance().annulerMessage();                
                    mainFrame.Navigate(typeof(AfficherProjets));
                    break;
                case "iClients":
                    SingletonMessageValidation.getInstance().annulerMessage();                
                    mainFrame.Navigate(typeof(AfficherClients));
                    break;
                case "iEmployes":
                    SingletonMessageValidation.getInstance().annulerMessage();
                    mainFrame.Navigate(typeof(AfficherEmployes));
                    break;
                case "iConnexion":
                    afficherConnexionAdmin();
                    break;
                case "iDeconnexion":
                    SingletonAdmin.getInstance().deconnexionAdmin();
                    afficherBoutonConnexion();
                    mainFrame.Navigate(typeof(AfficherProjets));
                    break;
                default:
                    break;
            }
        }
        private void navView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (mainFrame.CanGoBack)
                mainFrame.GoBack();
        }

        private async void afficherCreationAdmin()
        {
            ModalCreationAdmin ajoutAdmin = new ModalCreationAdmin();
            ajoutAdmin.XamlRoot = navView.XamlRoot;
            ajoutAdmin.Title = "Ajouter un compte administrateur";
            ajoutAdmin.PrimaryButtonText = "Ajouter";

            var resultat = await ajoutAdmin.ShowAsync();

            afficherBoutonConnexion();
            mainFrame.Navigate(typeof(AfficherProjets));
        }

        private async void afficherConnexionAdmin()
        {
            ModalConnexionAdmin connexionAdmin = new ModalConnexionAdmin();
            connexionAdmin.XamlRoot = navView.XamlRoot;
            connexionAdmin.Title = "Connexion compte administrateur";
            connexionAdmin.PrimaryButtonText = "Connexion";
            connexionAdmin.CloseButtonText = "Annuler";

            connexionAdmin.DefaultButton = ContentDialogButton.Primary;

            var resultat = await connexionAdmin.ShowAsync();

            afficherBoutonConnexion();
            mainFrame.Navigate(typeof(AfficherProjets));
        }

        public void afficherBoutonConnexion()
        {
            if (SingletonAdmin.getInstance().valideConnexion())
            {
                iConnexion.Visibility = Visibility.Collapsed;
                iDeconnexion.Visibility = Visibility.Visible;
            }
            else
            {
                iConnexion.Visibility = Visibility.Visible;
                iDeconnexion.Visibility = Visibility.Collapsed;
            }
        }
    }
}
