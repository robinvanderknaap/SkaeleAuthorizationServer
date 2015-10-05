using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static void Main()
        {
            var token = RequestToken();

            token.Wait();

            Console.WriteLine(token.Result);
            Console.ReadKey();
        }

        private async static Task<string> RequestToken()
        {
            var clientId = "ConsoleClient";
            var clientSecret = "secret";

            var encoding = Encoding.UTF8;
            var credentials = $"{clientId}:{clientSecret}";

            var headerValue = Convert.ToBase64String(encoding.GetBytes(credentials));

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44302/connect/"),
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", headerValue);


            var request = new HttpRequestMessage(HttpMethod.Post, "token");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", headerValue);
            

            var response = await httpClient.SendAsync(request);

            return await response.Content.ReadAsStringAsync();



        }
    }
}
