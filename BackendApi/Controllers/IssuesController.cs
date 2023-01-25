using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEndApi.Models;

namespace BackEndApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class IssuesController : ControllerBase
  {
    private readonly ApplicationContext _db;

    public IssuesController(ApplicationContext db)
    {
      _db = db;
    }

    // GET api/issues
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Issue>>> Get(string searchTerm)
    {
      IQueryable<Issue> query = _db.Issues.AsQueryable().Include(e => e.Solutions);
      if (searchTerm != null)
      {
        query = query.Where(entry => (entry.Name.Contains(searchTerm) || entry.Description.Contains(searchTerm)));
      }
      return await query.ToListAsync();
    }

    // GET: api/Issues/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Issue>> GetIssue(int id)
    {
      // Issue issue = await _db.Issues.Include(e=>e.Solutions).FindAsync(id);
      Issue issue = await _db.Issues.Include(issue => issue.Solutions).FirstOrDefaultAsync(issue => issue.IssueId == id);
      if (issue == null)
      {
        return NotFound();
      }

      return issue;
    }

    // POST api/issues
    [HttpPost]
    public async Task<ActionResult<Issue>> Post(Issue issue)
    {
      _db.Issues.Add(issue);
      await _db.SaveChangesAsync();
      return CreatedAtAction(nameof(GetIssue), new { id = issue.IssueId }, issue);
    }

    // PUT: api/Issues/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Issue issue)
    {
      if (id != issue.IssueId)
      {
        return BadRequest();
      }

      _db.Issues.Update(issue);

      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!IssueExists(id))
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
    private bool IssueExists(int id)
    {
      return _db.Issues.Any(e => e.IssueId == id);
    }

    // DELETE: api/Issues/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIssue(int id)
    {
      Issue issue = await _db.Issues.FindAsync(id);
      if (issue == null)
      {
        return NotFound();
      }

      _db.Issues.Remove(issue);
      await _db.SaveChangesAsync();

      return NoContent();
    }
  }
}