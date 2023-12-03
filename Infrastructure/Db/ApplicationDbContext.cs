using Domain.Entities.UserEntity;
using Domain.Entities.UserProfileEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
}