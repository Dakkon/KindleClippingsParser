using System;
using System.ComponentModel;

namespace KindleClippingsReader.Model
{
    public class Clipping : INotifyPropertyChanged
    {
        #region Private fields

        private string m_Title;
        private string m_Author;
        private DateTime m_Timestamp;
        private string m_Text;

        bool m_IsEnabled;

        #endregion Private fields
        #region Properties

        public string Title
        {
            get
            {
                return m_Title;
            }    
            set
            {
                m_Title = value;
            }
        }

        public string Author
        {
            get
            {
                return m_Author;
            }         
            set
            {
                m_Author = value;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return m_Timestamp;
            }
            set
            {
                m_Timestamp = value;
            }
        }

        public string Text
        {
            get
            {
                return m_Text;
            }
            set
            {
                m_Text = value;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return m_IsEnabled;
            }

            set
            {
                m_IsEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        #endregion Properties
        #region Ctors

        public Clipping()
        {
            IsEnabled = true;
        }

        public Clipping(string title, string author, string ts, string text)
            : this()
        {
            DateTime tsTemp;

            Title = title;
            Author = author;
            
            if (DateTime.TryParse(ts, out tsTemp))
            {
                Timestamp = tsTemp;
            }

            Text = text;
        }

        #endregion Ctors
        #region Private methods

        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion Private methods       
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events
    }
}