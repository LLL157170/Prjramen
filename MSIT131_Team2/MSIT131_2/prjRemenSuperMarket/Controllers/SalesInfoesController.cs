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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SalesInfoesController : ControllerBase
    {
        private readonly RamenSupermarketContext _context;

        public SalesInfoesController(RamenSupermarketContext context)
        {
            _context = context;
        }

        // GET: api/SalesInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<int?>>> GetspotSalesInfos()
        {            

            return await _context.SalesInfos.Where(r=>r.SalesStatesIdFk != 3).Select(r=>r.ProductIdFk).ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<int?>>> GetspSalesInfos()
        {

            return await _context.SalesInfos.Where(r => r.SalesStatesIdFk != 2).Select(r => r.ProductIdFk).ToListAsync();
        }

        // GET: api/SalesInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesInfo>> GetSalesInfo(int id)
        {
            var salesInfo = await _context.SalesInfos.FindAsync(id);

            if (salesInfo == null)
            {
                return NotFound();
            }

            return salesInfo;
        }

        // PUT: api/SalesInfoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesInfo(int id, SalesInfo salesInfo)
        {
            if (id != salesInfo.SalesInfoIdPk)
            {
                return BadRequest();
            }

            _context.Entry(salesInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesInfoExists(id))
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

        // POST: api/SalesInfoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SalesInfo>> PostSalesInfo(SalesInfo salesInfo)
        {
            _context.SalesInfos.Add(salesInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalesInfo", new { id = salesInfo.SalesInfoIdPk }, salesInfo);
        }

        // DELETE: api/SalesInfoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesInfo(int id)
        {
            var salesInfo = await _context.SalesInfos.FindAsync(id);
            if (salesInfo == null)
            {
                return NotFound();
            }

            _context.SalesInfos.Remove(salesInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalesInfoExists(int id)
        {
            return _context.SalesInfos.Any(e => e.SalesInfoIdPk == id);
        }
    }
}
