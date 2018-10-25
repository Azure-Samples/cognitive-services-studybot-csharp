# Qna-Luis-Botv4
This bot has been created using the [Microsoft Bot Framework](https://dev.botframework.com), in particular, the [Dispatch](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-tutorial-dispatch?view=azure-bot-service-4.0&tabs=csharp) feature which will "dispatch" user queries in a chat client to the right Microsoft Cognitive Service. This sample uses Dispatch to direct the user to [LUIS](https://luis.ai), which then directs the user to the right QnA Maker knowledge base (FAQ) stored in [qnamaker.ai](https://www.qnamaker.ai/). The new QnA Maker feature [Chitchat](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/chit-chat-knowledge-base) is used as one of the knowledge bases and is integrated into LUIS using the CLI Dispatch tool. Chitchat gives the chat client a more natural, conversational feel when a user chats off-topic.

# Prerequisite to run this bot locally
- [Create a Basic C# web app bot](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-quickstart?view=azure-bot-service-4.0) in the [Azure Portal](https://ms.portal.azure.com). If you don't have an Azure account, [create a free Azure account](https://azure.microsoft.com/en-us/free/).
- Update the `appsettings.json` file in the root of the bot project with the botFilePath and botFileSecret 
- You can find the botFilePath and botFileSecret in the Azure App Service application settings.

Your appsettings.json file should look like this:
```
{
    "botFilePath": "<copy value from App settings>",
    "botFileSecret": "<copy value from App settings>"
}
```

## Run in Visual Studio
- Open the .sln file with Visual Studio.
- Press F5.
## Run in Visual Studio Code
- Open the bot project folder with Visual Studio Code.
- Bring up a terminal.
- Type 'dotnet run'.
## Testing the bot using Bot Framework Emulator
[Microsoft Bot Framework Emulator](https://aka.ms/botframework-emulator) is a desktop application that allows bot developers to test and debug
their bots on localhost or running remotely through a tunnel.
- Install the Bot Framework Emulator from [here](https://aka.ms/botframework-emulator).
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

