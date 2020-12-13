using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;

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
        [Inject] ILogger<CheckLoginState> Logger { get; set; } = null!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var state = await AuthenticationState.GetAuthenticationStateAsync();
            var loggedIn = await AuthService.Refresh();
            if(!loggedIn) {
                Logger.LogWarning("User not logged in");
                NavigationManager.NavigateTo("/login");
            }
            Logger.LogInformation("Refreshed token");
            await base.OnParametersSetAsync();
        }
    }
}
