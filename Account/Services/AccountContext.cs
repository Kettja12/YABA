using Account.Model;
using Microsoft.EntityFrameworkCore;

public class AccountContext : DbContext
{
    ILoggerFactory loggerFactory = new LoggerFactory();

    public AccountContext(DbContextOptions options) : base(options)
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
    public virtual DbSet<UserUsergroup> UserUsergroups { get; set; }
    public virtual DbSet<Usergroup> Usergroups { get; set; }
    public virtual DbSet<UsergroupRight> UsergroupRights { get; set; }
    public virtual DbSet<Right> Rights { get; set; }


}