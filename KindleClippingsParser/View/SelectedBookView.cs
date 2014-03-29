using KindleClippingsParser.Model;
using System;

namespace KindleClippingsParser.View
{
    public class SelectedBookView : BaseClippingsView
    {
        #region Ctors

        public SelectedBookView(MainWindow mainWindow)
            : base(mainWindow)
        {

        }

        #endregion Ctors
        #region Override

        override public void RenderView()
        {
            if (m_ClippingsFileParserInstance == null)
            {
                throw (new Exception("RenderView() invoked for SelectedBookView before setting model instance"));
            }

            //Populate combobox
            m_MainWindowInstance.comboBoxClippingHeaders.ItemsSource = m_ClippingsFileParserInstance.ListOfHeaders;
            m_MainWindowInstance.comboBoxClippingHeaders.DisplayMemberPath = "HeaderText";

            //Select default item
            m_MainWindowInstance.comboBoxClippingHeaders.SelectedIndex = m_ClippingsFileParserInstance.ListOfHeaders.Count - 1;
        }

        override public void RefreshViewWhenAuthorOrTitleSelectionChanged()
        {
            throw (new NotImplementedException());
        }

        #endregion Override       
    }
}
