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
            lvEmployes.ItemsSource = SingletonEmploye.getInstance().Employes;
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
        }
    }
}
