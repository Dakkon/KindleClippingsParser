using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KindleClippingsParser.Model
{
    public class ClippingsFileParser
    {
        #region Constatnts
                
        const string CLIPPINGS_SEPARATOR = "==========";
        const string UNEXPECTED_FILE_STRUCTURE = "Unexpected file structure.";
        const string CURRENT_LINE_TEXT = "Current line text: ";
        const string UNEXPECTED_LINE_FORMAT = "Unexpected line format.";

        #endregion Constants
        #region Private fields

        private string m_PathToClippingsFile;
        private List<Clipping> m_ListOfMyClippings;
        private List<string> m_ListOfAllTitles;
        private List<string> m_ListOfAllAuthors;
        
        #endregion Private fields
        #region Properties

        /// <summary>
        /// List of clipping objects generated from My Clippings.txt file
        /// </summary>
        public List<Clipping> ListOfMyClippings
        {
            get
            {
                return this.m_ListOfMyClippings;
            }
        }

        #endregion Properties
        #region Ctors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="pathToClippingsFile">Full path to My Clippings.txt file (folders + file name)</param>
        public ClippingsFileParser(string pathToClippingsFile)
        {
            this.m_PathToClippingsFile = pathToClippingsFile;
            this.m_ListOfMyClippings = new List<Clipping>();

            this.m_ListOfAllTitles = new List<string>();
            this.m_ListOfAllAuthors = new List<string>();

            this.ParseMyClippingsFileToListOfClippingObject();
        }

        #endregion Ctors
        #region Private methods

        /// <summary>
        /// Opens, reads and converts My Clipping.txt file into the list of Clipping objects
        /// </summary>
        private void ParseMyClippingsFileToListOfClippingObject()
        {
            using (StreamReader sr = new StreamReader(this.m_PathToClippingsFile))
            {
                string currentLine = sr.ReadLine();

                while (!sr.EndOfStream && !string.IsNullOrEmpty(currentLine))
                {                    
                    string title = string.Empty;
                    string author = string.Empty;
                    this.SeparateTitleFromAuthor(currentLine, out title, out author);

                    currentLine = sr.ReadLine();
                    string ts = this.TryExtractTimeStampFromLineOfText(currentLine);

                    currentLine = sr.ReadLine(); //skip empty line
                    if (!string.IsNullOrEmpty(currentLine))
                    {
                        throw (new Exception(string.Format("{0} Empty line after timestamp was not found. {1}{2} {3}",
                            UNEXPECTED_FILE_STRUCTURE, Environment.NewLine, CURRENT_LINE_TEXT, currentLine)));
                    }

                    currentLine = sr.ReadLine(); //read clipping text

                    this.m_ListOfMyClippings.Add(new Clipping(title, author, ts, currentLine));

                    currentLine = sr.ReadLine(); //skip clippings separator
                    if (!string.Equals(currentLine, CLIPPINGS_SEPARATOR))
                    {
                        throw (new Exception(string.Format("{0} Clippings separator was not found. {1}{2} {3}",
                            UNEXPECTED_FILE_STRUCTURE, Environment.NewLine, CURRENT_LINE_TEXT, currentLine)));
                    }

                    currentLine = sr.ReadLine(); //read next line
                }
            }

            this.UpdateListsOfAllAuthorsAndTitles();
        }

        /// <summary>
        /// Attempts to extract clipping title (book title, article title etc.) and author from line of text
        /// </summary>
        /// <param name="lineOfText">First line of text from single clipping block in My Clippings.txt</param>
        /// <param name="title">Clipping source title (book title, article title etc.</param>
        /// <param name="author">Clipping author</param>
        private void SeparateTitleFromAuthor(string lineOfText, out string title, out string author)
        {            
            string[] splittedLine = lineOfText.Split(' ');
              
            int wordPosition = splittedLine.Length;

            if (splittedLine[wordPosition - 1].Contains(")"))
            {
                author = string.Empty;
                title = string.Empty;

                //Extract author
                do
                {
                    wordPosition--;

                    if (wordPosition < 0)
                    {
                        throw (new Exception(string.Format("{0} Unable to separate title from author. {1}{2} {3}",
                            UNEXPECTED_LINE_FORMAT, Environment.NewLine, CURRENT_LINE_TEXT, lineOfText)));
                    }

                    author = string.Format(" {0}{1}", splittedLine[wordPosition], author);
                }
                while (!splittedLine[wordPosition].Contains("("));

                author = this.RemoveParenthesisFromText(author);
                author = author.TrimStart();

                //Extract title
                while (wordPosition > 0)
                {
                    wordPosition--;
                    title = string.Format(" {0}{1}", splittedLine[wordPosition], title);
                }
                title = title.TrimStart();
            }
            else //when there's no author in parenthesis
            {
                author = string.Empty;
                title = lineOfText;
            }
        }

        /// <summary>
        /// Attempts to extract clipping timestamp from line of text
        /// </summary>
        /// <param name="lineOfText">Second line of text from single clipping block in My Clippings.txt</param>
        /// <returns>Clipping timestamp</returns>
        private string TryExtractTimeStampFromLineOfText(string lineOfText)
        {
            string[] splittedLineOfText = (lineOfText.Split(','));

            if (splittedLineOfText.Length < 4)
            {
                throw (new Exception(string.Format("{0} Unable to extract time stamp. {1}{2} {3}",
                    UNEXPECTED_LINE_FORMAT, Environment.NewLine, CURRENT_LINE_TEXT, lineOfText)));
            }

            return string.Format("{0} {1} {2}", splittedLineOfText[1].TrimStart(), splittedLineOfText[2].TrimStart(), 
                splittedLineOfText[3].TrimStart());
        }

        /// <summary>
        /// Removes parenthesis from text
        /// </summary>
        /// <param name="text">Text with parenthesis</param>
        /// <returns>Text without parenthesis</returns>
        private string RemoveParenthesisFromText(string text)
        {
            text = text.Replace("(", "");
            text = text.Replace(")", "");

            return text;
        }

        /// <summary>
        /// Updates private property ListOfAllTitles
        /// </summary>
        private void UpdateListOfAllTitles()
        {
            foreach (Clipping currentClipping in this.m_ListOfMyClippings)
            {
                if (!string.IsNullOrEmpty(currentClipping.Title))
                {
                    this.m_ListOfAllTitles.Add(currentClipping.Title);
                }                
            }

            this.m_ListOfAllTitles = this.m_ListOfAllTitles.Distinct().ToList();
        }

        /// <summary>
        /// Updates private property ListOfAllAuthors
        /// </summary>
        private void UpdateListOfAllAuthors()
        {
            foreach (Clipping currentClipping in this.m_ListOfMyClippings)
            {
                if (!string.IsNullOrEmpty(currentClipping.Author))
                {
                    this.m_ListOfAllAuthors.Add(currentClipping.Author);
                }
            }

            this.m_ListOfAllAuthors = this.m_ListOfAllAuthors.Distinct().ToList();
        }

        /// <summary>
        /// Updates private properties ListOfAllTitles and ListOfAllAuthors
        /// </summary>
        private void UpdateListsOfAllAuthorsAndTitles()
        {
            foreach (Clipping currentClipping in this.m_ListOfMyClippings)
            {
                if (!string.IsNullOrEmpty(currentClipping.Title))
                {
                    this.m_ListOfAllTitles.Add(currentClipping.Title);
                }  

                if (!string.IsNullOrEmpty(currentClipping.Author))
                {
                    this.m_ListOfAllAuthors.Add(currentClipping.Author);
                }                
            }

            this.m_ListOfAllTitles = this.m_ListOfAllTitles.Distinct().ToList();
            this.m_ListOfAllAuthors = this.m_ListOfAllAuthors.Distinct().ToList();
        }
                                
        #endregion Private methods
        #region Public methods

        /// <summary>
        /// Gets list of all titles found in My Clippings.txt (without duplicates)
        /// </summary>
        /// <returns>List of clipping titles</returns>
        public List<string> GetListOfTitles()
        {
            throw (new NotImplementedException());
        }

        /// <summary>
        /// Gets list of all authors found in My Clippings.txt (without duplicates)
        /// </summary>
        /// <returns>List of clipping authors</returns>
        public List<string> GetListOfAuthors()
        {
            throw (new NotImplementedException());
        }

        /// <summary>
        /// Enables/disables clippings with a given title (used for filtering)
        /// </summary>
        /// <param name="title">Title to be found</param>
        public void ToggleClippingsVisibilityByTitle(string title)
        {
            throw (new NotImplementedException());
        }

        /// <summary>
        /// Enables/disables clippings with a given author (used for filtering)
        /// </summary>
        /// <param name="author">Author to be found</param>
        public void ToggleClippingsVisiblityByAuthor(string author)
        {
            throw (new NotImplementedException());
        }

        /// <summary>
        /// Filters list of clippings by timestamp (only clippings with given timestamp will be shown)
        /// </summary>
        /// <param name="ts">Timestamp to be found</param>
        public void FilterListByTimestamp(DateTime ts)
        {
            throw (new NotImplementedException());
        }

        /// <summary>
        /// Filters list of clippings by timestamp
        /// </summary>
        /// <param name="ts">Only older clippings will be shown</param>
        /// <param name="showEqual">Show also clippings with timestamp equal to ts</param>
        public void FilterOlderThan(DateTime ts, bool showEqual)
        {
            throw (new NotImplementedException());
        }

        /// <summary>
        /// Filters list of clippings by timestamp
        /// </summary>
        /// <param name="ts">Only newer clippings will be shown</param>
        /// <param name = "showEqual">Show also clippings with timestamp equal to ts</param>
        public void FilterNewerThan(DateTime ts, bool showEqual)
        {
            throw (new NotImplementedException());
        }

        #endregion Public methods
    }
}
