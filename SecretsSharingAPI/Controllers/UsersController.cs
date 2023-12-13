using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecretsSharingAPI.Database;
using SecretsSharingAPI.Models;

namespace SecretsSharingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private DataContext db = new DataContext();

        // GET
        /// <summary>
        /// User authorization
        /// </summary>
        /// <remarks>
        /// Finds user in database and returns id
        /// </remarks>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <response code="200">Authorization success</response>
        /// <response code="404">User not found in database</response>
        [HttpGet("GetUser/login={login}&password={password}")]
        [Produces(typeof(int))]
        public IActionResult GetUser(string login, string password)
        {
            var user = db.User.Where(u => u.Login == login && u.Password == password).SingleOrDefault();

            if (user == null)
                return NotFound("User not found");

            return Ok(user.ID);
        }



        // POST
        /// <summary>
        /// User registration
        /// </summary>
        /// <remarks>
        /// Creates new user and returns id
        /// </remarks>
        /// <param name="user">User credentials</param>
        /// <returns></returns>
        /// <response code="200">Registration success</response>
        /// <response code="400">Registration error</response>
        [HttpPost("PostUser")]
        [Produces(typeof(int))]
        public IActionResult PostUser(RequiredUser user)
        {
            // Validate data
            if (string.IsNullOrWhiteSpace(user.Login))
                ModelState.AddModelError("EmptyLogin", "Login is required field");
            if (db.User.Where(u => u.Login == user.Login).Count() > 0)
                ModelState.AddModelError("DuplicateLogin", "Login already exists");
            if (string.IsNullOrWhiteSpace(user.Password))
                ModelState.AddModelError("Password", "Password is required field");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            // Create new user
            User newUser = new User(user);
            db.User.Add(newUser);
            db.SaveChanges();

            return Ok(newUser.ID);
        }
    }
}
