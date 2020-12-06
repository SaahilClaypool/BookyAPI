using System.Threading.Tasks;
using UI.Helpers;
using BookyApi.Shared.DTO;
using Microsoft.AspNetCore.Components;
using UI.Pages;
using Microsoft.AspNetCore.Components.Web;

namespace UI.Components
{
    public partial class SearchResult
    {
        [Parameter]
        public SearchItem Result { get; set; } = null!;

        [Parameter]
        public string Query { get; set; } = null!;
        bool ShowModal = false;

        void CloseModal()
        {
            ShowModal = false;
            System.Console.WriteLine($"Modal closing... {ShouldRender()}");
            // force re-render. Required when child changes parent
            StateHasChanged();
        }

        void OpenModal(MouseEventArgs ev)
        {
            ShowModal = true;
        }
    }
}