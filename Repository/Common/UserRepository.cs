using Microsoft.AspNetCore.Identity;
using Models.Classes;
using Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Common
{
    public interface IUserRepository
    {
        List<AppUser> Users { get;}
        List<LabelValue> GetUserList(string term);

    }

    public class UserRepository : IUserRepository
    {
        private UserManager<AppUser> userManager;

        public UserRepository(UserManager<AppUser> usrMng)
        {
            userManager = usrMng;
        }

        public List<AppUser> Users {
            get { return (userManager.Users).ToList(); }
        }

        public List<LabelValue> GetUserList(string term)
        {
            return userManager.Users.Where(t => t.UserName.StartsWith(term)).Select(tt=> new LabelValue { label= tt.UserName,value=tt.user_id.ToString() } ).ToList();
        }
    }
}
