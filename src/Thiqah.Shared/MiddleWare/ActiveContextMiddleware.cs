using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Thiqah.Shared.Context;
using Thiqah.Shared.Exceptions;

namespace Thiqah.Shared.MiddleWare;

public sealed class ActiveContextMiddleware
{
    private readonly RequestDelegate _next;

    public ActiveContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context, ActiveContext activeContext)
    {
        activeContext.RequestDate = DateTime.Now;
        activeContext.RequestId = context.TraceIdentifier;
        await _next(context);
    }
}
