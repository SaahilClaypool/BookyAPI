using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;

using UI;
using UI.Helpers;

namespace UI.Shared
{
    public partial class CheckLoginState
    {
        [Inject] protected HostEnvironmentService HostEnvironmentService { get; set; } = null!;
        [Inject] protected NavigationManager NavigationManager { get; set; } = null!;
        [Inject] protected IAuthService AuthService { get; set; } = null!;
        [Inject] AuthenticationStateProvider AuthenticationState { get; set; } = null!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            System.Console.WriteLine("RENDER");
            var state = await AuthenticationState.GetAuthenticationStateAsync();
            System.Console.WriteLine(state);
            var loggedIn = await AuthService.Refresh();
            if(!loggedIn) {
                System.Console.WriteLine("Not logged in!");
                NavigationManager.NavigateTo("/login");
            }
            System.Console.WriteLine("Refreshed token");
            await base.OnParametersSetAsync();
        }
    }
}
