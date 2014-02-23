using KindleClippingsParser.Model;
using KindleClippingsParser.View;
using System;
using System.Windows;
using System.Windows.Controls;

namespace KindleClippingsParser.Controller
{
    public class KindleClippingsParserController
    {
        #region Private fields

        private MainWindow m_MainWindow;
        private ClippingsFileParser m_ClippingsFileParserInstance;

        #endregion Private fields
        #region Ctors

        public KindleClippingsParserController(MainWindow mainWindow)
        {
            m_MainWindow = mainWindow;
            test_CreateClippingsFileParserInstance();
        }

        #endregion Ctors
        #region Private methods

        private TextBox GetNewTextBoxForAuthorAndTitle(string author, string title)
        {
            const string AUTHOR = "Author: ";
            const string TITLE = "Title: ";

            TextBox newTextBox = new TextBox();
            newTextBox.TextWrapping = TextWrapping.Wrap;
            newTextBox.FontWeight = FontWeights.Bold;
            newTextBox.Margin = new Thickness(0, 10, 0, 10);

            newTextBox.AppendText(AUTHOR);
            newTextBox.AppendText(author);
            newTextBox.AppendText(Environment.NewLine);
            newTextBox.AppendText(TITLE);
            newTextBox.AppendText(title);

            return newTextBox;
        }

        private TextBox GetNewTextBoxForClippingText(string author, string title, string clippingText)
        {
            const string TOOLTIP_TEXT = "Author: {0}{1}Title: {2}";

            TextBox newTextBox = new TextBox();
            newTextBox.TextWrapping = TextWrapping.Wrap;

            newTextBox.ToolTip = string.Format(TOOLTIP_TEXT, author, Environment.NewLine, title);
            newTextBox.AppendText(clippingText);
            return newTextBox;
        }
                
        #endregion Private methods
        #region Public methods

        public void DisplayClippings(StackPanel stackPanel)
        {
            const string UNKNOWN = "uknown";

            //Find titles with correct authors
            foreach (string author in m_ClippingsFileParserInstance.ListOfAllAuthors)
            {
                foreach (string title in m_ClippingsFileParserInstance.GetListOfAllTitlesForAuthor(author))
                {
                    TextBox newTextBox = GetNewTextBoxForAuthorAndTitle(author, title);                                        
                    stackPanel.Children.Add(newTextBox);

                    foreach(Clipping clipping in m_ClippingsFileParserInstance.GetSublistOfClippingsForAuthorAndTitle(author, title))
                    {
                        TextBox newClippingTextBox = GetNewTextBoxForClippingText(author, title, clipping.Text);
                        stackPanel.Children.Add(newClippingTextBox);
                    }
                }
            }

            //Find titles without authors
            foreach (string title in m_ClippingsFileParserInstance.GetListOfAllTitlesForAuthor(string.Empty))
            {
                TextBox newTextBox = GetNewTextBoxForAuthorAndTitle(UNKNOWN, title);                
                stackPanel.Children.Add(newTextBox);

                foreach (Clipping clipping in m_ClippingsFileParserInstance.GetSublistOfClippingsForAuthorAndTitle(string.Empty, title))
                {
                    TextBox newClippingTextBox = GetNewTextBoxForClippingText(UNKNOWN, title, clipping.Text);
                    stackPanel.Children.Add(newClippingTextBox);
                }
            }                                   
        }

        public void PopulateAuthorsList(ListBox listBoxAuthors)
        {
            if(m_ClippingsFileParserInstance != null)
            {                
                foreach (string author in m_ClippingsFileParserInstance.ListOfAllAuthors)
                {
                    CheckBox newCheckBox = new CheckBox();
                    newCheckBox.Content = author;
                    newCheckBox.IsChecked = true;

                    listBoxAuthors.Items.Add(newCheckBox);
                }
            }
        }

        public void PopulateTitlesList(ListBox listBoxTitles)
        {
            if (m_ClippingsFileParserInstance != null)
            {
                foreach (string title in m_ClippingsFileParserInstance.ListOfAllTitles)
                {
                    CheckBox newCheckBox = new CheckBox();
                    newCheckBox.Content = title;
                    newCheckBox.IsChecked = true;

                    listBoxTitles.Items.Add(newCheckBox);
                }
            }
        }

        public void ButtonMarkAuthorsClick(ListBox listBox)
        {
            if (m_MainWindow.IsAnyAuthorUnselected)
            {
                m_MainWindow.SetCheckBoxesSelection(listBox, true);
                //m_ClippingsFileParserInstance.ToggleAllClippingsVisibility(true);
            }
            else
            {
                m_MainWindow.SetCheckBoxesSelection(listBox, false);
                //m_ClippingsFileParserInstance.ToggleAllClippingsVisibility(false);
            }
        }
        
        public void ButtonMarkTitlesClick(ListBox listBox)
        {
            if(m_MainWindow.IsAnyTitleUnselected)
            {
                m_MainWindow.SetCheckBoxesSelection(listBox, true);
            }
            else
            {
                m_MainWindow.SetCheckBoxesSelection(listBox, false);
            }
        }

        #endregion Public methods


        #region FOR TESTING PURPOSES

        private void test_CreateClippingsFileParserInstance()
        {
            m_ClippingsFileParserInstance = new ClippingsFileParser("C:\\My Clippings.txt");
        }       

        #endregion FOR TESTING PURPOSES               
    
        
    
       
    }
}
