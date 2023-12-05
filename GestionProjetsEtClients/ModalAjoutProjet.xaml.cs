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
using System.Reflection;
using Windows.Globalization.NumberFormatting;
using Microsoft.UI;
using Microsoft.WindowsAppSDK.Runtime.Packages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestionProjetsEtClients
{
    public sealed partial class ModalAjoutProjet : ContentDialog
    {
        int indexClient;
        string titre;
        string dateDebutPrep;
        string dateDebut;
        string description;
        double budget;
        int nbrEmployeRequis;
        int idClient;

        public ModalAjoutProjet()
        {
            this.InitializeComponent();
            SetNumberBoxNumberFormatter();
            indexClient = SingletonClient.getInstance().getIndex();
            idClient = SingletonClient.getInstance().Clients[indexClient].Id;
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
                tblInvalidBudget.Text = "Veuillez entrer le budget";
                erreurSaisie = true;
                args.Cancel = true;
            }
            else if (nbxBudget.Value < 0)
            {
                nbxBudget.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidBudget.Visibility = Visibility.Visible;
                tblInvalidBudget.Text = "Le budget ne peut pas être négatif";
                erreurSaisie = true;
                args.Cancel = true;
            }
            else if (nbxBudget.Value < 100)
            {
                nbxBudget.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidBudget.Visibility = Visibility.Visible;
                tblInvalidBudget.Text = "Nous n'acceptons pas les projets à moins de 100$";
                erreurSaisie = true;
                args.Cancel = true;
            }
            else
            {
                nbxBudget.ClearValue(TextBox.BorderBrushProperty);
                tblInvalidBudget.Visibility = Visibility.Collapsed;
                budget = (double)nbxBudget.Value;
            }

            if (nbxNbrEmployeRequis.Value is double.NaN)
            {
                nbxNbrEmployeRequis.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidNbrEmployeRequis.Visibility = Visibility.Visible;
                tblInvalidNbrEmployeRequis.Text = "Veuillez entrer le nombre d'employés requis";
                erreurSaisie = true;
                args.Cancel = true;
            }
            else if (nbxNbrEmployeRequis.Value < 1 || nbxNbrEmployeRequis.Value > 5)
            {
                nbxNbrEmployeRequis.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidNbrEmployeRequis.Visibility = Visibility.Visible;
                tblInvalidNbrEmployeRequis.Text = "Entrer un nombre entre 1 et 5 inclus";
                erreurSaisie = true;
                args.Cancel = true;
            }
            else
            {
                nbxNbrEmployeRequis.ClearValue(TextBox.BorderBrushProperty);
                tblInvalidNbrEmployeRequis.Visibility = Visibility.Collapsed;
                nbrEmployeRequis = (int)nbxNbrEmployeRequis.Value;
            }

            if (SingletonVerification.getInstance().isDateDebutValide(Convert.ToString(calDateDebut.Date)))
            {
                calDateDebut.ClearValue(TextBox.BorderBrushProperty);
                tblInvalidDateDebut.Visibility = Visibility.Collapsed;
                dateDebutPrep = Convert.ToString(calDateDebut.Date);
                dateDebut = dateDebutPrep.Substring(0, 10);
            }
            else
            {
                calDateDebut.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidDateDebut.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
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
                if (SingletonProjet.getInstance().ajouter(titre, dateDebut, description, budget, nbrEmployeRequis, idClient) > 0)
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = true;
                    SingletonMessageValidation.getInstance().AfficherErreur = false;
                    SingletonMessageValidation.getInstance().Titre = "Succès";
                    SingletonMessageValidation.getInstance().Message = "L'ajout du projet a fonctionné";
                }
                else
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = false;
                    SingletonMessageValidation.getInstance().AfficherErreur = true;
                    SingletonMessageValidation.getInstance().Titre = "Erreur";
                    SingletonMessageValidation.getInstance().Message = "L'ajout du projet a échoué";
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
