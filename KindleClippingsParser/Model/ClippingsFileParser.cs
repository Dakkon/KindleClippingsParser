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
        private List<string> m_ListOfAllAuthors;
        private List<string> m_ListOfAllTitles;

        #endregion Private fields
        #region Properties

        public List<Clipping> ListOfMyClippings
        {
            get
            {
                return m_ListOfMyClippings;
            }
        }

        public List<string> ListOfAllAuthors
        {
            get
            {
                return m_ListOfAllAuthors;
            }
        }

        public List<string> ListOfAllTitles
        {
            get
            {
                return m_ListOfAllTitles;
            }
        }

        #endregion Properties
        #region Ctors

        public ClippingsFileParser(string pathToClippingsFile)
        {
            m_PathToClippingsFile = pathToClippingsFile;
            m_ListOfMyClippings = new List<Clipping>();

            m_ListOfAllAuthors = new List<string>();
            m_ListOfAllTitles = new List<string>();

            ParseMyClippingsFileToListOfClippingObjects();
        }

        #endregion Ctors
        #region Private methods

        private void ParseMyClippingsFileToListOfClippingObjects()
        {
            using (StreamReader sr = new StreamReader(m_PathToClippingsFile))
            {
                string currentLine = sr.ReadLine();

                while (!sr.EndOfStream && !string.IsNullOrEmpty(currentLine))
                {
                    string title = string.Empty;
                    string author = string.Empty;
                    SeparateTitleFromAuthor(currentLine, out title, out author);

                    currentLine = sr.ReadLine();
                    string ts = TryExtractTimeStampFromLineOfText(currentLine);

                    currentLine = sr.ReadLine(); //skip empty line
                    if (!string.IsNullOrEmpty(currentLine))
                    {
                        throw (new Exception(string.Format("{0} Empty line after timestamp was not found. {1}{2} {3}",
                            UNEXPECTED_FILE_STRUCTURE, Environment.NewLine, CURRENT_LINE_TEXT, currentLine)));
                    }

                    currentLine = sr.ReadLine(); //read clipping text

                    m_ListOfMyClippings.Add(new Clipping(title, author, ts, currentLine));

                    currentLine = sr.ReadLine(); //skip clippings separator
                    if (!string.Equals(currentLine, CLIPPINGS_SEPARATOR))
                    {
                        throw (new Exception(string.Format("{0} Clippings separator was not found. {1}{2} {3}",
                            UNEXPECTED_FILE_STRUCTURE, Environment.NewLine, CURRENT_LINE_TEXT, currentLine)));
                    }

                    currentLine = sr.ReadLine(); //read next line
                }
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

        private void UpdateListOfAllAuthors()
        {
            foreach (Clipping currentClipping in m_ListOfMyClippings)
            {                
                m_ListOfAllAuthors.Add(currentClipping.Author);                
            }

            m_ListOfAllAuthors = m_ListOfAllAuthors.Distinct().ToList();
            m_ListOfAllAuthors.Sort();
        }

        private void UpdateListOfAllTitles()
        {
            foreach (Clipping currentClipping in m_ListOfMyClippings)
            {
                if (!string.IsNullOrEmpty(currentClipping.Title))
                {
                    m_ListOfAllTitles.Add(currentClipping.Title);
                }
            }

            m_ListOfAllTitles = m_ListOfAllTitles.Distinct().ToList();
            m_ListOfAllTitles.Sort();
        }

        private void PopulateListsOfAllAuthorsAndTitles()
        {
            foreach (Clipping currentClipping in m_ListOfMyClippings)
            {
                if (!string.IsNullOrEmpty(currentClipping.Title))
                {
                    m_ListOfAllTitles.Add(currentClipping.Title);
                }
                                
                m_ListOfAllAuthors.Add(currentClipping.Author);                
            }

            m_ListOfAllAuthors = m_ListOfAllAuthors.Distinct().ToList();
            m_ListOfAllAuthors.Sort();
            m_ListOfAllTitles = m_ListOfAllTitles.Distinct().ToList();
            m_ListOfAllTitles.Sort();
        }

        #endregion Private methods
        #region Public methods

        public bool IsThereAtLeastOneClippingForAuthorEnabled(string author)
        {
            return m_ListOfMyClippings.Exists(clipping =>
                {
                    return (string.Equals(clipping.Author, author) && clipping.IsEnabled);
                });
        }

        public List<string> GetListOfAllTitlesForAuthor(string author)
        {
            List<string> listOfAllTitlesForAuthor = new List<string>();
                        
            listOfAllTitlesForAuthor.AddRange(from clipping in m_ListOfMyClippings
                                                where string.Equals(clipping.Author, author)
                                                select clipping.Title);            

            return listOfAllTitlesForAuthor.Distinct().ToList();
        }

        public List<string> GetListOfAllAuthorsForTitle(string title)
        {
            List<string> listOfAllAuthorsForTitle = new List<string>();

            listOfAllAuthorsForTitle.AddRange(from clipping in m_ListOfMyClippings
                                              where string.Equals(clipping.Title, title)
                                              select clipping.Author);

            return listOfAllAuthorsForTitle;
        }

        public List<Clipping> GetSublistOfClippingsForAuthorAndTitle(string author, string title)
        {
            List<Clipping> SublistOfClippingsForAuthorAndTitle = new List<Clipping>();

            var ClippingsForAuthorAndTitle = from clipping in m_ListOfMyClippings
                                             where string.Equals(clipping.Author, author) && string.Equals(clipping.Title, title)
                                             select clipping;

            SublistOfClippingsForAuthorAndTitle.AddRange(ClippingsForAuthorAndTitle.ToList());

            return SublistOfClippingsForAuthorAndTitle;
        }

        public void ToggleIsEnabledForAllClippings(bool state)
        {
            foreach (Clipping clipping in m_ListOfMyClippings)
            {
                clipping.IsEnabled = state;
            }
        }

        public void ToggleIsEnabledForAllClippingsOfSingleAuthor(string author, bool state)
        {
            m_ListOfMyClippings.FindAll(clipping =>
                {
                    if (string.Equals(clipping.Author, author))
                    {
                        clipping.IsEnabled = state;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
        }

        public void ToggleIsEnabledForAllClippingsOfSingleTitle(string title, bool state)
        {
            m_ListOfMyClippings.FindAll(clipping =>
            {
                if (string.Equals(clipping.Title, title))
                {
                    clipping.IsEnabled = state;
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        #endregion Public methods
    }
}
