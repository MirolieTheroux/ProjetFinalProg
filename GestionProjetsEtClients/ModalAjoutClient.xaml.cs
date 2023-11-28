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
using Microsoft.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestionProjetsEtClients
{
    public sealed partial class ModalAjoutClient : ContentDialog
    {
        string nom;
        string adresse;
        string noTelephone;
        string email;

        public ModalAjoutClient()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bool erreurSaisie = false;

            if (String.IsNullOrEmpty(tbxNom.Text))
            {
                tbxNom.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidNom.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else
            {
                tbxNom.ClearValue(TextBox.BorderBrushProperty);
                tblInvalidNom.Visibility = Visibility.Collapsed;
                nom = tbxNom.Text;
            }

            if (String.IsNullOrEmpty(tbxAdresse.Text))
            {
                tbxAdresse.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidAdresse.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else
            {
                tbxAdresse.ClearValue(TextBox.BorderBrushProperty);
                tblInvalidAdresse.Visibility = Visibility.Collapsed;
                adresse = tbxAdresse.Text;
            }

            if (String.IsNullOrEmpty(tbxNoTel.Text))
            {
                tbxNoTel.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidNoTel.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else
            {
                tbxNoTel.ClearValue(TextBox.BorderBrushProperty);
                tblInvalidNoTel.Visibility = Visibility.Collapsed;
                noTelephone = tbxNoTel.Text;
            }

            if (String.IsNullOrEmpty(tbxEmail.Text))
            {
                tbxEmail.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidEmail.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else
            {
                tbxEmail.ClearValue(TextBox.BorderBrushProperty);
                tblInvalidEmail.Visibility = Visibility.Collapsed;
                email = tbxEmail.Text;
            }

            if (!erreurSaisie)
            {
                if(SingletonClient.getInstance().ajouter(nom, adresse, noTelephone, email) > 0)
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = true;
                    SingletonMessageValidation.getInstance().AfficherErreur = false;
                    SingletonMessageValidation.getInstance().Titre = "Ajout";
                    SingletonMessageValidation.getInstance().Titre = "L'ajout d'un client a fonctionnée";
                }
                else
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = false;
                    SingletonMessageValidation.getInstance().AfficherErreur = true;
                    SingletonMessageValidation.getInstance().Titre = "Ajout";
                    SingletonMessageValidation.getInstance().Titre = "L'ajout d'un client a échouée";
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
