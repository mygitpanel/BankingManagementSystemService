using BankingManagement.API.Data;
using BankingManagementSystemService.DTO;
using BankingManagementSystemService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingManagementSystemService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly BankingDbContext _context;
        public AccountController(BankingDbContext context)
        {
            _context = context;
        }
        [HttpGet("account/{accountNumber}")]
        public async Task<IActionResult> GetAccountDetails(long accountNumber)
        {
            var account = await _context.Accounts
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
                return NotFound("Account not found");

            return Ok(new
            {
                account.AccountNumber,
                account.AccountType,
                account.Balance,

                account.User.FullName,
                account.User.Email,
                account.User.PhoneNumber,
                account.User.DateOfBirth,
                account.User.Gender,
                account.User.Nationality,
                account.User.MaritalStatus,
                account.User.Religion,
                account.User.AadhaarNumber,
                account.User.PANNumber,
                account.User.PermanentAddress,
                account.User.CurrentAddress,
                account.User.City,
                account.User.State,
                account.User.Pincode,
                account.User.NomineeName,
                account.User.NomineeDOB,
                account.User.NomineeRelation,
                account.User.NomineePhone
            });
        }

        [HttpGet("validate-account/{accountNumber}")]
        public async Task<IActionResult> ValidateAccount(long accountNumber)
        {
            var account = await _context.Accounts
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
                return NotFound("Account not found");

            return Ok(new
            {
                AccountNumber = account.AccountNumber,
                AccountHolderName = account.User.FullName
            });
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferMoney([FromBody] TransferRequestDto request)
        {
            using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var sender = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.AccountNumber == request.FromAccountNumber);

                var receiver = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.AccountNumber == request.ToAccountNumber);

                if (sender == null || receiver == null)
                    return BadRequest("Invalid account number");

                if (sender.AccountNumber == receiver.AccountNumber)
                    return BadRequest("Cannot transfer to same account");

                if (request.Amount <= 0)
                    return BadRequest("Invalid transfer amount");

                if (sender.Balance < request.Amount)
                    return BadRequest("Insufficient balance");

                // Deduct sender balance
                sender.Balance -= request.Amount;

                // Add receiver balance
                receiver.Balance += request.Amount;

                // Sender transaction record
                var senderTransaction = new Transactions
                {
                    AccountId = sender.AccountId,
                    TransactionType = "Debit",
                    Amount = request.Amount,
                    BalanceAfterTransaction = sender.Balance,
                    TransactionDate = DateTime.Now
                };

                // Receiver transaction record
                var receiverTransaction = new Transactions
                {
                    AccountId = receiver.AccountId,
                    TransactionType = "Credit",
                    Amount = request.Amount,
                    BalanceAfterTransaction = receiver.Balance,
                    TransactionDate = DateTime.Now
                };

                _context.Transactions.Add(senderTransaction);
                _context.Transactions.Add(receiverTransaction);

                await _context.SaveChangesAsync();

                await dbTransaction.CommitAsync();

                return Ok(new { message = "Transfer Successful" });
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                return StatusCode(500, "Transfer failed");
            }
        }

        [HttpGet("history/{accountNumber}")]
        public async Task<IActionResult> GetHistory(long accountNumber)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
                return NotFound();

            var transactions = await _context.Transactions
                .Where(t => t.AccountId == account.AccountId)
                .OrderByDescending(t => t.TransactionDate)
                .Select(t => new TransactionHistoryDto
                {
                    TransactionDate = t.TransactionDate,
                    TransactionType = t.TransactionType,
                    Amount = t.Amount,
                    BalanceAfterTransaction = t.BalanceAfterTransaction
                })
                .ToListAsync();

            return Ok(transactions);
        }

        [HttpGet("summary/{accountNumber}")]
        public async Task<IActionResult> GetSummary(long accountNumber)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
                return NotFound();

            var transactions = await _context.Transactions
                .Where(t => t.AccountId == account.AccountId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            var lastTransaction = transactions.FirstOrDefault();

            var response = new
            {
                AccountNumber = account.AccountNumber,
                Balance = account.Balance,
                TotalTransactions = transactions.Count,
                LastTransactionAmount = lastTransaction?.Amount,
                LastTransactionType = lastTransaction?.TransactionType,
                LastTransactionDate = lastTransaction?.TransactionDate
            };

            return Ok(response);
        }
    }
}
