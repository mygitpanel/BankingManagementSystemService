namespace BankingManagementSystemService.Models
{
    public class Accounts
    {
        public int AccountId { get; set; }
        public long AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }

        public int UserId { get; set; }
        public Users User { get; set; }

        public ICollection<Transactions> Transactions { get; set; }
    }
}
