namespace BankingManagementSystemService.DTO
{
    public class TransactionHistoryDto
    {
        public DateTime TransactionDate { get; set; }

        public string TransactionType { get; set; }

        public decimal Amount { get; set; }

        public decimal BalanceAfterTransaction { get; set; }
    }
}
