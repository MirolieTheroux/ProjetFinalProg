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

        public string Nom { get => nom; }
        public string Adresse { get => adresse; }
        public string NoTelephone { get => noTelephone; }
        public string Email { get => email; }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (String.IsNullOrEmpty(tbxNom.Text))
            {
                tbxNom.BorderBrush = new SolidColorBrush(Colors.Red);
                tblInvalidNom.Visibility = Visibility.Visible;
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
                args.Cancel = true;
            }
            else
            {
                tbxEmail.ClearValue(TextBox.BorderBrushProperty);
                tblInvalidEmail.Visibility = Visibility.Collapsed;
                email = tbxEmail.Text;
            }

        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = false;
        }
    }
}
