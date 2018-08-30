using System;
using System.Configuration;
using System.Threading.Tasks;
using LuisBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"],
            ConfigurationManager.AppSettings["LuisAPIKey"],
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])
        {
            SpellCheck = true,
            BingSpellCheckSubscriptionKey = ConfigurationManager.AppSettings["BingSpellCheckSubscriptionKey"]
        }))
        {
        }

        // Make reference to the knowledge base ID, endpoint host name, and authorization key... in Web.config
        static readonly string biologyKB = ConfigurationManager.AppSettings["BiologyKBId"];
        static readonly string biologyHostName = ConfigurationManager.AppSettings["BiologyQnAEndpointHostName"];
        static readonly string biologyAuthKey = ConfigurationManager.AppSettings["BiologyQnAAuthKey"];

        static readonly string geologyKB = ConfigurationManager.AppSettings["GeologyKBId"];
        static readonly string geologyHostName = ConfigurationManager.AppSettings["GeologyQnAEndpointHostName"];
        static readonly string geologyAuthKey = ConfigurationManager.AppSettings["GeologyQnAAuthKey"];

        static readonly string sociologyKB = ConfigurationManager.AppSettings["SociologyKBId"];
        static readonly string sociologyHostName = ConfigurationManager.AppSettings["SociologyQnAEndpointHostName"];
        static readonly string sociologyAuthKey = ConfigurationManager.AppSettings["SociologyQnAAuthKey"];

        // Instantiate the QnAMakerService class for each of your knowledge bases, QnAMakerService(QnAHostName, KBId, QnAEndpointKey)
        public QnAMakerService biologyQnAService = new QnAMakerService(biologyHostName, biologyKB, biologyAuthKey);
        public QnAMakerService sociologyQnAService = new QnAMakerService(sociologyHostName, sociologyKB, sociologyAuthKey);
        public QnAMakerService geologyQnAService = new QnAMakerService(geologyHostName, geologyKB, geologyAuthKey);

        // Default intents (None, Greeting, Cancel, Help)
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        // Custom-made intents (StudyBiology, StudySociology, StudyGeology)
        [LuisIntent("StudyBiology")]
        public async Task StudyBiologyIntent(IDialogContext context, LuisResult result)
        {
#if DEBUG
            await context.PostAsync($"Biology intent detected.");
#endif

            await context.PostAsync(biologyQnAService.GetAnswer(GetCorrectQuery(result)));
        }

        [LuisIntent("StudySociology")]
        public async Task StudySociologyIntent(IDialogContext context, LuisResult result)
        {
#if DEBUG
            await context.PostAsync($"Sociology intent detected.");
#endif

            await context.PostAsync(sociologyQnAService.GetAnswer(GetCorrectQuery(result)));
        }

        [LuisIntent("StudyGeology")]
        public async Task StudyGeologyIntent(IDialogContext context, LuisResult result)
        {
#if DEBUG
            await context.PostAsync($"Geology intent detected.");
#endif

            await context.PostAsync(geologyQnAService.GetAnswer(GetCorrectQuery(result)));
        }

        // Chooses the right query if misspelled or not
        private static string GetCorrectQuery(LuisResult result)
        {
            return result.AlteredQuery ?? result.Query;
        }

        // Default intents get some customization
        private async Task ShowLuisResult(IDialogContext context, LuisResult result)
        {
            string endResult = GetCorrectQuery(result);

            string biology = "biology";
            string sociology = "sociology";
            string geology = "geology";

            if (result.Intents[0].Intent.Equals("Greeting", StringComparison.InvariantCultureIgnoreCase))
            {
                await context.PostAsync($"Hello, welcome to Study Bot! Which subject would you like to study: {biology}, {sociology}, or {geology}?");
            }
            else if (result.Intents[0].Intent == "Help")
            {
                await context.PostAsync($"How to use Study Bot: type a topic you want to study, like {biology}, {sociology}, or {geology}. " +
                "Or, start typing a study term. For instance, typing 'lava' will show a definition of 'magma' from geology.");
            }
            else if (result.Intents[0].Intent == "Cancel")
            {
                await context.PostAsync("Thanks for studying with Study Bot! Goodbye.");
            }
            else if (result.Intents[0].Intent == "None")
            {
                await context.PostAsync("Your entry \"" + endResult + "\" is not in the study list. Perhaps it is misspelled?");
            }

            context.Wait(MessageReceived);
        }
    }
}
