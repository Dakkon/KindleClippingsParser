using System;
using System.ComponentModel;

namespace KindleClippingsParser.Model
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
                return this.m_Title;
            }
        }

        public string Author
        {
            get
            {
                return this.m_Author;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return this.m_Timestamp;
            }
        }

        public string Text
        {
            get
            {
                return this.m_Text;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return this.m_IsEnabled;
            }

            set
            {
                this.m_IsEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        #endregion Properties
        #region Ctors

        public Clipping()
        {
            this.IsEnabled = true;
        }

        public Clipping(string title, string author, string ts, string text)
            : this()
        {
            this.SetTitle(title);
            this.SetAuthor(author);
            this.SetTimeStamp(ts);
            this.SetText(text);
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
        #region Public methods

        public void SetTitle(string title)
        {
            this.m_Title = title;
        }

        public void SetAuthor(string author)
        {
            this.m_Author = author;
        }

        public void SetTimeStamp(string ts)
        {
            this.m_Timestamp = DateTime.Parse(ts);
        }

        public void SetText(string text)
        {
            this.m_Text = text;
        }

        #endregion Public methods
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events
    }
}