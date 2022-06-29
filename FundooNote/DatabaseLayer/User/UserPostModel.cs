using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabaseLayer.User
{
    public class UserPostModel
    {
        [Required]

        [RegularExpression("^[A-Z]{1}[A-Za-z]{6,}$", ErrorMessage = "First character in Upper Case and minimum 6 characters in FirstName")]
        public string FirstName { get; set; }
        [Required]

        [RegularExpression("^[A-Z]{1}[A-Za-z]{6,}$", ErrorMessage = "First character in Upper Case and minimum 6 characters In LastName")]
        public string LastName { get; set; }
        [Required]

        [RegularExpression("^[0-9a-zA-Z]+([+#%&_.-][a-zA-Z0-9]+)*[@][0-9]?[a-zA-Z]{2,}[.][a-zA-Z]{2,3}([.][a-zA-Z]{2,3})?$", ErrorMessage = "Enter a Valid Email-Id")]
        public string Email { get; set; }

        [Required]

        [RegularExpression("^(?=.*[A-Z]).{8,}$", ErrorMessage = "Password Have minimum 8 Characters, Should have at least 1 Upper Case and Should have at least 1 numeric number and Has exactly 1 Special Character")]
        public string Password { get; set; }


    }
}
