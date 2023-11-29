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
    public sealed partial class ModalAjouterEmployeProjet : ContentDialog
    {
        public ModalAjouterEmployeProjet()
        {
            this.InitializeComponent();
  
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            enleverMessagesErreurs();
            bool bErreur = false;

            if (!SingletonVerification.getInstance().isAdresseValide(txtBoxMatricule.Text))
            {
                txtBlErreurMatricule.Text = "Veuillez entrer un matricule";
                bErreur = true;
                args.Cancel = true;
            }

            if (!SingletonVerification.getInstance().isAdresseValide(txtBoxNoProjet.Text))
            {
                txtBlErreurNoProjet.Text = "Veuillez entrer un numéro de projet";
                bErreur = true;
                args.Cancel = true;
            }

            if (!SingletonVerification.getInstance().isTexteNonVideEtNum(txtBoxNbHeures.Text))
            {
                txtBlErreurNbHeures.Text = "Veuillez entrer un nombre d'heures";
                bErreur = true;
                args.Cancel = true;
            }

            if (!bErreur)
            {
                SingletonProjetEmploye.getInstance().ajouterProjetEmploye(txtBoxMatricule.Text, txtBoxNoProjet.Text, Convert.ToDouble(txtBoxNbHeures.Text));
                SingletonMessageValidation.getInstance().AfficherSucces = true;
                SingletonMessageValidation.getInstance().AfficherErreur = false;
                SingletonMessageValidation.getInstance().Titre = "Ajout";
                SingletonMessageValidation.getInstance().Message = "L'ajout d'un employé a fonctionné";
            }
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
        }

        private void enleverMessagesErreurs()
        {
            txtBlErreurMatricule.Text = string.Empty;
            txtBlErreurNoProjet.Text = string.Empty;
            txtBlErreurNbHeures.Text = string.Empty;
        }
    }
}
