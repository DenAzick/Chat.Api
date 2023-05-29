using Chat.Core.Context;
using Identity.Core.Context;
using Microsoft.EntityFrameworkCore;


namespace Chat.Api.Extentions;

public static class WebApplicationExtensions
{
    public static void MigrateChatDbContext(this WebApplication app)
    {
        if (app.Services.GetService<ChatDbContext>() != null)
        {
            var chatDb = app.Services.GetService<ChatDbContext>();
            chatDb.Database.Migrate();
        }
    }

    public static void MigrateIdentityDb(this WebApplication app)
    {
        if (app.Services.GetService<IdentityDbContext>() != null)
        {
            var identityDb = app.Services.GetRequiredService<IdentityDbContext>();
            identityDb.Database.Migrate();
        }
    }
}
