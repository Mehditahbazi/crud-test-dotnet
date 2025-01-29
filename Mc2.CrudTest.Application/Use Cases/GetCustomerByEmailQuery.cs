using Mc2.CrudTest.Presentation.Server.Models;
using MediatR;

namespace Mc2.CrudTest.Application.Use_Cases
{
    public class GetCustomerByEmailQuery : IRequest<Customer>
    {
        public string email { get; set; }
    }
}
