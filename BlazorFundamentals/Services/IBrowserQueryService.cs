// ------------------------------------------------------------
// Copyright (c) Brian Parker All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ------------------------------------------------------------

using BlazorFundamentals.Delegates;
using BlazorFundamentals.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorFundamentals.Services;

public interface IBrowserQueryService
{
    BrowserSizeDetails? BrowserSizeDetails { get; }

    event NotifyResize OnResize;
    event NotifyResizing OnResizing;

    ValueTask InitializeAsync();
    ValueTask<ElementBoundingRectangle> GetElementBoundingRectangleAsync(ElementReference elementReference);
    ValueTask<BrowserSizeDetails> GetWindowSizeAsync();
}
