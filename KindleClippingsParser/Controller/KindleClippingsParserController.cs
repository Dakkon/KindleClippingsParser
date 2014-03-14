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

        #endregion Private fields
        #region Ctors

        public KindleClippingsParserController(MainWindow mainWindow)
        {
            m_MainWindow = mainWindow;
        }

        #endregion Ctors
        #region Private methods

        private void PopulateAuthorsList()
        {
            if (m_ClippingsFileParserInstance != null)
            {
                foreach (string author in m_ClippingsFileParserInstance.ListOfAllAuthors)
                {
                    CheckBox newCheckBox = new CheckBox();
                    newCheckBox.Content = author;
                    newCheckBox.IsChecked = true;
                    newCheckBox.Checked += authorCheckBoxSelectionChanged;
                    newCheckBox.Unchecked += authorCheckBoxSelectionChanged;

                    m_MainWindow.listBoxAuthors.Items.Add(newCheckBox);
                }
            }
        }

        private void PopulateTitlesList()
        {
            if (m_ClippingsFileParserInstance != null)
            {
                foreach (string title in m_ClippingsFileParserInstance.ListOfAllTitles)
                {
                    CheckBox newCheckBox = new CheckBox();
                    newCheckBox.Content = title;
                    newCheckBox.IsChecked = true;
                    newCheckBox.Checked += titleCheckBoxSelectionChanged;
                    newCheckBox.Unchecked += titleCheckBoxSelectionChanged;

                    m_MainWindow.listBoxTitles.Items.Add(newCheckBox);
                }
            }
        }

        private TextBox GetNewTextBoxForAuthorAndTitle(string author, string title)
        {
            const string AUTHOR = "Author: ";
            const string TITLE = "Title: ";

            //Set properties
            TextBox newTextBox = new TextBox();
            newTextBox.TextWrapping = TextWrapping.Wrap;
            newTextBox.FontWeight = FontWeights.Bold;
            newTextBox.Margin = new Thickness(0, 10, 0, 10);

            //Bind TextBox.Visibility with CheckBox.IsChecked (on listBoxTitles)
            Binding textBoxAndCheckBoxBinding = new Binding("IsChecked");
            textBoxAndCheckBoxBinding.Source = m_MainWindow.FindCheckBoxOnTitleListBoxForTitle(title);
            textBoxAndCheckBoxBinding.Converter = new BooleanToVisibilityConverter();
            textBoxAndCheckBoxBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            newTextBox.SetBinding(TextBox.VisibilityProperty, textBoxAndCheckBoxBinding);

            //Set text
            newTextBox.AppendText(AUTHOR);
            newTextBox.AppendText(author);
            newTextBox.AppendText(Environment.NewLine);
            newTextBox.AppendText(TITLE);
            newTextBox.AppendText(title);

            return newTextBox;
        }

        private TextBox GetNewTextBoxForClippingText(string author, string title, Clipping clipping)
        {
            const string TOOLTIP_TEXT = "Author: {0}{1}Title: {2}";

            //Set properties
            TextBox newTextBox = new TextBox();
            newTextBox.TextWrapping = TextWrapping.Wrap;

            //Bind TextBox.Visibility with Clipping.IsEnabled property
            Binding textBoxAndClippingBinding = new Binding("IsEnabled");
            textBoxAndClippingBinding.Source = clipping;
            textBoxAndClippingBinding.Converter = new BooleanToVisibilityConverter();
            newTextBox.SetBinding(TextBox.VisibilityProperty, textBoxAndClippingBinding);

            //Set text
            newTextBox.ToolTip = string.Format(TOOLTIP_TEXT, author, Environment.NewLine, title);
            newTextBox.AppendText(clipping.Text);

            return newTextBox;
        }

        private void ToggleSelectionForAllTitlesOfSingleAuthor(string author, bool state)
        {
            List<string> ListOfAllTitlesForAuthor = m_ClippingsFileParserInstance.GetListOfAllTitlesForAuthor(author);

            foreach (string title in ListOfAllTitlesForAuthor)
            {
                foreach (CheckBox checkBox in m_MainWindow.listBoxTitles.Items)
                {
                    if (string.Equals(checkBox.Content.ToString(), title))
                    {
                        checkBox.IsChecked = state;
                    }
                }
            }
        }

        private void SynchronizeAuthorListBoxWithTitlesListBox(string title, bool isTitleSelected)
        {
            List<string> ListOfAllAuthorsForTitle = m_ClippingsFileParserInstance.GetListOfAllAuthorsForTitle(title);

            if (isTitleSelected)
            {
                foreach (string author in ListOfAllAuthorsForTitle)
                {
                    foreach (CheckBox checkBox in m_MainWindow.listBoxAuthors.Items)
                    {
                        if (string.Equals(checkBox.Content.ToString(), author))
                        {
                            checkBox.Checked -= authorCheckBoxSelectionChanged;
                            checkBox.Unchecked -= authorCheckBoxSelectionChanged;

                            checkBox.IsChecked = true;

                            checkBox.Checked += authorCheckBoxSelectionChanged;
                            checkBox.Unchecked += authorCheckBoxSelectionChanged;
                        }
                    }
                }
            }

            if (!isTitleSelected)
            {
                foreach (string author in ListOfAllAuthorsForTitle)
                {
                    foreach (CheckBox checkBox in m_MainWindow.listBoxAuthors.Items)
                    {
                        if (string.Equals(checkBox.Content.ToString(), author)
                            && !m_ClippingsFileParserInstance.IsThereAtLeastOneClippingForAuthorEnabled(author))
                        {
                            checkBox.Checked -= authorCheckBoxSelectionChanged;
                            checkBox.Unchecked -= authorCheckBoxSelectionChanged;

                            checkBox.IsChecked = false;

                            checkBox.Checked += authorCheckBoxSelectionChanged;
                            checkBox.Unchecked += authorCheckBoxSelectionChanged;
                        }
                    }
                }
            }
        }

        private void RenderClippingsInMCFileView()
        {
            if (m_ClippingsFileParserInstance != null)
            {
                m_MainWindow.textBoxWithOriginalMyClippingsFile.Text = m_ClippingsFileParserInstance.OriginalMyClippingsFileText;

                m_RenderedViews |= RenderedViews.MCFileView;
            }
        }

        private void RenderClippingsInTextPageView()
        {
            if (m_ClippingsFileParserInstance != null)
            {
                m_MainWindow.textBoxWithPageView.Clear();

                //Find titles with correct authors
                foreach (string author in m_ClippingsFileParserInstance.ListOfAllEnabledAuthors)
                {
                    foreach (string title in m_ClippingsFileParserInstance.GetListOfAllTitlesForAuthor(author))
                    {
                        if (m_ClippingsFileParserInstance.IsTitleEnabled(title))
                        {
                            string authorAndTitleLine = string.Format("{0} - \"{1}\"", author, title);

                            m_MainWindow.textBoxWithPageView.AppendText(new string('=', 10));
                            m_MainWindow.textBoxWithPageView.AppendText(Environment.NewLine);
                            m_MainWindow.textBoxWithPageView.AppendText(authorAndTitleLine);
                            m_MainWindow.textBoxWithPageView.AppendText(Environment.NewLine);
                            m_MainWindow.textBoxWithPageView.AppendText(new string('=', 10));
                            m_MainWindow.textBoxWithPageView.AppendText(Environment.NewLine);
                            m_MainWindow.textBoxWithPageView.AppendText(Environment.NewLine);

                            foreach (Clipping clipping in m_ClippingsFileParserInstance.GetSublistOfClippingsForAuthorAndTitle(author, title))
                            {
                                if (clipping.IsEnabled)
                                {
                                    m_MainWindow.textBoxWithPageView.AppendText(clipping.Text);
                                    m_MainWindow.textBoxWithPageView.AppendText(Environment.NewLine);
                                    m_MainWindow.textBoxWithPageView.AppendText(Environment.NewLine);
                                }
                            }

                            m_MainWindow.textBoxWithPageView.AppendText(Environment.NewLine);
                            m_MainWindow.textBoxWithPageView.AppendText(Environment.NewLine);
                        }
                    }
                }

                m_MainWindow.OpenBothExpanders();

                m_RenderedViews |= RenderedViews.TextPageView;
            }
        }

        private void RenderClippingsInEditView()
        {
            if (m_ClippingsFileParserInstance != null)
            {
                //Find titles with correct authors
                foreach (string author in m_ClippingsFileParserInstance.ListOfAllEnabledAuthors)
                {
                    foreach (string title in m_ClippingsFileParserInstance.GetListOfAllTitlesForAuthor(author))
                    {
                        TextBox newTextBox = GetNewTextBoxForAuthorAndTitle(author, title);
                        m_MainWindow.stackPanelClippings.Children.Add(newTextBox);

                        foreach (Clipping clipping in m_ClippingsFileParserInstance.GetSublistOfClippingsForAuthorAndTitle(author, title))
                        {
                            TextBox newClippingTextBox = GetNewTextBoxForClippingText(author, title, clipping);
                            m_MainWindow.stackPanelClippings.Children.Add(newClippingTextBox);
                        }
                    }
                }

                m_MainWindow.OpenBothExpanders();

                m_RenderedViews |= RenderedViews.EditView;
            }
        }

        private void RefreshAllViewsWhenAuthorOrTitleSelectionChanged()
        {
            //----- ClippingsInMCFileView -----
            //Refreshing not required


            //----- ClippingsInEditView -----
            //Textbox controls binded with checkboxes. 
            //Refreshing is done automatically.


            //----- ClippingsInTextPageView -----
            RenderClippingsInTextPageView();
        }

        #endregion Private methods
        #region Event handlers

        private void authorCheckBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            CheckBox clickedCheckBox = (CheckBox)sender;

            if (clickedCheckBox.IsChecked == true)
            {
                m_ClippingsFileParserInstance.ToggleIsEnabledForAllClippingsOfSingleAuthor(clickedCheckBox.Content.ToString(), true);
                ToggleSelectionForAllTitlesOfSingleAuthor(clickedCheckBox.Content.ToString(), true);
            }
            else
            {
                m_ClippingsFileParserInstance.ToggleIsEnabledForAllClippingsOfSingleAuthor(clickedCheckBox.Content.ToString(), false);
                ToggleSelectionForAllTitlesOfSingleAuthor(clickedCheckBox.Content.ToString(), false);
            }

            RefreshAllViewsWhenAuthorOrTitleSelectionChanged();
        }

        private void titleCheckBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            CheckBox clickedCheckBox = (CheckBox)sender;

            if (clickedCheckBox.IsChecked == true)
            {
                m_ClippingsFileParserInstance.ToggleIsEnabledForAllClippingsOfSingleTitle(clickedCheckBox.Content.ToString(), true);
                SynchronizeAuthorListBoxWithTitlesListBox(clickedCheckBox.Content.ToString(), true);
            }
            else
            {
                m_ClippingsFileParserInstance.ToggleIsEnabledForAllClippingsOfSingleTitle(clickedCheckBox.Content.ToString(), false);
                SynchronizeAuthorListBoxWithTitlesListBox(clickedCheckBox.Content.ToString(), false);
            }

            RefreshAllViewsWhenAuthorOrTitleSelectionChanged();
        }

        #endregion Event handlers
        #region Public methods

        public void ButtonMarkAuthorsClick()
        {
            if (m_MainWindow.IsAnyAuthorUnselected)
            {
                m_MainWindow.ToggleSelectionForAllCheckBoxesInListBox(m_MainWindow.listBoxAuthors, true);
            }
            else
            {
                m_MainWindow.ToggleSelectionForAllCheckBoxesInListBox(m_MainWindow.listBoxAuthors, false);
            }

            RefreshAllViewsWhenAuthorOrTitleSelectionChanged();
        }

        public void ButtonMarkTitlesClick()
        {
            if (m_MainWindow.IsAnyTitleUnselected)
            {
                m_MainWindow.ToggleSelectionForAllCheckBoxesInListBox(m_MainWindow.listBoxTitles, true);
            }
            else
            {
                m_MainWindow.ToggleSelectionForAllCheckBoxesInListBox(m_MainWindow.listBoxTitles, false);
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
                RenderClippingsInMCFileView();
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
                RenderClippingsInTextPageView();
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
                RenderClippingsInEditView();
            }

            m_MainWindow.ToggleFilterGroupBox(true);
            m_MainWindow.tabItemEditView.IsSelected = true;
        }

        #endregion Public methods
    }
}