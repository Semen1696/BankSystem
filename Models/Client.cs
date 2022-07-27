using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Models
{
    /// <summary>
    /// Модель клиента банка
    /// </summary>
    public class Client : BaseEntity, INotifyPropertyChanged
    {
        private string name;
        private string surname;
        /// <summary>
        /// Имя клиента
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        /// <summary>
        /// Фамилия клиента
        /// </summary>
        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged("Surname");
            }
        }
        /// <summary>
        /// Возраст клиента
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// Номер телефона клиента
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Тип клиента
        /// </summary>
        public int ClientTypeId { get; set; }
        [JsonIgnore]
        public ClientType ClientType 
        {
            get { return (ClientType)this.ClientTypeId; }
            set { this.ClientTypeId = (int)value; }
        }
        /// <summary>
        /// Событие для динамеческого обновления данных в таблице
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
