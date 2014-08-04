using KindleClippingsReader.Model;
using KindleClippingsReader.View;
using KindleClippingsReader.Helpers;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;

namespace KindleClippingsReader.Controller
{
    public class KindleClippingsReaderController
    {
        #region Private fields

        private MainWindow m_MainWindow;
        private ClippingsFileParser m_ClippingsFileParserInstance;        

        #endregion Private fields
        #region Ctors

        public KindleClippingsReaderController(MainWindow mainWindow)
        {
            m_MainWindow = mainWindow;
        }

        #endregion Ctors
        #region Private methods

        private void SetModelForAllViews(ClippingsFileParser clippingsFileParser)
        {
            BaseClippingsView.m_Model = clippingsFileParser;
        }

        private bool FilterPredicate(object obj)
        {
            ClippingsHeader headerToCheck = obj as ClippingsHeader;

            if (headerToCheck.Author.ToUpper().Contains(m_MainWindow.comboBoxClippingHeaders.Text.ToUpper()) ||
                headerToCheck.Title.ToUpper().Contains(m_MainWindow.comboBoxClippingHeaders.Text.ToUpper()))
            {
                return true;
            }

            return false;
        }

        private bool isKeyForbidden(Key key)
        {
            List<Key> forbiddenKeys = new List<Key>();

            forbiddenKeys.Add(Key.System);
            forbiddenKeys.Add(Key.RightShift);
            forbiddenKeys.Add(Key.LeftShift);
            forbiddenKeys.Add(Key.Up);
            forbiddenKeys.Add(Key.Down);
            forbiddenKeys.Add(Key.Left);
            forbiddenKeys.Add(Key.Right);
            forbiddenKeys.Add(Key.Enter);
            forbiddenKeys.Add(Key.LeftAlt);
            forbiddenKeys.Add(Key.RightAlt);
            forbiddenKeys.Add(Key.LeftCtrl);
            forbiddenKeys.Add(Key.RightCtrl);
            forbiddenKeys.Add(Key.F1);
            forbiddenKeys.Add(Key.F2);
            forbiddenKeys.Add(Key.F3);
            forbiddenKeys.Add(Key.F4);
            forbiddenKeys.Add(Key.F5);
            forbiddenKeys.Add(Key.F6);
            forbiddenKeys.Add(Key.F7);
            forbiddenKeys.Add(Key.F8);
            forbiddenKeys.Add(Key.F9);
            forbiddenKeys.Add(Key.F10);
            forbiddenKeys.Add(Key.F11);
            forbiddenKeys.Add(Key.F12);

            return forbiddenKeys.Contains(key);
        }

        private void OpenMyClippingsFile(string fullFilePath)
        {
            m_ClippingsFileParserInstance = new ClippingsFileParser(fullFilePath);

            SetModelForAllViews(m_ClippingsFileParserInstance);

            m_MainWindow.menuItemView.IsEnabled = true;
            MenuItemVerticalListViewClick();
        }

        private bool HeadersListForVerticalViewFilter(object item)
        {
            ClippingsHeader clippingsHeader = item as ClippingsHeader;

            return clippingsHeader.HeaderText.ToLower().Contains(m_MainWindow.textBoxVerticalListFilter.Text.ToLower());
        }

        #endregion Private methods
        #region Public methods

        public void OpenFileClick()
        {
            bool? isFileSelected;
            string fullFilePath;

            DiskIOHelper.OpenTextFile(out isFileSelected, out fullFilePath,
                m_MainWindow.Resources["dialogTitle"].ToString(), m_MainWindow.Resources["dialogFilter"].ToString());

            if (isFileSelected == true)
            {
                OpenMyClippingsFile(fullFilePath);
            }
        }

        public void ButtonOpenFoundClick(string fullFilePath)
        {
            OpenMyClippingsFile(fullFilePath);
        }

        public void MenuItemExitClick()
        {
            Application.Current.Shutdown();
        }

