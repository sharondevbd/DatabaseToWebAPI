using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseToWebAPI.Models;

namespace DatabaseToWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly APContext _context;

        public InvoicesController(APContext context)
        {
            _context = context;
        }
        // 01  Last 5 paid customer-------------------------------
        //GET: api/DueInvoices
        [HttpGet("01.Last5")]
        public async Task<ActionResult<IEnumerable<Invoice>>> Last5()
        {

            //var Invoices=await  _context.Invoices.ToListAsync();

            var last5 = _context.Invoices
                .FromSqlRaw("select TOP (5) * from Invoices where InvoiceTotal= PaymentTotal+CreditTotal order by InvoiceID DESC").ToList();

            //var Invoices = await  (from invoice in _context.Invoices where (invoice.InvoiceTotal - invoice.PaymentTotal - invoice.CreditTotal) > 0 select invoice).ToListAsync();

            return last5;
        }

        [HttpGet("02.MoreThan500")]
        public async Task<ActionResult<IEnumerable<Invoice>>> MoreThan500()
        {
            var data = _context.Invoices
                .FromSqlRaw("SELECT * FROM invoices WHERE (InvoiceDate > '2016-01-01' OR InvoiceTotal > 500) AND InvoiceTotal > (PaymentTotal + CreditTotal)").ToList();
            return data;
        }

        [HttpGet("03.ExceptStateVndors")]
        public async Task<ActionResult<IEnumerable<ExceptStateVndors>>> ExceptStateVndors()
        {
			//var joinData = (from i in _context.Invoices
			//                join v in _context.Vendors
			//                on i.VendorId equals v.VendorId
			//                where v.VendorState!in } into c
			//                select new ExceptStateVndors{
			//                };


			//var ExceptStateVndors = _context.Invoices
			//	.FromSqlRaw("SELECT * FROM Invoices join Vendors on InvoiceID = Vendors.VendorID WHERE VendorState NOT IN ('CA', 'NV', 'OR') AND InvoiceDate > '2016-01-01'").ToList();


			//var ExceptStateVndors = _context.Invoices
			//	.FromSqlRaw("SELECT i.InvoiceID, i.InvoiceDate, v.VendorName FROM Invoices AS i INNER JOIN Vendors AS v ON i.VendorID = v.VendorID WHERE v.VendorState NOT IN ('CA', 'NV', 'OR') AND i.InvoiceDate > '2016-01-01';").ToList();

			//return ExceptStateVndors;


			

			var invoices =  _context.Invoices
				.Join(_context.Vendors, i => i.VendorId, v => v.VendorId, (i, v) => new ExceptStateVndors
				{
					InvoiceNumber = i.InvoiceId,
					InvoiceVDate =(DateTime) i.InvoiceDate,
					VendorName = v.VendorName,
					VendorState = v.VendorState
				})
				.Where(vm => !new string[] { "CA", "NV", "OR" }.Contains(vm.VendorState))
				.ToList();

			return invoices;



		}

		[HttpGet("04.SpecificDateInvoices")]
		public async Task<ActionResult<IEnumerable<Invoice>>> SpDateInvo()
		{
			var data = _context.Invoices
				.FromSqlRaw("select * from Invoices where InvoiceDate >= '2015-12-01' AND InvoiceDate <= '2015-12-31'").ToList();
			return data;
		}

		[HttpGet("05.VendorSAN")]
		public async Task<ActionResult<IEnumerable<Vendor>>> VendorSAN()
		{
			var data = _context.Vendors
				.FromSqlRaw("SELECT * FROM Vendors WHERE VendorCity LIKE 'SAN%'").ToList();
			return data;
		}

		[HttpGet("06.VendorContactVowel")]
		public async Task<ActionResult<IEnumerable<Vendor>>> VendorContactVowel()
		{
			var data = _context.Vendors
				.FromSqlRaw("SELECT * FROM vendors WHERE LOWER (VendorContactFName+VendorContactLName) Like '%[aeiou]%'").ToList();
			return data;
		}

		[HttpGet("07.VendorstateFirstLetterNandAtoJ")]
		public async Task<ActionResult<IEnumerable<Vendor>>> VendorStateFilter()
		{
			var data = _context.Vendors
				.FromSqlRaw("SELECT * FROM Vendors WHERE VendorState LIKE 'N[A-J]%'").ToList();
			return data;
		}

		[HttpGet("08.VendorstateFirstLetterNandNOTKtoY")]
		public async Task<ActionResult<IEnumerable<Vendor>>> VendorStateFilterKtoY()
		{
			var data = _context.Vendors
				.FromSqlRaw("SELECT * FROM Vendors WHERE VendorState LIKE 'N%[A-J]' OR VendorState LIKE 'N%[Z]'").ToList();

			//SELECT* FROM Vendors WHERE VendorState LIKE 'N%' AND SUBSTRING(VendorState, 2, 1) NOT BETWEEN 'K' AND 'Y'

			return data;
		}

		[HttpGet("09.OffsetFetchVendor")]
		public async Task<ActionResult<IEnumerable<Vendor>>> OffsetFetchVendor()
		{
			var data = _context.Vendors
				.FromSqlRaw("  Select * From Vendors order by VendorID OFFSET 10 ROWS Fetch Next 10 rows only").ToList();
			return data;
		}

		//GET: api/DueInvoices
		[HttpGet("DueInvoices")]
        public async Task<ActionResult<IEnumerable<Invoice>>> DueInvoices()
        {
           
			//var Invoices=await  _context.Invoices.ToListAsync();
            var Invoices = await (from invoice in _context.Invoices where (invoice.InvoiceTotal - invoice.PaymentTotal - invoice.CreditTotal) > 0 select invoice).ToListAsync();

            return Invoices;
        }

		//GET: api/DueInvoices/1
		[HttpGet("DueInvoices/{id}")]
		public async Task<ActionResult<IEnumerable<Invoice>>> DueInvoices(int id)
		{

			//var Invoices=await  _context.Invoices.ToListAsync();
		
			var Invoices = await (from invoice in _context.Invoices
                                  where (invoice.InvoiceTotal - invoice.PaymentTotal - invoice.CreditTotal) > 0  && invoice.VendorId== id
                                  select invoice).ToListAsync();
			return Invoices;
		}


		// GET: api/Invoices
		[HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
          if (_context.Invoices == null)
          {
              return NotFound();
          }
            return await _context.Invoices.ToListAsync();
        }

        // GET: api/Invoices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
          if (_context.Invoices == null)
          {
              return NotFound();
          }
            var invoice = await _context.Invoices.FindAsync(id);

            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }

        // PUT: api/Invoices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(int id, Invoice invoice)
        {
            if (id != invoice.InvoiceId)
            {
                return BadRequest();
            }

            _context.Entry(invoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
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

        // POST: api/Invoices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
          if (_context.Invoices == null)
          {
              return Problem("Entity set 'APContext.Invoices'  is null.");
          }
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvoice", new { id = invoice.InvoiceId }, invoice);
        }

        // DELETE: api/Invoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            if (_context.Invoices == null)
            {
                return NotFound();
            }
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InvoiceExists(int id)
        {
            return (_context.Invoices?.Any(e => e.InvoiceId == id)).GetValueOrDefault();
        }
    }
}
