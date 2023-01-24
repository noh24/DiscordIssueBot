using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEndApi.Models;

namespace BackEndApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SolutionsController : ControllerBase
  {
    private readonly ApplicationContext _db;

    public SolutionsController(ApplicationContext db)
    {
      _db = db;
    }

    // GET api/solutions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Solution>>> Get()
    {
      IQueryable<Solution> query = _db.Solutions.AsQueryable().Include(e => e.Issue);
      return await query.ToListAsync();
    }

    // GET: api/Solutions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Solution>> GetSolution(int id)
    {
      // Solution solution = await _db.Solutions.Include(e => e.Issue).FindAsync(id);
      Solution solution = await _db.Solutions.Include(e => e.Issue).FirstOrDefaultAsync(e => e.SolutionId == id);

      if (solution == null)
      {
        return NotFound();
      }

      return solution;
    }

    // POST api/solutions
    [HttpPost]
    public async Task<ActionResult<Solution>> Post(Solution solution)
    {
      _db.Solutions.Add(solution);
      await _db.SaveChangesAsync();
      return CreatedAtAction(nameof(GetSolution), new { id = solution.SolutionId }, solution);
    }

    // PUT: api/Solutions/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Solution solution)
    {
      if (id != solution.SolutionId)
      {
        return BadRequest();
      }

      _db.Solutions.Update(solution);

      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!SolutionExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }
    private bool SolutionExists(int id)
    {
      return _db.Solutions.Any(e => e.SolutionId == id);
    }

    // DELETE: api/Solutions/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSolution(int id)
    {
      Solution solution = await _db.Solutions.FindAsync(id);
      if (solution == null)
      {
        return NotFound();
      }

      _db.Solutions.Remove(solution);
      await _db.SaveChangesAsync();

      return NoContent();
    }
  }
}