    using System.ComponentModel.DataAnnotations;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace LogReg.Models
    {
        public class User
        {
            [Key]
            public int UserId {get;set;}

            [Required]
            [MinLength(2, ErrorMessage="Name must be 2 chaeracters or longer!")]
            public string FirstName {get;set;}

            [Required]
            [MinLength(2, ErrorMessage="Name must be 2 chaeracters or longer!")]
            public string LastName {get;set;}

            [Required]
            [EmailAddress(ErrorMessage="Must be valid email address!")]
            public string Email {get;set;}

            [DataType(DataType.Password)]
            [Required]
            [MinLength(2, ErrorMessage="Password must be 2 characters or longer!")]
            public string Password {get;set;}
            public DateTime CreatedAt {get;set;} = DateTime.Now;
            public DateTime UpdatedAt {get;set;} = DateTime.Now;
            // Will not be mapped to your users table!
            [NotMapped]
            [Compare("Password")]
            [DataType(DataType.Password)]
            public string Confirm {get;set;}
        }            
    }
    
    

