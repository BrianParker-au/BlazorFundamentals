// ---------------------------------------------------------------
// Copyright (c) Brian Parker All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using BlazorFundamentals.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorFundamentals.Services.MediaQuery
{

    public class MediaQueryService : IMediaQueryService
    {
        private readonly IJSRuntime jsRuntime;
        private DotNetObjectReference<MediaQueryService>? objRef;
        private IJSObjectReference? module;
        private readonly System.Timers.Timer timer;
        private bool isResizing;
        private WindowSize windowSize;

        public MediaQueryService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
            this.timer = new(interval: 25);
            this.timer.Elapsed += async (s, e) => await GetDimensions(s, e);
            this.isResizing = false;
        }

        public WindowSize WindowSize => this.windowSize;

        public event NotifyResize OnResize;

        public event NotifyResizing OnResizing;

        public async ValueTask<ElementRectangle> GetElementBoundingRectangleAsync(ElementReference elementReference) =>
            await module.InvokeAsync<ElementRectangle>(identifier: "GetBoundingRectangle", elementReference);

        public async ValueTask InitializeAsync()
        {
            this.module = await this.jsRuntime.InvokeAsync<IJSObjectReference>(
               identifier: "import",
                args: "./_content/BSizeJsInterop/sizer.js");

            this.objRef = DotNetObjectReference.Create(this);
            await this.module.InvokeVoidAsync(identifier: "listenToWindowResize", this.objRef);
            await this.GetWindowSizeAsync();
        }

        public async ValueTask<WindowSize> GetWindowSizeAsync() =>
            this.windowSize = await this.module.InvokeAsync<WindowSize>(identifier: "GetWindowRectangle");

        [JSInvokable]
        public ValueTask WindowResizeEvent()
        {
            if (this.isResizing is not true)
            {
                this.isResizing = true;
                this.OnResizing?.Invoke(this.isResizing);
            }
            this.DebounceEvent();
            return ValueTask.CompletedTask;
        }

        private void DebounceEvent() => this.timer.Restart();

        private async ValueTask GetDimensions(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.timer.Stop();
            await this.GetWindowSizeAsync();
            this.OnResize?.Invoke();
            this.isResizing = false;
            this.OnResizing?.Invoke(this.isResizing);
        }
    }
}
