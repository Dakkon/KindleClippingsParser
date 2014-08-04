using KindleClippingsReader.Model;

namespace KindleClippingsReader.View
{
    public abstract class BaseClippingsView
    {
        #region Static

        public static ClippingsFileParser m_Model;

        #endregion Static
        #region Private fields

        protected MainWindow m_MainWindowInstance;        
        protected bool m_IsRendered;

        #endregion Private fields
        #region Properties

        public bool IsRendered
        {
            get
            {
                return m_IsRendered;
            }
        }

        #endregion Properties
        #region Ctors

        public BaseClippingsView(MainWindow mainWindow)
        {
            m_MainWindowInstance = mainWindow;
            m_IsRendered = false;
        }

        #endregion Ctors
        #region Abstract methods

        abstract public void RenderView();

        abstract public void ResetView();
        
        #endregion Abstract methods
    }
}
