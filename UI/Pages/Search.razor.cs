using System;
using System.Threading.Tasks;

using UI.Helpers;

using BookyApi.Shared.DTO;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Web;

namespace UI.Pages
{
    public partial class Search
    {
        [Parameter] public bool Disabled { get; set; } = false;
        [Inject] protected NavigationManager NavigationManger { get; set; }
        [Inject] protected HttpClient? Http { get; set; }

        protected SearchModel Model { get; set; } = new();
        protected override Task OnInitializedAsync()
        {
            Model.Http = Http;
            return base.OnInitializedAsync();
        }
    }

    public class SearchModel
    {
        private string _query = "";
        public HttpClient? Http { get; set; } = null;

        public async Task RunQuery(string query)
        {
            var result = await Http.GetFromJsonAsync<SearchResultDTO>($"api/Search/{HttpUtility.UrlEncode(query)}");
        }

        public string Query
        {
            get => _query;
            set
            {
                _query = value;
                if (Http is not null && Query.Length != 0)
                {
                    Console.WriteLine($"query: {Query} {Query.Length}");
                    Task.Run(async () => await RunQuery(new string(_query)));
                }
            }
        }
    }
}
