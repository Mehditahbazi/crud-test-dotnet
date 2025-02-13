using Mc2.CrudTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class CustomerDbContextFixture : IDisposable
{
    public CustomerDbContext Context { get; private set; }

    public CustomerDbContextFixture()
    {
        var options = new DbContextOptionsBuilder<CustomerDbContext>()
            .UseInMemoryDatabase(databaseName: "CustomerDB")
            .Options;

        Context = new CustomerDbContext(options);

        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}
