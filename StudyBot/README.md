# Study Bot

This sample is a UWP app that combines a Basic, C# web app bot made in the [Azure portal](https://ms.portal.azure.com/#home) with [QnA Maker](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/index) knowledge bases (botv4 of this sample includes [Chit-chat](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/chit-chat-knowledge-base)), [Language Understanding (LUIS)](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/), [Speech Service](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/), and [Bing Spell Check](https://docs.microsoft.com/en-us/azure/cognitive-services/bing-spell-check/) to provide a question and answer chat client for a student. It also contains a relevant website query based on the chat queries entered by the student, for instance, if the user wants to know the definition of a "virus". The queries will then act as search terms for an encyclopedia, Microsoft Academic, and a Bing search engine in their respective `WebView`s in the app. 

## Prerequisites

1. You will need the Qna-Luis-Bot built and published back to your Azure account before running this app. 

1. Follow the prerequisites for the Qna-Luis-Bot [v3](https://github.com/Azure-Samples/cognitive-services-studybot-csharp/blob/master/Qna-Luis-Bot/readme.md) or [v4](https://github.com/Azure-Samples/cognitive-services-studybot-csharp/tree/master/Qna-Luis-Botv4) that will help you create knowledge bases in qnamaker.ai with some accompanied LUIS intents/utterances in luis.ai, and a Basic C# web app bot in the Azure portal. 

1. Once your Qna-Luis-Bot sample is ready, you'll need its bot name and secret key for this sample (detailed below).

1. After cloning the main repo, open this Study Bot solution file in Visual Studio 2017+.

1. In `MainPage.xaml.cs`, add your Qna-Luis-Bot's bot's name (verbatim) you chose in Azure to `botHandle`. For example, Qna-Luis-Bot-v4.

1. Since the Study Bot UWP app is considered an external client that needs to access the bot in Azure, we'll need to connect it to a Channel called [Direct Line](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-channel-connect-directline?view=azure-bot-service-3.0). To do this go to the Channels menu in your web app bot resource in Azure and click the globe icon.

    <img src="/Assets/enable-directline.png">

1. This initializes the Direct Line channel. A popup appears that has your bot secret key.

1.  Click show and copy (either key will work), then paste into `MainPage.xaml.cs` at the top.
    
    <img src="/Assets/bot-secret-key.png">

1. Click "Done" at the bottom. Then you will see Direct Line has been added next to Web Chat.

    <img src="/Assets/directline-done.png">
    
1. In `MainPage.xaml.cs`, add your Direct Line secret key, bot name, Speech Service subscription key, and region to the top of the file where indicated.

1. The main NuGet packages needed:

    Microsoft.Bot.Connector.DirectLine
    
    Microsoft.NETCore.UniversalWindowsPlatform
    
    Microsoft.Rest.ClientRuntime
    
    Microsoft.CognitiveServices.Speech
    
    Newtonsoft.Json
    
## Run and Test the sample

1. Run your StudyBot solution file in Visual Studio.

1. In the UWP interface that appears, enter a query, such as "virus".

1. Enter any of your QnA Maker question terms you created in your bot as directed above.

1. The definition for this term gets returned, or if the term does not exist in any of the knowledge bases, it will say "No good match found in Study Bot".

1. The search query also shows the term as an encyclopedia, Microsoft Academic, and Bing search engine search.

1. Experiment by adding or removing your QnA Maker knowledge base question and answers (in qnamaker.ai) and your LUIS intents/utterances (in luis.ai) and try new queries based on these changes.
