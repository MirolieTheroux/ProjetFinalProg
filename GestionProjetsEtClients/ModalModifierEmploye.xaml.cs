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
using Windows.Globalization.NumberFormatting;

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
            SetNumberBoxNumberFormatter();
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
            nbBoxTauxHoraire.Value = SingletonEmploye.getInstance().Employes[iIndex].TauxHoraire;

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
            if (iNbAnciennete > 3 && SingletonEmploye.getInstance().Employes[iIndex].Statut == "Journalier")
            {
                cmbBoxStatut.IsEnabled = true;
            }
            else
            {
                cmbBoxStatut.IsEnabled = false;
            }
            txtBoxPhoto.Text = SingletonEmploye.getInstance().Employes[iIndex].LienPhoto;
        }
        private void SetNumberBoxNumberFormatter()
        {
            IncrementNumberRounder rounderArgent = new IncrementNumberRounder();
            rounderArgent.Increment = 0.01;
            rounderArgent.RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp;

            DecimalFormatter formatterArgent = new DecimalFormatter();
            formatterArgent.IntegerDigits = 1;
            formatterArgent.FractionDigits = 2;
            formatterArgent.NumberRounder = rounderArgent;
            nbBoxTauxHoraire.NumberFormatter = formatterArgent;
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
            else if (SingletonVerification.getInstance().isDataTooLong(txtBoxNom.Text, 50))
            {
                txtBlErreurNom.Text = "Le nom est trop long.";
                bErreur = true;
                args.Cancel = true;
            }

            if (!SingletonVerification.getInstance().isTexteNonVideEtNonNum(txtBoxPrenom.Text))
            {
                txtBlErreurPrenom.Text = "Veuillez entrer un prénom.";
                bErreur = true;
                args.Cancel = true;
            }
            else if (SingletonVerification.getInstance().isDataTooLong(txtBoxPrenom.Text, 50))
            {
                txtBlErreurPrenom.Text = "Le prénom est trop long.";
                bErreur = true;
                args.Cancel = true;
            }

            if (!SingletonVerification.getInstance().isCourrielValide(txtBoxCourriel.Text))
            {
                txtBlErreurCourriel.Text = "Veuillez entrer un courriel valide.";
                bErreur = true;
                args.Cancel = true;
            }
            else if (SingletonVerification.getInstance().isDataTooLong(txtBoxCourriel.Text, 255))
            {
                txtBlErreurCourriel.Text = "Le courriel est trop long.";
                bErreur = true;
                args.Cancel = true;
            }

            if (!SingletonVerification.getInstance().isChampValide(txtBoxAdresse.Text))
            {
                txtBlErreurAdresse.Text = "Veuillez entrer une adresse.";
                bErreur = true;
                args.Cancel = true;
            }
            else if (SingletonVerification.getInstance().isDataTooLong(txtBoxAdresse.Text, 255))
            {
                txtBlErreurAdresse.Text = "L'adresse est trop longue.";
                bErreur = true;
                args.Cancel = true;
            }

            if (nbBoxTauxHoraire.Value is double.NaN)
            {
                txtBlErreurTauxHoraire.Text = "Veuillez entrer un taux horaire valide.";
                bErreur = true;
                args.Cancel = true;
            }
            else if (!SingletonVerification.getInstance().isTauxHValide(nbBoxTauxHoraire.Value))
            {
                txtBlErreurTauxHoraire.Text = "Veuillez entrer un taux horaire entre 15$/h et 100$/h.";
                bErreur = true;
                args.Cancel = true;
            }

            if (!SingletonVerification.getInstance().isLienValide(txtBoxPhoto.Text))
            {
                txtBlErreurPhoto.Text = "Veuillez entrer un lien valide.";
                bErreur = true;
                args.Cancel = true;
            }
            else if (SingletonVerification.getInstance().isDataTooLong(txtBoxPhoto.Text, 255))
            {
                txtBlErreurPhoto.Text = "Le lien de la photo est trop long.";
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

            if (!bErreur)
            {
                string sMatricule = SingletonEmploye.getInstance().Employes[iIndex].Matricule;

                if (SingletonEmploye.getInstance().modifierEmployesBD(sMatricule, txtBoxNom.Text, txtBoxPrenom.Text, txtBoxCourriel.Text, txtBoxAdresse.Text,
                Convert.ToDouble(nbBoxTauxHoraire.Value), txtBoxPhoto.Text, cmbBoxStatut.SelectedItem as string) > 0)
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = true;
                    SingletonMessageValidation.getInstance().AfficherErreur = false;
                    SingletonMessageValidation.getInstance().Titre = "Succès";
                    SingletonMessageValidation.getInstance().Message = "La modification de l'employé a fonctionnée";
                }
            }
        }
        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            SingletonMessageValidation.getInstance().annulerMessage();
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
