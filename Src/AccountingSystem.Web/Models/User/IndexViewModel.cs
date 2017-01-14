using AccountingSystem.Entity;
using System.Collections.Generic;

namespace AccountingSystem.Models.User
{
    public class IndexViewModel
    {
        public AccountingSystem.Entity.User User { get; set; }
        public IList<StockViewModel> UserStockList { get; set; }

    }

    public class StockViewModel
    {
        public StockCode StockCode { get; set; }
        public int Prices { get; set; }
    }
}
