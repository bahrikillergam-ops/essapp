using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EssTeamApi.Data;
using EssTeamApi.Models;

namespace EssTeamApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ManagersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/managers/search?name=Ahmed
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Manager>>> SearchManagers(string name)
        {
            var managers = await _context.Managers
                .Where(m =>
                    m.FirstName.ToLower().Contains(name.ToLower()) ||
                    m.LastName.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            if (!managers.Any())
                return NotFound("No managers found");

            return managers;
        }
    }
}
