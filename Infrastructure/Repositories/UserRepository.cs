using Domain.Entities.UserEntity;
using Domain.Entities.UserEntity.IRepository;
using Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Shared;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{

    public UserRepository(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider)
        : base(applicationDbContext, serviceProvider)
    {
    }

    public async Task<CommandExecutionResult> Registration(User user)
    {
        if (_ApplicationDbContext.Users.Any(x=> x.Email == user.Email))
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "Choose another email, this email already exists" };
        }
        try
        {
            user.Password = HashPassword(user.Password);
            user.IsActive = true;
            var id = await Insert<User, int>(user);
            var token = GenerateToken(user);

            return new CommandExecutionResult()
            {
                ResultId = id.ToString(),
                Success = true,
                //ErrorMessage = new { Token = token }.ToString()

            };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult()
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }
    public async Task<CommandExecutionResult> Login( string email , string password)
    {
        var user =_ApplicationDbContext.Users.FirstOrDefault(x => x.Email == email);
        if (user.IsNull())
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "user not found" };
        }
        if (VerifyPassword(password,user.Password))
        {
            return new CommandExecutionResult() { Success = true};
        }
        else
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "Password incorrect" };
        }
    }
    public async Task<CommandExecutionResult> UpdateAsyncUser(User user)
    {
        var employe = _ApplicationDbContext.Users.FirstOrDefault(x => x.Id == user.Id);

        if (employe.IsNull())
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "user not found" };
        }
        try
        {
            employe.Password = HashPassword(user.Password);
            employe.Email = user.Email;
            employe.IsActive = user.IsActive;
            await Update(employe);
            return new CommandExecutionResult() { Success = true };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult()
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }
    public async Task<CommandExecutionResult> DeleteAsync(int id)
    {
        var user = _ApplicationDbContext.Users.FirstOrDefault(x => x.Id == id);

        if (user.IsNull())
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "user not found" };
        }
        user.IsActive = false;
        var result = UpdateAsyncUser(user);
        return await result;


    }
    public async Task<User> GetByIdAsync(int id)
    {
        return await _ApplicationDbContext.Users.FindAsync(id);
    }
    public async Task<List<User>> GetAllAsync()
    {
        return await _ApplicationDbContext.Users.ToListAsync();
    }
    private string HashPassword(string password)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
    private bool VerifyPassword(string enteredPassword, string hashedPassword)
    {
        string enteredPasswordHashed = HashPassword(enteredPassword);
        return string.Equals(enteredPasswordHashed, hashedPassword, StringComparison.OrdinalIgnoreCase);
    }
    private string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("bc25897AA@123*/%");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Name, user.Id.ToString()),
                // Дополнительные утверждения, если необходимо
            }),
            Expires = DateTime.UtcNow.AddHours(1), // Время действия токена
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}