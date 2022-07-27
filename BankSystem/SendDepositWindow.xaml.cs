using Logic;
using Models;
using System;
using System.Windows;

namespace BankSystem
{
    /// <summary>
    /// Interaction logic for SendDepositWindow.xaml
    /// </summary>
    public partial class SendDepositWindow : Window
    {
        private IBankAccount<Account> _bankRepository;
        private ClientRepository _clientRepository;
        private BankAccount _bankAccount;
        public SendDepositWindow(ClientRepository clientRepository, BankAccount bankAccount)
        {
            InitializeComponent();
            _bankRepository = new BankAccountProcess<Account>();
            _clientRepository = clientRepository;
            _bankAccount = bankAccount; 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!int.TryParse(txtboxAmount.Text, out int res1))
            {
                MessageBox.Show("Некорректная сумма");
                return;
            }
            BankAccount account = _bankRepository.GetAccountByNumber(txtboxNumber.Text);
            if (account == null)
            {
                MessageBox.Show("Не верный счет");
                return;
            }

            Client client = _clientRepository.GetClientById(account.ClientId);
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
            if(amount > _bankAccount.Amount)
            {
                MessageBox.Show("Недостаточно средств");
                return;
            }
            _bankAccount.Amount -= amount;
            _bankRepository.MakeTransfer(txtboxNumber.Text, _bankAccount.Number, amount);
            Close();
        }
    }
}
