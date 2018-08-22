using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Study_Bot
{
    public sealed partial class MainPage : Page
    {
        DirectLineClient _client;
        Conversation _conversation;
        ObservableCollection<Activity> _messagesFromBot;

        Activity newActivity;
        string botSecretKey = "<ENTER YOUR BOT SECRET KEY HERE>";
        string botHandle = "<ENTER YOUR BOT NAME HERE>";
        string query;

        // Option to create a user ID and name
        string userId = "";
        string userName = "";

        public MainPage()
        {
            this.InitializeComponent();

            // Set binding context to update message list items.
            DataContext = this;

            NewMessageTextBox.PlaceholderText = "Type want you want to study here.";
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // Create collection for bot messages.
            _messagesFromBot = new ObservableCollection<Activity>();
            // Initialize conversation with bot.
            await InitializeBotConversation();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            await sendMessageToBot();
        }

        // Handle button click when user wants to send message to bot.
        async Task sendMessageToBot()
        {
            // Activity object with name of the user and text.
            newActivity = new Activity { From = new ChannelAccount(userId, userName), Text = NewMessageTextBox.Text, Type = ActivityTypes.Message };

            // Post message to your bot. 
            if (_conversation != null)
            {
                await _client.Conversations.PostActivityAsync(_conversation.ConversationId, newActivity);
            }
        }

        async Task InitializeBotConversation()
        {
            // Initialize Direct Client with secret obtained in the Bot Portal.
            _client = new DirectLineClient(botSecretKey);
            // Initialize new converstation.
            _conversation = await _client.Conversations.StartConversationAsync();
            // Wait for the responses from bot.
            await ReadBotMessagesAsync(_client, _conversation.ConversationId);
        }

        // Handles messages from bot.
        async Task ReadBotMessagesAsync(DirectLineClient client, string conversationId)
        {
            // Optionally set watermark - this is last message id seen by bot. It is for paging.
            string watermark = null;

            while (true)
            {
                // Get all messages returned by bot.
                var convActivities = await client.Conversations.GetActivitiesAsync(conversationId, watermark);

                watermark = convActivities?.Watermark;

                // Get messages from your bot - From.Name should match your Bot Handle.
                var messagesFromBotText = from x in convActivities.Activities
                                          where x.From.Name == botHandle
                                          select x;

                // Iterate through all messages.
                foreach (Activity message in messagesFromBotText)
                {
                    message.Text = userName + message.Text;
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => {
                        // Add message to the list and update ListView source to display response on the UI.
                        if (!_messagesFromBot.Contains(message))
                        {
                            _messagesFromBot.Add(newActivity); // Adds user query to chat window.
                            _messagesFromBot.Add(message);
                        }
                        MessagesList.ItemsSource = _messagesFromBot;
                    });
                }

                await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);
            }
        }

        // Handles when 'Enter' pressed in keyboard, plus stashes query.
        private async void NewMessageTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = true;
                await sendMessageToBot();

                // Gets query for other uses.
                query = NewMessageTextBox.Text;

                // Set query into Encyclopedia and Microsoft Academics
                Encyclopedia.Navigate(new Uri("https://en.wikipedia.org/wiki/" + query));
                MicrosoftAcademic.Navigate(new Uri("https://academic.microsoft.com/#/search?iq=@" + query + "@&amp;q=" + query + "&filters=&from=0&sort=0"));

                // Clears text for next query.
                NewMessageTextBox.Text = String.Empty;
            }
        }

        // Back button for Pivot.
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (rootPivot.SelectedIndex > 0)
            {
                // If not at the first item, go back to the previous one.
                rootPivot.SelectedIndex -= 1;
            }
            else
            {
                // The first PivotItem is selected, so loop around to the last item.
                rootPivot.SelectedIndex = rootPivot.Items.Count - 1;
            }
        }

        // Next button for Pivot.
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (rootPivot.SelectedIndex < rootPivot.Items.Count - 1)
            {
                // If not at the last item, go to the next one.
                rootPivot.SelectedIndex += 1;
            }
            else
            {
                // The last PivotItem is selected, so loop around to the first item.
                rootPivot.SelectedIndex = 0;
            }
        }

        private void journals_LoadCompleted(object sender, NavigationEventArgs e)
        {

        }
    }
}
