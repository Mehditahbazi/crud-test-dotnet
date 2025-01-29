//using Mc2.CrudTest.Infrastructure.Persistence;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration; // Important!
//using YourProject.Infrastructure.Persistence;

//namespace YourProject.Persistence // Or a suitable namespace
//{
//    public class CustomerDbContextFactory : IDesignTimeDbContextFactory<CustomerDbContext>
//    {
//        public CustomerDbContext CreateDbContext(string[] args)
//        {g
//            // 1. Get Configuration (Crucial!)
//            IConfiguration configuration = new ConfigurationBuilder()
//                .AddJsonFile("appsettings.json") // Or your configuration source (e.g., appsettings.Development.json)
//                .Build();

//            // 2. Get Connection String
//            var connectionString = configuration.GetConnectionString("DefaultConnection"); // Your connection string name

//            // 3. Build DbContextOptions
//            var builder = new DbContextOptionsBuilder<CustomerDbContext>();
//            builder.UseSqlServer(connectionString); // Or your database provider

//            return new CustomerDbContext(builder.Options);
//        }
//    }
//}