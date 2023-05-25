using Condom.Domain.Global;
using Microsoft.JSInterop;

namespace Condom.Web.Helpers
{
    public class JS
    {
        protected IJSRuntime _Runtime;

        public JS(IJSRuntime runtime)
        {
            _Runtime = runtime;
        }

        public async Task ShowTracker(Tracker track)
        {
            foreach (var log in track.Logs)
            {
                await ShowMessage(log.GetTypeEnum().ToString(), log.GetMessage());
            }
        }

        public async Task ShowMessage(string type, string message)
        {
            await _Runtime.InvokeVoidAsync("showMessage", type.ToString(), message);
        }
    }
}
