# QnA Maker with LUIS bot in C#

This sample is a LUIS-based web app bot that integrates a QnA Maker chat client. It can be either a stand-alone web app bot that you run/test in a desktop chat client emulator or in the Azure portal, or you can build this bot to be your main bot for the Study Bot sample. If you want to integrate this sample into a website (optional), it can be embedded if you put the endpoint URL found in the Azure portal in a `WebView`, but that is not pursued in this sample.

With this bot sample, there are three knowledge bases from QnA Maker, that you will create, so a user can input a query into the chat client and get back an answer. Since there is more than one knowledge base, LUIS is used to manage them. You will create intents with LUIS to define the user intent for accessing each knowledge base. Each intent has a list of utterances that will be used to return valid answers to the user's questions from the appropriate knowledge base. How [intents](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/luis-concept-intent) and [utterances](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/luis-concept-utterance) work is explained more in the LUIS documentation.

## Prerequisites

1. For the QnA Maker part, you will need to [Create, train, and publish](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/quickstarts/create-publish-knowledge-base) three knowledge bases in [qnamaker.ai](https://www.qnamaker.ai). Refer to the text files in this sample in the `FAQs` folder named QA Biology, QA Sociology, and QA Geology for FAQs you can upload for use. Name your knowledge bases "Biology", "Sociology", and "Geology". You will want to add alternative keywords to your knowledge base questions in qnamaker.ai. These are found in the `Alt questions` folder in the `FAQs` folder. To add them to your knowledge bases, go to `My knowledge bases` in [qnamaker.ai](https://www.qnamaker.ai) and in each knowledge base click the "+" sign near each question (after your knowledge bases have been created).

    <img src="/Assets/alt-question-kb.png">

1. Create a [basic LUIS web app bot](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-quickstart?view=azure-bot-service-3.0) in the [Azure portal](https://ms.portal.azure.com), but instead of using the `Basic, C#` template, use the `Language Understanding, C#` template in the "Create" panel. NOTE: be sure to choose `Select` at the bottom of the panel after choosing a template, otherwise the default `Basic, C#` will be used. After this is created, Azure will automatically create a LUIS app (with the same name as your LUIS web app bot in Azure) in [luis.ai](https://www.luis.ai), where you will create intents later. From this Azure portal app, your LUIS keys (including your web app bot's Microsoft App ID and password) are generated. You will need these for the `Web.config` file in your bot.

1. In [luis.ai](https://www.luis.ai) you will see an app has been auto-created (from your web app bot in Azure) with default intents that get created for every LUIS app: `Greeting`, `Cancel`, and `Help`. You can leave them or delete them, they are optional. But make sure you leave the `None` intent. Although, it's recommended to leave them all. Now, [add intents](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/luis-how-to-add-intents) of your own according to your knowledge bases. For example, if you have a biology knowledge base, you'll want to make a "Biology" intent. This is how LUIS knows to send all user queries in the chat client to the Biology knowledge base in qnamaker.ai. The "add Intents" how-to guide above also shows you how to add utterances, which should mirror what your QnA Maker knowledge bases' words or phrases are in the "Question" part. For example, if you have a Biology knowledge base with 'What is a virus?' as a question... the utterances in the Biology intent (and the alternative words in the "Question" part of your Biology knowledge base) would be "virus", "viral", "viruses", and "bug". Any of these will return the definition (answer) of "virus" in the chat client.

1. After adding your intents, be sure to [train](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-how-to-train) and [publish](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/luis-how-to-publish-app) your luis.ai LUIS app. But right before you publish, make sure Bing Spell Check is enabled on the publish page. 

    <img src="/Assets/bing-spell-check-enabled.png">

1. To get the key for Bing Spell Check create a new resource for it in the [Azure portal](https://ms.portal.azure.com), then navigate to `Keys` in the left menu. This will go into your Web.config file, when you download the zip file of the web app bot from Azure (next steps).

1. Add additional intents for the rest of your knowledge bases. A new intent with its own utterances list should represent each knowledge base.

1. After the LUIS app intents and the QnA Maker knowledge bases are created, download the zip file of your web app bot in Azure by going to `Build` from the `Bot Management` section of your web app bot, then choose "Download zip file", the middle option on that page.

    <img src="/Assets/download-zip.png">
    
1. Open the solution file in Visual Studio 2017. Copy/paste relevant code from these files in the sample: `Models > QnAMakerService.cs` (entire folder/class), `Dialogs > BasicLuisDialog.cs` (Constructor and QnA Maker intents/instantiations) and `Web.config` (everything in the `<appSettings>` tags) into your Visual Studio web app bot's corresponding files.

1. Finally, provide the app-specific keys and IDs to your Web.config file. You can find this specific information for your LUIS web app bot in the Azure portal of `Application Settings` under App Service Settings. For QnA Maker knowledge bases, you can find host name, knowledge base ID, and authorization key by selecting "View Code" to the right of your knowledge base in qnamaker.ai. 

    <img src="/Assets/view-code.png">
    
1. Include your Bing Spell Check key from the resource you created in Azure, above, into your Web.config file as well.
    
1. Make sure your variable names (in case you change them) in Web.config match the instantiations in BasicLuisDialog.cs.

    <img src="/Assets/instantiation.png">

## Run and Test the sample

There are two ways to run and test this sample. One is in Visual Studio with a desktop Bot Emulator and the second way is through the Azure portal's App Service Editor.  

### Visual Studio
1. Download the [Bot Framework Emulator](https://github.com/microsoft/botframework-emulator) or [download here](https://github.com/Microsoft/BotFramework-Emulator/releases), so you can run the sample in Visual Studio and, while your app is running, use the chat client emulator. Running the sample will show a localhost web browser page. If you see text (see below), it was a success. You then use that port number in the URL in your emulator.

    <img src="/Assets/local-host.png">
    
1. Once your emulator is open and ready, be sure to add the URL: `http://localhost:{YOUR PORT NUMBER}/api/messages` to the top blue field (see below). Add your LUIS web app bot Microsoft App ID and password, then locale (ex: USWest) and choose "Connect". It takes 15-20 seconds to get a `POST 200` confirmation, but once you see it, you can begin typing in keywords(s) to access the knowledge bases. The app in Visual Studio must be running for the emulator to work, otherwise you will see a `POST net::ERR_CONNECTION_REFUSED` error in the emulator. You will also see this error if you have the wrong Microsoft App ID and Password. A successful knowledge base retrieval will return the definition(s) of the word(s) you entered.

    <img src="/Assets/emulator.png">

### Azure App Service Editor

Before you test your modified local bot in Azure, it needs to be published from Visual Studio back to Azure.

1. In Visual Studio's Solution Explorer, right-click on the project file and select `Publish` to open the `Publish` pane. Select `New Profile` beneath the profile name. A popup appears called "Pick a publish target". Select `Import Profile` in the lower left. Navigate to the `PostDeployScripts` folder in your app and select its `YourBotName.PublishSettings`. Back in the Publish pane, select `Publish`. The next time you publish, you only need to open the `Publish` pane and select `Publish`.

1. Open your bot you created in the Azure portal and choose `Build` from the left navigation panel. Then choose `Open online code editor`.
    
    <img src="/Assets/open-online-code-editor.png">

1. In the App Service Editor, right-click on `build.cmd` under Properties and select `Run from Console`. If you get an error, open the Kudu console (below) from the upper left section of the Azure portal. Once in Kudu, go to `D:\home\site\wwwroot>` then type `build.cmd` to build.

    <img src="/Assets/open-kudu-console.png">

1. Once built, you can return to the Azure portal and choose "Test in Web Chat" under "Bot Management". The chat bot will open for you to start entering queries.

### How to test

Whether or not you test in Visual Studio with the bot emulator or in the Azure portal, the process is the same. Enter queries for the subject matter for your knowledge bases. For example, if a user wants to study geology, they could enter the key terms for it, like "epoch" and see that a definition is returned for "epoch". There are three different kinds of geologic time scales in this sample, so say a student wanted to understand more about geologic time, but could not remember the key words, they can enter "time" and several different possible definitions will be returned. Ones for "era", "epoch", and "period". This is the magic of using LUIS. You can list "time" as an utterance, and select where this query would go (to the geology knowledge base), and what definitions might be returned. All of this happens through adding utterances and doing training in LUIS.
