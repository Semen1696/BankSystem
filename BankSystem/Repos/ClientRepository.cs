using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Repos
{
    /// <summary>
    /// Логика для работы с клиентом
    /// </summary>
    public class ClientRepository
    {
        private readonly Repository<Client> repository;
        private readonly Account bankRepository;
        public delegate void ClientHandler(string message);
        public event ClientHandler Notify;

        public ClientRepository(string clientsPath)
        {
            repository = new Repository<Client>(clientsPath);
            bankRepository = new Account();
        }
        /// <summary>
        /// Добавить нового клиента
        /// </summary>
        /// <param name="client"></param>
        public void AddClient(Client client)
        {
            client.Id = repository.LastId(client);
            repository.AddItem(client);
            Notify?.Invoke($"Добавлен пользователь {client.Name} {client.Surname}");
        }
        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClient(Client client)
        {
            var accounts = bankRepository.GetAccountsByClientId(client.Id);
            foreach (var account in accounts)
            {
                bankRepository.RemoveAccount(account);
            }
            repository.RemoveItem(client);
            Notify?.Invoke($"Пользователь {client.Name} {client.Surname} удален");
        }
        /// <summary>
        /// Получить клиента по уникальному номеру
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Client GetClientById(int id)
        {
            ObservableCollection<Client> clients = GetAllClients();
            Client client = clients.FirstOrDefault(c => c.Id == id);
            return client;
        }

        /// <summary>
        /// Получить всех клиентов
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Client> GetAllClients()
        {
            return repository.GetItems();
        }
        /// <summary>
        /// сохранить изменения
        /// </summary>
        public void SaveChanges()
        {
            repository.SaveItem();
            Notify?.Invoke("Данные пользователя изменены");
        }
        
    }
}
