using DatabaseLayer.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface IUserBL
    {
        public void AddUser(UserPostModel userPostModel);
        public string LogInUser(string Email, string Password);
    }
}
