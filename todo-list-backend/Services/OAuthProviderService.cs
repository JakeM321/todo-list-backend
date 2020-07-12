using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using todo_list_backend.Models.Auth;
using todo_list_backend.Types;

namespace todo_list_backend.Services
{
    public class OAuthProviderService : IOAuthProviderService
    {
        private IConfiguration _configuration;

        public OAuthProviderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<Dictionary<string, string>> GetPayload(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var payload = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            return payload;
        }

        public async Task<Option<OAuthUserInfo>> ProcessGoogleCallback(string code)
        {
            var tokenUrl = Utils.Authentication.Google.TokenRequestUrl(_configuration, code);
            var tokenReq = await new HttpClient().PostAsync(tokenUrl.Item1, tokenUrl.Item2);

            if (tokenReq.IsSuccessStatusCode)
            {
                var tokenReqPayload = await GetPayload(tokenReq);

                var tokenInfoUrl = Utils.Authentication.Google.TokenInfoUrl(tokenReqPayload["id_token"]);
                var tokenInfoReq = await new HttpClient().GetAsync(tokenInfoUrl);

                if (tokenInfoReq.IsSuccessStatusCode)
                {
                    var payload = await GetPayload(tokenInfoReq);

                    return new Option<OAuthUserInfo>(new OAuthUserInfo
                    {
                        FirstName = payload["given_name"],
                        LastName = payload["family_name"],
                        Email = payload["email"]
                    });
                }
                else
                {
                    return new Option<OAuthUserInfo>();
                }
            }
            else
            {
                return new Option<OAuthUserInfo>();
            }
        }
    }
}
