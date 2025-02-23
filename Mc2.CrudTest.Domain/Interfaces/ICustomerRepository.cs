using Mc2.CrudTest.Domain.Entities;

namespace Mc2.CrudTest.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<int> AddAsync(Customer customer);
        Task<bool> ExistsAsync(string firstName, string lastName, DateTime dateOfBirth, string email);
        Task<Customer> GetByIdAsync(int id);
        Task<List<Customer>> GetAllAsync();
        Task<bool> UpdateAsync(Customer customer, byte[] timestamp);
        Task<bool> DeleteAsync(int id, byte[] timestamp);
    }
}
