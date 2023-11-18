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
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestionProjetsEtClients
{
    public sealed partial class ModalConnexionAdmin : ContentDialog
    {
        string user;
        string password;

        public ModalConnexionAdmin()
        {
            this.InitializeComponent();
        }

        public string User { get => user; }
        public string Password { get => password; }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bool erreurSaisie = false;
            tblInvalidConnexion.Visibility = Visibility.Collapsed;

            if (String.IsNullOrEmpty(tbxUser.Text))
            {
                tbxUser.BorderBrush = new SolidColorBrush(Colors.Red);
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

            if (!erreurSaisie)
            {
                SingletonAdmin.getInstance().connexionAdmin(user, password);
                if (SingletonAdmin.connexion)
                {
                    args.Cancel = false;
                }
                else
                {
                    tbxUser.BorderBrush = new SolidColorBrush(Colors.Red);
                    pwbxMDP.BorderBrush = new SolidColorBrush(Colors.Red);
                    tblInvalidConnexion.Visibility = Visibility.Visible;
                    args.Cancel = true;
                }
            }
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = false;
        }
    }
}
