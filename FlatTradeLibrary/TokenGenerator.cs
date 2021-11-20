using FlatTradeLibrary.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FlatTradeLibrary
{
    public class TokenGenerator
    {
        public async Task<TokenResponse> LoginAndGetAccessTokenAsync(Request request)
        {
            if (request == null)
            {
                return null;
            }

            using (var httpClient = new HttpClient())
            {
                request.SessionId = await GetSessionId(httpClient).ConfigureAwait(false);
                string requestCode = await GetRequestCode(httpClient, request).ConfigureAwait(false);
                return await GetToken(request, requestCode, httpClient).ConfigureAwait(false);
            }
        }

        private static async Task<string> GetSessionId(HttpClient httpClient)
        {
            HttpResponseMessage sessionResult = await httpClient.PostAsync(Constants.SessionUrl, null).ConfigureAwait(false);
            await CheckAndThrowException(sessionResult).ConfigureAwait(false);

            return await sessionResult.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private static async Task<string> GetRequestCode(HttpClient httpClient, Request request)
        {
            request.Password = ComputeHash(request.Password);
            HttpResponseMessage authResult = await httpClient.PostAsync(Constants.AuthApiUrl,
                    new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);

            await CheckAndThrowException(authResult).ConfigureAwait(false);

            RedirectResponse redirectResponse = JsonConvert.DeserializeObject<RedirectResponse>(await authResult.Content.ReadAsStringAsync().ConfigureAwait(false));

            if (redirectResponse == null)
            {
                throw new ArgumentNullException(nameof(redirectResponse));
            }

            return GetQueryValueByName(new Uri(redirectResponse.RedirectUrl).Query, "code");
        }

        private static async Task<TokenResponse> GetToken(Request request, string requestCode, HttpClient httpClient)
        {
            HttpResponseMessage tokenResult = await httpClient.PostAsync(Constants.TokenUrl,
                new StringContent(JsonConvert.SerializeObject(new TokenRequest
                {
                    ApiKey = request.ApiKey,
                    RequestCode = requestCode,
                    ApiSecret = ComputeHash(string.Concat(request.ApiKey, requestCode, request.ApiSecret))
                }), Encoding.UTF8, "application/json")
            ).ConfigureAwait(false);

            await CheckAndThrowException(tokenResult).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<TokenResponse>(await tokenResult.Content.ReadAsStringAsync().ConfigureAwait(false), new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        private static string GetQueryValueByName(string query, string key) => HttpUtility.ParseQueryString(query).Get(key);

        private static async Task CheckAndThrowException(HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                BaseMessage baseMessage = JsonConvert.DeserializeObject<BaseMessage>(await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false));
                throw new HttpRequestException($"Error occurred when calling {responseMessage.RequestMessage.RequestUri} => " +
                                               $"status : {baseMessage?.Status}, message : {baseMessage?.Message}");
            }
        }

        private static string ComputeHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                return BitConverter
                    .ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(input)))
                    .Replace("-", "")
                    .ToLowerInvariant();
            }
        }
    }
}
