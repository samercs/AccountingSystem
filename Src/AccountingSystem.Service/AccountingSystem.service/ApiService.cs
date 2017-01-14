using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AccountingSystem.Service
{
    public class ApiService
    {
        public async Task<string> GetAuthorizationToken(string url, string userName, string password)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", userName),
                    new KeyValuePair<string, string>("password", password)
                });

                var result = await client.PostAsync(url, content);
                var strResult = await result.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<ResponseResult>(strResult);
                return model.access_token;
            }
        }


    }

    public class ResponseResult
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
}
