using Domain.Entities.UserEntity;
using Domain.Entities.UserEntity.IRepository;
using Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    private readonly byte[] keyBytes = new byte[32]; // 256 bits key

    public UserRepository(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider)
        : base(applicationDbContext, serviceProvider)
    {
    }

    public async Task<CommandExecutionResult> Registration(User user)
    {
        if (_ApplicationDbContext.Users.Any(x => x.Email == user.Email))
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "Choose another email, this email already exists" };
        }
        try
        { 
            
            user.Password = PasswordHelper.HashPassword(user.Password);
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

    public async Task<CommandExecutionResult> UpdateAsyncUser(User user)
    {
        var employe = _ApplicationDbContext.Users.FirstOrDefault(x => x.Id == user.Id);

        if (employe.IsNull())
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "user not found" };
        }
        try
        {
            employe.Password = PasswordHelper.HashPassword(user.Password);
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

    //public async Task<CommandExecutionResult> Login(string email, string password)
    //{
    //    var user = _ApplicationDbContext.Users.FirstOrDefault(x => x.Email == email);
    //    if (user.IsNull())
    //    {
    //        return new CommandExecutionResult() { Success = false, ErrorMessage = "user not found" };
    //    }
    //    if (PasswordHelper.VerifyPassword(password, user.Password))
    //    {
    //        //var token = GenerateToken(user);
    //        return new CommandExecutionResult()
    //        {
    //            Success = true,                
    //        };
    //    }
    //    else
    //    {
    //        return new CommandExecutionResult() { Success = false, ErrorMessage = "Password incorrect" };
    //    }
    //}





    //private string GenerateToken(User user)
    //{
    //    if (user == null) throw new ArgumentNullException(nameof(user));

    //    var claims = new Dictionary<string, string>()
    //    {
    //        { "email_address", user.Email },
    //    };      

    //    var accessToken = GenerateAccessToken(user.Id, claims);
    //    return accessToken;
    //}

    //private string GenerateAccessToken(int id, IDictionary<string, string> claims)
    //{      
    //    var convertedClaims = claims?.Select(x => new Claim(x.Key, x.Value)).ToList() ?? new List<Claim>();
    //    convertedClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, id.ToString()));
    //    convertedClaims.Add(new Claim(ClaimTypes.Name, id.ToString()));

    //    var accessToken = GenerateJwt(convertedClaims);
    //    var tokenResponse = new JwtSecurityTokenHandler().WriteToken(accessToken);    

    //    return tokenResponse;
    //}

    //private JwtSecurityToken GenerateJwt(IEnumerable<Claim> claims)
    //{
    //    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("B374A26A71490437AA024E4FADD5B497FDFF1A8EA6FF12F6FB65AF2720B59CCF"));
    //    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

    //    var jwtSecurityToken = new JwtSecurityToken(
    //        issuer: "Issuer",
    //        audience: "Audience",
    //        claims: claims,
    //        notBefore: DateTime.UtcNow,
    //        expires: DateTime.UtcNow.AddHours(10),
    //        signingCredentials: signingCredentials);

    //    return jwtSecurityToken;
    //}
    #region old generate ttoken code
    //private string GenerateToken(User user)
    //{
    //    var securityKey = new SymmetricSecurityKey(keyBytes);
    //    var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    //    var token = new JwtSecurityToken(
    //        issuer: "your_issuer",
    //        audience: "your_audience",
    //        claims: GetClaims(user),
    //        notBefore: DateTime.UtcNow,
    //        expires: DateTime.UtcNow.AddHours(1),
    //        signingCredentials: signingCredentials
    //    );

    //    var tokenHandler = new JwtSecurityTokenHandler();
    //    return tokenHandler.WriteToken(token);
    //}

    #endregion
    private Claim[] GetClaims(User user)
    {
        // Implement your logic to retrieve claims from the user
        // For example, return user roles as claims
        return new Claim[]
        {
                new Claim(ClaimTypes.Name, user.Username),
            // Add other claims as needed
        };
    }



    //private string GenerateToken(User user)
    //{
    //    var tokenHandler = new JwtSecurityTokenHandler();
    //    var key = Encoding.ASCII.GetBytes("bc25897AA@123*/%");
    //    var tokenDescriptor = new SecurityTokenDescriptor
    //    {
    //        Subject = new ClaimsIdentity(new Claim[]
    //        {
    //        new Claim(ClaimTypes.Name, user.Id.ToString()),
    //            // Дополнительные утверждения, если необходимо
    //        }),
    //        Expires = DateTime.UtcNow.AddHours(1), // Время действия токена
    //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    //    };
    //    var token = tokenHandler.CreateToken(tokenDescriptor);
    //    return tokenHandler.WriteToken(token);
    //}
}