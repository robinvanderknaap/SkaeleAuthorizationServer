using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ConsoleClient
{
    /// <summary>
    /// Demonstration of 'client credentials flow'.
    /// This flow is used with machine to machine communication. Not on behalf of a user.
    /// </summary>
    class Program
    {
        static void Main()
        {
            var token = RequestToken();

            token.Wait();

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:50929/")
            };

            httpClient.SetBearerToken(token.Result);

            var resultFromApi = httpClient.GetStringAsync("access");

            resultFromApi.Wait();

            Console.WriteLine(resultFromApi.Result);

            Console.ReadKey();
        }

        private async static Task<string> RequestToken()
        {
            var tokenClient = new TokenClient("https://localhost:44302/connect/token", "ConsoleClient", "secret");

            var response = await tokenClient.RequestClientCredentialsAsync("Api");

            return response.AccessToken;
        }
    }
}
