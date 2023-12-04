using Domain.Entities.UserProfileEntity.IRepository;
using Infrastructure.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserProfileRepository : BaseRepository , IUserProfileRepository
    {
        public UserProfileRepository(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider)
        : base(applicationDbContext, serviceProvider)
        {
        }
    }
}
