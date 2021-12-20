// ---------------------------------------------------------------
// Copyright (c) Brian Parker All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using BlazorFundamentals.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorFundamentals.Services.MediaQuery
{
    public interface IMediaQueryService
    {
        WindowSize WindowSize { get; }

        event NotifyResize OnResize;
        event NotifyResizing OnResizing;

        ValueTask InitializeAsync();
        ValueTask<ElementRectangle> GetElementBoundingRectangleAsync(ElementReference someDiv);
    }
}
