This sample is the full version of all combined Cognitive Services. It contains the other samples in the `cognitive-services-studybot-csharp` repository into a UWP, C# app.

## Prerequisites

1. Before you can run this comprehesive sample, make sure to follow the prerequisites for all other samples first.

1. For this project, in `MainPage.xaml`, add your bot's secret key inside of the `WebView` URL. 
  
    `<WebView x:Name="studyBot" Source="https://webchat.botframework.com/embed/<ADD YOUR BOT NAME HERE>?s=<ADD YOUR SECRET KEY HERE>" HorizontalAlignment="Center" Height="300" Width="1100" Margin="0,20,0,0" VerticalAlignment="Top" />`
    
    To find this key, go to your LUIS bot in the [Azure portal](https://ms.portal.azure.com) under the Channels menu item and copy your secret key.
    
    <img src="/Assets/bot-secret-key.png">