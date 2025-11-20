using System.Text.Json;

namespace Vkshit
{
    public class VkClipService
    {
        private readonly HttpClient _http;

        public VkClipService(HttpClient http)
        {
            _http = http;
        }

        public async Task<JsonElement?> GetClipInfo(string url)
        {
            var match = System.Text.RegularExpressions.Regex.Match(
                url,
                @"clip-?(\d+)_(\d+)"
            );

            if (!match.Success)
                return null;

            string ownerId = match.Groups[1].Value;
            string clipId = match.Groups[2].Value;

            string apiUrl =
                $"https://api.vk.com/method/video.get?videos={ownerId}_{clipId}&v=5.154&access_token=";

            var response = await _http.GetAsync(apiUrl);
            string json = await response.Content.ReadAsStringAsync();

            return JsonDocument.Parse(json).RootElement;
        }
    }
}
