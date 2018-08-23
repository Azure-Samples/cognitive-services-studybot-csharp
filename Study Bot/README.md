# Study Bot

This sample is a UWP app that combines a LUIS-based web app bot (with Bing Spell Check enabled in luis.ai) with three QnA Maker knowledge bases to provide a question and answer chat client for a student. It also contains a relevant website query based on the chat queries entered by the student, for instance, if the user wants to know the definition of a "virus". The queries also provide search terms for an encyclopedia and a Microsoft Academic `WebView` in the app. 

## Prerequisites

1. Follow the [prerequisites for the Qna-Luis-Bot](https://github.com/Azure-Samples/cognitive-services-studybot-csharp/blob/master/Qna-Luis-Bot/readme.md) insofar as creating three knowledge bases (qnamaker.ai) with some accompanied LUIS intents/utterances (luis.ai), and a LUIS-based web app bot (Azure portal). This bot will use the same concept for a Study Bot as the Qna-Luis-Bot uses so go ahead and use its FAQ text files, alternative words, and intents/utterances as mentioned in the prerequisites. There's no need to copy all those keys from Web.config, however. You only need a bot name and secret key for this sample (detailed below).

1. Open this Study Bot solution file in Visual Studio 2017+.

1. In `MainPage.xaml.cs`, add your bot's name and secret key to `botHandle` and `botSecretkey`, respectively. To find the key, go to your LUIS web app bot in the [Azure portal](https://ms.portal.azure.com) under the Channels menu item, then select `Edit` to the right of your bot. You'll see your secret key that you can show and copy to paste into the above URL. Your bot name, verbatum, is needed for the `botHandle`.
    
    <img src="/Assets/bot-secret-key.png">
    
1. NuGet packages needed: 

    Microsoft.Bot.Connector.DirectLine
    
    Microsoft.NETCore.UniversalWindowsPlatform
    
    Microsoft.Rest.ClientRuntime
    
    Newtonsoft.Json
    
## Run and Test the sample

1. In the UWP interface, enter a query, such as "biology".

1. Enter any of your QnA Maker question terms you created.

1. The definition for these terms gets returned, or if the term does not exist in any of the knowledge bases, it will say "No good match found in Study Bot".

1. The search query also shows the term in the encyclopedia and Microsoft Academic.

1. Experiment by adding or removing your QnA Maker knowledge base question and answers (in qnamaker.ai) and your LUIS intents/utterances (in luis.ai) and try new queries based on these changes.
