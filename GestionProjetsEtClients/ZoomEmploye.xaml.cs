using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.WindowsAppSDK.Runtime.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static System.Net.WebRequestMethods;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestionProjetsEtClients
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ZoomEmploye : Page
    {
        int index = -1;
        public ZoomEmploye()
        {
            this.InitializeComponent();
            SingletonFenetre.getInstance().NavView.Header = "Détails de l'employé";
            SingletonProjet.getInstance().getListeProjets();
            if (SingletonMessageValidation.getInstance().AfficherSucces)
            {
                infoBar.IsOpen = true;
                infoBar.Severity = InfoBarSeverity.Success;
                infoBar.Title = SingletonMessageValidation.getInstance().Titre.ToString();
                infoBar.Message = SingletonMessageValidation.getInstance().Message.ToString();
            }
            else
            {
                infoBar.IsOpen = false;
            }

            if (SingletonAdmin.getInstance().valideConnexion())
            {
                comModif.Visibility = Visibility.Visible;
            }
            else
            {
                comModif.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            InfosNavigation infos = (InfosNavigation)e.Parameter;
            var texte = infos.NomPage;
            index = infos.IndexEmploye;
            string mat = infos.MatEmploye;
            if (texte == "AfficherEmployes")
            {
                imgProfil.ImageSource = new BitmapImage(new Uri(SingletonEmploye.getInstance().Employes[index].LienPhoto));
                txtBlMatricule.Text = "Matricule : " + SingletonEmploye.getInstance().Employes[index].Matricule;
                txtBlNom.Text = SingletonEmploye.getInstance().Employes[index].Prenom + " " + SingletonEmploye.getInstance().Employes[index].Nom;
                txtBlDateNaissance.Text = "Date de naissance : " + SingletonEmploye.getInstance().Employes[index].DateNaissance;
                txtBlEmail.Text = "Courriel : " + SingletonEmploye.getInstance().Employes[index].Email;
                txtBlAdresse.Text = "Adresse : " + SingletonEmploye.getInstance().Employes[index].Adresse;
                txtBlDateEmbauche.Text = "Date d'embauche : " + SingletonEmploye.getInstance().Employes[index].DateEmbauche;
                string sEmbauche = SingletonEmploye.getInstance().Employes[index].DateEmbauche;
                int iAnneeEmbauche = Convert.ToInt32(sEmbauche.Substring(0, 4));
                int iNbAnciennete = DateTime.Now.Year - iAnneeEmbauche;
                txtBlAnciennete.Text = "Ancienneté : " + iNbAnciennete + " " + "an(s)";
                txtBlTauxHoraire.Text = "Taux horaire : " + SingletonEmploye.getInstance().Employes[index].TauxHoraireFormat.ToString() + "/h";
                txtBlStatut.Text = "Statut : " + SingletonEmploye.getInstance().Employes[index].Statut;

                SingletonProjet.getInstance().getProjetEmployeEnCours(SingletonEmploye.getInstance().Employes[index].Matricule);
                SingletonProjet.getInstance().getListeProjetsEmployeTermines(SingletonEmploye.getInstance().Employes[index].Matricule);
                lvProjetEnCours.ItemsSource = SingletonProjet.getInstance().ProjetEC;
                lvProjetsTermines.ItemsSource = SingletonProjet.getInstance().ProjetsT;
            }
            else
            {
                index = SingletonEmploye.getInstance().getIndexParMatricule(mat);
                imgProfil.ImageSource = new BitmapImage(new Uri(SingletonEmploye.getInstance().Employes[index].LienPhoto));
                txtBlMatricule.Text = "Matricule : " + SingletonEmploye.getInstance().Employes[index].Matricule;
                txtBlNom.Text = SingletonEmploye.getInstance().Employes[index].Prenom + " " + SingletonEmploye.getInstance().Employes[index].Nom;
                txtBlDateNaissance.Text = "Date de naissance : " + SingletonEmploye.getInstance().Employes[index].DateNaissance;
                txtBlEmail.Text = "Courriel : " + SingletonEmploye.getInstance().Employes[index].Email;
                txtBlAdresse.Text = "Adresse : " + SingletonEmploye.getInstance().Employes[index].Adresse;
                txtBlDateEmbauche.Text = "Date d'embauche : " + SingletonEmploye.getInstance().Employes[index].DateEmbauche;
                string sEmbauche = SingletonEmploye.getInstance().Employes[index].DateEmbauche;
                int iAnneeEmbauche = Convert.ToInt32(sEmbauche.Substring(0, 4));
                int iNbAnciennete = DateTime.Now.Year - iAnneeEmbauche;
                txtBlAnciennete.Text = "Ancienneté : " + iNbAnciennete + " " + "an(s)";
                txtBlTauxHoraire.Text = "Taux horaire : " + SingletonEmploye.getInstance().Employes[index].TauxHoraireFormat.ToString() + "/h";
                txtBlStatut.Text = "Statut : " + SingletonEmploye.getInstance().Employes[index].Statut;

                SingletonProjet.getInstance().getProjetEmployeEnCours(mat);
                SingletonProjet.getInstance().getListeProjetsEmployeTermines(mat);
                lvProjetEnCours.ItemsSource = SingletonProjet.getInstance().ProjetEC;
                lvProjetsTermines.ItemsSource = SingletonProjet.getInstance().ProjetsT;
            }
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            SingletonMessageValidation.getInstance().annulerMessage();
            SingletonEmploye.getInstance().setIndex(index);
            ModalModifierEmploye modifierEmploye = new ModalModifierEmploye();
            modifierEmploye.XamlRoot = grilleEmploye.XamlRoot;
            modifierEmploye.Title = "Modifier";
            modifierEmploye.PrimaryButtonText = "Modifier";
            modifierEmploye.SecondaryButtonText = "Annuler";
            modifierEmploye.DefaultButton = ContentDialogButton.Primary;

            InfosNavigation infos = new InfosNavigation()
            {
                NomPage = "ModalModifierEmploye",
                MatEmploye = SingletonEmploye.getInstance().Employes[index].Matricule
            };

            var resultat = await modifierEmploye.ShowAsync();

            this.Frame.Navigate(typeof(ZoomEmploye), infos);
        }
        private void lvProjetsEnCours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (lvProjetEnCours.SelectedIndex >= 0)
            {
                Projet projet = lvProjetEnCours.SelectedItem as Projet;

                InfosNavigation infos = new InfosNavigation()
                {
                    NomPage = "ZoomEmploye",
                    NoProjet = projet.NoProjet,
                };
                this.Frame.Navigate(typeof(ZoomProjet), infos);
            }
        }
        private void lvProjetsTermines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvProjetsTermines.SelectedIndex >= 0)
            {
                Projet projet = lvProjetsTermines.SelectedItem as Projet;
                InfosNavigation infos = new InfosNavigation()
                {
                    NomPage = "ZoomEmploye",
                    NoProjet = projet.NoProjet,
                };
                this.Frame.Navigate(typeof(ZoomProjet), infos);
            }
        }
    }
}
