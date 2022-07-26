using BankSystem.Models;
using BankSystem.Repos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BankSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ClientRepository clientRepository;
        private ObservableCollection<Client> clients;
        private ObservableCollection<BankAccount> selectedAccounts;
        private IBankAccount<Account> bankAccount;
        private Client selectedClient;
        private BankAccount selectedAccount;
        private Index index;
        public MainWindow()
        {
            InitializeComponent();
            Initialsettings();

        }
        private void Initialsettings()
        {
            string indexPath = "Index.json";
            
            //bankRepository = new Account();
            
            index = JsonConvert.DeserializeObject<Index>(CreateOrRead(indexPath));

            if (index == null)
            {
                index = new Index { AccountId = 0, ClientId = 0 };
                File.WriteAllText(indexPath, JsonConvert.SerializeObject(index));
            }
            MainClientTypeList.SelectedIndex = 0;
            clientRepository.Notify += BankContext.Log;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddClientWindow addClientWindow = new AddClientWindow(clientRepository)
            {
                Owner = this
            };
            addClientWindow.Show();

        }

        private void InfoTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedClient = (Client)PhisicalTable.SelectedItem;
                if (selectedClient == null)
                    return;
            }
            catch
            {
                return;
            }
            bankAccount = new BankAccount<Account>();
            selectedAccounts = bankAccount.GetAccountsByClientId(selectedClient.Id);
            PhisicalAccountTable.ItemsSource = selectedAccounts;
            DeleteBtn.Visibility = Visibility.Visible;
            OpenAccBtn.IsEnabled = true;
            DeleteBtn.IsEnabled = true;
            SendAccBtn.IsEnabled = false;
            DepositAccBtn.IsEnabled = false;
            DeleteAccBtn.IsEnabled = false;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            clientRepository.RemoveClient(selectedClient);
            selectedAccounts.Clear();
            clients.Remove(selectedClient);

            DeleteBtn.Visibility = Visibility.Hidden;
        }
        public string CreateOrRead(string path)
        {
            string json = string.Empty;
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            else
            {
                json = File.ReadAllText(path);
            }

            return json;
        }

        private void Bill_Click(object sender, RoutedEventArgs e)
        {
            AccountDialogWindow accountDialogWindow = new AccountDialogWindow();
            if (accountDialogWindow.ShowDialog()==true)
            {
                string result = accountDialogWindow.Result;
                switch (result)
                {
                    case "Sample":
                        bankAccount = new BankAccount<Account>();
                        break;
                    case "Deposit":
                        bankAccount = new BankAccount<DepositAccount>();
                        break;
                    default:
                        break;
                }
                var account = bankAccount.CreateAccount(selectedClient);
                selectedAccounts.Add(account);
                MessageBox.Show("Счет открыт");
            }
           
        }

        private void DelAcc_Click(object sender, RoutedEventArgs e)
        {
            bankAccount = new BankAccount<Account>();
            bankAccount.RemoveAccount(selectedAccount);
            selectedAccounts.Remove(selectedAccount);
            DeleteAccBtn.IsEnabled = false;
            SendAccBtn.IsEnabled = false;
            DepositAccBtn.IsEnabled = false;
        }

        private void AccountTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedAccount = (BankAccount)PhisicalAccountTable.SelectedItem;
                if (selectedAccount == null)
                    return;
            }
            catch
            {
                return;
            }


        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            
            SendDepositWindow sendDepositWindow = new SendDepositWindow
                (clientRepository, selectedAccount)
            {
                Owner = this
            };
            sendDepositWindow.ShowDialog();          
            SendAccBtn.IsEnabled = false;
            DepositAccBtn.IsEnabled = false;
            DeleteAccBtn.IsEnabled = false;
            
        }

        private void Deposit_Click(object sender, RoutedEventArgs e)
        {
            DepositWindow depositWindow = new DepositWindow()
            {
                Owner = this
            };
            depositWindow.ShowDialog();
            bankAccount.MakeDeposit(selectedAccount, depositWindow.Amount);
            SendAccBtn.IsEnabled = false;
            DepositAccBtn.IsEnabled = false;
            DeleteAccBtn.IsEnabled = false;
        }

        private void DataGridRow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DeleteAccBtn.IsEnabled = true;
            SendAccBtn.IsEnabled = true;
            DepositAccBtn.IsEnabled = true;
            DeleteBtn.IsEnabled = false;
        }
        private void Account_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SaveBtn.IsEnabled = true;
        }

        private void MainClientTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem ComboItem = (ComboBoxItem)MainClientTypeList.SelectedItem;
            string name = ComboItem.Name;
            string path = string.Empty;
            switch (name)
            {
                case "Phisical":
                    path = "Clients.json";
                    break;
                case "Vip":
                    path = "VipClients.json";
                    break;
                case "Legal":
                    path = "LegalClients.json";
                    break;
            }
            clientRepository = new ClientRepository(path);
            clients = clientRepository.GetAllClients();
            PhisicalTable.ItemsSource = clients;
            PhisicalAccountTable.ItemsSource = new ObservableCollection<BankAccount>();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var result = CheckInput(selectedClient);
            if (result.Count != 0)
            {
                MessageBox.Show(string.Join("\n", result), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            clientRepository.SaveChanges();
            SaveBtn.IsEnabled = false;
            MessageBox.Show("Изменения сохранены!");
        }
        private List<string> CheckInput(Client client)
        {
            
            List<string> errors = new List<string>();
            if (!client.Name.Any(c => char.IsLetter(c)))
            {
                errors.Add("Неверный формат имени");
            }
            if (!client.Surname.Any(c => char.IsLetter(c)))
            {
                errors.Add("Неверный формат фамилии");
            }
            if (!Int32.TryParse(client.Age.ToString(), out int res))
            {
                errors.Add("Неверный формат возраста");
            }
            if (!Int32.TryParse(client.Phone.ToString(), out int res1))
            {
                errors.Add("Неверный формат телефона");
            }
            return errors;
        }

        private void PhisicalTable_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            SaveBtn.IsEnabled = true;
        }

        private void PhisicalTable_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                SaveBtn.IsEnabled = false;
            }
        }
    }
}
