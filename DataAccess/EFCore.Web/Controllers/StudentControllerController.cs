﻿using EFCore.Web.Models;
using EFCore.Web.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentControllerController : ControllerBase
    {
        private readonly SchoolContext _context;

        public StudentControllerController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetTopFive")]
        public async Task<List<Student>> Get()
        {
            return await _context.Students.Take(5).ToListAsync();
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<Student> GetById(int id)
        {
            return await _context.Students
                .SingleAsync(b => b.Id == id);
        }
    }
}