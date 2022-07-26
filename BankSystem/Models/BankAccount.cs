using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    /// <summary>
    /// Модель банковского счета
    /// </summary>
    public class BankAccount : BaseEntity, INotifyPropertyChanged
    {
        private decimal amount;
        /// <summary>
        /// Уникальный номер клиента
        /// </summary>
        public int ClientId { get; set; }
        /// <summary>
        /// Номер счета
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Сумма на счету
        /// </summary>
        public decimal Amount 
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }
        /// <summary>
        /// Дата создания счета
        /// </summary>
        public DateTime Createdate { get; set; }
        /// <summary>
        /// Тип счета
        /// </summary>
        public AccountType AccountType { get; set; }
        /// <summary>
        /// Событие для динамеческого обновления данных в таблице
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

       

        /// <summary>
        /// привязка к событию
        /// </summary>
        /// <param name="prop"></param>
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
