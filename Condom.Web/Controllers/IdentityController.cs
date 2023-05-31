using Condom.Domain.Global;
using Condom.Domain.Models;
using Condom.Infra.App;
using Condom.Views.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Condom.Web.Controllers
{
    [Controller()]
    public class IdentityController : Controller
    {
        readonly UserManager<Users> _UserManager;
        readonly SignInManager<Users> _SignInManager;
        readonly IdentityApp _GoUserApp;

        public IdentityController(UserManager<Users> userManager, SignInManager<Users> signInManager, IdentityApp goUserApp)
        {
            _UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _GoUserApp = goUserApp ?? throw new ArgumentNullException(nameof(goUserApp));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("identity/login")]
        public async Task<JsonResult> Login([FromForm] UsersView view)
        {
            //await _GoEmail.SendConfirmation("renato.santos@sap.com", "XXXXXXXX");

            if (view == null)
            {
                view = new UsersView();
            }

            view.SetCrud(CondEnum.CrudEnum.Read);

            return Json((await _GoUserApp.OnAction(view)).GetTracker());
        }

        [HttpPost]
        [Route("identity/logout")]
        public async Task<JsonResult> Logout()
        {
            await _SignInManager.SignOutAsync();

            Tracker track = new Tracker();

            track.AddLog(MessageTypeEnum.Success, "Você foi desconectado");

            return Json(track);
        }

    }
}
