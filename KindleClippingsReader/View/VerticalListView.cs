using KindleClippingsReader.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace KindleClippingsReader.View
{
    public class VerticalListView : BaseClippingsView
    {
        #region Private fields

        private ObservableCollection<ClippingsHeader> m_ObservableCollectionOfHeaders;

        #endregion Private fields
        #region Properties

        public ListCollectionView HeadersListCollectionView;

        #endregion Properties
        #region Ctors

        public VerticalListView(MainWindow mainWindow)
            : base(mainWindow)
        {

        }

        #endregion Ctors
        #region Override

        override public void RenderView()
        {
            if (m_ClippingsFileParserInstance == null)
            {
                throw (new Exception("RenderView() invoked for VerticalListView before setting model instance"));
            }

            //Populate list of titles
            m_ObservableCollectionOfHeaders = new ObservableCollection<ClippingsHeader>(m_ClippingsFileParserInstance.ListOfHeaders);
            HeadersListCollectionView = new ListCollectionView(m_ObservableCollectionOfHeaders);
            
            m_MainWindowInstance.listBoxClippingHeaders.ItemsSource = HeadersListCollectionView;
            m_MainWindowInstance.listBoxClippingHeaders.DisplayMemberPath = "HeaderText";
        }

        public override void ResetView()
        {
            m_MainWindowInstance.textBoxVerticalListFilter.Text = string.Empty;
        }

        #endregion Override
    }
}
