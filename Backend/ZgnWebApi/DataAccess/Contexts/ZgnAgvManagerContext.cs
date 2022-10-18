using Microsoft.EntityFrameworkCore;
using ZgnWebApi.Entities;
#nullable disable
namespace ZgnWebApi.DataAccess.Contexts
{
    public class ZgnAgvManagerContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<Authority> Authorities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAuthority> UserAuthorities { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<AuthorityOperationClaim> AuthorityOperationClaims { get; set; }
        public DbSet<AuthorityStation> AuthorityStations { get; set; }
        public DbSet<UserStation> UserStations { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<StationNode> StationNodes { get; set; }
        public DbSet<StationGroupCode> StationGroupCodes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
