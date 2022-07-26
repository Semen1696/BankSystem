using BankSystem.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;

namespace BankSystem.Repos
{
    /// <summary>
    /// CRUD операции сданными
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> where T : BaseEntity
    {
        private readonly string _path;
        private readonly ObservableCollection<T> _items;
        private Index index;

        public Repository(string path)
        {
            _path = path;
            _items = GetAllData();
            index = new Index();
        }
        /// <summary>
        /// Добавляет элемент в общую коллекцию
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(T item)
        {            
            _items.Add(item);
            WriteDataToFile();
        }
        /// <summary>
        /// Удалить элемент из коллекции
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(T item)
        {
            _items.Remove(item);
            WriteDataToFile();
    
        }

        /// <summary>
        /// Получить все элементы коллекции
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<T> GetItems()
        {
            return _items;
        }

        /// <summary>
        /// Сохранить элменты коллекции в файл
        /// </summary>
        public void SaveItem()
        {
            WriteDataToFile();
        }
  
        /// <summary>
        /// Получить все данные из файла
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<T> GetAllData()
        {
            string json = CreateOrRead(_path);
            ObservableCollection<T> items = JsonConvert.DeserializeObject<ObservableCollection<T>>(json);
            if (items == null)
                return new ObservableCollection<T>();
            return items;
        }

        /// <summary>
        /// Получение последнего значения id 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int LastId(T type)
        {
            string json = File.ReadAllText("Index.json");
            int Id = 0;
            Index ind = JsonConvert.DeserializeObject<Index>(json);
            System.Type t = type.GetType();
            switch (t.Name)
            {
                case "Client":
                    ind.ClientId = ind.ClientId + 1;
                    Id = ind.ClientId;
                    break;
                case "BankAccount":
                    ind.AccountId = ind.AccountId + 1;
                    Id = ind.AccountId;
                    break;
            }
            File.WriteAllText("Index.json", JsonConvert.SerializeObject(ind));
            return Id;

        }
        
        /// <summary>
        /// запись данных в файл
        /// </summary>
        private void WriteDataToFile()
        {
            string json = JsonConvert.SerializeObject(_items, Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(_path, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(json);
            }
        }

        /// <summary>
        /// Чтение данных из файла 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string CreateOrRead(string path)
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

    }
}
