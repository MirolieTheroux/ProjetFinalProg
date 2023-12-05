using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Bcpg;
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
            SingletonFenetre.getInstance().NavView.Header = "Détails du projet";
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

            if (SingletonAdmin.getInstance().valideConnexion())
            {
                commandBarHaut.Visibility = Visibility.Visible;
                commandBarBas.Visibility = Visibility.Visible;
            }
            else
            {
                commandBarHaut.Visibility = Visibility.Collapsed;
                commandBarBas.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            InfosNavigation infos = (InfosNavigation)e.Parameter;
            var texte = infos.NomPage;
            index = infos.IndexProjet;
            string noProjet = infos.NoProjet;
            if (texte == "AfficherProjets" || texte == "ModalAjouterEmployeProjet" || texte == "ZoomClient")
            {
                tblTitrePage.Text = $"{SingletonProjet.getInstance().Projets[index].NoProjet.ToString()} {SingletonProjet.getInstance().Projets[index].Titre.ToString()}";
                tblDateDebut.Text = $"Date de début : {SingletonProjet.getInstance().Projets[index].DateDebut.ToString()}";
                tblBudget.Text = $"Budget : {SingletonProjet.getInstance().Projets[index].BudgetFormat.ToString()}";
                tblClient.Text = $"Client : {SingletonProjet.getInstance().Projets[index].NomClient.ToString()}";
                tblStatut.Text = $"{SingletonProjet.getInstance().Projets[index].Statut.ToString()}";
                tblTotalSalaire.Text = $"Total des salaires : {SingletonProjet.getInstance().Projets[index].TotalSalaireFormat.ToString()}";

                Run run = new Run();
                run.Text = $"Description :\n{SingletonProjet.getInstance().Projets[index].Description.ToString()}";
                rtbpDescription.Inlines.Add(run);
                tblEmployeRequis.Text = $"(Max {SingletonProjet.getInstance().Projets[index].NbrEmployeRequis.ToString()} employés)";

                SingletonProjetEmploye.getInstance().getListeProjetsEmploye(SingletonProjet.getInstance().Projets[index].NoProjet.ToString());
                 lvProjetsEmploye.ItemsSource = SingletonProjetEmploye.getInstance().ProjetsEmploye;
                

                if (SingletonProjet.getInstance().Projets[index].NbrEmployeRequis > SingletonProjetEmploye.getInstance().ProjetsEmploye.Count && SingletonAdmin.getInstance().valideConnexion())
                {
                    abAjouterEmployer.Visibility = Visibility.Visible;
                }
                else
                {
                    abAjouterEmployer.Visibility = Visibility.Collapsed;
                }

                if (SingletonProjet.getInstance().Projets[index].Statut == "terminé")
                {
                    abModifierProjet.Visibility = Visibility.Collapsed;
                    abAjouterEmployer.Visibility = Visibility.Collapsed;
                    abTerminerProjet.Visibility = Visibility.Collapsed;
                    tblStatut.Foreground = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    abTerminerProjet.Visibility = Visibility.Visible;
                    tblStatut.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
            else 
            {
                index = SingletonProjet.getInstance().getIndexParNoProjet(noProjet);
                tblTitrePage.Text = $"{SingletonProjet.getInstance().Projets[index].NoProjet.ToString()} {SingletonProjet.getInstance().Projets[index].Titre.ToString()}";
                tblDateDebut.Text = $"Date de début : {SingletonProjet.getInstance().Projets[index].DateDebut.ToString()}";
                tblBudget.Text = $"Budget : {SingletonProjet.getInstance().Projets[index].BudgetFormat.ToString()}";
                tblClient.Text = $"Client : {SingletonProjet.getInstance().Projets[index].NomClient.ToString()}";
                tblStatut.Text = $"{SingletonProjet.getInstance().Projets[index].Statut.ToString()}";
                tblTotalSalaire.Text = $"Total des salaires : {SingletonProjet.getInstance().Projets[index].TotalSalaireFormat.ToString()}";

                Run run = new Run();
                run.Text = $"Description :\n{SingletonProjet.getInstance().Projets[index].Description.ToString()}";
                rtbpDescription.Inlines.Add(run);
                tblEmployeRequis.Text = $"(Max {SingletonProjet.getInstance().Projets[index].NbrEmployeRequis.ToString()} employés)";

                SingletonProjetEmploye.getInstance().getListeProjetsEmploye(SingletonProjet.getInstance().Projets[index].NoProjet.ToString());
                lvProjetsEmploye.ItemsSource = SingletonProjetEmploye.getInstance().ProjetsEmploye;


                if (SingletonProjet.getInstance().Projets[index].NbrEmployeRequis > SingletonProjetEmploye.getInstance().ProjetsEmploye.Count && SingletonAdmin.getInstance().valideConnexion())
                {
                    abAjouterEmployer.Visibility = Visibility.Visible;
                }
                else
                {
                    abAjouterEmployer.Visibility = Visibility.Collapsed;
                }

                if (SingletonProjet.getInstance().Projets[index].Statut == "terminé")
                {
                    abAjouterEmployer.Visibility = Visibility.Collapsed;
                    abTerminerProjet.Visibility = Visibility.Collapsed;
                    abModifierProjet.Visibility = Visibility.Collapsed;
                    tblStatut.Foreground = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    abTerminerProjet.Visibility = Visibility.Visible;
                    tblStatut.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
        }

        private async void abModifierProjet_Click(object sender, RoutedEventArgs e)
        {
            SingletonMessageValidation.getInstance().annulerMessage();
            SingletonProjet.getInstance().setIndex(index);
            ModalModifierProjet modifProjet = new ModalModifierProjet();
            modifProjet.XamlRoot = grilleProjet.XamlRoot;
            modifProjet.Title = "Modifier un client";
            modifProjet.PrimaryButtonText = "Modifier";
            modifProjet.SecondaryButtonText = "Annuler";
            modifProjet.DefaultButton = ContentDialogButton.Primary;
            InfosNavigation infos = new InfosNavigation()
            {
                NomPage = "ModalModifierProjet",
                NoProjet = SingletonProjet.getInstance().Projets[index].NoProjet
            };
            var resultat = await modifProjet.ShowAsync();
        

            this.Frame.Navigate(typeof(ZoomProjet), infos);
        }

        private async void abAjouterEmployer_Click(object sender, RoutedEventArgs e)
        {
            SingletonProjet.getInstance().setIndex(index);
            ModalAjouterEmployeProjet ajoutEmployeProjet = new ModalAjouterEmployeProjet();
            ajoutEmployeProjet.XamlRoot = grilleProjet.XamlRoot;
            ajoutEmployeProjet.Title = "Ajouter un employé à un projet";
            ajoutEmployeProjet.PrimaryButtonText = "Ajouter";
            ajoutEmployeProjet.SecondaryButtonText = "Annuler";
            ajoutEmployeProjet.DefaultButton = ContentDialogButton.Primary;
            var resultat = await ajoutEmployeProjet.ShowAsync();
            InfosNavigation infos = new InfosNavigation()
            {
                NomPage = "ModalAjouterEmployeProjet",
                IndexProjet = index,
            };
            this.Frame.Navigate(typeof(ZoomProjet), infos);
        }

        private void lvProjetsEmploye_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SingletonMessageValidation.getInstance().annulerMessage();
            if (lvProjetsEmploye.SelectedIndex >= 0)
            {
                ProjetEmploye pe = lvProjetsEmploye.SelectedItem as ProjetEmploye;

                InfosNavigation infos = new InfosNavigation()
                {
                    NomPage= "ZoomProjet",
                    MatEmploye= pe.Matricule
                };
                this.Frame.Navigate(typeof(ZoomEmploye),infos );
            }
        }

        private async void abTerminerProjet_Click(object sender, RoutedEventArgs e)
        {
            string no_projet = SingletonProjet.getInstance().Projets[index].NoProjet.ToString();
            SingletonProjet.getInstance().getListeProjets();
            index = SingletonProjet.getInstance().getIndexParNoProjet(no_projet);
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = grilleProjet.XamlRoot;
            dialog.Title = $"Terminer le projet #{no_projet}";
            dialog.PrimaryButtonText = "Terminer";
            dialog.CloseButtonText = "Annuler";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.Content = "Êtes-vous sûr de vouloir terminer le projet?";
            InfosNavigation infos = new InfosNavigation()
            {
                NomPage = "ModalTerminerProjet",
                NoProjet = SingletonProjet.getInstance().Projets[index].NoProjet
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (SingletonProjet.getInstance().terminer(no_projet) > 0)
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = true;
                    SingletonMessageValidation.getInstance().AfficherErreur = false;
                    SingletonMessageValidation.getInstance().Titre = "Succès";
                    SingletonMessageValidation.getInstance().Message = "La fermeture du projet a fonctionnée";
                }
                else
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = false;
                    SingletonMessageValidation.getInstance().AfficherErreur = true;
                    SingletonMessageValidation.getInstance().Titre = "Erreur";
                    SingletonMessageValidation.getInstance().Titre = "La fermeture du projet a échouée";
                }
            }
            this.Frame.Navigate(typeof(ZoomProjet), infos);
        }
    }
}
