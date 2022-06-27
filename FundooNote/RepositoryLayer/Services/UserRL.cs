using DatabaseLayer.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL
    {
        FundooContext fundooContext;

        IConfiguration configuration;

        private readonly string _secret;
         

        public UserRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
            this._secret = configuration.GetSection("JwtConfig").GetSection("SecretKey").Value;
        }
        public void AddUser(UserPostModel userPostModel)
        {
            try
            {
                User user = new User();
                user.FirstName = userPostModel.FirstName;
                user.LastName = userPostModel.LastName;
                user.Email = userPostModel.Email;
                user.Password = PwdEncryptDecryptService.EncryptPassword(userPostModel.Password);
                user.CreatedDate = DateTime.Now;
                user.ModifiedDate = DateTime.Now;
                fundooContext.Add(user);
                fundooContext.SaveChanges();

            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public string LogInUser(string Email, string Password)
        {
            try 
            {
                var user = fundooContext.Users.Where(u => u.Email == Email).FirstOrDefault();
                if(user != null)
                {
                    var password = PwdEncryptDecryptService.DecryptPassword(user.Password);


                    if(password == Password)
                    {
                        return GenerateJwtToken(Email, user.UserId);
                    }
                    throw new Exception("Password is Invalid");
                }

                throw new Exception ("Email does not Exist");
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private string GenerateJwtToken(string email, int userId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(this._secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Email,email),
                    new Claim("UserId", userId.ToString())
                }),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
