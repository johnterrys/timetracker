using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TimeCats.Session;
using TimeCats.Models;
using TimeCats.Services;
using TimeCats.DTOs;

namespace TimeCats.Controllers
{
    public class UserController : HomeController
    {
        public UserController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {}

        /// <summary>
        ///     Returns a hashed version of the passed password
        /// </summary>
        /// <returns></returns>
        public static string GenerateHash(string password)
        {
            var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        
        [HttpPost]
        public IActionResult ChangePassword([FromBody] object json)
        {
            var JsonString = json.ToString();
            var user = JsonConvert.DeserializeObject<User>(JsonString);

            var crypto = new CryptographyService();
            var salt = crypto.GenerateSalt();

            if (IsAdmin())
            {
                user.password = crypto.CalculateHash(salt, "password");
                user.newPassword = crypto.CalculateHash(salt, user.newPassword);
                user.Salt = salt;

                if (_userService.ChangePasswordA(user)) return Ok();
                return StatusCode(500); //Query failed
            }

            if (user.userID == GetUserID())
            {
                user.password = crypto.CalculateHash(salt, user.password);
                user.newPassword = crypto.CalculateHash(salt, user.newPassword);
                user.Salt = salt;

                if (_userService.ChangePassword(user)) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized(); //Not an Admin or the current user, Unathorized (401)
        }

        /// <summary>
        ///     Updates the passed user in the database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ChangeUser([FromBody] object json)
        {
            var JsonString = json.ToString();
            var user = JsonConvert.DeserializeObject<User>(JsonString);
            if (user.username == null || user.username.Length < 1)
                return StatusCode(400); //Didn't pass a valid username, Bad Request (400)
            user.username = user.username.ToLower();
            var checkUser = _userService.GetUser(user.username);
            if (checkUser != null && checkUser.userID != user.userID)
                return StatusCode(403); //Username already exists, Forbidden (403)

            if (IsAdmin())
            {
                if (_userService.ChangeUser(user)) return Ok();
                return StatusCode(500); //Query failed
            }

            if (user.userID == GetUserID())
            {
                if (_userService.ChangeUser(user)) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized(); //Not an Admin or the current user, Unauthorized (401)
        }

        /// <summary>
        ///     Returns OK if admmin or ID's match
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetUser([FromBody] object json)
        {
            var JsonString = json.ToString();
            var sentUser = JsonConvert.DeserializeObject<User>(JsonString);
            var currentUser = HttpContext.Session.GetObjectFromJson<User>("user");
            if (currentUser.type == 'A' || currentUser.userID == sentUser.userID)
            {
                var dbUser = _userService.GetUserByID(sentUser.userID);
                return Ok(dbUser);
            }

            return NoContent();
        }

        /// <summary>
        ///     Returns dashboard for a given userID
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetDashboard()
        {
            var dashboard = _timeTrackerService.GetDashboardsForUser(GetUserID());
            return Ok(dashboard);
        }

        /// <summary>
        ///     Returns all users in the system after verifying access.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUsers()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            //checks if user is admin
            if (IsAdmin())
            {
                var users = _timeTrackerService.GetUsers();
                return Ok(users);
            }

            return NoContent();
        }

        /// <summary>
        ///     Allows a user to log in. Returns an OK (200) if successful, No Content (204) if the
        ///     username doesn't exist, and Unauthorized (401) if the password is incorrect
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LoginUser([FromBody] object json)
        {
            var JsonString = json.ToString();
            //Username and Password must be here, everything else can be empty
            var loginUser = JsonConvert.DeserializeObject<User>(JsonString);

            //Check database for User and create a session
            var user = _timeTrackerService.GetUserByUsername(loginUser.username);
            var crypto = new CryptographyService();

            if (crypto.Verify(user.password, user.Salt, loginUser.password))
            {
                if (!user.isActive) return StatusCode(403); //return Forbidden (403) if the user's account isn't active

                // We found a user! Send them to the Dashboard and save their Session
                HttpContext.Session.SetObjectAsJson("user", user);
                return Ok();
            }

            //return Unauthorized (401) if the username or password is wrong
            return Unauthorized();
        }

        /// <summary>
        ///     Returns OK if a users session succesfully ended. 204 otherwise
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.DestroySession<User>("user");

            var user = HttpContext.Session.GetObjectFromJson<User>("user");
            if (user == null)
                return Ok();
            return null;
        }

        /// <summary>
        ///     Registers a User, returns a 200 status code if successful
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RegisterUser([FromBody] object json)
        {
            var JsonString = json.ToString();
            var user = JsonConvert.DeserializeObject<User>(JsonString);

            _timeTrackerService.AddUser(user);

            //Store Session information for this user using Username
            HttpContext.Session.SetObjectAsJson("user", user);

            return Ok();
        }
    }
}
