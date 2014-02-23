﻿using System;
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

        KindleClippingsParserController m_Controller;

        #endregion Private fields
        #region Properties

        public bool IsAnyAuthorUnselected
        {
            get
            {
                foreach(object item in listBoxAuthors.Items)
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
            test_DisplayClippings();
        }

        #endregion Ctors
        #region Event handlers

        private void buttonMarkAuthors_Click(object sender, RoutedEventArgs e)
        {
            m_Controller.ButtonMarkAuthorsClick(listBoxAuthors);
        }

        private void buttonMarkTitles_Click(object sender, RoutedEventArgs e)
        {
            m_Controller.ButtonMarkTitlesClick(listBoxTitles);
        }

        #endregion Event handlers
        #region Public methods

        public void SetCheckBoxesSelection(ListBox listBox, bool state)
        {
            foreach(object item in listBox.Items)
            {
                if (item.GetType() == typeof(CheckBox))
                {
                    ((CheckBox)item).IsChecked = state;
                }                
            }
        }

        #endregion Public methods


        #region FOR TESTING PURPOSES

        private void test_DisplayClippings()
        {
            m_Controller.DisplayClippings(stackPanelClippings);
            m_Controller.PopulateAuthorsList(listBoxAuthors);
            m_Controller.PopulateTitlesList(listBoxTitles);
        }

        #endregion FOR TESTING PURPOSES
    }   
}
