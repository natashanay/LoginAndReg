using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LogReg.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;



namespace LogReg.Controllers
{
    public class HomeController : Controller
    {

        private LRContext dbContext;
        // here we can "inject" our context service into the constructor
        public HomeController(LRContext context)
        {
            dbContext = context;
        }
 
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            Wrapper w = new Wrapper();
            return View(w);
        }
        [HttpPost]
        [Route("")]
        public IActionResult Register(Wrapper modeluser)
        {

            User submittedUser = modeluser.NewUser;
            // Check initial ModelState
            if(ModelState.IsValid)
            {
                // If a User exists with provided email
                if(dbContext.Users.Any(u => u.Email == submittedUser.Email))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("NewUser.Email", "Email already in use!");
                    Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&& IN THE EMAIL CHECK");
                    // You may consider returning to the View at this point
                    return View("Index");
                }
                

                // Initializing a PasswordHasher object, providing our User class as its
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                submittedUser.Password = Hasher.HashPassword(submittedUser, submittedUser.Password);
                // Console.WriteLine("SubmittedUSerPASWORD******************" + submittedUser.Password);
                //Save your user object to the database
                dbContext.Add(submittedUser);
                dbContext.SaveChanges();
                //Set session variable and it logs them into the success page 
                HttpContext.Session.SetInt32("regUserId", submittedUser.UserId);

                return RedirectToAction("Success");

                
            }
            
            // Console.WriteLine("****************************You are inside the else which means Model State is Valid failed");
            return View("Index");

            

        }

        [HttpGet]
        [Route("displaylogin")]
        public IActionResult DisplayLogin()
        {
            return View("Login");
        }
        

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(Wrapper userSubmission)
        {
            LoginUser LogSubmission = userSubmission.NewLogUser;

            if(ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == LogSubmission.Email);
                // If no user exists with provided email
                if(userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("NewLogUser.Email", "Invalid Email/Password");
                    return View("Login");
                }
                
                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();
                
                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(LogSubmission, userInDb.Password, LogSubmission.Password);
                
                // result can be compared to 0 for failure
                if(result == 0)
                {
                    // handle failure (this should be similar to how "existing email" is handled)
                    ModelState.AddModelError("NewLogUser.Password Fail", "Password already in use!");

                }
                var logIdUserInDb = dbContext.Users.FirstOrDefault(l=> l.Email == LogSubmission.Email); 
                int logId = logIdUserInDb.UserId;
                HttpContext.Session.SetInt32("regUserId", logId);

                return RedirectToAction("Success");
            }
            return View("Login");
    
        }

        [HttpGet]
        [Route("success")]
        public IActionResult Success()
        {
            //test to see if user is logged in and is a session variable
            int? IntVariable = HttpContext.Session.GetInt32("regUserId");
            if(IntVariable != null)
            {
                return View("Success");
            }
            //if user is not logged in send them back to Register Page
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            //code that clears the session
            HttpContext.Session.Clear();
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
