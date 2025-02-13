using Mc2.CrudTest.Domain.Entities;
using MediatR;

namespace Mc2.CrudTest.Application.Use_Cases
{
    public class GetCustomerByEmailQuery : IRequest<Customer>
    {
        public string email { get; set; }
    }
}
