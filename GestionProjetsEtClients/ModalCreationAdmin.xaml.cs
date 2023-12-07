using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.WindowsAppSDK.Runtime.Packages;
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
    public sealed partial class ModalCreationAdmin : ContentDialog
    {
        string user;
        string password;

        public ModalCreationAdmin()
        {
            this.InitializeComponent();
        }

        public string User { get => user; }
        public string Password { get => password; }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bool erreurSaisie = false;

            if (String.IsNullOrEmpty(tbxUser.Text))
            {
                tbxUser.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidUser.Text = "Veuillez entrer un nom d'utilisateur";
                tblInvalidUser.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else if (SingletonVerification.getInstance().isDataTooLong(tbxUser.Text, 50))
            {
                tbxUser.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidUser.Text = "Il y a trop de caract�res";
                tblInvalidUser.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else
            {
                tbxUser.ClearValue(TextBox.BorderBrushProperty);
                tblInvalidUser.Visibility = Visibility.Collapsed;
                user = tbxUser.Text;
            }

            if (String.IsNullOrEmpty(pwbxMDP.Password))
            {
                pwbxMDP.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidMDP.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else
            {
                pwbxMDP.ClearValue(PasswordBox.BorderBrushProperty);
                tblInvalidMDP.Visibility = Visibility.Collapsed;
                password = pwbxMDP.Password;
            }

            if (String.IsNullOrEmpty(pwbxConfirmMDP.Password))
            {
                pwbxConfirmMDP.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidConfirmMDP.Text = "Veuillez entrer la confirmation de votre mot de passe";
                tblInvalidConfirmMDP.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else if(pwbxConfirmMDP.Password != password)
            {
                pwbxConfirmMDP.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidConfirmMDP.Text = "Les 2 mots de passe de correspondent pas";
                tblInvalidConfirmMDP.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else
            {
                pwbxConfirmMDP.ClearValue(PasswordBox.BorderBrushProperty);
                tblInvalidConfirmMDP.Visibility = Visibility.Collapsed;
            }

            if(!erreurSaisie)
            {
                if (SingletonAdmin.getInstance().ajouter(user, password) > 0)
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = true;
                    SingletonMessageValidation.getInstance().AfficherErreur = false;
                    SingletonMessageValidation.getInstance().Titre = "Succ�s";
                    SingletonMessageValidation.getInstance().Message = "L'ajout du compte administrateur a fonctionn�e";
                }
                else
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = false;
                    SingletonMessageValidation.getInstance().AfficherErreur = true;
                    SingletonMessageValidation.getInstance().Titre = "Erreur";
                    SingletonMessageValidation.getInstance().Message = "L'ajout du compte administrateur a �chou�e";
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
