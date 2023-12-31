using Microsoft.UI;
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
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestionProjetsEtClients
{
    public sealed partial class ModalModificationClient : ContentDialog
    {
        int index;
        int id;
        string nom;
        string adresse;
        string noTelephone;
        string email;
        public ModalModificationClient()
        {
            this.InitializeComponent();
            index = SingletonClient.getInstance().getIndex();

            id = SingletonClient.getInstance().Clients[index].Id;
            tbxNom.Text = SingletonClient.getInstance().Clients[index].Nom.ToString();
            tbxAdresse.Text = SingletonClient.getInstance().Clients[index].Adresse.ToString();
            tbxNoTel.Text = SingletonClient.getInstance().Clients[index].NoTelephone.ToString();
            tbxEmail.Text = SingletonClient.getInstance().Clients[index].Email.ToString();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bool erreurSaisie = false;

            if (String.IsNullOrEmpty(tbxNom.Text))
            {
                tbxNom.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidNom.Text = "Veuillez entrer un nom";
                tblInvalidNom.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else if (SingletonVerification.getInstance().isDataTooLong(tbxNom.Text, 100))
            {
                tbxNom.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidNom.Text = "Le nom du client est trop long";
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
                tblInvalidAdresse.Text = "Veuillez entrer une adresse";
                tblInvalidAdresse.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else if (SingletonVerification.getInstance().isDataTooLong(tbxAdresse.Text, 255))
            {
                tbxAdresse.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidAdresse.Text = "L'adresse est trop longue";
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
                tblInvalidNoTel.Text = "Veuillez entrer un num�ro de t�l�phone";
                tblInvalidNoTel.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else if (SingletonVerification.getInstance().isDataTooLong(tbxNoTel.Text, 20))
            {
                tbxNoTel.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidNoTel.Text = "Le num�ro de t�l�phone est trop long";
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

            if (!SingletonVerification.getInstance().isCourrielValide(tbxEmail.Text))
            {
                tbxEmail.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidEmail.Text = "Veuillez entrer un courriel valide";
                tblInvalidEmail.Visibility = Visibility.Visible;
                erreurSaisie = true;
                args.Cancel = true;
            }
            else if (SingletonVerification.getInstance().isDataTooLong(tbxEmail.Text, 255))
            {
                tbxEmail.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidEmail.Text = "Le courriel est trop long ";
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
                if(SingletonClient.getInstance().modifier(id, nom, adresse, noTelephone, email) > 0)
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = true;
                    SingletonMessageValidation.getInstance().AfficherErreur = false;
                    SingletonMessageValidation.getInstance().Titre = "Succ�s";
                    SingletonMessageValidation.getInstance().Message = "La modification du client a fonctionn�e";
                }
                else
                {
                    SingletonMessageValidation.getInstance().AfficherSucces = false;
                    SingletonMessageValidation.getInstance().AfficherErreur = true;
                    SingletonMessageValidation.getInstance().Titre = "Erreur";
                    SingletonMessageValidation.getInstance().Message = "La modification du client a �chou�e";
                }
            }
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = false;
        }
    }
}
