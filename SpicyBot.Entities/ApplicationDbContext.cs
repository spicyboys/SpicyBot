using Microsoft.EntityFrameworkCore;
using SpicyBot.Entities.Sumo;

namespace SpicyBot.Entities;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<SumoBet> SumoBets { get; set; }
    public DbSet<SumoGame> SumoGames { get; set; }
}