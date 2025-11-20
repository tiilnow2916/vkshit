using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Vkshit;

namespace Vkshit.Pages
{
    public class IndexModel : PageModel
    {
        private readonly VkClipService _vk;

        public JsonElement? ClipInfo { get; private set; }

        [BindProperty]
        public string ClipUrl { get; set; }

        public IndexModel(VkClipService vk)
        {
            _vk = vk;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ClipInfo = await _vk.GetClipInfo(ClipUrl);
            return Page();
        }
    }
}
