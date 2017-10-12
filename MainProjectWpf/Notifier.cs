using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MyNamespace
{
    public class Notifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    [Serializable]
    public class BlockItem : Notifier
    {
        private string _position;

        public string Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }

        private string _date;

        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        private string _speaker;

        public string Speaker
        {
            get { return _speaker; }
            set
            {
                _speaker = value;
                OnPropertyChanged("Speaker");
            }
        }

        private string _speakerRole;

        public string SpeakerRole
        {
            get { return _speakerRole; }
            set
            {
                _speakerRole = value;
                OnPropertyChanged("SpeakerRole");
            }
        }

        private string _note;

        public string Note
        {
            get { return _note; }
            set
            {
                _note = value;
                OnPropertyChanged("Note");
            }
        }

        private string _refName;

        public string RefName
        {
            get { return _refName; }
            set
            {
                _refName = value;
                OnPropertyChanged("RefName");
            }
        }

        private string _link;

        public string Link
        {
            get { return _link; }
            set
            {
                _link = value;
                OnPropertyChanged("Link");
            }
        }

        public override string ToString()
        {
            //Your special code here
            return base.ToString();
        }

        public void Serialize(string path)
        {
            // your code here
        }

        public static BlockItem Deserialize()
        {
            //your code
            return null;
        }

    }
}
