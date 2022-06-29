using BusinessLayer.Interfaces;
using DatabaseLayer.User;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost("LogIn/{email}/{password}")]
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

        [HttpPost("Forgotpassword /{email}")]
        public IActionResult ForgotPassword(string email)
        {
            try
            {

                var user = fundooContext.Users.FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    return this.BadRequest(new { success = false, message = " Email does not exist" });
                }

                                
                bool token = this.userBL.ForgetPassword(email);
                return this.Ok(new { success = true, message = "Forgot Password Email Sent" });

            }

            catch (Exception e)
            { 
                throw e;
            }
        }

        [Authorize]
        [HttpPut("Resetpassword")]
        public IActionResult Resetpassword(UserPasswordModel userPasswordModel)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                var result = fundooContext.Users.Where(u => u.UserId == UserID).FirstOrDefault();
                string Email = result.Email.ToString();
                if (userPasswordModel.Password != userPasswordModel.ConfirmPassword)
                {
                    return BadRequest(new { success = false, message = "Password and Confirm password must be same" });
                }
                bool res = this.userBL.ResetPassword(Email, userPasswordModel);
                if (res == false)
                {
                    return this.BadRequest(new { sucess = false, message = "Enter the valid Email" });
                }
                return this.Ok(new { succes = true, message = "Password change successfully" });
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }

    
}
