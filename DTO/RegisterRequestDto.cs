namespace BankingManagementSystemService.DTO
{
    public class RegisterRequestDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
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

        public string AccountType { get; set; }
        public Decimal Balance { get; set; }
    }
}
