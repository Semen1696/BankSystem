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
            DepositBtn.Click += delegate
            {
                Amount = Convert.ToDecimal(txtboxAmount.Text);
                DialogResult = true;
            };
        }

    }
}
