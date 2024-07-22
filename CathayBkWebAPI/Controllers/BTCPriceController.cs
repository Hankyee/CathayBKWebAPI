using CathayBkWebAPI.DataAccess;
using CathayBkWebAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CathayBkWebAPI.Controllers
{

    [ApiController]
    [Route("api/btcprices")]
    public class BTCPriceController : ControllerBase
    {
        private readonly MyDbcentext _myDbcentext;

        public BTCPriceController(MyDbcentext centext)
        {
            _myDbcentext = centext;
        }

        // GET: api/btcprices
        [HttpGet]
        public ActionResult<IEnumerable<BTCPrice>> GetAllBTCPrices()
        {
            return _myDbcentext.BTCPrices.ToList();
        }


        // POST: api/btcprices
        //[HttpPost]
        //public ActionResult<BTCPrice> AddBTCPrice(BTCPrice btcPrice)
        //{
        //    var rate = new BTCPrice
        //    {
        //        //UpdateTime =??,
        //        USDChineseName = "",
        //        USDRate = 0,
        //        GBPChineseName ="",
        //        GBPRate = 0,
        //        EURChineseName ="",
        //        EURRate = 0
        //    };

        //    _myDbcentext.BTCPrices.Add(btcPrice);
        //    _myDbcentext.SaveChanges();

        //    return Ok(btcPrice);
        //}

        // PUT: api/btcprices/5
        //[HttpPut("{id}")]
        //public IActionResult UpdateBTCPrice(int id, BTCPrice btcPrice)
        //{

        //    if (id != btcPrice.Id)
        //    {
        //        return BadRequest();
        //    }


        //    _myDbcentext.Entry(btcPrice).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //    _myDbcentext.SaveChanges();

        //    return NoContent();
        //}


        // DELETE: api/btcprices/5
        [HttpDelete("{id}")]
        public IActionResult DeleteBTCPrice(int id)
        {
            var btcPrice = _myDbcentext.BTCPrices.Find(id);
            if (btcPrice == null)
            {
                return NotFound();
            }

            _myDbcentext.BTCPrices.Remove(btcPrice);
            _myDbcentext.SaveChanges();

            return NoContent();
        }

    }
}

