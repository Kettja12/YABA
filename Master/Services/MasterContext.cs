using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Master.Model.Master;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;

public class MasterContext : DbContext
{
    ILoggerFactory loggerFactory = new LoggerFactory();

    public MasterContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(loggerFactory);
        optionsBuilder.LogTo(Console.WriteLine).EnableDetailedErrors();
        base.OnConfiguring(optionsBuilder);
    }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Database> Databases { get; set; }
    public virtual DbSet<DatabaseUser> DatabaseUsers { get; set; }

}