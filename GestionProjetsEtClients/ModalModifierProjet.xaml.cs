using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI;
using Windows.Globalization.NumberFormatting;
using System.Reflection;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestionProjetsEtClients
{
    public sealed partial class ModalModifierProjet : ContentDialog
    {
        int index;
        int indexClient;

        string no_projet;
        string titre;
        string dateDebutPrep;
        string dateDebut;
        string description;
        double budget;
        int nbrEmployeRequis;
        int idClient;

        public ModalModifierProjet()
        {
            this.InitializeComponent();
            SetNumberBoxNumberFormatter();

            index = SingletonProjet.getInstance().getIndex();

            no_projet = SingletonProjet.getInstance().Projets[index].NoProjet.ToString();
            tbxTitre.Text = SingletonProjet.getInstance().Projets[index].Titre.ToString();
            nbxBudget.Value = SingletonProjet.getInstance().Projets[index].Budget;

            nbxNbrEmployeRequis.Value = SingletonProjet.getInstance().Projets[index].NbrEmployeRequis;
            nbrEmployeRequis = SingletonProjet.getInstance().Projets[index].NbrEmployeRequis;

            tbxDescription.Text = SingletonProjet.getInstance().Projets[index].Description.ToString();
            idClient = SingletonProjet.getInstance().Projets[index].IdClient;

            string sDateDebut = SingletonProjet.getInstance().Projets[index].DateDebut;

            // Convertir la chaîne de date de naissance en objet DateTime
            if (DateTime.TryParse(sDateDebut, out DateTime dateDebutAffichage))
            {
                // Assigner la date de naissance à calDateNaissance.Date
                calDateDebut.Date = dateDebutAffichage;
            }

            dateDebut = SingletonProjet.getInstance().Projets[index].DateDebut.ToString();
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
            nbxBudget.NumberFormatter = formatterArgent;

            IncrementNumberRounder rounderInt = new IncrementNumberRounder();
            rounderInt.Increment = 1;
            rounderInt.RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp;

            DecimalFormatter formatterInt = new DecimalFormatter();
            formatterInt.IntegerDigits = 1;
            formatterInt.FractionDigits = 0;
            formatterInt.NumberRounder = rounderInt;
            nbxNbrEmployeRequis.NumberFormatter = formatterInt;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bool erreurSaisie = false;

            if (String.IsNullOrEmpty(tbxTitre.Text))
            {
                tbxTitre.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidTitre.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else if (SingletonVerification.getInstance().isDataTooLong(tbxTitre.Text, 50))
            {
                tbxTitre.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidTitre.Text = "Il y a trop de caractères";
                tblInvalidTitre.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else
            {
                tbxTitre.ClearValue(TextBox.BorderBrushProperty);
                tblInvalidTitre.Visibility = Visibility.Collapsed;
                titre = tbxTitre.Text;
            }

            if (nbxBudget.Value is double.NaN)
            {
                nbxBudget.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidBudget.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else
            {
                nbxBudget.ClearValue(TextBox.BorderBrushProperty);
                tblInvalidBudget.Visibility = Visibility.Collapsed;
                budget = (double)nbxBudget.Value;
            }

            if (String.IsNullOrEmpty(tbxDescription.Text))
            {
                tbxDescription.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidDescription.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else
            {
                tbxDescription.ClearValue(TextBox.BorderBrushProperty);
                tblInvalidDescription.Visibility = Visibility.Collapsed;
                description = tbxDescription.Text;
            }

            if (!erreurSaisie)
            {
                if (SingletonProjet.getInstance().modifier(no_projet, titre, description, budget) > 0)
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = true;
                    SingletonMessageValidation.getInstance().AfficherErreur = false;
                    SingletonMessageValidation.getInstance().Titre = "Succès";
                    SingletonMessageValidation.getInstance().Message = "La modification du projet a fonctionné";
                }
                else
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = false;
                    SingletonMessageValidation.getInstance().AfficherErreur = true;
                    SingletonMessageValidation.getInstance().Titre = "Erreur";
                    SingletonMessageValidation.getInstance().Message = "La modification du projet a échoué";
                }
            }
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            SingletonMessageValidation.getInstance().annulerMessage();
            args.Cancel = false;
        }
    }
}
