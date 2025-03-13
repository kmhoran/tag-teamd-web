using System.Collections.Concurrent;
using System.Net.Http.Json;
using TagTeamdWeb.Common;
using TagTeamdWeb.Common.Interfaces.Jwt;
using TagTeamdWeb.Common.Models.Jwt;
using Newtonsoft.Json;

namespace TagTeamdWeb.Services.Jwt
{
    public class JwtValidationService : IJwtValidationService
    {
        private static readonly ConcurrentDictionary<string, JwtCacheEntry> _cache = new();
        private readonly IHttpClientFactory _httpClientFactory;

        public JwtValidationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> ValidateTokenAsync(string username, string token)
        {
            var cacheKey = username;
            if (_cache.TryGetValue(cacheKey, out var cacheEntry))
            {
                if (cacheEntry.Expiration > DateTime.UtcNow)
                {
                    return true;
                }
            }

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PostAsJsonAsync(Constants.JwtValidationUrl, new { username, token });
            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<JwtValidationResponse>(await response.Content.ReadAsStringAsync());
                if (data?.Session?.Expiration != null && data.Session.Expiration > DateTime.UtcNow)
                {
                    _cache[cacheKey] = new JwtCacheEntry
                    {
                        Token = token,
                        Username = username,
                        Expiration = data.Session.Expiration
                    };
                    return true;
                }
            }

            return false;
        }
    }
}