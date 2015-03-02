using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PasswordStrengthChecker.ORM;

namespace PasswordStrengthChecker.BLL
{
    public interface IValidationStrategy
    {
        bool IsValidPassword(string pwd);
    }

    public class UserPasswordValidation : IValidationStrategy
    {
        public bool IsValidPassword(string pwd)
        {
            Regex reg1 = new Regex(@"[A-Za-z]");
            Regex reg2 = new Regex(@"[0-9]");

            if (!String.IsNullOrEmpty(pwd) && pwd.Length >= 7 && reg1.IsMatch(pwd)
                && reg2.IsMatch(pwd))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class AdminPasswordValidation : IValidationStrategy
    {
        public bool IsValidPassword(string pwd)
        {
            Regex reg1 = new Regex(@"[A-Za-z]");
            Regex reg2 = new Regex(@"[0-9]");
            Regex reg3 = new Regex(@"[_\*\%$#@/-]");

            if (!String.IsNullOrEmpty(pwd) && pwd.Length >= 10 && reg1.IsMatch(pwd) 
                && reg2.IsMatch(pwd) && reg3.IsMatch(pwd))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class PasswordChecker
    {
        private IUserRepository rep;

        public PasswordChecker(IUserRepository rep)
        {
            this.rep = rep;
        }

        //Implementation of the method using strategy pattern.

        public Tuple<bool, string> Verify(string pwd, string username, bool admin = false)
        {
            IValidationStrategy validationStrategy;
            bool answer;
            if (admin)
            {
                validationStrategy = new AdminPasswordValidation();
                answer = validationStrategy.IsValidPassword(pwd);
                if (!answer)
                {
                    return Tuple.Create(false,
                   "Password for admins should be at least 10 chatacters and contains at least " +
                   "1 alphabetical, 1 digit and 1 special chatacter!");
                }
            }
            else
            {
                validationStrategy = new UserPasswordValidation();
                answer = validationStrategy.IsValidPassword(pwd);
                if (!answer)
                {
                    return Tuple.Create(false,
                  "Password should be at least 7 chatacters and contains at least " +
                  "1 alphabetical and 1 digit chatacter!");
                }

            }
            try
            { 
            User user = new User()
                {
                    Name = username,
                    Password = pwd,
                    Role = admin
                };
                int id = rep.Create(user);
            }
            catch (InvalidOperationException e)
            {
                return Tuple.Create(false, e.Message);
            }
            return Tuple.Create(true, "The user is created successfully!");
        }

        // The first version of the method with if,else.
        
        //public Tuple<bool, string> Verify(string pwd, string username, bool admin = false)
        //{
        //    if (String.IsNullOrEmpty(pwd))
        //    {
        //        return Tuple.Create(false, "Password is empty!");
        //    }
        //    if (pwd.Length <= 7)
        //    {
        //        return Tuple.Create(false, "Password should be at least 7 chatacters!");
        //    }
        //    Regex reg = new Regex(@"[A-Za-z]");
        //    if (!reg.IsMatch(pwd))
        //    {
        //        return Tuple.Create(false, "Password should contains at least 1 alphabetical chatacter!");
        //    }
        //    Regex reg2 = new Regex(@"[0-9]");
        //    if (!reg2.IsMatch(pwd))
        //    {
        //        return Tuple.Create(false, "Password should contains at least 1 digit chatacter!");
        //    }
        //    Regex reg3 = new Regex(@"[_\*\/-]");
        //    if (admin == true & pwd.Length <= 10 & !reg3.IsMatch(pwd))
        //    {
        //        return Tuple.Create(false, "Admin`s password should be at least 10 chatacters and contains at least one special character!");
        //    }
        //    try
        //    {
        //        int id = rep.Create(pwd, username, admin);
        //    }
        //    catch (ArgumentException e)
        //    {
        //        return Tuple.Create(true, e.Message);
        //    }
        //    catch (InvalidDataException e)
        //    {
        //        return Tuple.Create(true, e.Message);
        //    }
        //    return Tuple.Create(true, "The user successfully created!");
        //}
    }
}
