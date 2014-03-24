using KindleClippingsParser.Model;
using KindleClippingsParser.View;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Data;

namespace KindleClippingsParser.Controller
{
    public class KindleClippingsParserController
    {
        #region Private fields

        private MainWindow m_MainWindow;
        private ClippingsFileParser m_ClippingsFileParserInstance;
        private RenderedViews m_RenderedViews;

        private Dictionary<string, CheckBox> m_AuthorsCheckBoxes;
        private Dictionary<string, CheckBox> m_TitlesCheckBoxes;

        #endregion Private fields
        #region Ctors

        public KindleClippingsParserController(MainWindow mainWindow)
        {
            m_MainWindow = mainWindow;
        }

        #endregion Ctors
        #region Private methods

        private void SetModelForAllViews(ClippingsFileParser clippingsFileParser)
        {
            m_MainWindow.MCFileViewInstance.SetModel(clippingsFileParser);
            m_MainWindow.TextPageViewInstance.SetModel(clippingsFileParser);
            m_MainWindow.EditViewInstance.SetModel(clippingsFileParser);
        }

        private void PopulateAuthorsList()
        {
            if (m_ClippingsFileParserInstance != null)
            {
                m_AuthorsCheckBoxes = new Dictionary<string, CheckBox>();

                foreach (string author in m_ClippingsFileParserInstance.ListOfAllAuthors)
                {
                    CheckBox newCheckBox = new CheckBox();
                    newCheckBox.Content = author;
                    newCheckBox.IsChecked = true;
                    newCheckBox.Checked += m_MainWindow.authorCheckBoxSelectionChanged;
                    newCheckBox.Unchecked += m_MainWindow.authorCheckBoxSelectionChanged;

                    m_MainWindow.listBoxAuthors.Items.Add(newCheckBox);
                    m_AuthorsCheckBoxes.Add(author, newCheckBox);
                }
            }
        }

        private void PopulateTitlesList()
        {
            if (m_ClippingsFileParserInstance != null)
            {
                m_TitlesCheckBoxes = new Dictionary<string, CheckBox>();

                foreach (string title in m_ClippingsFileParserInstance.ListOfAllTitles)
                {
                    CheckBox newCheckBox = new CheckBox();
                    newCheckBox.Content = title;
                    newCheckBox.IsChecked = true;
                    newCheckBox.Checked += m_MainWindow.titleCheckBoxSelectionChanged;
                    newCheckBox.Unchecked += m_MainWindow.titleCheckBoxSelectionChanged;

                    m_MainWindow.listBoxTitles.Items.Add(newCheckBox);
                    m_TitlesCheckBoxes.Add(title, newCheckBox);
                }
            }
        }

        private void RefreshAllViewsWhenAuthorOrTitleSelectionChanged()
        {           
            m_MainWindow.MCFileViewInstance.RefreshViewWhenAuthorOrTitleSelectionChanged();
            m_MainWindow.EditViewInstance.RefreshViewWhenAuthorOrTitleSelectionChanged();
            m_MainWindow.TextPageViewInstance.RefreshViewWhenAuthorOrTitleSelectionChanged();            
        }

        private void SynchronizeListOfCheckBoxesAfterAuthorSelectionChange(string author, List<ClippingsHeader> toggledHeaders, bool isAuthorEnabled)
        {
            foreach (ClippingsHeader header in toggledHeaders)
            {
                CheckBox titleCheckBox = m_TitlesCheckBoxes[header.Title];

                if (titleCheckBox != null)
                {
                    titleCheckBox.Checked -= m_MainWindow.titleCheckBoxSelectionChanged;
                    titleCheckBox.Unchecked -= m_MainWindow.titleCheckBoxSelectionChanged;

                    titleCheckBox.IsChecked = isAuthorEnabled;

                    titleCheckBox.Checked += m_MainWindow.titleCheckBoxSelectionChanged;
                    titleCheckBox.Unchecked += m_MainWindow.titleCheckBoxSelectionChanged;
                }
            }

            RefreshAllViewsWhenAuthorOrTitleSelectionChanged();
        }

        private void SynchronizeListOfCheckBoxesAfterTitleSelectionChange(string title, List<string> toggledAuthors, bool isTitleEnabled)
        {
            foreach (string author in toggledAuthors)
            {
                CheckBox authorCheckBox = m_AuthorsCheckBoxes[author];

                if (authorCheckBox != null)
                {
                    authorCheckBox.Checked -= m_MainWindow.authorCheckBoxSelectionChanged;
                    authorCheckBox.Unchecked -= m_MainWindow.authorCheckBoxSelectionChanged;

                    authorCheckBox.IsChecked = isTitleEnabled;

                    authorCheckBox.Checked += m_MainWindow.authorCheckBoxSelectionChanged;
                    authorCheckBox.Unchecked += m_MainWindow.authorCheckBoxSelectionChanged;
                }
            }

            RefreshAllViewsWhenAuthorOrTitleSelectionChanged();
        }

