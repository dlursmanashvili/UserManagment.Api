using Domain.Shared;

namespace Domain.Entities.UserProfileEntity;

public class UserProfile : BaseEntity<int>
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonalNumber { get; set; }
}
