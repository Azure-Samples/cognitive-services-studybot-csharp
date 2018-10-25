# Qna-Luis-Botv4
This sample bot has been created using the [Microsoft Bot Framework](https://dev.botframework.com), in particular, the [Dispatch](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-tutorial-dispatch?view=azure-bot-service-4.0&tabs=csharp) feature which will "dispatch" user queries in a chat client to the right Microsoft Cognitive Service. Dispatch is used to direct the user to [LUIS](https://luis.ai), which then directs the user to the right QnA Maker knowledge bases (FAQs) stored in [qnamaker.ai](https://www.qnamaker.ai/). 

The new QnA Maker feature [Chitchat](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/chit-chat-knowledge-base) is used as one of the knowledge bases and is integrated into LUIS using the CLI Dispatch tool. Chitchat gives the chat client a more natural, conversational feel when a user chats off-topic, asking questions such as "How are you?", "You're boring", or "Can we be friends?". There are three different personalities you can set Chitchat to when creating it in [qnamaker.ai](https://www.qnamaker.ai/): The Professional, The Friend, or The Comic. This sample uses The Comic setting, since the Study Bot targets high school students.

This sample is meant as a guide (not as a direct download), but instructions below show you how to create your own sample with your own Cognitive Services resources to create a Study Bot chat client.

## Prerequisites - Azure Bot and Emulator
1. [Create a Basic C# web app bot](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-quickstart?view=azure-bot-service-4.0) in the [Azure Portal](https://ms.portal.azure.com). If you don't have an Azure account, [create a free Azure account](https://azure.microsoft.com/en-us/free/).
1. Download your bot code locally, once it has been created. To do this go to the Build section of the Bot management menu.

    <img src="/Assets/download-bot-code.png">
    
1. Once your code is local, open the solution file in Visual Studio.
1. Update the `appsettings.json` file in the root of the bot project with the botFilePath and botFileSecret. 
1. To find the botFilePath and botFileSecret, go to your bot resource in Azure and look under the Application Settings menu. 

    <img src="/Assets/bot-secret-location.png">

1. The update should look something like this, replacing <TEXT> with your unique values: 
```json
{
  "botFileSecret": "<YOUR BOT SECRET>",
  "botFilePath": "./<YOUR BOT NAME>.bot"
}
```
1. [Download the Bot Emulator](https://github.com/Microsoft/BotFramework-Emulator/releases) in preparation to test chat queries with Visual Studio.
1. You'll need to [download Ngrok](https://ngrok.com/download) for the emulator. If Ngrok is not configured, you'll see a link in your emulator where you can click to configure it.

    <img src="/Assets/configure-ngrok.png">
    
## Prerequisites - Creating the Cognitive Services: LUIS and QnA Maker
1. For the QnA Maker part, you will need to [Create, train, and publish](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/quickstarts/create-publish-knowledge-base) three knowledge bases (KBs) in [qnamaker.ai](https://www.qnamaker.ai). Refer to the text files in this sample in the `Qna-Luis-Bot/FAQs` folder named QA Biology, QA Sociology, and QA Geology for FAQs you can upload for use. Name your knowledge bases "Biology", "Sociology", and "Geology". 
1. If you want to include Chitchat, [create a new knowledge base](https://www.qnamaker.ai/Create) but leave it empty (don't upload any files or URLs) and in Step 4, enable the Chitchat personality of your choice by selecting a radio button and choosing "Create your KB" at the bottom of the page. Once you create it, you will see it has been populated with lots of standard Chitchat questions and answers. Be sure to train and publish it in "My knowledge bases".
1. You will want to add alternative keywords to your knowledge base questions in qnamaker.ai. These are found in the `Alt questions` folder in the `FAQs` folder. To add them to your knowledge bases, go to "My knowledge bases" in [qnamaker.ai](https://www.qnamaker.ai) and in each knowledge base click the "+" sign near each question (after your knowledge bases have been created). Type in the alternative question. This is only needed for the Biology, Geology, and Sociology KBs.

    <img src="/Assets/alt-question-kb.png">
    
1. Be sure to train and publish your knowledge base again after any changes are made.
    
1. After you have created your web app bot (above), you will see a LUIS app has been auto-created for you in [luis.ai](https://www.luis.ai) with default intents that get created for every LUIS app: `Greeting`, `Cancel`, `Help`, and `None`. You can leave them or delete them, they are optional, but make sure you leave the `None` intent. Although, it's recommended to leave them all. 
1. Now, [add intents](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/luis-how-to-add-intents) of your own according to your knowledge bases. For example, if you have a biology knowledge base, you'll want to make a "Biology" intent. This is how LUIS knows to send all user queries in the chat client to the Biology knowledge base in qnamaker.ai. The "Add Intents" how-to guide above also shows you how to add utterances, which should mirror what your QnA Maker knowledge bases' words or phrases are in the "Question" part. For example, if you have a Biology knowledge base with 'What is a virus?' as a question... the utterances in the Biology intent (and the alternative words in the "Question" part of your Biology knowledge base) would be "virus", "viral", "viruses", and/or "bug". Any of these will return the definition (answer) of "virus" in the chat client.

## Prerequisites - Creating Dispatch
Dispatch is a command line tool that will create the Dispatch keys and IDs (.dispatch file), a list of all your LUIS utterances that match your QnA Maker knowledge base questions (.json file), and connect all your Cognitive Services to the Dispatch system.


### Connect to bot using Bot Framework Emulator
- Launch the Bot Framework Emulator
- File -> Open bot and navigate to the bot project folder
- Select `<your-bot-name>.bot` file

# Deploy this bot to Azure
## Publish from Visual Studio
- Open the .PublishSettings file you find in the PostDeployScripts folder
- Copy the userPWD value
- Right click on the Project and click on "Publish..."
- Paste the password you just copied and publish

## Publish using the CLI tools
You can use the [MSBot](https://github.com/microsoft/botbuilder-tools) Bot Builder CLI tool to clone and configure any services this sample depends on. 
To install all Bot Builder tools - 
```bash:
npm i -g msbot chatdown ludown qnamaker luis-apis botdispatch luisgen
```
```To clone this bot, run:
msbot clone services -f deploymentScripts/msbotClone -n <BOT-NAME> -l <Azure-location> --subscriptionId <Azure-subscription-id>
```
# Further reading
- [Bot Framework Documentation](https://docs.botframework.com)
- [Bot basics](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-basics?view=azure-bot-service-4.0)
- [Activity processing](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-concept-activity-processing?view=azure-bot-service-4.0)
- [LUIS](https://luis.ai)
- [Prompt Types](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-prompts?view=azure-bot-service-4.0&tabs=javascript)
- [Azure Bot Service Introduction](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-overview-introduction?view=azure-bot-service-4.0)
- [Channels and Bot Connector Service](https://docs.microsoft.com/en-us/azure/bot-service/bot-concepts?view=azure-bot-service-4.0)
- [QnA Maker](https://qnamaker.ai)

