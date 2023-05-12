﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NewNLogWebApi.Models;

namespace NewNLogWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersContext _context;

        public UsersController(UsersContext context)
        {
            _context = context;
        }
        //--------------------------READ--------------------------
        [HttpGet("Read")]
        public async Task<ActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users == null ? NotFound() : Ok(users);
        }

        [HttpGet("GetOnlyNames")]
        public async Task<IActionResult> Get2()
        {
            var users = await _context.Users.ToListAsync();
            if (users == null) { return NotFound(); }

            var names = new List<string>();
            foreach (var user in users)
            {
                names.Add(user.FirstName);
            }

            return Ok(names);
        }
        [HttpGet("AVG_Age")]
        public async Task<ActionResult> Get3()
        {
            var users = await _context.Users.ToListAsync();
            if (users == null) { return NotFound(); }

            int count = 0;
            int sum = 0;
            foreach (var user in users)
            {
                sum += user.Age;
                count++;
            }
            sum /= count;
            return Ok(sum);
        }
        //----------------------END-READ--------------------------
        //-----------------------CREATE---------------------------
        [HttpPost("CreateTable")]
        public async Task<IActionResult> CreateTable()
        {
            try
            {
                string query = "Create table MyTable (id int PRIMARY KEY, name varchar(50))";
                await _context.Database.ExecuteSqlRawAsync(query);
                return Ok("Таблица успешно создана!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при создании таблицы: {ex.Message}");
            }
        }
        [HttpPost("AddUsers")]
        public async Task<ActionResult<string>> AddUserWithData()
        {
            User userModel = CreateUserModel("Erik", "Egnatosyan", "erikegn@mail.com", "+87987546546", "Pushkinskiy", 30);
            try
            {
                _context.Users.Add(userModel);
                await _context.SaveChangesAsync();
                return Ok("User added successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest("Error adding user: {ex.Message}");
            }
        }
        public static User CreateUserModel(string FirstName,  string LastName, string Email, string Phone, string Address, int Age)
        {
            return new User
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Phone = Phone,
                Address = Address,
                Age = Age
            };
        }
    }
}