using Mc2.CrudTest.Domain.Entities;
using MediatR;

namespace Mc2.CrudTest.Application.Use_Cases;

public class GetCustomerByIdQuery : IRequest<Customer>
{
    public GetCustomerByIdQuery()
    {
    }

    public GetCustomerByIdQuery(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}
