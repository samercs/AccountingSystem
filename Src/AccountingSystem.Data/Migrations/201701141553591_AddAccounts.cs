namespace AccountingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccounts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserStockCodes", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserStockCodes", "StockCode_StockCodeId", "dbo.StockCodes");
            DropIndex("dbo.UserStockCodes", new[] { "User_Id" });
            DropIndex("dbo.UserStockCodes", new[] { "StockCode_StockCodeId" });
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentId = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        InitBalance = c.Decimal(nullable: false, precision: 18, scale: 3),
                        CreatedUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AccountId);
            
            DropTable("dbo.Languages");
            DropTable("dbo.StockCodes");
            DropTable("dbo.UserStockCodes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserStockCodes",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        StockCode_StockCodeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.StockCode_StockCodeId });
            
            CreateTable(
                "dbo.StockCodes",
                c => new
                    {
                        StockCodeId = c.Int(nullable: false, identity: true),
                        CodeName = c.String(nullable: false, maxLength: 50),
                        CreatedUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StockCodeId);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        LanguageId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.LanguageId);
            
            DropTable("dbo.Accounts");
            CreateIndex("dbo.UserStockCodes", "StockCode_StockCodeId");
            CreateIndex("dbo.UserStockCodes", "User_Id");
            AddForeignKey("dbo.UserStockCodes", "StockCode_StockCodeId", "dbo.StockCodes", "StockCodeId", cascadeDelete: true);
            AddForeignKey("dbo.UserStockCodes", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
