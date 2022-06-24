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
                this.userBL.AddUser(userPostModel);
                var user = fundooContext.Users.FirstOrDefault(u => u.Email == userPostModel.Email);
                if (user != null)
                {
                    return this.BadRequest(new { success = false, message = " Email Already Exist" });
                }

                return this.Ok(new { success = true, message = "Registration Successful" });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
