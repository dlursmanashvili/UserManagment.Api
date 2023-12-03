using Domain.Entities.UserEntity;
using Domain.Entities.UserEntity.IRepository;
using Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Shared;
using System.Text;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{

    public UserRepository(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider)
        : base(applicationDbContext, serviceProvider)
    {
    }

    public async Task<CommandExecutionResult> Registration(User user)
    {
        try
        {
            user.Password = HashPassword(user.Password);
            user.IsActive = true;
            var id = await Insert<User, int>(user);

            return new CommandExecutionResult()
            {
                ResultId = id.ToString(),
                Success = true,
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
            return new CommandExecutionResult() { Success = true, };

        }
        else
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "Password incorrect" };

        }
    }
    public async Task<CommandExecutionResult> UpdateAsyncUser(User user)
    {
        try
        {
            user.Password = HashPassword(user.Password);
            await Update(user);
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
}