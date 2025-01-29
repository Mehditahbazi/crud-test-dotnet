using MediatR;

namespace Mc2.CrudTest.Application.Use_Cases
{
    public class DeleteCustomerCommand : IRequest
    {
        public int Id { get; set; }
        public string Timestamp { get; set; }
    }
}
