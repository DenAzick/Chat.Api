using Identity.Core.Context;
using Identity.Core.Entities;
using Identity.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Identity.Core.Managers;

public class UserManager
{
    private readonly IdentityDbContext _context;
    private ILogger<UserManager> _logger;
    private readonly JwtTokenManager _jwtTokenManager;

    public UserManager(IdentityDbContext context, ILogger<UserManager> logger, JwtTokenManager jwtTokenManager)
    {
        _context = context;
        _logger = logger;
        _jwtTokenManager = jwtTokenManager;
    }

    public async Task<User> Register(CreateUserModel createUserModel)
    {
        if (await _context.Users.AnyAsync(u => u.Username == createUserModel.Username))
        {
            throw new Exception("ro'yxatdan o'than");
        }
        var user = new User()
        {
            Username = createUserModel.Username,
            Name = createUserModel.Name,
        };
        user.PasswordHash = new PasswordHasher<User>()
            .HashPassword(user, createUserModel.Password);

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<string> Login(LoginUserModel loginUserModel)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginUserModel.Username);

        if (user == null)
        {
            throw new Exception("username yoki parol xato");
        }

        var result  = new PasswordHasher<User>()
            .VerifyHashedPassword(user, user.PasswordHash, loginUserModel.Password);

        if (result != PasswordVerificationResult.Success)
        {
            throw new Exception("username yoki parol xato");
        }

        var token = _jwtTokenManager.GenerateToken(user);

        return token;
    }

    public async Task<User> GetUser(Guid userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User> GetUser(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
}
