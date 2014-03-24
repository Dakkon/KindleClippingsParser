using KindleClippingsParser.Model;
using System;

namespace KindleClippingsParser.View
{
    public class MCFileView : BaseClippingsView
    {
        #region Ctors

        public MCFileView(MainWindow mainWindow)
            : base(mainWindow)
        {

        }

        #endregion Ctors
        #region Override

        override public void RenderView()
        {
            if (m_ClippingsFileParserInstance == null)
            {
                throw (new Exception("RenderView() invoked for MCFileView before setting model instance"));
            }
                        
            m_MainWindowInstance.textBoxWithOriginalMyClippingsFile.Text = m_ClippingsFileParserInstance.OriginalMyClippingsFileText;
        }

        override public void RefreshViewWhenAuthorOrTitleSelectionChanged()
        {
            //Refreshing not required
        }

        #endregion Override
    }
}
