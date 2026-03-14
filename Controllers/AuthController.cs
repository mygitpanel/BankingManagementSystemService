using BankingManagement.API.Data;
using BankingManagementSystemService.DTO;
using BankingManagementSystemService.Helpers;
using BankingManagementSystemService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly BankingDbContext _context;

        public AuthController(BankingDbContext context)
        {
            _context = context;
        }

        // REGISTER (Admin creates user + account)
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            try
            {
                if (await _context.Users.AnyAsync(x => x.Email == request.Email))
                    return BadRequest("User already exists");

                var user = new Users
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    PasswordHash = PasswordHasher.HashPassword(request.Password),

                    PhoneNumber = request.PhoneNumber,
                    Role = "User",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,

                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,
                    Nationality = request.Nationality,
                    MaritalStatus = request.MaritalStatus,
                    Religion = request.Religion,

                    AadhaarNumber = request.AadhaarNumber,
                    PANNumber = request.PANNumber,

                    PermanentAddress = request.PermanentAddress,
                    CurrentAddress = request.CurrentAddress,
                    City = request.City,
                    State = request.State,
                    Pincode = request.Pincode,

                    NomineeName = request.NomineeName,
                    NomineeRelation = request.NomineeRelation,
                    NomineeDOB = request.NomineeDOB,
                    NomineePhone = request.NomineePhone
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Account creation
                var account = new Accounts
                {
                    UserId = user.UserId,
                    AccountType = request.AccountType,
                    Balance = request.Balance,
                    AccountNumber = await GenerateAccountNumber()
                };

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Account created successfully",
                    accountNumber = account.AccountNumber
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var hashedPassword = PasswordHasher.HashPassword(request.Password);

            var user = await _context.Users
                .Include(x => x.Accounts)
                .FirstOrDefaultAsync(x =>
                    x.Email == request.Email &&
                    x.PasswordHash == hashedPassword &&
                    x.IsActive);

            if (user == null)
                return Unauthorized("Invalid credentials");

            return Ok(new
            {
                userId = user.UserId,
                fullName = user.FullName,
                email = user.Email,
                role = user.Role,
                accountNumber = user.Accounts.FirstOrDefault()?.AccountNumber
            });
        }
        private async Task<long> GenerateAccountNumber()
        {
            var lastAccount = await _context.Accounts
                .OrderByDescending(x => x.AccountNumber)
                .FirstOrDefaultAsync();

            if (lastAccount == null)
                return 100236701;

            return lastAccount.AccountNumber + 10;
        }
    }
}