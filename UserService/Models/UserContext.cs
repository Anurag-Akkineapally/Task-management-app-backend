using Microsoft.EntityFrameworkCore;

namespace UserService.Models;
public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }
}