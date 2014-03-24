using KindleClippingsParser.Model;
using System;

namespace KindleClippingsParser.View
{
    public class TextPageView : BaseClippingsView
    {
        #region Ctors

        public TextPageView(MainWindow mainWindow)
            : base(mainWindow)
        {

        }

        #endregion Ctors
        #region Override

        override public void RenderView()
        {
            if (m_ClippingsFileParserInstance == null)
            {
                throw (new Exception("RenderView() invoked for TextPageView before setting model instance"));
            }            

            foreach (ClippingsHeader header in m_ClippingsFileParserInstance.ListOfHeaders)
            {
                if (header.IsEnabled)
                {
                    string authorAndTitleLine = string.Format("{0} - \"{1}\"", header.Author, header.Title);

                    m_MainWindowInstance.textBoxWithPageView.AppendText(new string('=', 10));
                    m_MainWindowInstance.textBoxWithPageView.AppendText(Environment.NewLine);
                    m_MainWindowInstance.textBoxWithPageView.AppendText(authorAndTitleLine);
                    m_MainWindowInstance.textBoxWithPageView.AppendText(Environment.NewLine);
                    m_MainWindowInstance.textBoxWithPageView.AppendText(new string('=', 10));
                    m_MainWindowInstance.textBoxWithPageView.AppendText(Environment.NewLine);
                    m_MainWindowInstance.textBoxWithPageView.AppendText(Environment.NewLine);

                    foreach (Clipping clipping in header.ListOfClippings)
                    {
                        m_MainWindowInstance.textBoxWithPageView.AppendText(clipping.Text);
                        m_MainWindowInstance.textBoxWithPageView.AppendText(Environment.NewLine);
                        m_MainWindowInstance.textBoxWithPageView.AppendText(Environment.NewLine);
                    }
                }
            }
        }

        override public void RefreshViewWhenAuthorOrTitleSelectionChanged()
        {
            m_MainWindowInstance.textBoxWithPageView.Clear();
            RenderView();
        }

        #endregion Override
    }
}
