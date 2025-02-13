using System.ComponentModel.DataAnnotations;

namespace Mc2.CrudTest.Domain.Entities;

public class Customer
{
    public Customer()
    {
    }

    public Customer(int id, string firstName, string lastName, DateTime dateOfBirth, string phoneNumber, string email, string bankAccountNumber, byte[] timeStamp)
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

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [StringLength(15)]
    public string PhoneNumber { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(20)]
    public string BankAccountNumber { get; set; }
    public byte[] TimeStamp { get; set; }
}