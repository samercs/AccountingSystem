namespace AccountingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStockCodeUserRelation : DbMigration
    {
        public override void Up()
        {
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
                "dbo.UserStockCodes",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        StockCode_StockCodeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.StockCode_StockCodeId })
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.StockCodes", t => t.StockCode_StockCodeId, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.StockCode_StockCodeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserStockCodes", "StockCode_StockCodeId", "dbo.StockCodes");
            DropForeignKey("dbo.UserStockCodes", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserStockCodes", new[] { "StockCode_StockCodeId" });
            DropIndex("dbo.UserStockCodes", new[] { "User_Id" });
            DropTable("dbo.UserStockCodes");
            DropTable("dbo.StockCodes");
        }
    }
}
