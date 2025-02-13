using MediatR;

namespace Mc2.CrudTest.Application.Use_Cases
{
    public class UpdateCustomerCommand : IRequest<bool>
    {
        public UpdateCustomerCommand()
        {
        }

        public UpdateCustomerCommand(int id, string? firstName, string? lastName, DateTime? dateOfBirth, string? phoneNumber, string? email, string? bankAccountNumber, string timeStamp)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            PhoneNumber = phoneNumber;
            Email = email;
            BankAccountNumber = bankAccountNumber;
            TimeStamp = timeStamp;
        }

        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? BankAccountNumber { get; set; }

        public string TimeStamp { get; set; }
    }
}
