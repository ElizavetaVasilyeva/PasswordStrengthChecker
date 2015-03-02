using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using PasswordStrengthChecker.ORM;

namespace PasswordStrengthChecker.BLL
{
    public class UserRepository : IUserRepository
    {
        private UserContext context;
        public UserRepository()
        {
            context = new UserContext();
        }
        public int Create(User user)
        {
           User newUser;
            if (CheckUser(user.Name)) throw new InvalidOperationException("User with this name already exist!");
            using (context)
            {
                context.Users.Add(user);
                context.SaveChanges();

                newUser = (from item in context.Users
                               where item.Name == user.Name & item.Password == user.Password
                               select item).SingleOrDefault();

                if (newUser == null)
                    throw new InvalidOperationException("Problem with database! Try again!");
            }
            return newUser.Id;
        }

        private bool CheckUser(string name)
        {
            var user =(from item in context.Users
                where item.Name == name
                select item).SingleOrDefault();
            if (user == null) return false;
            return true;
        }
    }
}
