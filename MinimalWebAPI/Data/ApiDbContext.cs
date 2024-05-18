using Microsoft.EntityFrameworkCore;
using MinimalWebAPI.Models;

namespace MinimalWebAPI.Data;

public class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options) {
    public DbSet<Item> Items{ get; set; }
}