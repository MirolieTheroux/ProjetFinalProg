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
    public sealed partial class ModalModifierEmploye : ContentDialog
    {
        int iIndex;

        public ModalModifierEmploye()
        {
            this.InitializeComponent();

            iIndex = SingletonEmploye.getInstance().getIndex();
            txtBoxNom.Text = SingletonEmploye.getInstance().Employes[iIndex].Nom;
            txtBoxPrenom.Text = SingletonEmploye.getInstance().Employes[iIndex].Prenom;

            string sDateNaissance = SingletonEmploye.getInstance().Employes[iIndex].DateNaissance;

            // Convertir la chaîne de date de naissance en objet DateTime
            if (DateTime.TryParse(sDateNaissance, out DateTime dateNaissance))
            {
                // Assigner la date de naissance à calDateNaissance.Date
                calDateNaissance.Date = dateNaissance;
            }

            txtBoxCourriel.Text = SingletonEmploye.getInstance().Employes[iIndex].Email;
            txtBoxAdresse.Text = SingletonEmploye.getInstance().Employes[iIndex].Adresse;

            string sDateEmbauche = SingletonEmploye.getInstance().Employes[iIndex].DateEmbauche;

            // Convertir la chaîne de date de naissance en objet DateTime
            if (DateTime.TryParse(sDateEmbauche, out DateTime dateEmbauche))
            {
                // Assigner la date de naissance à calDateNaissance.Date
                calDateEmbauche.Date = dateEmbauche;
            }
            txtBoxTauxHoraire.Text = SingletonEmploye.getInstance().Employes[iIndex].TauxHoraire.ToString();

            switch (SingletonEmploye.getInstance().Employes[iIndex].Statut)
            {
                case "Journalier":
                    cmbBoxStatut.SelectedIndex = 0;
                    break;
                case "Temps plein":
                    cmbBoxStatut.SelectedIndex = 1;
                    break;
                default:
                    break;
            }

            int iAnneeEmbauche = Convert.ToInt32(sDateEmbauche.Substring(0, 4));
            int iNbAnciennete = DateTime.Now.Year - iAnneeEmbauche;
            if (iNbAnciennete > 3)
            {
                cmbBoxStatut.IsEnabled = true;
            }
            else
            {
                cmbBoxStatut.IsEnabled = false;
            }
            txtBoxPhoto.Text = SingletonEmploye.getInstance().Employes[iIndex].LienPhoto;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            enleverMessagesErreurs();
            bool bErreur = false;

            if (!SingletonVerification.getInstance().isTexteNonVideEtNonNum(txtBoxNom.Text))
            {
                txtBlErreurNom.Text = "Veuillez entrer un nom.";
                bErreur = true;
                args.Cancel = true;
            }

            if (!SingletonVerification.getInstance().isTexteNonVideEtNonNum(txtBoxPrenom.Text))
            {
                txtBlErreurPrenom.Text = "Veuillez entrer un prénom.";
                bErreur = true;
                args.Cancel = true;
            }

            if (!SingletonVerification.getInstance().isCourrielValide(txtBoxCourriel.Text))
            {
                txtBlErreurCourriel.Text = "Veuillez entrer un email valide.";
                bErreur = true;
                args.Cancel = true;
            }

            if (!SingletonVerification.getInstance().isAdresseValide(txtBoxAdresse.Text))
            {
                txtBlErreurAdresse.Text = "Veuillez entrer une adresse.";
                bErreur = true;
                args.Cancel = true;
            }

            if (!SingletonVerification.getInstance().isTauxHValide(txtBoxTauxHoraire.Text))
            {
                txtBlErreurTauxHoraire.Text = "Veuillez entrer un taux horaire valide.";
                bErreur = true;
                args.Cancel = true;
            }

            if (!SingletonVerification.getInstance().isLienValide(txtBoxPhoto.Text))
            {
                txtBlErreurPhoto.Text = "Veuillez entrer un lien valide.";
                bErreur = true;
                args.Cancel = true;
            }

            if (cmbBoxStatut.IsEnabled)
            {


                if (!SingletonVerification.getInstance().isStatutValide(cmbBoxStatut.SelectedIndex))
                {
                    txtBlErreurStatut.Text = "Veuillez sélectionner un statut.";
                    bErreur = true;
                    args.Cancel = true;
                }
            }

            string sEmbauche = Convert.ToString(calDateEmbauche.Date);
            int iAnneeEmbauche = Convert.ToInt32(sEmbauche.Substring(0, 4));
            int iNbAnciennete = DateTime.Now.Year - iAnneeEmbauche;

            if (iNbAnciennete > 3 && cmbBoxStatut.SelectedIndex == 0)
            {
                txtBlErreurStatut1.Text = "Impossible de changer pour journalier";
                bErreur = true;
                args.Cancel = true;
            }

            if (!bErreur)
            {
                string sMatricule = SingletonEmploye.getInstance().Employes[iIndex].Matricule;

                if (SingletonEmploye.getInstance().modifierEmployesBD(sMatricule, txtBoxNom.Text, txtBoxPrenom.Text, txtBoxCourriel.Text, txtBoxAdresse.Text,
                Convert.ToDouble(txtBoxTauxHoraire.Text), txtBoxPhoto.Text, cmbBoxStatut.SelectedItem as string) > 0)
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = true;
                    SingletonMessageValidation.getInstance().AfficherErreur = false;
                    SingletonMessageValidation.getInstance().Titre = "Modification";
                    SingletonMessageValidation.getInstance().Titre = "La modification de l'employé a fonctionnée";
                }
            }
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = false;
        }

        /// <summary>
        /// Permet d'enlever les messages d'erreurs
        /// </summary>
        private void enleverMessagesErreurs()
        {
            txtBlErreurNom.Text = string.Empty;
            txtBlErreurPrenom.Text = string.Empty;
            txtBlErreurCourriel.Text = string.Empty;
            txtBlErreurAdresse.Text = string.Empty;
            txtBlErreurTauxHoraire.Text = string.Empty;
            txtBlErreurPhoto.Text = string.Empty;
            txtBlErreurStatut.Text = string.Empty;
        }
    }
}
