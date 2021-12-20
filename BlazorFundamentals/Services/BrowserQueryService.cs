// ------------------------------------------------------------
// Copyright (c) Brian Parker All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ------------------------------------------------------------

using BlazorFundamentals.Delegates;
using BlazorFundamentals.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorFundamentals.Services;

public class BrowserQueryService : IBrowserQueryService
{
    public BrowserSizeDetails? BrowserSizeDetails { get; }

    public event NotifyResize? OnResize;
    public event NotifyResizing? OnResizing;

    public ValueTask<ElementBoundingRectangle> GetElementBoundingRectangleAsync(ElementReference elementReference) =>
        throw new NotImplementedException();

    public ValueTask<BrowserSizeDetails> GetWindowSizeAsync() =>
        throw new NotImplementedException();

    public ValueTask InitializeAsync() =>
        throw new NotImplementedException();
}
