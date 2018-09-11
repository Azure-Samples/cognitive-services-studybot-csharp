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
using System.Text.RegularExpressions;
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

namespace StudyBot
{
    public sealed partial class MainPage : Page
    {
        DirectLineClient _client;
        Conversation _conversation;
        ObservableCollection<Activity> _messagesFromBot;

        Activity newActivity;
        string botSecretKey = "ADD YOUR SECRET KEY HERE";
        string botHandle = "ADD YOUR BOT NAME HERE";
        string query;
        string subject;
        string kbName1 = "biology";
        string kbName2 = "geology";
        string kbName3 = "sociology";

        // Option to create a user ID and name
        string userId = "";
        string userName = "";

        // Will handle query to add to Bing Search
        static string[] biologyQuestions = new string[]{"biology", "virus", "bug", "bacteria", "parasite", "asexual",
                       "sexual", "sex", "reproduction", "cancer", "tumor", "blood brain barrier"};
        static string[] geologyQuestions = new string[]{"geology", "magnitude", "magma", "lava", "rock", "metamorphic",
                                                                "era", "period", "epoch", "time" };
        static string[] sociologyQuestions = new string[]{"sociology", "poverty", "minority", "cultural", "pluralism",
                                     "sterotype", "affirmative action", "apartheid", "bicultural"};
        HashSet<string> set1 = new HashSet<string>(biologyQuestions);
        HashSet<string> set2 = new HashSet<string>(geologyQuestions);
        HashSet<string> set3 = new HashSet<string>(sociologyQuestions);

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

        // Send button
        private async void button_Click(object sender, RoutedEventArgs e)
        {
            await sendMessageToBot();

            inputQueryToWebsites();
        }

        // Handle button click when user wants to send message to bot.
        async Task sendMessageToBot()
        {
            // Activity object with (optional) name of the user and text.
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

                        // Auto-scrolls to last item in chat
                        MessagesList?.ScrollIntoView(MessagesList.Items[_messagesFromBot.Count - 1], ScrollIntoViewAlignment.Leading);
                    });
                }

                await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);
            }
        }

        // Removes punctuation from query 
        static string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                return Regex.Replace(strIn, @"[^\w\\'\.-]", " ", // won't remove chars in [], otherwise will with empty space
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters, return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        // Handles when 'Enter' pressed after chat entry
        private async void NewMessageTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = true;
                await sendMessageToBot();

                inputQueryToWebsites();
            }
        }

	// Decides how to search in websites based on query
        private void inputQueryToWebsites()
        {
            // Gets query for other uses.
            query = NewMessageTextBox.Text;

            // Strip query of punctuation
            query = CleanInput(query);

            // Get subject of query (topic of your knowledge base)
            if (set1.Contains(query))
            {
                if (query == kbName1)
                {
                    subject = " "; // don't add a subject if query is already subject word
                }
                else
                {
                    subject = kbName1;
                }
            }
            else if (set2.Contains(query))
            {
                if (query == kbName2)
                {
                    subject = " "; // don't add a subject if query is already subject word
                }
                else
                {
                    subject = kbName2;
                }
            }
            else if (set3.Contains(query))
            {
                if (query == kbName3)
                {
                    subject = " "; // don't add a subject if query is already subject word
                }
                else
                {
                    subject = kbName3;
                }
            }
            else // if no subject, then must be a LUIS default intent (Greeting, Cancel, Help, or None)
            {
                subject = "";
		query = "";

		// Microsoft academic needs the root URL to render, rather than empty query/subject in URL
		MicrosoftAcademic.Navigate(new Uri("https://academic.microsoft.com/"));
	    }

            // Set query into Encyclopedia, Microsoft Academics, and Bing Search
            Encyclopedia.Navigate(new Uri("https://en.wikipedia.org/wiki/" + query));
	    NewsBlogs.Navigate(new Uri("https://www.bing.com/search?q=" + query + "%20" + subject + "&qs=n&form=QBRE&sp=-1&pq=" + query + "%20" + subject + "&sc=8-5&sk=&cvid=92D86BDF64C049B3AD2DC5444AB33E25"));

	if (subject != "" && query != "")
		MicrosoftAcademic.Navigate(new Uri("https://academic.microsoft.com/#/search?iq=@" + query + "@&amp;q=" + query + "&filters=&from=0&sort=0"));

	// Clears text for next query.
	NewMessageTextBox.Text = String.Empty;
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