        public void MenuItemVerticalListViewClick()
        {
            m_MainWindow.menuItemVerticalListView.IsChecked = true;

            m_MainWindow.menuItemSelectedBookView.IsChecked = false;
            m_MainWindow.menuItemMCFileView.IsChecked = false;

            if (!m_MainWindow.VerticalListViewInstance.IsRendered)
            {
                m_MainWindow.VerticalListViewInstance.RenderView();
            }

            m_MainWindow.tabItemVerticalListView.IsSelected = true;

            m_MainWindow.ResetAllViews();
        }

        public void MenuItemSelectedBookViewClick()
        {
            m_MainWindow.menuItemSelectedBookView.IsChecked = true;

            m_MainWindow.menuItemVerticalListView.IsChecked = false;
            m_MainWindow.menuItemMCFileView.IsChecked = false;

            if (!m_MainWindow.SelectedBookViewInstance.IsRendered)
            {
                m_MainWindow.SelectedBookViewInstance.RenderView();               
            }

            m_MainWindow.tabItemSelectedBookView.IsSelected = true;

            m_MainWindow.ResetAllViews();
        }

        public void MenuItemMCFileViewClick()
        {
            m_MainWindow.menuItemMCFileView.IsChecked = true;

            m_MainWindow.menuItemVerticalListView.IsChecked = false;
            m_MainWindow.menuItemSelectedBookView.IsChecked = false;

            if (!m_MainWindow.MCFileViewInstance.IsRendered)
            {
                m_MainWindow.MCFileViewInstance.RenderView();                
            }

            m_MainWindow.tabItemMCFileView.IsSelected = true;
        }

        public void MenuItemClick()
        {
            Window dialogAbout = new DialogAbout();
            dialogAbout.Resources = m_MainWindow.Resources;
            dialogAbout.Owner = m_MainWindow;

            dialogAbout.ShowDialog();
        }

        #endregion Public methods
        #region Public methods - SelectedBookView

        public void ComboBoxClippingHeadersSelectionChanged()
        {
            m_MainWindow.textBoxWithPageView.Text =
                m_ClippingsFileParserInstance.GetListOfAllClippingsForSingleHeader((ClippingsHeader)m_MainWindow.comboBoxClippingHeaders.SelectedItem);
        }

        public void ComboBoxClippingHeadersKeyDown(KeyEventArgs e)
        {
            if (!isKeyForbidden(e.Key))
            {
                m_MainWindow.comboBoxClippingHeaders.SelectedIndex = -1;
                m_MainWindow.comboBoxClippingHeaders.IsDropDownOpen = true;
            }
            else
            {
                m_MainWindow.comboBoxClippingHeaders.IsDropDownOpen = false;
            }
        }

        public void ComboBoxClippingHeadersKeyUp(KeyEventArgs e)
        {
            if (m_MainWindow.SelectedBookViewInstance.HeadersListCollectionView != null && !isKeyForbidden(e.Key))
            {
                m_MainWindow.SelectedBookViewInstance.HeadersListCollectionView.Filter += FilterPredicate;
            }
        }

        public void ComboBoxClippingHeadersDropDownClosed()
        {
            if (m_MainWindow.SelectedBookViewInstance.HeadersListCollectionView != null)
            {
                m_MainWindow.SelectedBookViewInstance.HeadersListCollectionView.Filter = null;
            }
        }

        #endregion Public methods - SelectedBookView
        #region Public methods - VerticalListView

        public void ListBoxClippingHeadersSelectionChanged()
        {
            m_MainWindow.textBoxForVerticalListView.Text =
                m_ClippingsFileParserInstance.GetListOfAllClippingsForSingleHeader((ClippingsHeader)m_MainWindow.listBoxClippingHeaders.SelectedItem);
        }

        public void TextBoxVerticalListFilterTextChanged()
        {
            ICollectionView headersListCollectionView = CollectionViewSource.GetDefaultView(m_MainWindow.VerticalListViewInstance.HeadersListCollectionView);
            headersListCollectionView.Filter = HeadersListForVerticalViewFilter;
        }

        #endregion Public methods - VerticalListView
    }
}