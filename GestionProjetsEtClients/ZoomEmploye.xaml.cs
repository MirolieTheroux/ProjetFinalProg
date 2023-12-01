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
            //faire un if selon le nom de la page et dans le else je vais faire une recherche par le matricule-> faire une procédure 
            //select ... where matricule=matricule.
            if (texte == "AfficherEmployes")
            {             
                    imgProfil.ImageSource = new BitmapImage(new Uri(SingletonEmploye.getInstance().Employes[infos.IndexEmploye].LienPhoto));
                    txtBlMatricule.Text = "Matricule : " + SingletonEmploye.getInstance().Employes[infos.IndexEmploye].Matricule;
                    txtBlNom.Text = SingletonEmploye.getInstance().Employes[infos.IndexEmploye].Prenom + " " + SingletonEmploye.getInstance().Employes[infos.IndexEmploye].Nom;
                    txtBlDateNaissance.Text = "Date de naissance : " + SingletonEmploye.getInstance().Employes[infos.IndexEmploye].DateNaissance;
                    txtBlEmail.Text = "Courriel : " + SingletonEmploye.getInstance().Employes[infos.IndexEmploye].Email;
                    txtBlAdresse.Text = "Adresse : " + SingletonEmploye.getInstance().Employes[infos.IndexEmploye].Adresse;
                    txtBlDateEmbauche.Text = "Date d'embauche : " + SingletonEmploye.getInstance().Employes[infos.IndexEmploye].DateEmbauche;
                    string sEmbauche = SingletonEmploye.getInstance().Employes[infos.IndexEmploye].DateEmbauche;
                    int iAnneeEmbauche = Convert.ToInt32(sEmbauche.Substring(0, 4));
                    int iNbAnciennete = DateTime.Now.Year - iAnneeEmbauche;
                    txtBlAnciennete.Text = "Ancienneté : " + iNbAnciennete + " " + "an(s)";
                    txtBlTauxHoraire.Text = "Taux horaire : " + SingletonEmploye.getInstance().Employes[infos.IndexEmploye].TauxHoraireFormat.ToString() + "/h";
                    txtBlStatut.Text = "Statut : " + SingletonEmploye.getInstance().Employes[infos.IndexEmploye].Statut;
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
            }

        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            SingletonEmploye.getInstance().setIndex(index);
            ModalModifierEmploye modifierEmploye = new ModalModifierEmploye();
            modifierEmploye.XamlRoot = grilleEmploye.XamlRoot;
            modifierEmploye.Title = "Modifier";
            modifierEmploye.PrimaryButtonText = "Modifier";
            modifierEmploye.SecondaryButtonText = "Annuler";
            modifierEmploye.DefaultButton = ContentDialogButton.Primary;
            var resultat = await modifierEmploye.ShowAsync();

            //raffraichis la liste des employés
            SingletonEmploye.getInstance().getListeEmployesBD();
            this.Frame.Navigate(typeof(ZoomEmploye), index);
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
