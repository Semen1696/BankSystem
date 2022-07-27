using System;
using System.Windows;

namespace BankSystem
{
    /// <summary>
    /// Interaction logic for DepositWindow.xaml
    /// </summary>
    public partial class DepositWindow : Window
    {
        public decimal Amount;
        public DepositWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Amount = Convert.ToDecimal(txtboxAmount.Text);
            DialogResult = true;
        }
    }
}
