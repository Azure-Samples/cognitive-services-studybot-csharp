---
topic: sample
languages: 
  - csharp
services: cognitive-services, qnamaker, chitchat, luis, language-understanding, bing spell check, speech service
products: 
  - azure
author: wiazur
---
# Study Bot

These samples create a Study Bot chat client using [QnA Maker](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/index) (with [Chitchat](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/chit-chat-knowledge-base)), [LUIS](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/) and [Bing Spell Check](https://docs.microsoft.com/en-us/azure/cognitive-services/bing-spell-check/). Each query into the chat bot (v3 or v4) will be accompanied by relevant search results in an encyclopedia, Microsoft Academic, and a general Bing search as a study aid. Teachers are able to create their own question and answer FAQs to create a study guide as input for the chat bot if they want it to follow a preferred curriculum. However, demo FAQs are available for this sample, included in the Qna-Luis-Bot/FAQs folder. The focus of this app is to enable a more relevant experience of studying, where students can study a subject with a customized chat bot along with multiple web resources.

## Features

* **QnA Maker with LUIS**: There are 3 knowledge bases (created from FAQs in [qnamaker.ai](https://www.qnamaker.ai)) that LUIS directs the user to after a query is received in the chat bot. LUIS has utterances created (in [luis.ai](https://www.luis.ai)) that will allow the right knowledge base to be used. For instance, if the user types in "virus", LUIS knows to go to the biology knowledge base to retrieve a definition (answer) to the user's input. The QnA Maker feature [Chitchat](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/chit-chat-knowledge-base) has been added to the bot v4 as a 4th knowledge base.

* **Speech Service**: One option the user has when inputting queries into the bot is the option to speak the query instead of typing it. The user presses the microphone button to speak, then the bot stops recording shortly after the user finishes speaking (no need for a stop button) and a bot response to the user's spoken query appears in the chat window, in addition to what the bot "heard" the user say.

* **Bing Spell Check**: This enables the user to make spelling mistakes for pre-defined words. For instance, from the sociology knowledge base, "Apartheid" can be recognized if the user inputs "apartide", "aparteid", "apartaid", etc.

* The web resources will take a student query, like "virus", and return relevant information about it in an encyclopedia, Microsoft Academic, as well as a general Bing search that returns mostly news and blogs on the query.

## Prerequisites

1. Start with the Qna-Luis-Bot (either v3 or v4) sample. Once that is up and running, then build the Study Bot sample. The Study Bot depends on the bot you build in Qna-Luis-Bot. Follow the README files in each sample.

1. Visual Studio 2017+

1. Qna-Luis-Bot (v3 or v4) is a C# .NET Standard app

1. Study Bot is a C# UWP app

## Resources

* [QnA Maker Dcoumentation](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/index)
* [QnA Maker API V4.0](https://westus.dev.cognitive.microsoft.com/docs/services/5a93fcf85b4ccd136866eb37/operations/5ac266295b4ccd1554da75ff)
* [Add Chitchat to a QnA Maker knowledge base](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/chit-chat-knowledge-base) - we don't use this method of enabling a knowledge base (KB) with Chitchat in this sample, because we have more than one knowledge base, so we create a stand-alone KB with Chitchat only. If you only have one KB, then it's preferred to enable it in that knowledge base. For more information on the stand-alone Chitchat KB, refer to the [Qna-Luis-Botv4 sample's README](https://github.com/Azure-Samples/cognitive-services-studybot-csharp/blob/master/Qna-Luis-Botv4/README.md).
* [Language Understanding (LUIS) Documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/)
* [LUIS Programmatic APIs v2.0](https://westus.dev.cognitive.microsoft.com/docs/services/5890b47c39e2bb17b84a55ff/operations/5890b47c39e2bb052c5b9c2f)
* [LUIS Endpoint API](https://westus.dev.cognitive.microsoft.com/docs/services/5819c76f40a6350ce09de1ac/operations/5819c77140a63516d81aee78)
* [Bing Spell Check API Documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/bing-spell-check/)
* [Bing Spell Check API v7 reference](https://docs.microsoft.com/en-us/rest/api/cognitiveservices/bing-spell-check-api-v7-reference)
* [Azure bot service](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-overview-introduction?view=azure-bot-service-4.0)
* [Integrating QnA Maker and LUIS bot v3 tutorial](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/tutorials/integrate-qnamaker-luis)
* [Integrating QnA Maker and LUIS bot v4 tutorial, using Dispatch](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-tutorial-dispatch?view=azure-bot-service-4.0&tabs=csharp)
* [Speech Services Documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/)
