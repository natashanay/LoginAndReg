using System.ComponentModel.DataAnnotations;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace LogReg.Models
    {

        public class LoginUser
        {
            // No other fields!
            [Required]
            public string Email {get; set;}
            [DataType(DataType.Password)]
            [Required]
            [MinLength(2, ErrorMessage="Password must be 2 characters or longer!")]
            public string Password { get; set; }
        }


    }


