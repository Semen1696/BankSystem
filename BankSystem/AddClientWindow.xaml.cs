using Common;
using Logic;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


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

            try
            {
                CheckInput();
            }
            catch(ValidateException ex)
            {
                MessageBox.Show(ex.Message, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void CheckInput()
        {
            List<string> errors = new List<string>();
            if (!txtboxName.Text.Any(c => char.IsLetter(c)))
            {
                throw new ValidateException("Неверный формат имени");
            }
            if (!txtboxSurname.Text.Any(c => char.IsLetter(c)))
            {
                throw new ValidateException("Неверный формат фамилии");
            }
            if (!int.TryParse(txtboxAge.Text, out int res))
            {
                throw new ValidateException("Неверный формат возраста");
            }
            if (!int.TryParse(txtboxPhone.Text, out int res1))
            {
                throw new ValidateException("Неверный формат телефона");
            }
        }
    }
}
