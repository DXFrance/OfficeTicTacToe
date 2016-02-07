using Microsoft.OData.Client;
using OfficeTicTacToe.Common;
using OfficeTicTacToe.Graph;
using OfficeTicTacToe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;
using System.Diagnostics;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

using OfficeTicTacToe.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OfficeTicTacToe.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BoardView : IRefreshPage
    {
        private CancellationTokenSource tokenSource;
        public GameViewModel Game { get; set; }
        public CancellationTokenSource TokenSource
        {
            get
            {
                return this.tokenSource;
            }
            set
            {
                this.tokenSource = value;
            }
        }

        public bool IsRefreshButtonEnabled
        {
            get
            {
                return AppShell.Current.RefreshButton.IsEnabled;
            }
            set
            {
                AppShell.Current.RefreshButton.IsEnabled = value;
            }
        }

        public string Title
        {
            get
            {
                return "office tic tac toe";
            }
        }

        TaskScheduler uiScheduler;
        public BoardView()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            ((App)(App.Current)).Channel.PushNotificationReceived += Channel_PushNotificationReceived;
        }

        private async void Channel_PushNotificationReceived(Windows.Networking.PushNotifications.PushNotificationChannel sender, Windows.Networking.PushNotifications.PushNotificationReceivedEventArgs args)
        {
            Debug.WriteLine("Notification received");
            await Task.Factory.StartNew(Refresh, CancellationToken.None, TaskCreationOptions.None, uiScheduler);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Game = e?.Parameter as GameViewModel ?? new GameViewModel();
            DataContext = Game;
            using (this.TokenSource = new CancellationTokenSource())
            {
                try
                {
                    this.IsRefreshButtonEnabled = false;
                    await Refresh(this.TokenSource.Token, false);

                    
                }
                catch (OperationCanceledException ex)
                {
                    Debug.WriteLine("Operation canceled " + ex.Message);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    this.IsRefreshButtonEnabled = true;
                }

            }

        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        public async Task Refresh()
        {
            using (this.TokenSource = new CancellationTokenSource())
            {
                try
                {
                    this.IsRefreshButtonEnabled = false;
                    await Refresh(this.TokenSource.Token, true);
                }
                catch (OperationCanceledException ex)
                {
                    Debug.WriteLine("Operation canceled " + ex.Message);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    this.IsRefreshButtonEnabled = true;
                }

            }
        }
        public async Task Refresh(CancellationToken token, bool forceRefresh = false)
        {

            if (Game != null)
                await Game.Refresh();

            StackPanelLoader.Visibility = Visibility.Visible;
            StackPanelLoader.Opacity = 1.0d;

            //ProgressRingLoader.Visibility = Visibility.Visible;
            //ProgressRingLoader.IsActive = true;


            //DoubleAnimation animationOpacity = new DoubleAnimation();
            //animationOpacity.To = 0.0d;
            //animationOpacity.From = 1.0d;
            //Storyboard.SetTarget(animationOpacity, StackPanelLoader);
            //Storyboard.SetTargetProperty(animationOpacity, "Opacity");

            //DoubleAnimation animationOpacity2 = new DoubleAnimation();
            //animationOpacity2.To = 1.0d;
            //animationOpacity2.From = 0.0d;
            //Storyboard.SetTarget(animationOpacity2, EventViewList);
            //Storyboard.SetTargetProperty(animationOpacity2, "Opacity");

            //Storyboard sb = new Storyboard();
            //sb.Children.Add(animationOpacity);
            //sb.Children.Add(animationOpacity2);

            //sb.Duration = animationOpacity.Duration = animationOpacity2.Duration = TimeSpan.FromMilliseconds(150);

            //sb.Completed += (s, o) =>
            //{

            //    ProgressRingLoader.IsActive = false;
            //    ProgressRingLoader.Visibility = Visibility.Collapsed;
            //    StackPanelLoader.Visibility = Visibility.Collapsed;
            //    AppShell.Current.SetTitle("next meetings");

            //};

            // sb.Begin();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            var cell = Convert.ToInt16(button.Tag);
            if ((cell < 0) || (cell > Game.Board.Length))
                return;
            var board = Game.InitialBoard.ToArray();
            if (board[cell] == ' ')
            {
                board[cell] = Game.CurrentPawn;
                Game.Board = new string(board);
            }

            await Game.Update();
        }
    }
}
