using KindleClippingsReader.Model;

namespace KindleClippingsReader.View
{
    public abstract class BaseClippingsView
    {
        #region Private fields

        protected ClippingsFileParser m_Model;
        protected MainWindow m_MainWindowInstance;
        protected bool m_IsRendered;

        #endregion Private fields
        #region Properties

        public ClippingsFileParser Model
        {
            set
            {
                m_Model = value;
                m_IsRendered = false;
            }
        }

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
