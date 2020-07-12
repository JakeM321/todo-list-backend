using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

namespace todo_list_backend.Utils
{
    public static class Authentication
    {
        public static class PasswordEncryption
        {
            private static byte[] Hash(string password, byte[] salt)
            {
                return KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8);
            }

            public static Tuple<string, string> GenerateHashAndSalt(string password)
            {
                byte[] salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                var hash = Hash(password, salt);

                return new Tuple<string, string>(
                    Convert.ToBase64String(hash),
                    Convert.ToBase64String(salt)
                );
            }

            public static bool VerifyPassword(string hash, string salt, string password)
            {
                string test = Convert.ToBase64String(
                    Hash(password, Convert.FromBase64String(salt))
                );

                return hash == test;
            }
        }

        public static class Jwt
        {
            public static T ReadToken<T>(string token, string secret)
            {
                var json = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(secret)
                    .MustVerifySignature()
                    .Decode(token);

                return JsonConvert.DeserializeObject<T>(json);
            }

            public static string BuildToken(Dictionary<string, string> claims, string secret)
            {
                var builder = new JwtBuilder()
                   .WithAlgorithm(new HMACSHA256Algorithm())
                   .WithSecret(secret);

                claims.ToList().ForEach(claim => builder.AddClaim(claim.Key, claim.Value));

                return builder.Encode();
            }
        }

        public static class Google
        {
            private static string AssertUrl(IConfiguration configuration)
            {
                var appUrl = configuration.GetValue<string>("appUrl");
                return String.Format("{0}/assert", appUrl);
            }

            public static string RedirectUrl(IConfiguration configuration)
            {
                var clientId = configuration.GetValue<string>("TodoListApp:google:clientId");

                var builder = new UriBuilder("https://accounts.google.com/o/oauth2/v2/auth");
                var query = HttpUtility.ParseQueryString(builder.Query);

                query["scope"] = "openid https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";
                query["response_type"] = "code";
                query["client_id"] = clientId;
                query["redirect_uri"] = AssertUrl(configuration);

                builder.Query = query.ToString();

                return builder.ToString();
            }

            public static Tuple<string, HttpContent> TokenRequestUrl(IConfiguration configuration, string code)
            {
                var clientId = configuration.GetValue<string>("TodoListApp:google:clientId");
                var secret = configuration.GetValue<string>("TodoListApp:google:secret");

                var redirect = AssertUrl(configuration);

                return new Tuple<string, HttpContent>(
                    "https://oauth2.googleapis.com/token",
                    new FormUrlEncodedContent(new Dictionary<string, string> {
                        { "code", code },
                        { "client_id", clientId },
                        { "client_secret", secret },
                        { "grant_type", "authorization_code" },
                        { "redirect_uri", redirect }
                    })
                );
            }

            public static string TokenInfoUrl(string code)
            {
                return String.Format("https://oauth2.googleapis.com/tokeninfo?id_token={0}", code);
            }
        }

        
    }
}
