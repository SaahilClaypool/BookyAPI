using System;
using System.Threading.Tasks;

using UI.Helpers;

using BookyApi.Shared.DTO;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Web;
using Microsoft.AspNetCore.Components.Web;
using System.Linq;
using System.Collections.Generic;

namespace UI.Pages
{
    public partial class Search
    {
        [Parameter] public bool Disabled { get; set; } = false;
        [Inject] protected NavigationManager NavigationManger { get; set; }
        [Inject] protected HttpClient? Http { get; set; }

        private SearchModel Model { get; set; } = new();


        protected override Task OnInitializedAsync()
        {
            Model.Http = Http;
            Model.NotifyStateChanged = () =>
            {
                StateHasChanged();
            };
            return base.OnInitializedAsync();
        }

        private void Submit()
        {
            if (Model.Selected is SearchItem selected)
            {
                NavigationManger.NavigateTo(selected.Url);
            }
        }

        private void HandleKeyPresss(KeyboardEventArgs args)
        {
            switch (args.Key)
            {
                case "ArrowDown":
                    Model.Cursor += 1;
                    break;
                case "ArrowUp":
                    Model.Cursor -= 1;
                    break;
            };
        }
    }

    public class SearchModel
    {
        public Action? NotifyStateChanged { get; set; }
        private string _query = "";
        public HttpClient? Http { get; set; } = null;

        private int _cursor = -1;
        public int Cursor
        {
            get => Math.Min(Results.Result.Count - 1, _cursor);
            set
            {
                var oldValue = _cursor;
                _cursor = Math.Min(Results.Result.Count - 1, value);
                if (oldValue != _cursor && NotifyStateChanged is not null)
                {
                    NotifyStateChanged();
                }
            }
        }

        private SearchResultDTO Results { get; set; } = new();

        public List<SearchItem> Items => Results.Result.Select((result, i) =>
            new SearchItem()
            {
                Content = result.Content,
                Url = result.Url,
                IsSelected = i == Cursor
            }
        ).ToList();
        public SearchItem? Selected => Cursor > 0 ? Items[Cursor] : null;

        public async Task RunQuery(string query)
        {
            Results = (await Http!.GetFromJsonAsync<SearchResultDTO>($"api/Search/{HttpUtility.UrlEncode(query)}"))!;
            if (NotifyStateChanged is not null)
            {
                NotifyStateChanged();
            }
        }

        public string Query
        {
            get => _query;
            set
            {
                _query = value;
                if (Http is not null && Query.Length != 0)
                {
                    Task.Run(async () => await RunQuery(new string(_query)));
                }
                else
                {
                    Results = new();
                }
            }
        }
    }

    public class SearchItem
    {
        public string Url { get; set; } = null!;
        public string? Content { get; set; } = null;
        public bool IsSelected { get; set; }
        public string ClassName => IsSelected ? "selected" : "";
    }
}
