using CathayBkWebAPI.DataAccess;
using CathayBkWebAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CathayBkWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CodeController : ControllerBase
    {
        private readonly MyDbcontext _myDbContext;

        public CodeController(MyDbcontext myDbcentext)
        {
            _myDbContext = myDbcentext;
        }

        [HttpGet("currencies", Name = "GetCurrencies")]
        public ActionResult<IEnumerable<Currency>> GetCurrencies()
        {
            //return _myDbcentext.Currencies.ToList();
            var currencies = _myDbContext.Currencies.OrderBy(c => c.Code).ToList();
            return currencies;
        }

        [HttpPost("add-currency", Name = "AddCurrency")]
        public async Task<IActionResult> AddCurrency([FromBody] Currency currency)
        {
            _myDbContext.Currencies.Add(currency);
            await _myDbContext.SaveChangesAsync();
            return Ok(currency);
        }

        [HttpPut("update-currency/{id}", Name = "UpdateCurrency")]
        public async Task<IActionResult> UpdateCurrency(int id, [FromBody] Currency currency)
        {
            if (id != currency.Id)
            {
                return BadRequest("Currency ID mismatch");
            }

            _myDbContext.Entry(currency).State = EntityState.Modified;

            try
            {
                await _myDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurrencyExists(id))
                {
                    return NotFound("Currency not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("delete-currency/{id}", Name = "DeleteCurrency")]
        public async Task<IActionResult> DeleteCurrency(int id)
        {
            var currency = await _myDbContext.Currencies.FindAsync(id);
            if (currency == null)
            {
                return NotFound("Currency not found");
            }

            _myDbContext.Currencies.Remove(currency);
            await _myDbContext.SaveChangesAsync();

            return Ok(currency);
        }

        private bool CurrencyExists(int id)
        {
            return _myDbContext.Currencies.Any(c => c.Id == id);
        }
    }
}
