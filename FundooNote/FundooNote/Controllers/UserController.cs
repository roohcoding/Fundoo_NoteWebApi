using BusinessLayer.Interfaces;
using DatabaseLayer.User;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Services;
using System;
using System.Linq;

namespace FundooNote.Controllers
{
    /*[Route("api/[controller]")]
    [ApiController] */
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        IUserBL userBL;
        FundooContext fundooContext;
        public UserController(IUserBL userBL, FundooContext fundooContext)
        {
            this.userBL = userBL;
            this.fundooContext = fundooContext;
        }
        [HttpPost("Register")]
        public IActionResult AddUser(UserPostModel userPostModel)
        {
            try
            {
                
                var user = fundooContext.Users.FirstOrDefault(u => u.Email == userPostModel.Email);
                if (user != null)
                {
                    return this.BadRequest(new { success = false, message = " Email Already Exist" });
                }
                this.userBL.AddUser(userPostModel);
                return this.Ok(new { success = true, message = "Registration Successful" });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [HttpPost("Login")]
        public IActionResult LogIn(string email, string password)
        {
            try
            {

                var user = fundooContext.Users.FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    return this.BadRequest(new { success = false, message = " Email does not Exist" });
                }

                string Password = PwdEncryptDecryptService.DecryptPassword(user.Password);
                var userdata1 = fundooContext.Users.FirstOrDefault(u => u.Email == email && Password == password);

                if (userdata1 == null)
                {
                    return this.BadRequest(new { success = false, message = " Password Invalid" });
                }

                string token = this.userBL.LogInUser(email , password );
                return this.Ok(new { success = true, message = "Login Successful", data = token});

            }

            catch (Exception e)
            {
                throw e;
            }
        }
    }
   
}
