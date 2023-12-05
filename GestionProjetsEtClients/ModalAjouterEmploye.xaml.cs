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
    public sealed partial class ModalAjouterEmploye : ContentDialog
    {
        public ModalAjouterEmploye()
        {
            this.InitializeComponent();
            SetNumberBoxNumberFormatter();
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
            else if(SingletonVerification.getInstance().isDataTooLong(txtBoxNom.Text, 50))
            {
                txtBlErreurNom.Text = "Le nom est trop long.";
                bErreur = true;
                args.Cancel = true;
            }

            if (!SingletonVerification.getInstance().isTexteNonVideEtNonNum(txtBoxPrenom.Text))
            {
                txtBlErreurPrenom.Text = "Veuillez entrer un pr�nom.";
                bErreur = true;
                args.Cancel = true;
            }
            else if (SingletonVerification.getInstance().isDataTooLong(txtBoxPrenom.Text, 50))
            {
                txtBlErreurPrenom.Text = "Le pr�nom est trop long.";
                bErreur = true;
                args.Cancel = true;
            }

            if (!SingletonVerification.getInstance().isDateValide(Convert.ToString(calDateNaissance.Date)))
            {
                txtBlErreurDdn.Text = "Veuillez entrer une date de naissance valide.";
                bErreur = true;
                args.Cancel = true;
            }

            if (!SingletonVerification.getInstance().isCourrielValide(txtBoxCourriel.Text))
            {
                txtBlErreurCourriel.Text = "Veuillez entrer un email valide.";
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

            if (!SingletonVerification.getInstance().isDateEmbaucheValide(Convert.ToString(calDateEmbauche.Date)))
            {
                txtBlErreurDateEmbauche.Text = "Veuillez entrer une date d'embauche valide.";
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

            if (calDateEmbauche.Date != null)
            {
                string sEmbauche = Convert.ToString(calDateEmbauche.Date);
                int iAnneeEmbauche = Convert.ToInt32(sEmbauche.Substring(0, 4));
                int iNbAnciennete = DateTime.Now.Year - iAnneeEmbauche;

                if (iNbAnciennete < 3 && cmbBoxStatut.SelectedIndex == 1)
                {
                    txtBlErreurStatut1.Text = "Le statut ne peut pas �tre TP, selon l'ancienntet�";
                    bErreur = true;
                    args.Cancel = true;
                }
            }

            if (!SingletonVerification.getInstance().isStatutValide(cmbBoxStatut.SelectedIndex))
            {
                txtBlErreurStatut.Text = "Veuillez s�lectionner un statut.";
                bErreur = true;
                args.Cancel = true;
            }

            if (!bErreur)
            {
                string sNaissance = Convert.ToString(calDateNaissance.Date);
                string sDateNaissance = sNaissance.Substring(0, 10);

                string sEmbauche = Convert.ToString(calDateEmbauche.Date);
                string sDateEmbauche = sEmbauche.Substring(0, 10);

                if (SingletonEmploye.getInstance().ajouterEmployesBD(txtBoxNom.Text, txtBoxPrenom.Text, sDateNaissance, txtBoxCourriel.Text, txtBoxAdresse.Text,
                 sDateEmbauche, Convert.ToDouble(nbBoxTauxHoraire.Value), txtBoxPhoto.Text, cmbBoxStatut.SelectedItem as string) > 0)
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = true;
                    SingletonMessageValidation.getInstance().AfficherErreur = false;
                    SingletonMessageValidation.getInstance().Titre = "Succ�s";
                    SingletonMessageValidation.getInstance().Message = "L'ajout d'un employ� a fonctionn�";
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
            txtBlErreurDdn.Text = string.Empty;
            txtBlErreurCourriel.Text = string.Empty;
            txtBlErreurAdresse.Text = string.Empty;
            txtBlErreurDateEmbauche.Text = string.Empty;
            txtBlErreurTauxHoraire.Text = string.Empty;
            txtBlErreurPhoto.Text = string.Empty;
            txtBlErreurStatut.Text = string.Empty;
        }

      
    }
}
