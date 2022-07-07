using DatabaseLayer.User;
using Experimental.System.Messaging;
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
                    Expires = DateTime.UtcNow.AddHours(3),
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

        public bool ForgetPassword(string Email)
        {

            try
            {
                var user = fundooContext.Users.Where(u => u.Email == Email).FirstOrDefault();

                if (user == null)
                {
                    return false;
                }
                else
                {

                    MessageQueue queue;
                    //ADD MESSAGE TO QUEUE
                    if (MessageQueue.Exists(@".\Private$\FundooQueue"))
                    {
                        queue = new MessageQueue(@".\Private$\FundooQueue");
                    }
                    else
                    {
                        queue = MessageQueue.Create(@".\Private$\FundooQueue");
                    }

                    Message MyMessage = new Message();
                    MyMessage.Formatter = new BinaryMessageFormatter();
                    MyMessage.Body = GenerateJwtToken(Email, user.UserId);
                    MyMessage.Label = "Forget Password Email";
                    queue.Send(MyMessage);


                    Message msg = queue.Receive();
                    msg.Formatter = new BinaryMessageFormatter();
                    EmailService.SendEmail(Email, msg.Body.ToString());
                    queue.ReceiveCompleted += new ReceiveCompletedEventHandler(msmqQueue_ReceiveCompleted);

                    queue.BeginReceive();
                    queue.Close();


                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void msmqQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                MessageQueue queue = (MessageQueue)sender;
                Message msg = queue.EndReceive(e.AsyncResult);
                EmailService.SendEmail(e.Message.ToString(), GenerateToken(e.Message.ToString()));
                queue.BeginReceive();

            }
            catch (MessageQueueException ex)
            {
                if (ex.MessageQueueErrorCode ==
                    MessageQueueErrorCode.AccessDenied)
                {
                    Console.WriteLine("Access is denied. " +
                        "Queue might be a system queue.");
                }
            }
        }

        private string GenerateToken(string Email)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(this._secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Email,Email)

                    }),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool ResetPassword(string email, UserPasswordModel userPasswordModel)
        {
            try
            {
                var user = fundooContext.Users.Where(u => u.Email == email).FirstOrDefault();

                if (user == null)
                {
                    return false;
                }
                if (userPasswordModel.Password == userPasswordModel.ConfirmPassword)
                {
                    user.Password = PwdEncryptDecryptService.EncryptPassword(userPasswordModel.Password);
                    fundooContext.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
    
}
