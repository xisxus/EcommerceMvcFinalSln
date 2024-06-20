namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class iniy : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Unit = c.Int(),
                        Image = c.String(),
                        CategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Contact = c.String(),
                        Address = c.String(),
                        Unit = c.Int(nullable: false),
                        Qty = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        InvoiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.InvoiceId);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvoiceId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Bill = c.Int(nullable: false),
                        Payment = c.String(),
                        InvoiceDate = c.DateTime(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.InvoiceId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        RoleType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.UserInvoices",
                c => new
                    {
                        InvoiceId = c.Int(nullable: false, identity: true),
                        Customer = c.String(),
                        Bill = c.Int(nullable: false),
                        Payment = c.String(),
                        InvoiceDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.InvoiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Invoices", "UserId", "dbo.Users");
            DropForeignKey("dbo.Orders", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Invoices", new[] { "UserId" });
            DropIndex("dbo.Orders", new[] { "InvoiceId" });
            DropIndex("dbo.Orders", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropTable("dbo.UserInvoices");
            DropTable("dbo.Users");
            DropTable("dbo.Invoices");
            DropTable("dbo.Orders");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
        }
    }
}
