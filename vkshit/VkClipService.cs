using System.Text.Json;

namespace vkshit;

public class VkClipService
{
    private readonly HttpClient _http;

    public VkClipService(HttpClient http)
    {
        _http = http;
    }

    public async Task<VkVideoItem?> GetClipAsync(string token, string clipUrl)
    {
        // пример:
        // https://vk.com/clip-188633896_456246882

        if (!clipUrl.Contains("clip"))
            return null;

        var t = clipUrl.Replace("https://vk.com/clip", "");
        var parts = t.Split('_');

        string ownerId = parts[0];   // -188633896
        string videoId = parts[1];   // 456246882

        string req =
            $"https://api.vk.com/method/video.get?videos={ownerId}_{videoId}&access_token={token}&v=5.230";

        var json = await _http.GetStringAsync(req);

        var data = JsonSerializer.Deserialize<VkVideoResponse>(json);

        return data?.response?.items?.FirstOrDefault();
    }
}
