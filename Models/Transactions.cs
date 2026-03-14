namespace BankingManagementSystemService.Models
{
    public class Transactions
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfterTransaction { get; set; }
        public DateTime TransactionDate { get; set; }

        public Accounts Account { get; set; }
    }
}
