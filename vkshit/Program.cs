using vkshit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Runtime;
using vkshit;

var builder = WebApplication.CreateBuilder(args);

// VK settings
builder.Services.Configure<VkSettings>(
    builder.Configuration.GetSection("Vk")
);

// Auth cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login";
    });

// Razor Pages
builder.Services.AddRazorPages();

// VK service
builder.Services.AddHttpClient<VkClipService>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Routes
app.MapGet("/auth/login", async context =>
{
    var cfg = context.RequestServices.GetRequiredService<IConfiguration>();
    string clientId = cfg["Vk:ClientId"];
    string redirect = cfg["Vk:RedirectUri"];

    string url =
        $"https://oauth.vk.com/authorize?client_id={clientId}&display=page&redirect_uri={redirect}&scope=video&response_type=code&v=5.230";

    context.Response.Redirect(url);
});

// callback
app.MapGet("/auth/callback", async context =>
{
    var cfg = context.RequestServices.GetRequiredService<IConfiguration>();
    var http = context.RequestServices.GetRequiredService<IHttpClientFactory>().CreateClient();

    string code = context.Request.Query["code"];

    var url =
        $"https://oauth.vk.com/access_token?client_id={cfg["Vk:ClientId"]}&client_secret={cfg["Vk:ClientSecret"]}&redirect_uri={cfg["Vk:RedirectUri"]}&code={code}";

    var response = await http.GetFromJsonAsync<VkTokenResponse>(url);
    if (response?.access_token == null)
    {
        await context.Response.WriteAsync("Ошибка авторизации!");
        return;
    }

    context.Response.Cookies.Append("vk_token", response.access_token);

    context.Response.Redirect("/");
});

app.Run();
