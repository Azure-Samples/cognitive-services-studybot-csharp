# QnA Maker with LUIS bot in C#

This sample is a LUIS-based web app bot that integrates a QnA Maker chat client. It can be either a stand-alone web app bot that you run/test in a desktop chat client emulator or in the Azure portal, or you can build this bot to be your main bot for the Study Bot sample. If you want to integrate this sample into a website (optional), it can be embedded if you put the endpoint URL found in the Azure portal in a `WebView`, but that is not pursued in this sample.

With this bot sample, there are three knowledge bases from QnA Maker, that you will create, so a user can input a query into the chat client and get back an answer. Since there is more than one knowledge base, LUIS is used to manage them. You will create intents with LUIS to define the user intent for accessing each knowledge base. Each intent has a list of utterances that will be used to return valid answers to the user's questions from the appropriate knowledge base. How [intents](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/luis-concept-intent) and [utterances](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/luis-concept-utterance) work is explained more in the LUIS documentation.

## Prerequisites

1. For the QnA Maker part, you will need to [Create, train, and publish](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/quickstarts/create-publish-knowledge-base) three knowledge bases in [qnamaker.ai](https://www.qnamaker.ai). Refer to the text files in this sample in the `FAQs` folder named QA Biology, QA Sociology, and QA Geology for FAQs you can upload for use. Name your knowledge bases "Biology", "Sociology", and "Geology". You will want to add alternative keywords to your knowledge base questions in qnamaker.ai. These are found in the `Alt questions` folder in the `FAQs` folder. To add them to your knowledge bases, go to `My knowledge bases` in [qnamaker.ai](https://www.qnamaker.ai) and in each knowledge base click the "+" sign near each question (after your knowledge bases have been created).

    <img src="/Assets/alt-question-kb.png">

1. Create a [basic LUIS web app bot](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-quickstart?view=azure-bot-service-3.0) in the [Azure portal](https://ms.portal.azure.com), but using the `Basic Bot (C#)` template in the "Create" panel. After this is created, Azure will automatically create a LUIS app (with the same name as your LUIS web app bot in Azure) in [luis.ai](https://www.luis.ai), where you will create intents later. From this Azure portal app, your LUIS keys (including your web app bot's Microsoft App ID and password) are generated. You will need these for the `Web.config` file in your bot.

1. In [luis.ai](https://www.luis.ai) you will see an app has been auto-created (from your web app bot in Azure) with default intents that get created for every LUIS app: `Greeting`, `Cancel`, and `Help`. You can leave them or delete them, they are optional. But make sure you leave the `None` intent. Although, it's recommended to leave them all. Now, [add intents](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/luis-how-to-add-intents) of your own according to your knowledge bases. For example, if you have a biology knowledge base, you'll want to make a "Biology" intent. This is how LUIS knows to send all user queries in the chat client to the Biology knowledge base in qnamaker.ai. The "add Intents" how-to guide above also shows you how to add utterances, which should mirror what your QnA Maker knowledge bases' words or phrases are in the "Question" part. For example, if you have a Biology knowledge base with 'What is a virus?' as a question... the utterances in the Biology intent (and the alternative words in the "Question" part of your Biology knowledge base) would be "virus", "viral", "viruses", and "bug". Any of these will return the definition (answer) of "virus" in the chat client.

1. After adding your intents, be sure to [train](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-how-to-train) and [publish](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/luis-how-to-publish-app) your luis.ai LUIS app. But right before you publish, make sure Bing Spell Check is enabled on the publish page. 

    <img src="/Assets/bing-spell-check-enabled.png">

1. To get the key for Bing Spell Check create a new resource for it in the [Azure portal](https://ms.portal.azure.com), then navigate to `Keys` in the left menu. This will go into your Web.config file, when you download the zip file of the web app bot from Azure (next steps).

1. Add additional intents for the rest of your knowledge bases. A new intent with its own utterances list should represent each knowledge base.

1. After the LUIS app intents and the QnA Maker knowledge bases are created, download the zip file of your web app bot in Azure by going to `Build` from the `Bot Management` section of your web app bot, then choose "Download zip file", the middle option on that page. This step is critical, as it creates a connection from Azure to your local copy in Visual Studio. If you only try to clone and run the Github sample with added keys, you will have problems with your bot, because the connection to Azure won't be there. However, it's still possible to go this route, see below under "Alternative routes" for more information.

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

Whether or not you test in Visual Studio with the bot emulator or in the Azure portal, the process is the same. Enter queries for the subject matter for your knowledge bases into the chat client. For example, if a user wants to study geology, they could enter the key terms for it, like "epoch" and see that a definition is returned for "epoch". In the geology knowledge base, there are three different kinds of geologic time scales. If a student wanted to understand more about geologic time, but could not remember the key words, they can enter "time" and several different possible definitions will be returned: ones for "era", "epoch", and "period". This is the magic of using LUIS. You can list "time" as an utterance (or any word/phrase you want), and select where this query would go (to the geology knowledge base), and what definitions might be returned. All of this happens through adding utterances and doing training in LUIS.

## Alternative routes

### Using your premade LUIS app

If you already have a LUIS app with intents and utterances, it's possible to import that into the autogenerated app that Azure creates in luis.ai when you first create your LUIS web app bot in Azure.

1. Go to luis.ai and click on your premade app in My Apps.

1. Select the "Settings" tab in the upper right. On the "Settings" page you will see "Versions" at the bottom. Select the three dots to the far right of your app version and select "Export". Then save the JSON file.

    <img src="/Assets/export-json.png">
    
1. Go back to My Apps and select your autogenerated app that was made when you created your LUIS web app bot in Azure. Again, go to the "Settings" tab and look under "Versions" for the "Import new version" button. Select this and upload the JSON file.

    <img src="/Assets/import-json.png">

1. To confirm it has been uploaded, go to the Build tab and you should see all your custom-made intents from your uploaded LUIS app.

### Connecting a web app bot to Azure that was not downloaded directly from Azure

1. If you do not download the zip file from Azure of your web app bot, for example, if you clone a sample app from Github and want to connect it to your Azure account, you can only debug in Visual Studio via remote debugging. This tutorial, [Remote debug your Azure App Service Web App](https://blogs.msdn.microsoft.com/benjaminperkins/2016/09/22/remote-debug-your-azure-app-service-web-app/), shows you how to attach a debugger to your web app on Azure.

1. More on remote debugging web apps: [Troubleshoot a web app in Azure App Service using Visual Studio](https://docs.microsoft.com/en-us/Azure/app-service/web-sites-dotnet-troubleshoot-visual-studio).

## Troubleshooting

### LUIS authoring key has changed or the web app bot's subscription for LUIS has expired

If your LUIS key has been expired or changed, you will have an "Internal Server Error: 500" message in your emulator when you test. Then each query will be answered with, "My bot code has a problem."

Whether you changed (reset) your authoring key in luis.ai or if the web app bot's LUIS subscription has expired, you can easily create a new LUIS resource in Azure and use the key from that. When your web app bot is first made, it automatically created your LUIS app in luis.ai. This means your `LuisAppId`, `LuisAPIKey`, and `LuisAPIHostName` in your web app bot are autogenerated. You can find these in your web app bot's Application Settings menu. But, for example, if you need to reset your authoring key in your LUIS account on luis.ai, the new authoring key must be added to your LUIS app autogenerated from the bot. The authoring key is the same as the `LuisAPIKey`.

1. To view your authoring key, go to luis.ai and click your user name in the top right corner. In that dropdown menu you'll see "Settings", select it. Your authoring key is there. It should be the same key as your LuisAPIKey in your web app bot under Application Settings in Azure. 

    <img src="/Assets/luis-authoring-key.png">
    
1. To correct this 500 error, let's make an independent LUIS resource in Azure. Currently your web app bot is using a LUIS resource, but it's dependent on (not separate from) your bot. But to make it independent, create a new resource for LUIS in Azure. This will help you create a resource: [Create a Cognitive Services APIs account in the Azure portal](https://docs.microsoft.com/en-us/azure/cognitive-services/cognitive-services-apis-create-account), but instead of searching for AI + Machine Learning, search directly for LUIS and fill out the necessary information.

1. Once your resource is created go to your LUIS app, by clicking on it in [luis.ai](https://www.luis.ai/applications) and select the publish tab. At the bottom under Resources and Keys, select the "Add Key" button and fill out the required information in the popup. 

    <img src="/Assets/add-key-luis.png">

In the popup, When selecting a key, make sure you select the name of your new LUIS resource you created in the Azure portal. It will copy the keys from that resource. Once added, you will see the new key in Resources and Keys.

1. From this new key added, you can use either of the two "Key String"s listed, just be sure the one you choose is consistently used throughout your app in both Visual Studio and Azure. In other words, the keys are not interchangeable.

1. Copy one of the keys in your new key you just added, by clicking on the blue papers icon next to the key.

1. Look in your `Web.config` file in the `<appSettings>` tags of the Qna-Luis-Bot sample. Add this key as a value to `LuisAPIKey`.

1. Now we can go back to the Azure portal and "recycle" the web app bot. To do this select your web app bot and go to "All App service settings".

    <img src="/Assets/all-app-service.png">
    
1. Select the "Stop" button, then select "Start". This recycles your web app bot and configures the bot with the new key.

    <img src="/Assets/recycle-bot.png">
    
1. Rebuild and run the sample. The bot chat client should work now!
