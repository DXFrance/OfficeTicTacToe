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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OfficeTicTacToe.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BoardView : IRefreshPage
    {
        private CancellationTokenSource tokenSource;

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

        public BoardView()
        {
            this.InitializeComponent();
         
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }
      
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

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
       

    }


}
