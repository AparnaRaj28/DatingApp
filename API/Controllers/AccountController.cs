using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context,ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;       
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
             //checks if user exists
             if(await UserExists(registerDto.Username)) return BadRequest("Username already exists");

            using var hmac = new HMACSHA512(); //creating a new instance of hmac class

            //creating a new user to store in the database
            var user = new AppUser{
                UserName = registerDto.Username.ToLower(),  //username is saved into the database in lower case
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);  //Adds a user by not saving into the databse yet
            await _context.SaveChangesAsync(); //save the changes in the database

            // returns the user object
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        //This method checks whether a user exists or not
        private async Task<bool> UserExists(string username)
        {
             return await _context.Users.AnyAsync(x=>x.UserName == username.ToLower()); //returns true or false
        }

        // Login endpoint
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
             var user = await _context.Users.SingleOrDefaultAsync(x=> x.UserName == loginDto.Username);

             if (user==null) return Unauthorized("Invalid username");

             using var hmac = new HMACSHA512(user.PasswordSalt); //we pass the hmac key to generate the password hash

             var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

             //comparing the user password hash and the computed hash by comparing each elements of the byte array
             for(int i=0; i< computedHash.Length ; i++){
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
             }

             return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
             };
        }

        
    }
}