using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollAPI.Models;

namespace PollAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        public static Poll poll = new Poll();
        private readonly IConfiguration _configuration;
        private readonly PollAppDBContext _context;

        public PollsController(IConfiguration configuration, PollAppDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("createpoll")]
        public async Task<ActionResult<Poll>> CreatePoll (PollCreate pollCreateRequest)
        {
            Poll poll = new Poll();

            poll.PollId = pollCreateRequest.PollId;
            poll.PollTitle = pollCreateRequest.PollTitle;
            poll.PollType = pollCreateRequest.PollType;
            poll.PollAnswer = pollCreateRequest.PollAnswer;

            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

            return Ok(poll);
        }

        //// GET: api/Polls
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Poll>>> GetPolls()
        //{
        //    return await _context.Polls.ToListAsync();
        //}

        //// GET: api/Polls/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Poll>> GetPoll(int id)
        //{
        //    var poll = await _context.Polls.FindAsync(id);

        //    if (poll == null)
        //    {
        //        return NotFound();
        //    }

        //    return poll;
        //}

        //// PUT: api/Polls/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPoll(int id, Poll poll)
        //{
        //    if (id != poll.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(poll).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PollExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Polls
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        ////[HttpPost]
        ////public async Task<ActionResult<Poll>> PostPoll(Poll poll)
        ////{
        ////    _context.Polls.Add(poll);
        ////    await _context.SaveChangesAsync();

        ////    return CreatedAtAction("GetPoll", new { id = poll.Id }, poll);
        ////}

        //// DELETE: api/Polls/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeletePoll(int id)
        //{
        //    var poll = await _context.Polls.FindAsync(id);
        //    if (poll == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Polls.Remove(poll);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool PollExists(int id)
        //{
        //    return _context.Polls.Any(e => e.Id == id);
        //}
    }
}
