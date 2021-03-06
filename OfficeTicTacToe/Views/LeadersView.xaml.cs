﻿using OfficeTicTacToe.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OfficeTicTacToe.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LeadersView
    {
        public LeadersView()
        {
            this.InitializeComponent();
        }
        public string Title
        {
            get
            {
                return "settings";
            }
        }
        private void DisconnectionButton_Click(object sender, RoutedEventArgs e)
        {
            AppShell.Current.Navigate(typeof(DisconnectPage));
        }
    }
}
