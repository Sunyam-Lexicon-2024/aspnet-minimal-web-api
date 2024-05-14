using Microsoft.EntityFrameworkCore;
using MinimalApi.Models;

namespace MinimalApi.Data;

public class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options) {
    public DbSet<Item> Items{ get; set; }
}