using System;

namespace KindleClippingsParser.Model
{
    public class Clipping
    {
        #region Private fields

        private string m_Title;
        private string m_Author;
        private DateTime m_Timestamp;
        private string m_Text;

        bool m_IsEnabled;

        #endregion Private fields
        #region Properties

        /// <summary>
        /// Clipping source title (book title, article title etc.)
        /// </summary>
        public string Title
        {
            get
            {
                return this.m_Title;
            }
        }

        /// <summary>
        /// Clipping author
        /// </summary>
        public string Author
        {
            get
            {
                return this.m_Author;
            }
        }

        /// <summary>
        /// Time, when the clipping was taken
        /// </summary>
        public DateTime Timestamp
        {
            get
            {
                return this.m_Timestamp;
            }
        }

        /// <summary>
        /// Clipping text
        /// </summary>
        public string Text
        {
            get
            {
                return this.m_Text;
            }
        }

        /// <summary>
        /// Flag which determine whether clipping:
        /// - is visible to the user (used e.g. for filtering)
        /// - will be saved when user decides to update his clipping file
        /// Default value is true
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return this.m_IsEnabled;
            }

            set
            {
                this.m_IsEnabled = value;
            }
        }

        #endregion Properties
        #region Ctors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Clipping()
        {
            this.IsEnabled = true;
        }

        /// <summary>
        /// Constructor which initilalizes all clipping's properties
        /// </summary>
        /// <param name="title">Clipping source title (book title, article title etc.)</param>
        /// <param name="author">Clipping author</param>
        /// <param name="timeStamp">Time, when the clipping was taken</param>
        /// <param name="text">Clipping text</param>
        public Clipping(string title, string author, string ts, string text) 
            : this()
        {
            this.SetTitle(title);
            this.SetAuthor(author);
            this.SetTimeStamp(ts);
            this.SetText(text);            
        }

        #endregion Ctors
        #region Public methods

        /// <summary>
        /// Sets clipping title
        /// </summary>
        /// <param name="title">Clipping source title (book title, article title etc.)</param>
        public void SetTitle(string title)
        {
            this.m_Title = title;
        }

        /// <summary>
        /// Sets clipping author
        /// </summary>
        /// <param name="author">Clipping author</param>
        public void SetAuthor(string author)
        {
            this.m_Author = author;
        }

        /// <summary>
        /// Sets clipping timestamp
        /// </summary>
        /// <param name="timeStamp">Time, when the clipping was taken</param>
        public void SetTimeStamp(string ts)
        {
            this.m_Timestamp = DateTime.Parse(ts);
        }

        /// <summary>
        /// Sets clipping text
        /// </summary>
        /// <param name="text">Clipping text</param>
        public void SetText(string text)
        {
            this.m_Text = text;
        }

        #endregion Public methods
    }
}
