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
    public sealed partial class ModalAjouterEmploye : ContentDialog
    {
        public ModalAjouterEmploye()
        {
            this.InitializeComponent();
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

            if (!SingletonVerification.getInstance().isAdresseValide(txtBoxAdresse.Text))
            {
                txtBlErreurAdresse.Text = "Veuillez entrer une adresse.";
                bErreur = true;
                args.Cancel = true;
            }

            if (!SingletonVerification.getInstance().isDateEmbaucheValide(Convert.ToString(calDateEmbauche.Date)))
            {
                txtBlErreurDateEmbauche.Text = "Veuillez entrer une date d'embauche valide.";
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

            if (calDateEmbauche.Date != null)
            {
                string sEmbauche = Convert.ToString(calDateEmbauche.Date);
                int iAnneeEmbauche = Convert.ToInt32(sEmbauche.Substring(0, 4));
                int iNbAnciennete = DateTime.Now.Year - iAnneeEmbauche;

                if (iNbAnciennete < 3 && cmbBoxStatut.SelectedIndex == 1)
                {
                    txtBlErreurStatut1.Text = "Le statut ne peut pas être TP, selon l'anciennteté";
                    bErreur = true;
                    args.Cancel = true;
                }
            }

            if (!SingletonVerification.getInstance().isStatutValide(cmbBoxStatut.SelectedIndex))
            {
                txtBlErreurStatut.Text = "Veuillez sélectionner un statut.";
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
                 sDateEmbauche, Convert.ToDouble(txtBoxTauxHoraire.Text), txtBoxPhoto.Text, cmbBoxStatut.SelectedItem as string) > 0)
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = true;
                    SingletonMessageValidation.getInstance().AfficherErreur = false;
                    SingletonMessageValidation.getInstance().Titre = "Ajout";
                    SingletonMessageValidation.getInstance().Message = "L'ajout d'un employé a fonctionné";
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
