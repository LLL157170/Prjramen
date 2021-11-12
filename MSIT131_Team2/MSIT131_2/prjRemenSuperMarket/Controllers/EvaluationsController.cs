using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjRemenSuperMarket.Models;

namespace prjRemenSuperMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationsController : ControllerBase
    {
        private readonly RamenSupermarketContext _context;

        public EvaluationsController(RamenSupermarketContext context)
        {
            _context = context;
        }

        // GET: api/Evaluations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evaluation>>> GetEvaluations()
        {
            return await _context.Evaluations.ToListAsync();
        }

        // GET: api/Evaluations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evaluation>> GetEvaluation(int id)
        {
            var evaluation = await _context.Evaluations.FindAsync(id);

            if (evaluation == null)
            {
                return NotFound();
            }

            return evaluation;
        }

        // PUT: api/Evaluations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvaluation(int id, Evaluation evaluation)
        {
            if (id != evaluation.EvaluationIdPk)
            {
                return BadRequest();
            }

            _context.Entry(evaluation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluationExists(id))
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

        // POST: api/Evaluations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Evaluation>> PostEvaluation(Evaluation evaluation)
        {
            _context.Evaluations.Add(evaluation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvaluation", new { id = evaluation.EvaluationIdPk }, evaluation);
        }

        // DELETE: api/Evaluations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluation(int id)
        {
            var evaluation = await _context.Evaluations.FindAsync(id);
            if (evaluation == null)
            {
                return NotFound();
            }

            _context.Evaluations.Remove(evaluation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EvaluationExists(int id)
        {
            return _context.Evaluations.Any(e => e.EvaluationIdPk == id);
        }
    }
}
