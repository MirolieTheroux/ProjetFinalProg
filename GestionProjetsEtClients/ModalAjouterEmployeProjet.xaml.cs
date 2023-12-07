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
    public sealed partial class ModalAjouterEmployeProjet : ContentDialog
    {
        public ModalAjouterEmployeProjet()
        {
            this.InitializeComponent();
            SetNumberBoxNumberFormatter();
        }
        private void SetNumberBoxNumberFormatter()
        {
            IncrementNumberRounder rounderHeure = new IncrementNumberRounder();
            rounderHeure.Increment = 0.01;
            rounderHeure.RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp;

            DecimalFormatter formatterHeure = new DecimalFormatter();
            formatterHeure.IntegerDigits = 1;
            formatterHeure.FractionDigits = 2;
            formatterHeure.NumberRounder = rounderHeure;
            nbBoxNbHeures.NumberFormatter = formatterHeure;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            enleverMessagesErreurs();
            bool bErreur = false;
            int indexProjet = SingletonProjet.getInstance().getIndex();

            if (!SingletonVerification.getInstance().isChampValide(asbxMatricule.Text))
            {
                txtBlErreurMatricule.Text = "Veuillez entrer un matricule";
                bErreur = true;
                args.Cancel = true;
            }
           
            if (nbBoxNbHeures.Value is double.NaN)
            {
                txtBlErreurNbHeures.Text = "Veuillez entrer un nombre d'heures";
                bErreur = true;
                args.Cancel = true;
            }

            if (!bErreur)
            {
                if(SingletonProjetEmploye.getInstance().ajouterProjetEmploye(asbxMatricule.Text, SingletonProjet.getInstance().Projets[indexProjet].NoProjet, nbBoxNbHeures.Value) > 0)
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = true;
                    SingletonMessageValidation.getInstance().AfficherErreur = false;
                    SingletonMessageValidation.getInstance().Titre = "Succès";
                    SingletonMessageValidation.getInstance().Message = "L'ajout d'un employé a fonctionné";
                }
                else
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = false;
                    SingletonMessageValidation.getInstance().AfficherErreur = true;
                }
            }
        }
        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            SingletonMessageValidation.getInstance().annulerMessage();
            args.Cancel = true;
        }

        private void enleverMessagesErreurs()
        {
            txtBlErreurMatricule.Text = string.Empty;
            txtBlErreurNbHeures.Text = string.Empty;
        }

        private void asbxMatricule_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            List<string> listeMatricule = new List<string>();

            foreach (Employe employe in SingletonEmploye.getInstance().Employes)
            {
                if (employe.Matricule.Contains(sender.Text))
                {
                    listeMatricule.Add(employe.Matricule);
                }
            }

            asbxMatricule.ItemsSource = listeMatricule;
        }
    }
}
