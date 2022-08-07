using Common;
using Logic;
using Models;
using System;
using System.Data;
using System.Windows;

namespace BankSystem
{
    /// <summary>
    /// Interaction logic for SendDepositWindow.xaml
    /// </summary>
    public partial class SendDepositWindow : Window
    {
        private SendDepositWindow() { InitializeComponent(); }
        public SendDepositWindow(ClientRepository clientRepository, DataRowView bankAccount, IBankAccount account) : this()
        {
            SendBtn.Click += delegate
            {
                if (!int.TryParse(txtboxAmount.Text, out int res1))
                {
                    MessageBox.Show("Некорректная сумма");
                    return;
                }
                var acc = account.GetAccountByNumber(txtboxNumber.Text);
                if (acc == null)
                {
                    MessageBox.Show("Не верный счет");
                    return;
                }

                Client client = clientRepository.GetClientById(Convert.ToInt32(acc["ClientId"]));
                MessageBoxResult result = MessageBox.Show(
                    $"Выполнить перевод?" +
                    $"\nИмя: {client.Name}" +
                    $"\nФамилия: {client.Surname}" +
                    $"\nСумма: {txtboxAmount.Text}", "", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
                decimal amount = Convert.ToDecimal(txtboxAmount.Text);
                if (amount > Convert.ToDecimal(bankAccount["Amount"]))
                {
                    MessageBox.Show("Недостаточно средств");
                    return;
                }
                account.MakeTransfer(txtboxNumber.Text, bankAccount["Number"].ToString(), amount);
                DialogResult = true;
            };
           
        }
    }
}
