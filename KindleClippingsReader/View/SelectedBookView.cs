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
            if (m_Model == null)
            {
                throw (new Exception("RenderView() invoked for SelectedBookView before setting model instance"));
            }

            //Populate combobox
            m_ObservableCollectionOfHeaders = new ObservableCollection<ClippingsHeader>(m_Model.ListOfHeaders);
            HeadersListCollectionView = new ListCollectionView(m_ObservableCollectionOfHeaders);

            m_MainWindowInstance.comboBoxClippingHeaders.ItemsSource = HeadersListCollectionView;
            m_MainWindowInstance.comboBoxClippingHeaders.DisplayMemberPath = "HeaderText";

            //Display appropriate text in comboBoxClippingHeaders
            m_MainWindowInstance.comboBoxClippingHeaders.Text = m_Model.ListOfHeaders[0].HeaderText;

            m_IsRendered = true;
        }

        public override void ResetView()
        {
            m_MainWindowInstance.comboBoxClippingHeaders.SelectedIndex = 0;
        }

        #endregion Override       
    }
}
