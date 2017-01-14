using System.Collections.Generic;
using AccountingSystem.Entity;

namespace AccountingSystem.Models.User
{
    public class AddStockViewModel
    {
        public IList<StockCode> UserStockCodes { get; set; }
        public IEnumerable<StockCode> AllStockCodes { get; set; }
    }
}
