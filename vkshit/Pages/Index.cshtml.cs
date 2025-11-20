using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using vkshit;   // <- обязательно!

namespace vkshit.Pages;   // <- ВАЖНО!!!

public class IndexModel : PageModel
{
    private readonly VkClipService _vk;

    public IndexModel(VkClipService vk)
    {
        _vk = vk;
    }

    [BindProperty]
    public string ClipUrl { get; set; }

    public bool IsAuthed { get; set; }
    public VkVideoItem? Clip { get; set; }

    public void OnGet()
    {
        IsAuthed = Request.Cookies.ContainsKey("vk_token");
    }

    public async Task<IActionResult> OnPost()
    {
        IsAuthed = Request.Cookies.ContainsKey("vk_token");

        if (!IsAuthed)
            return Redirect("/auth/login");

        string token = Request.Cookies["vk_token"]!;
        Clip = await _vk.GetClipAsync(token, ClipUrl);

        return Page();
    }
}
