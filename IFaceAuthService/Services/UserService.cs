using IFaceAuthService.Data;
using IFaceAuthService.Entities;
using Microsoft.EntityFrameworkCore;

namespace IFaceAuthService.Services;

public class UserService(AppDbContext dbContext)
{
    public Task<User?> GetByEmailAsync(string email)
    {
        return dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User> CreateAsync(User user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return user;
    }
}
