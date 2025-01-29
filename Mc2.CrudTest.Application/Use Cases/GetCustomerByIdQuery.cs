using Mc2.CrudTest.Presentation.Server.Models;
using MediatR;

namespace Mc2.CrudTest.Application.Use_Cases;

public class GetCustomerByIdQuery : IRequest<Customer>
{
    public int Id { get; set; }
}
