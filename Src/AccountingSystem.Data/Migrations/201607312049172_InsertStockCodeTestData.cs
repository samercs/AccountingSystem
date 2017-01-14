using System.Collections.Generic;

namespace AccountingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsertStockCodeTestData : DbMigration
    {
        public override void Up()
        {
            IList<string>  testCodes = new List<string>()
            {
                "HPQ - Hewlett-Packard Company",
                "FB - Facebook",
                "YHOO - Yahoo! Inc",
                "MA - Master Card",
                "INCT - Intel ",
                "AAPL - Apple",
                "MSFT - Microsoft",
                "DELL - Dell Inc",
                "IBM - nternational Business Machines Corp",
                "GOOAV - Google Inc"
            };
            foreach (string code in testCodes)
            {
                Sql($"insert into StockCodes(CodeName, CreatedUtc) values('{code}', '{DateTime.Now}')");
            }
        }
        
        public override void Down()
        {
        }
    }
}
