using System;
using System.Net.Http;
using System.Threading.Tasks;

using BookyApi.API.Models;

using Microsoft.Extensions.Logging;

namespace BookyApi.API.Services.UseCases
{
    public class PopulateFromClipboard
    {
        HttpClient Http { get; }
        public ILogger<PopulateFromClipboard> Logger { get; }

        public PopulateFromClipboard(HttpClient http, ILogger<PopulateFromClipboard> logger)
        {
            Http = http;
            Logger = logger;
        }


        public async Task<Bookmark> Populate(string clipboardContents)
        {
            if (Uri.TryCreate(clipboardContents, UriKind.Absolute, out var uri))
            {
                // create from url contents
                var response = await Http.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Logger.LogInformation(content);
                    var titleStart = content.IndexOf("<title>") + "<title>".Length;
                    var titleEnd = content.IndexOf("</title>");
                    return new Bookmark
                    {
                        Url = clipboardContents,
                        Content = content[titleStart..titleEnd]
                    };
                }
                else
                {
                    return new Bookmark { Notes = $"Failed to download '{clipboardContents}'" };
                }
            }
            else
            {
                return new Bookmark { Notes = clipboardContents };
            }
        }
    }
}