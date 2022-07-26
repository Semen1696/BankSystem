using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BankSystem
{
    /// <summary>
    /// Interaction logic for AccountDialogWindow.xaml
    /// </summary>
    public partial class AccountDialogWindow : Window
    {
        public string Result;

        public AccountDialogWindow()
        {
            InitializeComponent();
           
        }

        private void Sample_Click(object sender, RoutedEventArgs e)
        {
            Result = "Sample";
            DialogResult = true;
        }

        private void Deposit_Click(object sender, RoutedEventArgs e)
        {
            Result = "Deposit";
            DialogResult = true;
        }
    }
}
