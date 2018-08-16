using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

// NOTE: Install the Newtonsoft.Json NuGet package.
using Newtonsoft.Json;

namespace Study_Bot
{
    class QnAMaker
    {
        static string host = "https://westus.api.cognitive.microsoft.com";
        static string service = "/qnamaker/v4.0";
        static string method = "/knowledgebases/{0}/{1}/qna/";

        // NOTE: Replace this with a valid subscription key.
        static string key = "ENTER KEY HERE";

        // NOTE: Replace this with a valid knowledge base ID.
        static string study_bot_biology_kb = "3b2e0ac6-11bd-48c8-bdfb-759462d8502a";
        static string study_bot_sociology_kb = "43c68327-b8c1-4398-882b-11b09cfdae7c";

        // NOTE: Replace this with "test" or "prod".
        static string env = "test";

        // Prints JSON nicely
        static string PrettyPrint(string s)
        {
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(s), Formatting.Indented);
        }

        // Helper for GetQnA()
        async Task<string> Get(string uri)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(uri);
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);

                var response = await client.SendAsync(request);
                return await response.Content.ReadAsStringAsync();
            }
        }

        // Downloads KB
        async void GetQnA()
        {
            var method_with_id = String.Format(method, study_bot_biology_kb, env);
            var uri = host + service + method_with_id;
            Console.WriteLine("Calling " + uri + ".");
            var response = await Get(uri);
            Console.WriteLine(PrettyPrint(response));
            Console.WriteLine("Press any key to continue.");
        }
 
    }
}
