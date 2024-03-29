﻿using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Identity.Core.Providers;

public class UserProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserProvider(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    protected HttpContext? Context => _contextAccessor.HttpContext;

    public string UserName => Context.User.FindFirstValue(ClaimTypes.Name);

    public Guid UserId => Guid.Parse(Context.User.FindFirstValue(ClaimTypes.NameIdentifier));
}
