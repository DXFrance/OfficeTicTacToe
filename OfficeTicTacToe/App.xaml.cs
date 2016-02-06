using Microsoft.WindowsAzure.Messaging;
using OfficeTicTacToe.Common;
using OfficeTicTacToe.Graph;
using OfficeTicTacToe.ViewModels;
using OfficeTicTacToe.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OfficeTicTacToe
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            
        }

        private async void InitNotificationsAsync()
        {
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            var hub = new NotificationHub("OfficeTicTacToeNotificationHub", "Endpoint=sb://tictactoenotifications.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=iojKHPCa3oXRAL7WBK8o+ulDGx8SV0QM6CwiGP8pgv0=");
            Registration result = null;

            try
            {
                result = await hub.RegisterNativeAsync(channel.Uri);
                
            }
            catch (RegistrationException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            // Displays the registration ID so you know it was successful
            if (result?.RegistrationId != null)
            {
                Debug.WriteLine("Channel Registered: " + result.RegistrationId);
            }

            
        }
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            InitNotificationsAsync();

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                //this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Try to authenticate
            var token = await AuthenticationHelper.TryAuthenticateSilentlyAsync();

            GameViewModel game = new GameViewModel();
            game.Board = "    X    ";
            game.CreatedDate = DateTime.UtcNow;
            game.UserIdCreator = "spertus@microsoft.com";
            game.UserIdCurrent = game.UserIdCreator;
            game.UserIdOpponent = "jarvis@tictactoe.com";

            var games = await GameHelper.Current.GetGamesAsync();

            var gameO = games[0];

            var gameP = await GameHelper.Current.UpdateGameAsync(gameO);


            while (!game.IsTerminated)
            {
                game = await GameHelper.Current.GetJarvisMoveAsync("spertus@microsoft.com", game);
                Debug.WriteLine(game.Board);
            }

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                NavigationHelper.Current.RegisterRootFrame(rootFrame);
                NavigationHelper.Current.RegisterEntryPage(typeof(LoginPage));

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;

            }



            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!String.IsNullOrEmpty(token))
                    rootFrame.Navigate(typeof(AppShell), e.Arguments);
                else
                    rootFrame.Navigate(NavigationHelper.Current.EntryPage, e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }





        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
