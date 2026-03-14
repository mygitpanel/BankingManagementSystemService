namespace BankingManagementSystemService.DTO
{
    public class TransferRequestDto
    {
        public long FromAccountNumber { get; set; }
        public long ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
