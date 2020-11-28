using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using UI;

namespace UI.Shared
{
    public partial class SSR
    {
        [Inject] protected HostEnvironmentService hostEnvironmentService { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        public bool IsServer => hostEnvironmentService.IsServer;
    }
}