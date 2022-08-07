using Common;
using Logic;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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
        private IBankAccount bankAccount;
        DataRowView row;
        DataRowView selectedAccount;
        public MainWindow()
        {
            InitializeComponent();
            Initialsettings();

        }
        private void Initialsettings()
        {
            BankContext.Connect();
            clientRepository = new ClientRepository();
            bankAccount = new Account();
            MainClientTypeList.SelectedIndex = 0;
            bankAccount.Notify += BankContext.Log;
            clientRepository.Notify += BankContext.Log;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddClientWindow addClientWindow = new AddClientWindow(clientRepository)
            {
                Owner = this
            };
            addClientWindow.ShowDialog();

        }

        private void InfoTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            try
            {
                row = (DataRowView)PhisicalTable.SelectedItem;
                if (row == null)
                    return;
            }
            catch
            {
                return;
            }

           // bankAccount = new Account();
            bankAccount.FillAccountTableByClientId(Convert.ToInt32(row["id"]));
            PhisicalAccountTable.DataContext = BankContext.AccountTable.DefaultView;
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
            clientRepository.RemoveClient(Convert.ToInt32(row["id"]));

            DeleteBtn.Visibility = Visibility.Hidden;
        }

        private void Bill_Click(object sender, RoutedEventArgs e)
        {
            AccountDialogWindow accountDialogWindow = new AccountDialogWindow();
            if (accountDialogWindow.ShowDialog().Value)
            {
                string result = accountDialogWindow.Result;
                switch (result)
                {
                    case "Sample":
                        bankAccount = new Account();
                        break;
                    case "Deposit":
                        bankAccount = new DepositAccount();
                        break;
                    default:
                        break;
                }
                bankAccount.Notify += BankContext.Log;
                Client client = clientRepository.GetClientById(Convert.ToInt32(row["id"]));
                bankAccount.CreateAccount(client);
                MessageBox.Show("Счет открыт");
            }
           
        }

        private void DelAcc_Click(object sender, RoutedEventArgs e)
        {
            bankAccount.RemoveAccount(Convert.ToInt32(selectedAccount["Id"]));
            DeleteAccBtn.IsEnabled = false;
            SendAccBtn.IsEnabled = false;
            DepositAccBtn.IsEnabled = false;
        }

        private void AccountTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedAccount = (DataRowView)PhisicalAccountTable.SelectedItem;
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
                (clientRepository, selectedAccount, bankAccount)
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

            bankAccount.MakeDeposit(Convert.ToInt32(selectedAccount["Id"]), depositWindow.Amount);
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
            string id = string.Empty;
            switch (name)
            {
                case "Phisical":
                    id = "0";
                    break;
                case "Vip":
                    id = "1";
                    break;
                case "Legal":
                    id = "2";
                    break;
                default:
                    break;
            }
            BankContext.ClientTable = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string sql = $@"SELECT * FROM Clients WHERE ClientTypeId={id}";
            da.SelectCommand = new SqlCommand(sql, BankContext.con);
            da.Fill(BankContext.ClientTable);

            PhisicalTable.DataContext = BankContext.ClientTable.DefaultView;
            
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var result = CheckInput(row);
            if (result.Count != 0)
            {
                MessageBox.Show(string.Join("\n", result), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            row.EndEdit();
            clientRepository.SaveChanges();
            SaveBtn.IsEnabled = false;
            MessageBox.Show("Изменения сохранены!");
        }
        private List<string> CheckInput(DataRowView client)
        {
            
            List<string> errors = new List<string>();
            if (!client["Name"].ToString().Any(c => char.IsLetter(c)))
            {
                errors.Add("Неверный формат имени");
            }
            if (!client["Surname"].ToString().Any(c => char.IsLetter(c)))
            {
                errors.Add("Неверный формат фамилии");
            }
            if (!Int32.TryParse(client["Age"].ToString(), out int res))
            {
                errors.Add("Неверный формат возраста");
            }
            if (!Int32.TryParse(client["Phone"].ToString(), out int res1))
            {
                errors.Add("Неверный формат телефона");
            }
            return errors;
        }

        private void PhisicalTable_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            row.BeginEdit();
            SaveBtn.IsEnabled = true;
        }

        private void PhisicalTable_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                SaveBtn.IsEnabled = false;
            }
            
        }

        private void PhisicalTable_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            row.BeginEdit();
            SaveBtn.IsEnabled = true;
        }
    }
}
