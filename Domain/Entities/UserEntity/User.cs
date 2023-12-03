using Domain.Entities.UserProfileEntity;
using Domain.Shared;

namespace Domain.Entities.UserEntity
{
    public class User : BaseEntity<int>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public UserProfile Profile { get; set; }
    }
}
