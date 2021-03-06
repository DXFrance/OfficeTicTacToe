﻿using OfficeTicTacToe.Common;
using OfficeTicTacToe.Graph;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OfficeTicTacToe.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DisconnectPage
    {
        public bool IsPageEnabled
        {
            get
            {
                return AppShell.Current.Splitter.IsPaneOpen;
            }
            set
            {
                AppShell.Current.Splitter.IsPaneOpen = value;
                AppShell.Current.Splitter.IsEnabled = value;
                AppShell.Current.ShellFrame.IsEnabled = value;

            }
        }

        private async System.Threading.Tasks.Task Disconnect()
        {

            this.IsPageEnabled = false;

            await AuthenticationHelper.SignOutAsync();

            this.IsPageEnabled = true;

            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(LoginPage));
        }

        public string Title
        {
            get
            {
                return "disconnection in progress...";
            }
        }
        public DisconnectPage()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await Disconnect();
        }
       
    }
}
