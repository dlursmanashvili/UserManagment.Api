using Domain.Shared;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.UserProfileEntity;

public class UserProfile : BaseEntity<int>
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [MaxLength(11)]
    public string PersonalNumber { get; set; }
}
