using System.ComponentModel.DataAnnotations;

namespace BankingManagementSystemService.Models
{
    public class Users
    {
        public int UserId { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Role { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string MaritalStatus { get; set; }
        public string Religion { get; set; }
        public string AadhaarNumber { get; set; }
        public string PANNumber { get; set; }

        public string PermanentAddress { get; set; }
        public string CurrentAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }

        public string NomineeName { get; set; }
        public string NomineeRelation { get; set; }
        public DateTime? NomineeDOB { get; set; }
        public string NomineePhone { get; set; }

        public ICollection<Accounts> Accounts { get; set; }
    }
}
