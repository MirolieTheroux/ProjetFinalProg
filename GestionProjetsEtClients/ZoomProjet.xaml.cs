using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
    public sealed partial class ZoomProjet : Page
    {
        int index = -1;
        public ZoomProjet()
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
            if (e.Parameter is not null)
            {
                index = (int)e.Parameter;

                tblTitrePage.Text = $"{SingletonProjet.getInstance().Projets[index].NoProjet.ToString()} {SingletonProjet.getInstance().Projets[index].Titre.ToString()}";
                tblDateDebut.Text = $"Date de début : {SingletonProjet.getInstance().Projets[index].DateDebut.ToString()}";
                tblBudget.Text = $"Budget : {SingletonProjet.getInstance().Projets[index].BudgetFormat.ToString()}";
                tblClient.Text = $"Client : {SingletonProjet.getInstance().Projets[index].NomClient.ToString()}";
                tblStatut.Text = $"Statut : {SingletonProjet.getInstance().Projets[index].Statut.ToString()}";
                tblTotalSalaire.Text = $"Total des salaires : {SingletonProjet.getInstance().Projets[index].TotalSalaireFormat.ToString()}";

                Run run = new Run();
                run.Text = $"Description :\n{SingletonProjet.getInstance().Projets[index].Description.ToString()}";
                rtbpDescription.Inlines.Add(run);
                tblEmployeRequis.Text = $"(Max {SingletonProjet.getInstance().Projets[index].NbrEmployeRequis.ToString()} employés)";
            }
        }

        private async void abModifierProjet_Click(object sender, RoutedEventArgs e)
        {
            SingletonProjet.getInstance().setIndex(index);
            ModalModifierProjet modifProjet = new ModalModifierProjet();
            modifProjet.XamlRoot = grilleProjet.XamlRoot;
            modifProjet.Title = "Modifier un client";
            modifProjet.PrimaryButtonText = "Modifier";
            modifProjet.SecondaryButtonText = "Annuler";
            modifProjet.DefaultButton = ContentDialogButton.Primary;
            var resultat = await modifProjet.ShowAsync();
            this.Frame.Navigate(typeof(ZoomProjet), index);
        }

        private void abAjouterEmployer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void abModiferHeures_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
