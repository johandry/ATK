using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using CsvHelper.Configuration;

namespace ATK
{
    class Command : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        private string _itemName;

        [CsvField(Index = 3, Name = "Name")]
        public String ItemName
        {
            get { return _itemName; }
            set { _itemName = value; NotifyPropertyChanged("ItemName"); }
        }

        private string _itemCommand;

        [CsvField(Index = 0, Name = "Client Command")]
        public String ItemCommand
        {
            get { return _itemCommand; }
            set { _itemCommand = value; NotifyPropertyChanged("ItemCommand"); }
        }

        private string _itemDescription;

        [CsvField(Index = 4, Name = "Description")]
        public String ItemDescription
        {
            get { return _itemDescription; }
            set { _itemDescription = value; NotifyPropertyChanged("ItemDescription"); }
        }
    }
}
