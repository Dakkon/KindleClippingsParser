using System;
using System.Collections.Generic;

namespace KindleClippingsParser.Model
{
    class ClippingsFileParser
    {
        #region Private fields

        private string m_PathToClippingsFile;
        private List<Clipping> m_ListOfMyClippings;        
        
        #endregion Private fields
        #region Properties

        /// <summary>
        /// List of clipping objects generated from MyClippings.txt file
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
        /// <param name="pathToClippingsFile">Full path to MyClippings.txt file (folders + file name)</param>
        public ClippingsFileParser(string pathToClippingsFile)
        {
            this.m_PathToClippingsFile = pathToClippingsFile;
        }

        #endregion Ctors
        #region Public methods

        /// <summary>
        /// Gets list of all titles found in MyClippings.txt (without duplicates)
        /// </summary>
        /// <returns>List of clipping titles</returns>
        public List<string> GetListOfTitles()
        {
            throw (new NotImplementedException());
        }

        /// <summary>
        /// Gets list of all authors found in MyClippings.txt (without duplicates)
        /// </summary>
        /// <returns>List of clipping authors</returns>
        public List<string> GetListOfAuthors()
        {
            throw (new NotImplementedException());
        }

        /// <summary>
        /// Filters list of clippings by title (only clippings with given title will be shown)
        /// </summary>
        /// <param name="title">Title to be found</param>
        public void FilterListByTitle(string title)
        {
            throw (new NotImplementedException());
        }

        /// <summary>
        /// Filters list of clippings by author (only clippings with given author will be shown)
        /// </summary>
        /// <param name="author">Author to be found</param>
        public void FilterListByAuthor(string author)
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
