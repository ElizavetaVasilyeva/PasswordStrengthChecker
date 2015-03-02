using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordStrengthChecker.ORM;

namespace PasswordStrengthChecker.BLL
{
    public interface IUserRepository
    {
        int Create(User user);
    }
}
