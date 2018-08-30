using StudyBot.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBot.Controls
{
    class TranslateAnalyzeTextControl
    {

        private string[] detectedLanguage;
        private string translatedText;

        public ObservableCollection<string> KeyPhrases { get; set; } = new ObservableCollection<string>();

        // Gets keywords from user query, to be used for Bing Search
        private async Task GetKeyPhrasesAsync(string query)
        {
            try
            {
                KeyPhrasesResult keyPhrasesResult = await TextAnalyticsHelper.GetKeyPhrasesAsync(query);

                this.KeyPhrases.AddRange(keyPhrasesResult.KeyPhrases.OrderBy(i => i));
            }
            catch (Exception ex)
            {
                await Util.GenericApiCallExceptionHandler(ex, "Error during Text Analytics 'Key Phrases' call.");
            }
        }

        //private async void TranslateQuery(string query)
        //{
        //    this.translatedText = query;  // we don't translate English input, so set "translated text" to the input to cover that scenario
        //    await this.DetectLanguageAsync();

        //    if (this.detectedLanguage[0] != "en")
        //    {
        //        await this.TranslateTextAsync(query);
        //    }
        //    await this.AnalyzeTextAsync(query);
        //}

        private Task DetectLanguageAsync()
        {
            throw new NotImplementedException();
        }

        private async Task TranslateTextAsync(string query)
        {
            try
            {
                if (!string.IsNullOrEmpty(query))
                {
                    TranslateTextResult textTranslateResult = await TranslatorTextHelper.GetTranslatedTextAsync(query, this.detectedLanguage[0]);
                    this.translatedText = textTranslateResult.TranslatedText;
                }
                else
                {
                    this.translatedText = string.Empty;
                }
            }
            catch (Exception ex)
            {
                await Util.GenericApiCallExceptionHandler(ex, "Error during Translation Text call.");
            }
        }

        // Puts query through sentiment analysis for internet search
        //private async Task AnalyzeTextAsync(string query)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(query))
        //        {
        //            SentimentResult textAnalysisResult = await TextAnalyticsHelper.GetTextSentimentAsync(this.translatedText, this.detectedLanguage[0]);
        //            this.sentimentControl.Sentiment = textAnalysisResult.Score;
        //        }
        //        else
        //        {
        //            this.sentimentControl.Sentiment = 0.5;
        //        }
        //        this.OnSpeechRecognitionAndSentimentProcessed(new SpeechRecognitionAndSentimentResult
        //        {
        //            SpeechRecognitionText = query,
        //            TextAnalysisSentiment = this.sentimentControl.Sentiment,
        //            DetectedLanguage = this.detectedLanguage[1],
        //            TranslatedText = this.translatedText
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        await Util.GenericApiCallExceptionHandler(ex, "Error during Text Analytics 'Sentiment' call.");
        //    }
        //}
    }

    public class SpeechRecognitionAndSentimentResult
    {
        public string DetectedLanguage { get; set; }
        public string SpeechRecognitionText { get; set; }
        public double TextAnalysisSentiment { get; set; }
        public string TranslatedText { get; set; }
    }
}
