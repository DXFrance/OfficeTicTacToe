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
using OfficeTicTacToe.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OfficeTicTacToe.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamesView : INotifyPropertyChanged
    {
        private static readonly object lockObject = new object();
        public ObservableCollection<UserViewModel> Users { get; set; }
        public ObservableCollection<GameViewModel> Games { get; set; }
        public ObservableCollection<UserViewModel> TeamWork { get; set; }


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
            Users = new ObservableCollection<UserViewModel>();
            Games = new ObservableCollection<GameViewModel>();
            TeamWork = new ObservableCollection<UserViewModel>();

            AutoSuggestBox.Text = string.Empty;
            this.NavigationCacheMode = NavigationCacheMode.Required;
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
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var games = await GameHelper.Current.GetGamesByUserIdAsync(UserViewModel.CurrentUser);
            if (games != null)
            {
                Games.Clear();
                foreach (var game in games)
                {
                    Games.Add(game);
                }
            }

            List<string> users = new List<string>();
            foreach(var g in Games)
            {
                users.Add(g.UserIdCreator);
                users.Add(g.UserIdOpponent);
            }

            this.TeamWork.Clear();

            var me = UserViewModel.GetUser(UserViewModel.CurrentUser);
            var me2 = await SharePointSearchHelper.SPGetUsers(new[] { me.UserPrincipalName });

            if (me2.Count <= 0)
                return;

            var teamWork = await SharePointSearchHelper.SPGetWorkingWithUsers(me2[0].DocId);

            List<UserViewModel> users = new List<UserViewModel>();
            foreach (var u in teamWork)
            {
                UserViewModel uvm = UserViewModel.MergeFromSharepoint(UserViewModel.GetUser(u.UserName), u);
                this.TeamWork.Add(uvm);
            }
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
        private async void UserViewModel_ItemClick(object sender, ItemClickEventArgs e)
        {
            var user = e?.ClickedItem as UserViewModel;
            if (user == null)
                return;
            var game = new GameViewModel();
            game.CreatedDate = DateTime.Now;
            game.UserIdCreator = UserViewModel.CurrentUser;
            game.UserIdOpponent = user.UserPrincipalName;
            game = await GameHelper.Current.CreateGameAsync(game);
            AppShell.Current.Navigate(typeof(BoardView), game);
        }
        private void GamesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var game = e?.ClickedItem as GameViewModel;
            if (game == null)
                return;
            //GameCommand.Execute(null);
            AppShell.Current.Navigate(typeof(BoardView), game);
        }
    }
}
