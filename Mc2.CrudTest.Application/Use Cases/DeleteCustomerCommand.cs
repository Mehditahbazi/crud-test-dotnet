using MediatR;

namespace Mc2.CrudTest.Application.Use_Cases
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public DeleteCustomerCommand()
        {
        }

        public DeleteCustomerCommand(int id, string timestamp)
        {
            Id = id;
            Timestamp = timestamp;
        }

        public int Id { get; set; }
        public string Timestamp { get; set; }
    }
}
