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

        private void SetModelForAllViews(ClippingsFileParser clippingsFileParser)
        {
            m_MainWindow.SelectedBookViewInstance.SetModel(clippingsFileParser);
            m_MainWindow.MCFileViewInstance.SetModel(clippingsFileParser);
        }

        #endregion Private methods
        #region Public methods

        public void MenuItemOpenClick()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";

            bool? isFileSelected = dlg.ShowDialog();

            if (isFileSelected == true)
            {
                m_ClippingsFileParserInstance = new ClippingsFileParser(dlg.FileName);

                SetModelForAllViews(m_ClippingsFileParserInstance);

                MenuItemSelectedBookViewClick();
            }
        }

        public void MenuItemExitClick()
        {
            Application.Current.Shutdown();
        }

        public void MenuItemSelectedBookViewClick()
        {
            m_MainWindow.menuItemSelectedBookView.IsChecked = true;

            m_MainWindow.menuItemMCFileView.IsChecked = false;

            if (!((m_RenderedViews & RenderedViews.SelectedBookView) == RenderedViews.SelectedBookView))
            {
                m_MainWindow.SelectedBookViewInstance.RenderView();
                m_RenderedViews |= RenderedViews.SelectedBookView;
            }

            m_MainWindow.tabItemSelectedBookView.IsSelected = true;
        }

        public void MenuItemMCFileViewClick()
        {
            m_MainWindow.menuItemMCFileView.IsChecked = true;

            m_MainWindow.menuItemSelectedBookView.IsChecked = false;

            if (!((m_RenderedViews & RenderedViews.MCFileView) == RenderedViews.MCFileView))
            {
                m_MainWindow.MCFileViewInstance.RenderView();
                m_RenderedViews |= RenderedViews.MCFileView;
            }

            m_MainWindow.tabItemMCFileView.IsSelected = true;            
        }

        public void ComboBoxClippingHeadersSelectionChanged()
        {
            m_MainWindow.textBoxWithPageView.Text = 
                m_ClippingsFileParserInstance.GetListOfAllClippingsForSingleHeader((ClippingsHeader)m_MainWindow.comboBoxClippingHeaders.SelectedItem);
        }

        #endregion Public methods
    }
}