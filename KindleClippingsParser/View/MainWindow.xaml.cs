using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KindleClippingsParser.Controller;

namespace KindleClippingsParser.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>   

    public partial class MainWindow : Window
    {
        #region Private fields

        MCFileView m_MCFileView;
        TextPageView m_TextPageView;
        EditView m_EditView;

        KindleClippingsParserController m_Controller;

        #endregion Private fields
        #region Properties

        public MCFileView MCFileViewInstance
        {
            get
            {
                return m_MCFileView;
            }
        }

        public TextPageView TextPageViewInstance
        {
            get
            {
                return m_TextPageView;
            }
        }

        public EditView EditViewInstance
        {
            get
            {
                return m_EditView;
            }
        }

        public bool IsAnyAuthorUnselected
        {
            get
            {
                foreach (object item in listBoxAuthors.Items)
                {
                    if (item.GetType() == typeof(CheckBox) && !(bool)(((CheckBox)item).IsChecked))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsAnyTitleUnselected
        {
            get
            {
                foreach (object item in listBoxTitles.Items)
                {
                    if (item.GetType() == typeof(CheckBox) && !(bool)(((CheckBox)item).IsChecked))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        #endregion Properties
        #region Ctors

        public MainWindow()
        {
            InitializeComponent();
            m_Controller = new KindleClippingsParserController(this);

            InitializeViews();

            SubscribeToEvents();
        }

        #endregion Ctors
        #region Private methods

        private void InitializeViews()
        {
            m_MCFileView = new MCFileView(this);
            m_TextPageView = new TextPageView(this);
            m_EditView = new EditView(this);
        }

        private void SubscribeToEvents()
        {
            menuItemMCFileView.Checked += menuItemMCFileView_Checked;
            menuItemTextPageView.Checked += menuItemTextPageView_Checked;
            menuItemEditView.Checked += menuItemEditView_Checked;
        }

        #endregion Private methods
        #region Event handlers

        public void authorCheckBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            m_Controller.AuthorCheckBoxSelectionChanged((CheckBox)sender);
        }

        public void titleCheckBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            m_Controller.TitleCheckBoxSelectionChanged((CheckBox)sender);
        }

        private void buttonMarkAuthors_Click(object sender, RoutedEventArgs e)
        {
            m_Controller.ButtonMarkAuthorsClick();
        }

        private void buttonMarkTitles_Click(object sender, RoutedEventArgs e)
        {
            m_Controller.ButtonMarkTitlesClick();
        }

        private void menuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            m_Controller.MenuItemOpenClick();
        }

        private void menuItemExit_Click(object sender, RoutedEventArgs e)
        {
            m_Controller.MenuItemExitClick();
        }

        private void menuItemMCFileView_Checked(object sender, RoutedEventArgs e)
        {
            m_Controller.MenuItemMCFileViewChecked();
        }

        private void menuItemTextPageView_Checked(object sender, RoutedEventArgs e)
        {
            m_Controller.MenuItemTextPageViewChecked();
        }

        private void menuItemEditView_Checked(object sender, RoutedEventArgs e)
        {
            m_Controller.menuItemEditViewChecked();
        }

        #endregion Event handlers
        #region Public methods

        public void ToggleSelectionForAllCheckBoxesInAuthorListBox(bool state)
        {
            foreach (object item in listBoxAuthors.Items)
            {
                if (item.GetType() == typeof(CheckBox))
                {
                    //((CheckBox)item).Checked -= authorCheckBoxSelectionChanged;
                    //((CheckBox)item).Unchecked -= authorCheckBoxSelectionChanged;

                    ((CheckBox)item).IsChecked = state;

                    //((CheckBox)item).Checked += authorCheckBoxSelectionChanged;
                    //((CheckBox)item).Unchecked += authorCheckBoxSelectionChanged;
                }
            }
        }

        public void ToggleSelectionForAllCheckBoxesInTitlesListBox(bool state)
        {
            foreach (object item in listBoxTitles.Items)
            {
                if (item.GetType() == typeof(CheckBox))
                {
                    //((CheckBox)item).Checked -= titleCheckBoxSelectionChanged;
                    //((CheckBox)item).Unchecked -= titleCheckBoxSelectionChanged;

                    ((CheckBox)item).IsChecked = state;

                    //((CheckBox)item).Checked += titleCheckBoxSelectionChanged;
                    //((CheckBox)item).Unchecked += titleCheckBoxSelectionChanged;
                }
            }
        }

        public void OpenBothExpanders()
        {
            expanderAuthors.IsExpanded = true;
            expanderTitles.IsExpanded = true;
        }

        public CheckBox FindCheckBoxOnTitleListBoxForTitle(string title)
        {
            foreach (CheckBox checkBox in listBoxTitles.Items)
            {
                if (string.Equals(checkBox.Content.ToString(), title))
                {
                    return checkBox;
                }
            }

            return null;
        }

        public void ToggleFilterGroupBox(bool isEnabled, string disablingReason = "")
        {
            groupBoxHeader.IsEnabled = isEnabled;
            buttonMarkAuthors.IsEnabled = isEnabled;
            buttonMarkTitles.IsEnabled = isEnabled;

            groupBoxHeader.ToolTip = disablingReason;
        }

        #endregion Public methods
    }
}
