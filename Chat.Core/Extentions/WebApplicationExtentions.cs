using Chat.Core.Context;
using Microsoft.EntityFrameworkCore;

namespace Chat.Core.Extentions;

public static class WebApplicationExtensions
{
    public static void MigrateChatDbContext(this WebApplication app)
    {
        if (app.Services.GetService)
        {

        }
    }
}
