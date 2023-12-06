using Shared;

namespace Domain.Entities.UserEntity.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<List<User>> GetAllAsync();
        Task<CommandExecutionResult> Registration(User user);
        Task<CommandExecutionResult> UpdateAsyncUser(User user);
        Task<CommandExecutionResult> DeleteAsync(int id);
        //Task<CommandExecutionResult> Login(string email, string password);
    }
}
