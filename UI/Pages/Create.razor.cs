using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

using BookyApi.Shared.DTO;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

using UI.Helpers;

namespace UI.Pages
{
    public partial class Create
    {
        [Inject] HttpClient Http { get; set; } = null!;
        [Inject] IJSRuntime JSRuntime { get; set; } = null!;
        IJSObjectReference clipboardControl = null!;
        protected bool Saved { get; set; } = true;
        BookmarkDetailsDTO Bookmark { get; set; } = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                clipboardControl = await JSRuntime.InvokeAsync<IJSObjectReference>(
                    "import", "./JS/clipboardControl.js");
            }
        }


        async Task SaveChanges()
        {
            await Http.PostAsJsonAsync("api/Bookmark", Bookmark);
        }
        string SaveText => Saved ? "Up to date." : "Changes will be saved automatically";

        async Task CopyFomClipboard()
        {
            var contents = await clipboardControl.InvokeAsync<string>("GetClipboardContents");

            var query = $"api/Bookmark/Populate?clipboardContents={HttpUtility.UrlEncode(contents)}";
            Bookmark = await Http.GetFromJsonAsync<BookmarkDetailsDTO>(query) ?? throw new System.Exception("Failed to populate from clipboard");
        }
    }
}
