using Microsoft.OData.ProxyExtensions;
using OfficeTicTacToe.Common;
using OfficeTicTacToe.Graph;
using OfficeTicTacToe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
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
    public sealed partial class GamesView : INotifyPropertyChanged
    {
        public ObservableCollection<UserViewModel> Users { get; set; }
        private static object lockObject = new object();

        private Microsoft.Graph.GraphService graph = AuthenticationHelper.GetGraphService();
        private CancellationTokenSource tokenSource;

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

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

        public GamesView()
        {
            this.InitializeComponent();
            this.Users = new ObservableCollection<UserViewModel>();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            AutoSuggestBox.Text = string.Empty;

        }
        public string Title
        {
            get
            {
                return "search people";
            }
        }

        public Microsoft.Graph.GraphService Graph
        {
            get
            {
                return this.graph;
            }
        }

        public async Task<List<Microsoft.Graph.IUser>> Search(string val, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var lstUsers = new List<UserViewModel>();

            var allusers = await this.Graph.Users.GetUsersLike(val);

            return allusers;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        public void CancelTokenSource()
        {
            if (tokenSource != null)
            {
                lock (lockObject)
                {
                    if (tokenSource != null)
                    {
                        try
                        {
                            tokenSource.Cancel();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

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

        private bool isLoading = false;

        /// <summary>
        /// you can use IsLoading for any loading purpose
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }

            set
            {
                isLoading = value;
                RaisePropertyChanged(nameof(IsLoading));
            }
        }

        private async void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.QueryText))
            {
                this.CancelTokenSource();

                using (TokenSource = new CancellationTokenSource())
                {
                    try
                    {
                        this.IsRefreshButtonEnabled = false;
                        this.IsLoading = true;
                        Users.Clear();

                        List<Microsoft.Graph.IUser> allusers = await Search(sender.Text, TokenSource.Token);

                        this.ListViewResearch.Focus(FocusState.Programmatic);

                        // Updating all organizers
                        List<string> usersMail = new List<string>();

                        foreach (var ev in allusers)
                            usersMail.AddRange(allusers.Select(u => u.UserPrincipalName).ToList());

                        var distinctUsers = usersMail.Distinct().ToList();
                        await UserViewModel.UpdateUsersFromSharepointAsync(distinctUsers, TokenSource.Token);

                        foreach (var user in allusers)
                            Users.Add(UserViewModel.GetUser(user.UserPrincipalName));

                        this.IsLoading = false;
                    }
                    catch (TaskCanceledException ex)
                    {
                        Debug.WriteLine("Task canceled " + ex.Message);
                    }
                    catch (OperationCanceledException ex)
                    {
                        Debug.WriteLine("Operation canceled " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception " + ex.Message);
                    }
                    finally
                    {
                        this.IsRefreshButtonEnabled = true;
                        this.IsLoading = false;
                        this.ListViewResearch.Focus(FocusState.Programmatic);
                    }
                }
            }
        }
        private void UserViewModel_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as UserViewModel;

            if (e == null)
                return;

            item.UserCommand.Execute(null);
        }

       
    }
}
