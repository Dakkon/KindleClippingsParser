using KindleClippingsReader.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace KindleClippingsReader.View
{
    public class SelectedBookView : BaseClippingsView
    {
        #region Private fields

        private ObservableCollection<ClippingsHeader> m_ObservableCollectionOfHeaders;

        #endregion Private fields
        #region Properties

        public ListCollectionView HeadersListCollectionView;

        #endregion Properties
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
            m_ObservableCollectionOfHeaders = new ObservableCollection<ClippingsHeader>(m_ClippingsFileParserInstance.ListOfHeaders);
            HeadersListCollectionView = new ListCollectionView(m_ObservableCollectionOfHeaders);

            m_MainWindowInstance.comboBoxClippingHeaders.ItemsSource = HeadersListCollectionView;
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
