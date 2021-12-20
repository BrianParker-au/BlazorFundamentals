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
            await module!.InvokeAsync<ElementRectangle>(identifier: "GetBoundingRectangle", elementReference);

        public async ValueTask InitializeAsync()
        {
            this.module = await jsRuntime.InvokeAsync<IJSObjectReference>(
               identifier: "import",
                args: "./_content/BSizeJsInterop/sizer.js");

            this.objRef = DotNetObjectReference.Create(this);
            await module!.InvokeVoidAsync(identifier: "listenToWindowResize", this.objRef);
            this.windowSize = await module!.InvokeAsync<WindowSize>(identifier: "GetWindowRectangle");
        }

        [JSInvokable]
        public ValueTask WindowResizeEvent()
        {
            if (this.isResizing is not true)
            {
                this.isResizing = true;
                OnResizing?.Invoke(this.isResizing);
            }
            DebounceEvent();
            return ValueTask.CompletedTask;
        }

        private void DebounceEvent() => this.timer.Restart();

        private async ValueTask GetDimensions(object? sender, System.Timers.ElapsedEventArgs e)
        {
            this.timer.Stop();
            this.windowSize = await module!.InvokeAsync<WindowSize>(identifier: "GetWindowRectangle");
            OnResize?.Invoke();
            isResizing = false;
            OnResizing?.Invoke(this.isResizing);
        }
    }
}
