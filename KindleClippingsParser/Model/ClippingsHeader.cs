using System.Collections.Generic;
using System.ComponentModel;

namespace KindleClippingsParser.Model
{
    public class ClippingsHeader : INotifyPropertyChanged
    {
        #region Private fields

        private string m_Title;
        private string m_Author;        

        bool m_IsEnabled;

        private List<Clipping> m_ListOfClippings;

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

        public string HeaderText
        {
            get
            {
                string HeaderTextTemplate = "{0} - \"{1}\"";
                return string.Format(HeaderTextTemplate, m_Author, m_Title);
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

                if (m_ListOfClippings != null)
                {
                    foreach (Clipping clipping in m_ListOfClippings)
                    {
                        clipping.IsEnabled = value;
                    }
                }

                OnPropertyChanged("IsEnabled");
            }
        }

        public List<Clipping> ListOfClippings
        {
            get
            {
                return m_ListOfClippings;
            }
        }

        #endregion Properties
        #region Ctors

        public ClippingsHeader()
        {
            m_IsEnabled = true;
        }

        public ClippingsHeader(string author, string title)
            : this()
        {
            Title = title;
            Author = author;

            m_ListOfClippings = new List<Clipping>();
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
