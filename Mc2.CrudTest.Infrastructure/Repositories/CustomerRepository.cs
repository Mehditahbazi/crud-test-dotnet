using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Domain.Interfaces;
using Mc2.CrudTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Customer customer)
        {
            if (await ExistsAsync(customer.FirstName, customer.LastName, customer.DateOfBirth, customer.Email))
                throw new InvalidOperationException("Customer already exists.");

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer.Id;
        }

        public async Task<bool> ExistsAsync(string firstName, string lastName, DateTime dateOfBirth, string email)
        {
            return await _context.Customers
                .AnyAsync(c => c.FirstName == firstName && c.LastName == lastName && c.DateOfBirth == dateOfBirth && c.Email == email);
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Customer customer, byte[] timestamp)
        {
            var existingCustomer = await _context.Customers.FindAsync(customer.Id);
            if (existingCustomer == null)
                throw new InvalidOperationException("Customer not found.");

            // Validate timestamp for concurrency
            if (!existingCustomer.TimeStamp.SequenceEqual(timestamp))
                throw new InvalidOperationException("Customer data has changed. Please refresh and try again.");

            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.PhoneNumber = customer.PhoneNumber;
            existingCustomer.Email = customer.Email;
            existingCustomer.BankAccountNumber = customer.BankAccountNumber;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, byte[] timestamp)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return false;

            // Validate timestamp for concurrency
            if (!customer.TimeStamp.SequenceEqual(timestamp))
                throw new InvalidOperationException("Customer data has changed. Please refresh and try again.");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
