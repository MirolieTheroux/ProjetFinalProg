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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestionProjetsEtClients
{
    public sealed partial class ModalAjoutProjet : ContentDialog
    {
        int indexClient;
        string titre;
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

        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = false;
        }
    }
}
