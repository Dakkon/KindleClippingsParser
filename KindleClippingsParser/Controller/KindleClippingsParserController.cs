using KindleClippingsParser.Model;
using System;
using System.Windows.Controls;

namespace KindleClippingsParser.Controller
{
    public class KindleClippingsParserController
    {
        #region Private fields

        private ClippingsFileParser m_ClippingsFileParserInstance;

        #endregion Private fields
        #region Ctors

        public KindleClippingsParserController()
        {
            this.test_CreateClippingsFileParserInstance();
        }

        #endregion Ctors
        #region Public methods

        public void DisplayClippings(TextBox textBox)
        {
            foreach (Clipping clipping in m_ClippingsFileParserInstance.ListOfMyClippings)
            {
                textBox.AppendText(clipping.Author);
                textBox.AppendText(Environment.NewLine);
                textBox.AppendText(clipping.Title);
                textBox.AppendText(Environment.NewLine);
                textBox.AppendText(clipping.Text);
                textBox.AppendText(Environment.NewLine);
                textBox.AppendText(Environment.NewLine);
            }                                  
        }

        #endregion Public methods


        #region FOR TESTING PURPOSES

        private void test_CreateClippingsFileParserInstance()
        {
            this.m_ClippingsFileParserInstance = new ClippingsFileParser("C:\\My Clippings.txt");
        }       

        #endregion FOR TESTING PURPOSES
    }
}
