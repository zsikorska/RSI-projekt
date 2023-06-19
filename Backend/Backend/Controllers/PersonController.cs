using Backend.Models;
using BlogManager.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Backend.Controllers
{
    [ApiController]
    [Route("/persons")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly AppDbContext _context;

        public PersonController(AppDbContext context, ILogger<PersonController> logger) {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> GetAllPersons()
        {
            return await _context.Persons.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPersonById(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return StatusCode(404);
            }
            return person;
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePerson([Bind("ID, Name, Email, Height, Birthdate")] Person person)
        {
            var p = await _context.Persons.FindAsync(person.ID);
            if (p == null)
            {
                return NotFound();
            }
            if (_context.Persons.Where(p => p.Email == person.Email).FirstOrDefault() != null)
            {
                return Conflict();
            }
            if (!Regex.IsMatch(person.Name, @"^[a-zA-Z]+$"))
            {
                return BadRequest("Invalid name");
            }
            if (!Regex.IsMatch(person.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                return BadRequest("Invalid email");
            }
            if (50 > person.Height || 230 < person.Height)
            {
                return BadRequest("Invalid height");
            }
            if (person.Birthdate.Year < 1900 || person.Birthdate > DateOnly.FromDateTime(DateTime.Now))
            {
                return BadRequest("Invalid birthdate");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(p).CurrentValues.SetValues(person);
                    p = person;
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                    return StatusCode(500);
                }
            }
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Person>> AddPerson([Bind("Name, Email, Height, Birthdate")] Person person)
        {
            if(!Regex.IsMatch(person.Name, @"^[a-zA-Z]+$")){
                return BadRequest("Invalid name");
            }
            if (!Regex.IsMatch(person.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                return BadRequest("Invalid email");
            }
            if (50 > person.Height || 230 < person.Height)
            {
                return BadRequest("Invalid height");
            }
            if(person.Birthdate.Year < 1900 || person.Birthdate > DateOnly.FromDateTime(DateTime.Now))
            {
                return BadRequest("Invalid birthdate");
            }
            if(_context.Persons.Where(p => p.Email == person.Email).FirstOrDefault() != null)
            {
                return Conflict();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(person);
                    await _context.SaveChangesAsync();

                    return person;
                }
            }
            catch (DbUpdateException /* ex */)
            {

                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return StatusCode(500);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePerson(int id)
        {
            var p = await _context.Persons.FindAsync(id);
            if (p == null)
            {
                return NotFound();
            }
            _context.Persons.Remove(p);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("size")]
        public async Task<ActionResult<int>> GetPersonsCount()
        {
            return _context.Persons.Count();
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<List<Person>>> GetByNameJson(string name)
        {
            var persons = _context.Persons.ToArray();
            List<Person> list = new List<Person>();
            foreach (Person person in persons)
            {
                if (person.Name.ToLower().Contains(name.ToLower()))
                {
                    list.Add(person);
                }
            }
            return list;
        }

        [HttpGet("authors")]
        public async Task<ActionResult<string>> GetAuthors()
        {
            return "Piotr Łazik 260371, Zuzanna Sikorska 260464";
        }
    }
}