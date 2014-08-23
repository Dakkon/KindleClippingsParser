using KindleClippingsReader.Model;
using System;

namespace KindleClippingsReader.View
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
            if (m_Model == null)
            {
                throw (new Exception("RenderView() invoked for MCFileView before setting model instance"));
            }

            m_MainWindowInstance.textBoxWithOriginalMyClippingsFile.Text = m_Model.OriginalMyClippingsFileText;

            m_IsRendered = true;
        }

        override public void ResetView()
        {
            //Reset not required
        }

        #endregion Override
    }
}
