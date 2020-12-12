using System.Threading.Tasks;

using BookyApi.Shared.DTO;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using UI.Helpers;

namespace UI.Pages
{
    public partial class Login
    {
        [Inject]
        protected IAuthService AuthServiceInstance { get; set; }
        [Inject]
        protected NavigationManager NavigationManger { get; set; }

        protected LoginDTO LoginModel = new LoginDTO { Username = "", Password = "" };
        protected bool ShowErrors;
        protected string Error = "";

        protected async Task HandleLogin()
        {
            ShowErrors = false;

            var result = await this.AuthServiceInstance.Login(LoginModel);

            if (result.Success)
            {
                this.NavigationManger.NavigateTo("/");
            }
            else
            {
                Error = "Error";
                ShowErrors = true;
            }
        }

        protected void GoToLogout()
        {
            NavigationManger.NavigateTo("logout");
        }
    }
}
