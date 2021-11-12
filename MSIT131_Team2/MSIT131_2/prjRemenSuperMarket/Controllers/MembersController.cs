using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjRemenSuperMarket.Models;
using prjRemenSuperMarket.ViewModel;

namespace prjRemenSuperMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : Controller
    {
        private readonly RamenSupermarketContext _context;

        public MembersController(RamenSupermarketContext context)
        {
            _context = context;
        }

        // GET: api/Members
        [HttpGet]
        public IActionResult GetMembers()
        {
            var q = _context.Members;
            IEnumerable<Member> ra = _context.Members;
            List<CMemberViewModel> list = new List<CMemberViewModel>();
            foreach (Member item in ra)
            {
                CMemberViewModel cm = new CMemberViewModel();
                cm.member = item;
                list.Add(cm);
            }
            return Json(list);
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public IActionResult GetEmployee(string id)
        {
            var q = _context.Members.Where(p => p.Name.Contains(id) || p.Address.Contains(id));
            List<CMemberViewModel> list = new List<CMemberViewModel>();
            if (q == null)
            {
                return NotFound();
            }
            foreach(Member item in q)
            {
                CMemberViewModel cm = new CMemberViewModel();
                cm.member = item;
                list.Add(cm);
            }
            return Json(list);
        }

        // PUT: api/Members/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int id, Member member)
        {
            if (id != member.MemberIdPk)
            {
                return BadRequest();
            }

            _context.Entry(member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
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

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member member)
        {
            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMember", new { id = member.MemberIdPk }, member);
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            var x = from a in _context.RamenStores
                    where a.MemberId == id
                    select a;



            foreach(var f in x)
            {
                var h = from n in _context.RamenProductInfos
                        where n.RamenStoreId == f.RamenStoreId
                        select n;
                foreach(var a in h)
                {
                    _context.RamenProductInfos.Remove(a);
                }
                _context.RamenStores.Remove(f);
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberIdPk == id);
        }
    }
}
