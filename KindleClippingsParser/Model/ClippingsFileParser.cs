using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
        private List<ClippingsHeader> m_ListOfHeaders;

        private List<string> m_ListOfAllAuthors;
        private List<string> m_ListOfAllEnabledAuthors;
        private List<string> m_ListOfAllTitles;
        private List<string> m_ListOfAllEnabledTitles;
        private string m_OriginalMyClippingsFileText;

        #endregion Private fields
        #region Properties

        public List<Clipping> ListOfMyClippings
        {
            get
            {
                return m_ListOfMyClippings;
            }
        }

        public List<ClippingsHeader> ListOfHeaders
        {
            get
            {
                return m_ListOfHeaders;
            }
        }

        public List<string> ListOfAllAuthors
        {
            get
            {
                return m_ListOfAllAuthors;
            }
        }

        public List<string> ListOfAllEnabledAuthors
        {
            get
            {
                return m_ListOfAllEnabledAuthors;
            }
        }

        public List<string> ListOfAllTitles
        {
            get
            {
                return m_ListOfAllTitles;
            }
        }

        //public List<string> ListOfAllEnabledTitles
        //{
        //    get
        //    {
        //        return m_ListOfAllEnabledTitles;
        //    }
        //}

        public string OriginalMyClippingsFileText
        {
            get
            {
                return m_OriginalMyClippingsFileText;
            }
        }

        #endregion Properties
        #region Ctors

        public ClippingsFileParser(string pathToClippingsFile)
        {
            m_PathToClippingsFile = pathToClippingsFile;
            m_ListOfMyClippings = new List<Clipping>();
            m_ListOfHeaders = new List<ClippingsHeader>();

            m_ListOfAllAuthors = new List<string>();
            m_ListOfAllEnabledAuthors = new List<string>();
            m_ListOfAllTitles = new List<string>();
            m_ListOfAllEnabledTitles = new List<string>();

            ParseMyClippingsFile();
        }

        #endregion Ctors
        #region Private methods

        private void ParseMyClippingsFile()
        {
            using (StreamReader sr = new StreamReader(m_PathToClippingsFile))
            {
                StringBuilder stringBuilder = new StringBuilder();

                string currentLine = sr.ReadLine();
                stringBuilder.AppendLine(currentLine);

                while (!sr.EndOfStream && !string.IsNullOrEmpty(currentLine))
                {
                    string title = string.Empty;
                    string author = string.Empty;
                    SeparateTitleFromAuthor(currentLine, out title, out author);

                    currentLine = sr.ReadLine();
                    stringBuilder.AppendLine(currentLine);
                    string ts = TryExtractTimeStampFromLineOfText(currentLine);

                    currentLine = sr.ReadLine(); //skip empty line
                    stringBuilder.AppendLine(currentLine);
                    if (!string.IsNullOrEmpty(currentLine))
                    {
                        throw (new Exception(string.Format("{0} Empty line after timestamp was not found. {1}{2} {3}",
                            UNEXPECTED_FILE_STRUCTURE, Environment.NewLine, CURRENT_LINE_TEXT, currentLine)));
                    }

                    currentLine = sr.ReadLine(); //read clipping text
                    stringBuilder.AppendLine(currentLine);

                    Clipping newClipping = new Clipping(title, author, ts, currentLine);
                    m_ListOfMyClippings.Add(newClipping);

                    UpdateListOfHeader(newClipping);                   

                    currentLine = sr.ReadLine(); //skip clippings separator
                    stringBuilder.AppendLine(currentLine);
                    if (!string.Equals(currentLine, CLIPPINGS_SEPARATOR))
                    {
                        throw (new Exception(string.Format("{0} Clippings separator was not found. {1}{2} {3}",
                            UNEXPECTED_FILE_STRUCTURE, Environment.NewLine, CURRENT_LINE_TEXT, currentLine)));
                    }

                    currentLine = sr.ReadLine(); //read next line     
                    stringBuilder.AppendLine(currentLine);
                }

                m_OriginalMyClippingsFileText = stringBuilder.ToString();
            }

            PopulateListsOfAllAuthorsAndTitles();
        }

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

                author = RemoveParenthesisFromText(author);
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
                author = "<unknown>";
                title = lineOfText;
            }
        }

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

        private string RemoveParenthesisFromText(string text)
        {
            text = text.Replace("(", "");
            text = text.Replace(")", "");

            return text;
        }

        private ClippingsHeader FindClippingsHeader(string author, string title)
        {
            return m_ListOfHeaders.Find(header =>
                {
                    return (string.Equals(header.Author, author) && string.Equals(header.Title, title));                   
                });
        }

        private List<ClippingsHeader> FindClippingsHeadersForTitle(string title)
        {
            List<ClippingsHeader> headersForTitle = m_ListOfHeaders.FindAll(x =>
                {
                    if (x.Title == title)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });

            return headersForTitle;
        }

        private void UpdateListOfHeader(Clipping clipping)
        {
            ClippingsHeader header = FindClippingsHeader(clipping.Author, clipping.Title);

            if(header == null)
            {
                header = new ClippingsHeader(clipping.Author, clipping.Title);
                m_ListOfHeaders.Add(header);
            }

            header.ListOfClippings.Add(clipping);
        }
        
        private void UpdateListOfAllEnabledAuthors(string author, bool isAdded)
        {
            //TODO: Used only in ToggleIsEnabledForAllClippingsOfSingleAuthor() 
            //Remove this method when ToggleAll() is ready 

            if (isAdded)
            {
                m_ListOfAllEnabledAuthors.Add(author);
                m_ListOfAllEnabledAuthors = m_ListOfAllEnabledAuthors.Distinct().ToList();
            }
            else
            {
                m_ListOfAllEnabledAuthors = m_ListOfAllEnabledAuthors.Where(x => x != author).ToList();
            }
        }        

        private void PopulateListsOfAllAuthorsAndTitles()
        {
            foreach (ClippingsHeader currentHeader in m_ListOfHeaders)
            {
                if (!string.IsNullOrEmpty(currentHeader.Title))
                {
                    m_ListOfAllTitles.Add(currentHeader.Title);

                    if (currentHeader.IsEnabled)
                    {
                        m_ListOfAllEnabledTitles.Add(currentHeader.Title);
                    }
                }

                m_ListOfAllAuthors.Add(currentHeader.Author);
                m_ListOfAllEnabledAuthors.Add(currentHeader.Author);
            }

            m_ListOfAllAuthors = m_ListOfAllAuthors.Distinct().ToList();
            m_ListOfAllAuthors.Sort();

            m_ListOfAllEnabledAuthors = m_ListOfAllEnabledAuthors.Distinct().ToList();
            m_ListOfAllEnabledAuthors.Sort();

            m_ListOfAllTitles = m_ListOfAllTitles.Distinct().ToList();
            m_ListOfAllTitles.Sort();

            m_ListOfAllEnabledTitles = m_ListOfAllEnabledTitles.Distinct().ToList();
            m_ListOfAllEnabledTitles.Sort();
        }

        private void UpdateListOfAllEnabledTitles(string title, bool isAdded)
        {
            //TODO: Used only in ToggleIsEnabledForAllClippingsOfSingleTitle()
            //Remove this method when ToggleAll() is ready
            if (isAdded)
            {
                m_ListOfAllEnabledTitles.Add(title);
                m_ListOfAllEnabledTitles = m_ListOfAllEnabledTitles.Distinct().ToList();
            }
            else
            {
                m_ListOfAllEnabledTitles = m_ListOfAllEnabledAuthors.Where(x => x != title).ToList();
            }
        }

        private bool IsThereAtLeastOneHeaderForAuthorEnabled(string author)
        {
            return m_ListOfHeaders.Exists(header =>
            {
                return (string.Equals(header.Author, author) && header.IsEnabled);
            });
        }

        #endregion Private methods
        #region Public methods

        public List<ClippingsHeader> ToggleSingleAuthor(string author, bool isEnabled)
        {
            List<ClippingsHeader> toggledHeaders = m_ListOfHeaders.FindAll(header =>
                {
                    if (string.Equals(header.Author, author))
                    {
                        header.IsEnabled = isEnabled;
                        return true;                        
                    }
                    else
                    {
                        return false;
                    }
                });

            return toggledHeaders;
        }

        public List<string> ToggleSingleTitle(string title, bool isTitleEnabled)
        {
            List<string> toggledAuthors = new List<string>();

            foreach (ClippingsHeader header in FindClippingsHeadersForTitle(title))
            {
                if (isTitleEnabled && !IsThereAtLeastOneHeaderForAuthorEnabled(header.Author))
                {
                    toggledAuthors.Add(header.Author); //author turned on
                }

                header.IsEnabled = isTitleEnabled;

                if (!IsThereAtLeastOneHeaderForAuthorEnabled(header.Author))
                {
                    toggledAuthors.Add(header.Author); //author turned off
                }
            }

            return toggledAuthors;
        }

        #endregion Public methods
    }
}