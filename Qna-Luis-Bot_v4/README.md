# Qna-Luis-Botv4

This sample bot has been created using the [Microsoft Bot Framework](https://dev.botframework.com), in particular, the [Dispatch](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-tutorial-dispatch?view=azure-bot-service-4.0&tabs=csharp) feature which will "dispatch" user queries in a chat client to the right Microsoft Cognitive Service. Dispatch is used to direct the user to [LUIS](https://luis.ai), which then directs the user to the right QnA Maker knowledge bases (FAQs) stored in [qnamaker.ai](https://www.qnamaker.ai/). 

The new QnA Maker feature [Chitchat](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/chit-chat-knowledge-base) is used as one of the knowledge bases and is integrated into LUIS using the CLI Dispatch tool. Chitchat gives the chat client a more natural, conversational feel when a user chats off-topic, asking questions such as "How are you?", "You're boring", or "Can we be friends?". There are three different personalities you can set Chitchat to when creating it in [qnamaker.ai](https://www.qnamaker.ai/): The Professional, The Friend, or The Comic. This sample uses The Comic setting, since the Study Bot targets high school students.

[BingSpellCheck](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-tutorial-bing-spellcheck) was added to the bot to correct light misspellings.

[Speech Service](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/) has been added to the enclosing UWP app that this bot embeds into. To understand how Speech Service is incorporated into the UWP app, check out this [sample](https://github.com/Microsoft/Windows-universal-samples/tree/master/Samples/XamlBottomUpList) (Scenario 2) or refer to the [Study Bot app](https://github.com/Azure-Samples/cognitive-services-studybot-csharp/tree/master/StudyBot) itself to view the Speech code. The UWP app "talks" with the embedded bot through [Direct Line](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-channel-connect-directline?view=azure-bot-service-4.0).

This sample is meant as a guide (not as a direct download), but instructions below show you how to create your own sample with your own Cognitive Service resources to create a Study Bot chat client.

## Prerequisites - Azure Bot and Emulator

1. [Create a Basic C# web app bot](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-quickstart?view=azure-bot-service-4.0) in the [Azure Portal](https://ms.portal.azure.com). If you don't have an Azure account, [create a free Azure account](https://azure.microsoft.com/en-us/free/).
1. Download your bot code locally once it has been created. To do this go to the Build section of the Bot management menu.

    <img src="/Assets/download-bot-code.png">

1. Once your code is local, open the solution file in Visual Studio.
1. Update the `appsettings.json` file in the root of the bot project with the botFilePath and botFileSecret. 
   To find the botFilePath and botFileSecret, go to your bot resource in Azure and look under the Application Settings menu. 

    <img src="/Assets/bot-secret-location.png">

   The update should look something like this, replacing `<TEXT>` with your unique values: 
    ```json
    {
      "botFileSecret": "<YOUR BOT SECRET>",
      "botFilePath": "./<YOUR BOT NAME>.bot"
    }
    ```
1. [Download the Bot Emulator](https://github.com/Microsoft/BotFramework-Emulator/releases) in preparation to test chat queries with Visual Studio.
1. You'll need to [download Ngrok](https://ngrok.com/download) for the emulator. Ngrok has a free version and you don't need to create an account, just download it. If Ngrok is not configured, you'll see a link in your emulator where you can click to configure it.

    <img src="/Assets/configure-ngrok.png">
    
## Prerequisites - Creating the Cognitive Services: QnA Maker and LUIS

### QnA Maker

1. For the QnA Maker part, you'll need to [Create, train, and publish](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/quickstarts/create-publish-knowledge-base) three knowledge bases (KBs) in [qnamaker.ai](https://www.qnamaker.ai). Refer to the `.tsv` files in this sample in the [Qna-Luis-Bot_v4/FAQs](https://github.com/Azure-Samples/cognitive-services-studybot-csharp/tree/master/Qna-Luis-Bot_v4/FAQs) folder named `StudyBiology.tsv`, `StudySociology.tsv`, and `StudyGeology.tsv` for FAQs you can upload into qnamaker.ai when creating a new knowledge base. Choose "Create a knowledge base" in qnamaker.ai in the top menu bar, go through all the steps, and name your knowledge bases "StudyBiology", "StudySociology", and "StudyGeology". In Step 4 you will see where you can upload a file.
1. If you want to include Chitchat, [create a new knowledge base](https://www.qnamaker.ai/Create) but leave it empty (don't upload any files or URLs) and in Step 4, enable the Chitchat personality of your choice by selecting a radio button and choosing "Create your KB" at the bottom of the page. Once you create it, you will see it has been populated with lots of standard Chitchat questions and answers. Be sure to train and publish it in "My knowledge bases".
1. You might want to add alternative keywords to your knowledge base questions in qnamaker.ai in addition to the ones already there. To add them to your knowledge bases, go to "My knowledge bases" in [qnamaker.ai](https://www.qnamaker.ai) and in each knowledge base click the "+" sign near each question (after your knowledge bases have been created). Type in the alternative question (term). This is only needed for the Biology, Geology, and Sociology KBs.

    <img src="/Assets/alt-question-kb.png">
    
1. Be sure to train and publish your knowledge base again after any changes are made.

### LUIS

After you have created your web app bot (above), you will see a LUIS app has been auto-created for you in [luis.ai](https://www.luis.ai). We won't actually use this app, we'll replace it with our Dispatch app later in this tutorial. This app will be created through Dispatch commands.

## Prerequisites - Creating Dispatch

### Install BotBuilder Tools

1. Ensure you have [Node.js](https://nodejs.org/) version 8.5 or higher
1. From a command prompt/terminal navigate to your bot project folder and type the command:
    ```bash
    npm i -g msbot chatdown ludown qnamaker luis-apis botdispatch luisgen
    ```
### Create Dispatch service

[Dispatch](https://github.com/Microsoft/botbuilder-tools/tree/master/packages/Dispatch) is a command line tool that will create the Dispatch keys and IDs (.dispatch file), a list of all your LUIS utterances that match your QnA Maker knowledge base questions (.json file), create a new Dispatch app in your LUIS account, and connect all your Cognitive Services to the Dispatch system.

1. To connect your LUIS app and QnA Maker knowledge bases to Dispatch, enter the commands below (one line at a time) into your terminal. You can name your Dispatch service anything you'd like, Study-Bot-Dispatch would work well. Your LUIS authoring key is found in the "Settings" menu when you right click on your account name in the upper right of your luis.ai account. Example for region: westus.
1. Your QnA Maker KB IDs can be found by going to "My knowledge bases" in qnamaker.ai and clicking the "View code" on the far right side of your knowledge base. Your KB ID is the string of numbers/letters in the first line. The QnaKey is your QnA Maker key from your resource in Azure, found in the "Keys" section of the menu in your resource. You have two keys, use either one. This is the resource (your Azure QnA service) you used when creating your knowledge bases in qnamaker.ai. 
1. From your project's root folder, execute these Dispatch commands, one line at a time.
    ```bash
    dispatch init -n {DispatchName} --luisAuthoringKey xxxxxxxxxxxxxxxxxxxx --luisAuthoringRegion {LUISauthoringRegion} --culture en-us
    dispatch add -t qna -i {kbId1} -k {QnaKey from Azure}
    dispatch add -t qna -i {kbId2} -k {QnaKey from Azure}
    dispatch add -t qna -i {kbId3} -k {QnaKey from Azure}
    dispatch add -t qna -i {chitChatKbId} -k {QnaKey from Azure}
    dispatch create
    ```
1. With all your services added, you can view them in the `<YOUR-BOT-NAME>.dispatch` file that was just created (by the Dispatch commands). Also notice the `<YOUR-BOT-NAME>.json` file now contains a very long list of every utterance you have from your LUIS Dispatch app from all its intents.
1. This Dispatch sequence also creates a special LUIS app for the Dispatch service in luis.ai. Note: you'll use the authoring and endpoint keys from this app in your .bot file later.
1. Go to your account in luis.ai and find the Dispatch app just created. You can see there is a `None` intent (default) and then your knowledge base intents. However, these are not named well, as they are a string of random characters. Make sure to rename them (click pencil icon near title) to match the naming in your .bot file for these QnA knowledge bases. For instance, the geology KB is named StudyGeology, in luis.ai, qnamaker.ai, and in the .bot file (name field of each object). They all need to match.
1. After renaming your LUIS intents, train and publish them. It might take a minute or two to see the changes reflected in your responses in the chat client (if already testing).

#### Enable Bing Spell Check

In the Startup.cs file of this sample, copy the BingSpellCheckKey variable and add it to your Startup.cs file at the top. Add your Bing Spell Check Key where indicated. If you do not yet have a Bing Spell Check resource in Azure, [get a free trial](https://azure.microsoft.com/en-us/try/cognitive-services/my-apis/?api=spellcheck-api), or create a new Bing Spell Check v7 resource in the Azure portal and fetch its key.

## Prerequisites - Syncing the code

Now that your Dispatch structure is set in your bot and in luis.ai, you only need to copy/paste missing code when comparing your bot with this sample.

1. Compare BotServices.cs files of the sample with your own and add any missing pieces to yours. It might be easier to copy/paste the entire file.
1. Create a NlpDispatchBot.cs file in your project structure in Visual Studio and copy/paste code from the sample's file of this name. Be sure the variable names match your knowledge base names in your .bot file, including the `DispatchKey`. You can change the `Welcome Text` to be whatever you'd like.
1. Compare/copy/paste the Startup.cs file with the one in this sample. Be sure that the `botConfig` variable reflects your .bot file name so it knows to check resources there, like this:
    ```C#
    var botConfig = BotConfiguration.Load(botFilePath ?? @".\<YOUR-BOTNAME>.bot", secretKey);
    ```
1. Finally, take the StudyBotCsharp.bot file of this sample and see what is missing in your .bot file. The beginning and end should look like this sample, but the objects in the list can vary. Make sure to paste these beginning/end parts over those in your bot:
   ```json
     "name": "<YOUR-BOT-NAME>",
     "description": "",
     "services": [
  
     ...
  
      ],
       "padlock": "",
       "version": "2.0",
       "secretKey": ""
   ```
1. One object that needs replacing in your .bot file is the auto-generated LUIS app. You'll see it's the only object with type "luis". That is what gets generated when you first create the web app bot in Azure, but since you created your own Dispatch app in LUIS, you want to use that one instead. So paste the code below over your default LUIS app object. the appId and authoringKey can be found in your LUIS app under the "Manage" menu, when you open your Dispatch app in luis.ai. The subscription ID is your main key in the Azure Portal. It's the same for every service you create, which can be found under the service resource's "Overview" menu item.
    ```json
    {
      "type": "dispatch",
      "serviceIds": [
        "5",
        "6",
        "7",
        "8",
        "9",
        "10"
      ],
      "name": "Study-Bot-Dispatch",
      "appId": "<YOUR LUIS DISPATCH APP ID>",
      "authoringKey": "<YOUR AUTHORING KEY FOR THE APP>",
      "subscriptionKey": "<YOUR SUBSCRIPTION ID IN AZURE PORTAL>",
      "version": "Dispatch",
      "region": "westus",
      "id": "161"
    },
    ```
1. For the rest of the .bot file, you will need to fill in the keys, IDs, endpoints, and hostnames for each service if applicable. Much of this file was auto-created, so only add missing items to your .bot file.
1. This sample uses Dispatch serviceIds 7, 8, 9, 10 (shown above) which are the IDs of the QnA objects in the .bot file. Be sure to change the Dispatch serviceIds to match your specific service IDs in your .bot file. Basically, all your knowledge base "id"s.

## Run and test your bot

### Connect to bot using Bot Framework Emulator

1. Build/run your bot project. You'll see a browser window open that confirms success.
1. Launch the Bot Framework Emulator
1. File -> Open bot and navigate to your bot project folder
1. Select `<YOUR-BOT-NAME>.bot` file and it opens in the emulator.
1. When you see `[19:15:57]POST 200 conversations.replyToActivity`, your bot is ready to take input.
1. Type any question of your knowledge bases (from any one) and the answer should be returned. 
1. Note: your project must be running in order to use the emulator.

## Deploy this bot to Azure

### Publish from Visual Studio

1. Open the .PublishSettings file you find in the PostDeployScripts folder
1. Copy the userPWD value
1. Right-click on your Project of the Solution Explorer in Visual Studio and click the menu item "Publish".
1. Click the "Publish" button when the file opens and then paste the password you just copied into the popup.

### Troubleshooting for the Azure Web Chat

Due to the dispatch commands, it's possible after you publish your code back to Azure that testing in Web Chat won't work, even when your bot works well locally. This is likely due to the app password in your bot being encrypted. If this is the case, changing the app password in your production endpoint should fix it. To do this:

1. In Azure, go to the Resource Group of your bot. You can find this by clicking on your web app bot and finding the Resource Group in the Overview menu.
1. After clicking on the Resource Group, click the Deployments section under Settings in your menu. You will see a list of all of those resources' apps.
1. Find your bot in that list, it will have the language of your bot and several characters attached to the end of the title, but your bot's name should be there.
1. Click on that bot name and a panel will open to the right, horizontal scroll to view it.
1. A few lines down you will see `Deployment details(Download)`, click on the download link.
1. Open that downloaded .zip and find the `deployment.json` and open it.
1. On about line 49, you'll see an app password. Copy that value.
1. Go back to your local copy of your bot and open the `.bot` file.
1. Paste the app password over the `appPassword` in your `production` endpoint object.
1. Save and publish back to Azure, then refresh your bot and retest your bot in Web Chat. If this does not resolve the issue, put in a support request in Azure.

## Further reading

- [Bot Framework Documentation](https://docs.botframework.com)
- [Bot basics](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-basics?view=azure-bot-service-4.0)
- [Azure Bot Service Introduction](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-overview-introduction?view=azure-bot-service-4.0)
- The [14.nlp-with-dispatch](https://github.com/Microsoft/BotBuilder-Samples/tree/master/samples/csharp_dotnetcore/14.nlp-with-dispatch) sample. The Qna-Luis-Botv4 sample is largely based on this sample.
- [LUIS](https://luis.ai)
- [QnA Maker](https://qnamaker.ai)
- [Activity processing](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-concept-activity-processing?view=azure-bot-service-4.0)
- [Prompt Types](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-prompts?view=azure-bot-service-4.0&
- [Channels and Bot Connector Service](https://docs.microsoft.com/en-us/azure/bot-service/bot-concepts?view=azure-bot-service-4.0)
- [Channels and Bot Connector Service](https://docs.microsoft.com/en-us/azure/bot-service/bot-concepts?view=azure-bot-service-4.0)