        #endregion Private methods        
        #region Public methods

        public void AuthorCheckBoxSelectionChanged(CheckBox clickedCheckBox)
        {            
            string author = clickedCheckBox.Content.ToString();

            if (clickedCheckBox.IsChecked == true)
            {
                List<ClippingsHeader> toggledHeaders = m_ClippingsFileParserInstance.ToggleSingleAuthor(author, true);
                SynchronizeListOfCheckBoxesAfterAuthorSelectionChange(author, toggledHeaders, true);
            }
            else
            {
                List<ClippingsHeader> toggledHeaders = m_ClippingsFileParserInstance.ToggleSingleAuthor(author, false);
                SynchronizeListOfCheckBoxesAfterAuthorSelectionChange(author, toggledHeaders, false);
            }
        }

        public void TitleCheckBoxSelectionChanged(CheckBox clickedCheckBox)
        {            
            string title = clickedCheckBox.Content.ToString();

            if (clickedCheckBox.IsChecked == true)
            {
                List<string> toggledAuthors = m_ClippingsFileParserInstance.ToggleSingleTitle(title, true);
                SynchronizeListOfCheckBoxesAfterTitleSelectionChange(title, toggledAuthors, true);
            }
            else
            {
                List<string> toggledAuthors = m_ClippingsFileParserInstance.ToggleSingleTitle(title, false);
                SynchronizeListOfCheckBoxesAfterTitleSelectionChange(title, toggledAuthors, false);
            }
        }        

        public void ButtonMarkAuthorsClick()
        {
            if (m_MainWindow.IsAnyAuthorUnselected)
            {
                m_MainWindow.ToggleSelectionForAllCheckBoxesInAuthorListBox(true);
            }
            else
            {
                m_MainWindow.ToggleSelectionForAllCheckBoxesInAuthorListBox(false);
            }

            RefreshAllViewsWhenAuthorOrTitleSelectionChanged();
        }

        public void ButtonMarkTitlesClick()
        {
            if (m_MainWindow.IsAnyTitleUnselected)
            {
                m_MainWindow.ToggleSelectionForAllCheckBoxesInTitlesListBox(true);
            }
            else
            {
                m_MainWindow.ToggleSelectionForAllCheckBoxesInTitlesListBox(false);
            }

            RefreshAllViewsWhenAuthorOrTitleSelectionChanged();
        }

        public void MenuItemOpenClick()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";

            bool? isFileSelected = dlg.ShowDialog();

            if (isFileSelected == true)
            {
                m_ClippingsFileParserInstance = new ClippingsFileParser(dlg.FileName);
                PopulateAuthorsList();
                PopulateTitlesList();

                SetModelForAllViews(m_ClippingsFileParserInstance);

                if (m_MainWindow.menuItemMCFileView.IsChecked)
                {
                    MenuItemMCFileViewChecked();
                }
                else
                {
                    m_MainWindow.menuItemMCFileView.IsChecked = true;
                }
            }
        }

        public void MenuItemExitClick()
        {
            Application.Current.Shutdown();
        }

        public void MenuItemMCFileViewChecked()
        {
            m_MainWindow.menuItemTextPageView.IsChecked = false;
            m_MainWindow.menuItemEditView.IsChecked = false;

            if (!((m_RenderedViews & RenderedViews.MCFileView) == RenderedViews.MCFileView))
            {
                m_MainWindow.MCFileViewInstance.RenderView();
                m_RenderedViews |= RenderedViews.MCFileView;
            }

            m_MainWindow.ToggleFilterGroupBox(false, "(filtering is not available in this view)");
            m_MainWindow.tabItemMCFileView.IsSelected = true;
        }

        public void MenuItemTextPageViewChecked()
        {
            m_MainWindow.menuItemEditView.IsChecked = false;
            m_MainWindow.menuItemMCFileView.IsChecked = false;

            if (!((m_RenderedViews & RenderedViews.TextPageView) == RenderedViews.TextPageView))
            {
                m_MainWindow.TextPageViewInstance.RenderView();
                m_RenderedViews |= RenderedViews.TextPageView;
            }
            
            m_MainWindow.ToggleFilterGroupBox(true);
            m_MainWindow.tabItemTextPageView.IsSelected = true;            
        }

        public void menuItemEditViewChecked()
        {
            m_MainWindow.menuItemTextPageView.IsChecked = false;
            m_MainWindow.menuItemMCFileView.IsChecked = false;

            if (!((m_RenderedViews & RenderedViews.EditView) == RenderedViews.EditView))
            {
                m_MainWindow.EditViewInstance.RenderView();
                m_RenderedViews |= RenderedViews.EditView; 
            }

            m_MainWindow.ToggleFilterGroupBox(true);
            m_MainWindow.tabItemEditView.IsSelected = true;
        }

        #endregion Public methods
    }
}