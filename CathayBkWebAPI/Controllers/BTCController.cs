using CathayBkWebAPI.DataAccess;
using CathayBkWebAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.ComponentModel;


namespace CathayBkWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BTCController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly MyDbcontext _myDbContext;

        public BTCController(IHttpClientFactory httpClientFactory,MyDbcontext myDbcontext)
        {
            _httpClient = httpClientFactory.CreateClient();
            _myDbContext = myDbcontext;
        }
        //載入所有最新匯率資料
        [HttpGet("insert-current-price", Name = "GetBTCPrice")]
        public async Task<IActionResult> Get()
        {
            var requestUrl = "https://api.coindesk.com/v1/bpi/currentprice.json";
            var response = await _httpClient.GetAsync(requestUrl);
            var data = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(data);

            //BTCPrice currentBTCPrice = new()
            //{
            //    UpdateTime = json["time"]["updatedISO"].Value<DateTime>(),
            //    USDChineseName = "美金",
            //    USDRate = json["bpi"]["USD"]["rate"].Value<double>(),
            //    GBPChineseName = "英鎊",
            //    GBPRate = json["bpi"]["GBP"]["rate"].Value<double>(),
            //    EURChineseName = "歐元",
            //    EURRate = json["bpi"]["EUR"]["rate"].Value<double>()
            //};

            //_myDbcentext.BTCPrices.Add(currentBTCPrice);
            //_myDbcentext.SaveChanges();
            //return Ok();

            var currencies = _myDbContext.Currencies.ToList();
            var updateTime = json["time"]["updatedISO"].Value<DateTime>();

            foreach (var currency in currencies)
            {
                var rate = json["bpi"][currency.Code]["rate_float"].Value<double>();
                BTCPrice currentBTCPrice = new BTCPrice
                {
                    UpdateTime = updateTime,//json["time"]["updatedISO"].Value<DateTime>(),
                    CurrencyCode = currency.Code,
                    CurrencyChineseName = currency.ChineseName,
                    Rate = rate
                };

                _myDbContext.BTCPrices.Add(currentBTCPrice);
            }

            await _myDbContext.SaveChangesAsync();
            return Ok();
        }

        //讀取已載入的所有最新匯率資料，按照幣別英文代碼排序
        [HttpGet("get-all-prices", Name = "AllBTCPrices")]
        public ActionResult<IEnumerable<BTCPrice>> GetAllBTCPrices()
        {
            //return _myDbContext.BTCPrices.ToList();
            var distinctPrices = _myDbContext.BTCPrices
                                   .GroupBy(p => p.CurrencyCode)
                                   .Select(group => group.OrderByDescending(p => p.Id).First())
                                   .ToList();
            //return distinctPrices;
            var formattedTime = distinctPrices.Select(p => new
            {
                Id = p.Id,
                UpdateTime = p.UpdateTime.ToString("yyyy/MM/dd HH:mm:ss"),
                CurrencyCode = p.CurrencyCode,
                CurrencyChineseName = p.CurrencyChineseName,
                Rate = p.Rate
            });

            return Ok(formattedTime);
        }
        //刪除已載入的匯率資料
        [HttpDelete("{id}", Name = "DeleteBTCPrice")]
        public IActionResult Delete(int id)
        {
            var btcPrice = _myDbContext.BTCPrices.Find(id);
            if (btcPrice == null)
            {
                return NotFound();
            }

            _myDbContext.BTCPrices.Remove(btcPrice);
            _myDbContext.SaveChanges();
            return NoContent();
        }
        
    }
}
