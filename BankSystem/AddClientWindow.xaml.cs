﻿using BankSystem.Models;
using BankSystem.Repos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        private readonly ClientRepository clientRepository;
        public AddClientWindow(ClientRepository client)
        {
            InitializeComponent();
            clientRepository = client;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var result = CheckInput();
            if(result.Count != 0)
            {
                MessageBox.Show(string.Join("\n", result), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Client client = new Client
            {
                Name = txtboxName.Text,
                Surname = txtboxSurname.Text,
                Age = Convert.ToInt32(txtboxAge.Text),
                Phone = txtboxPhone.Text,
            };
            string type = ClientTypeList.Text;
            switch (type)
            {
                case "Физ. лицо":
                    client.ClientType = ClientType.Phisical;
                    break;
                case "Вип клиент":
                    client.ClientType = ClientType.Vip;
                    break;
                case "Юр. лицо":
                    client.ClientType = ClientType.Legal;
                    break;
                default:
                    break;
            }
            clientRepository.AddClient(client);



            Close();
        }

        private List<string> CheckInput()
        {
            List<string> errors = new List<string>();
            if (!txtboxName.Text.Any(c => char.IsLetter(c)))
            {
                errors.Add("Неверный формат имени");
            }
            if (!txtboxSurname.Text.Any(c => char.IsLetter(c)))
            {
                errors.Add("Неверный формат фамилии");
            }
            if (!int.TryParse(txtboxAge.Text, out int res))
            {
                errors.Add("Неверный формат возраста");
            }
            if (!int.TryParse(txtboxPhone.Text, out int res1))
            {
                errors.Add("Неверный формат телефона");
            }
            return errors;
        }
    }
}