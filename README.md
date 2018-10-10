---
services: cognitive-services, qnamaker, luis, language-understanding, bing spell check
platforms: dotnet, C#
author: wiazur
---
# Study Bot 

These samples create a Study Bot chat client using [QnA Maker](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/index), [LUIS](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/) and [Bing Spell Check](https://docs.microsoft.com/en-us/azure/cognitive-services/bing-spell-check/). Each query into the chat bot will be accompanied by relevant search results in an encyclopedia, Microsoft Academic, and News/Blogs sections as a study aid. Teachers are able to create their own question and answer FAQs to create a study guide as input for the chat bot if they want it to follow a preferred curriculum. However, demo FAQs are available for this sample, included in the Qna-Luis-Bot/FAQs folder. The focus of this app is to enable a more relevant experience of studying, where students can study a subject with a customized chat bot along with multiple web resources.

## Features

* **QnA Maker with LUIS**: There are 3 knowledge bases (created from FAQs in [qnamaker.ai](https://www.qnamaker.ai)) that LUIS directs the user to after a query is received in the chat bot. LUIS has utterances created (in [luis.ai](https://www.luis.ai)) that will allow the right knowledge base to be used. For instance, if the user types in "virus", LUIS knows to go to the biology knowledge base to retrieve a definition (answer) to the user's input.

* **Bing Spell Check**: This enables the user to make spelling mistakes for pre-defined words. For instance, from the sociology knowledge base, "Apartheid" can be recognized if the user inputs "apartide", "aparteid", "apartaid", etc.

* The web resources will take a student query, like "virus", and return relevant information about it in an encyclopedia, Microsoft Academic, as well as a general Bing search that returns mostly news and blogs on the query.

## Prerequisites

1. Start with the Qna-Luis-Bot sample. Once that is up and running, then build the Study Bot sample. The Study Bot depends on the bot you build in Qna-Luis-Bot. Follow the README files in each sample.

1. Visual Studio 2017+

1. Qna-Luis-Bot is a C# .NET Standard app

1. Study Bot is a C# UWP app

## Resources

* [QnA Maker Dcoumentation](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/index)
* [QnA Maker API V4.0](https://westus.dev.cognitive.microsoft.com/docs/services/5a93fcf85b4ccd136866eb37/operations/5ac266295b4ccd1554da75ff)
* [Language Understanding (LUIS) Documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/)
* [LUIS Programmatic APIs v2.0](https://westus.dev.cognitive.microsoft.com/docs/services/5890b47c39e2bb17b84a55ff/operations/5890b47c39e2bb052c5b9c2f)
* [LUIS Endpoint API](https://westus.dev.cognitive.microsoft.com/docs/services/5819c76f40a6350ce09de1ac/operations/5819c77140a63516d81aee78)
* [Bing Spell Check API Documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/bing-spell-check/)
* [Bing Spell Check API v7 reference](https://docs.microsoft.com/en-us/rest/api/cognitiveservices/bing-spell-check-api-v7-reference)
* [General QnA Maker and LUIS bot tutorial, using bot v3](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/tutorials/integrate-qnamaker-luis)
