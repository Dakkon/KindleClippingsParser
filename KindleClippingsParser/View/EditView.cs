using KindleClippingsParser.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KindleClippingsParser.View
{
    public class EditView : BaseClippingsView
    {
        #region Ctors

        public EditView(MainWindow mainWindow)
            : base(mainWindow)
        {

        }

        #endregion Ctors
        #region Private methods

        private TextBox GetNewTextBoxForClippingsHeader(ClippingsHeader header)
        {
            const string AUTHOR = "Author: ";
            const string TITLE = "Title: ";

            //Set properties
            TextBox newTextBox = new TextBox();
            newTextBox.TextWrapping = TextWrapping.Wrap;
            newTextBox.FontWeight = FontWeights.Bold;
            newTextBox.Margin = new Thickness(0, 10, 0, 10);

            //Bind TextBox.Visibility with Header.IsEnabled property
            Binding textBoxAndHeaderBinding = new Binding("IsEnabled");
            textBoxAndHeaderBinding.Source = header;
            textBoxAndHeaderBinding.Converter = new BooleanToVisibilityConverter();
            newTextBox.SetBinding(TextBox.VisibilityProperty, textBoxAndHeaderBinding);

            //Set text
            newTextBox.AppendText(AUTHOR);
            newTextBox.AppendText(header.Author);
            newTextBox.AppendText(Environment.NewLine);
            newTextBox.AppendText(TITLE);
            newTextBox.AppendText(header.Title);

            return newTextBox;
        }

        private TextBox GetNewTextBoxForClippingText(Clipping clipping)
        {
            const string TOOLTIP_TEXT = "Author: {0}{1}Title: {2}";

            //Set properties
            TextBox newTextBox = new TextBox();
            newTextBox.TextWrapping = TextWrapping.Wrap;
            newTextBox.Margin = new Thickness(10, 0, 0, 0);

            //Bind TextBox.Visibility with Clipping.IsEnabled property
            Binding textBoxAndClippingBinding = new Binding("IsEnabled");
            textBoxAndClippingBinding.Source = clipping;
            textBoxAndClippingBinding.Converter = new BooleanToVisibilityConverter();
            newTextBox.SetBinding(TextBox.VisibilityProperty, textBoxAndClippingBinding);

            //Set text
            newTextBox.ToolTip = string.Format(TOOLTIP_TEXT, clipping.Author, Environment.NewLine, clipping.Title);
            newTextBox.AppendText(clipping.Text);

            return newTextBox;
        }                

        #endregion Private methods
        #region Override

        override public void RenderView()
        {
            if (m_ClippingsFileParserInstance == null)
            {
                throw (new Exception("RenderView() invoked for EditView before setting model instance"));
            }

            foreach (ClippingsHeader header in m_ClippingsFileParserInstance.ListOfHeaders)
            {
                TextBox newHeaderTextBox = GetNewTextBoxForClippingsHeader(header);
                m_MainWindowInstance.stackPanelClippings.Children.Add(newHeaderTextBox);

                foreach (Clipping clipping in header.ListOfClippings)
                {
                    TextBox newClippingTextBox = GetNewTextBoxForClippingText(clipping);
                    m_MainWindowInstance.stackPanelClippings.Children.Add(newClippingTextBox);
                }
            }

            m_MainWindowInstance.OpenBothExpanders();
        }

        override public void RefreshViewWhenAuthorOrTitleSelectionChanged()
        {
            //Textbox controls binded with model. 
            //Refreshing is done automatically.
        }

        #endregion Override
    }
}
