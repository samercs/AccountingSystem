using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AccountingSystem.Controllers
{
    [Authorize]
    public class StockController : ApiController
    {
        public IHttpActionResult Get([FromUri] int[] codes)
        {

            var result = new List<StockResponse>();
            var random = new Random();
            foreach (var code in codes)
            {
                result.Add(new StockResponse()
                {
                    StockCodeId = code,
                    Prices = random.Next(1, 1000)
                });
            }
            return Ok(result);
        }
    }

    public class StockResponse
    {
        public int StockCodeId { get; set; }
        public int Prices { get; set; }
    }
}
