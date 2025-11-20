namespace vkshit;

public class VkSettings
{
    public string ClientId { get; set; } = "";
    public string ClientSecret { get; set; } = "";
    public string RedirectUri { get; set; } = "";
}

public class VkTokenResponse
{
    public string access_token { get; set; }
    public int user_id { get; set; }
}

public class VkVideoResponse
{
    public VkVideoInfo response { get; set; }
}

public class VkVideoInfo
{
    public List<VkVideoItem> items { get; set; }
}

public class VkVideoItem
{
    public string title { get; set; }
    public int duration { get; set; }
    public string player { get; set; }
}
