using KindleClippingsParser.Model;

namespace KindleClippingsParser.View
{
    public abstract class BaseClippingsView
    {
        #region Private fields
        
        protected MainWindow m_MainWindowInstance;
        protected ClippingsFileParser m_ClippingsFileParserInstance; 

        #endregion Private fields
        #region Ctors

        public BaseClippingsView(MainWindow mainWindow)
        {
            m_MainWindowInstance = mainWindow;            
        }

        #endregion Ctors
        #region Abstract methods

        abstract public void RenderView();

        abstract public void RefreshViewWhenAuthorOrTitleSelectionChanged();
        
        #endregion Abstract methods
        #region Public methods

        public void SetModel(ClippingsFileParser clippingsFileParser)
        {
            m_ClippingsFileParserInstance = clippingsFileParser;
        }

        #endregion Public methods
    }
}
