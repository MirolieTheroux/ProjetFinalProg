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
    public sealed partial class AfficherProjets : Page
    {
        public AfficherProjets()
        {
            this.InitializeComponent();
            SingletonProjet.getInstance().getListeProjets();
            lvListeProjets.ItemsSource = SingletonProjet.getInstance().Projets;

            if (SingletonAdmin.getInstance().valideConnexion())
            {
                commandBar.Visibility = Visibility.Visible;
            }
            else
            {
                commandBar.Visibility = Visibility.Collapsed;
            }

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

        private void lvListeProjets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvListeProjets.SelectedIndex >= 0)
            {
                SingletonMessageValidation.getInstance().annulerMessage();
                this.Frame.Navigate(typeof(ZoomProjet), lvListeProjets.SelectedIndex);
            }
        }

        private async void abbExportProjet_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(SingletonFenetre.getInstance().Fenetre);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

            picker.SuggestedFileName = "info_projets";
            picker.FileTypeChoices.Add("Fichier texte", new List<string>() { ".csv" });

            //crée le fichier
            Windows.Storage.StorageFile monFichier = await picker.PickSaveFileAsync();

            List<Projet> liste = new List<Projet>();
            foreach (Projet projet in SingletonProjet.getInstance().Projets)
            {
                liste.Add(projet);
            }

            if (monFichier != null)
            {
                // La fonction ToString de la classe Client retourne: nom + ";" + prenom
                await Windows.Storage.FileIO.WriteLinesAsync(monFichier, liste.ConvertAll(x => x.stringCSV()), Windows.Storage.Streams.UnicodeEncoding.Utf8);
            }
        }
    }
}
