namespace WebLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AltKeyOrderBooks : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.OrderBooks", name: "BookId", newName: "BooksId");
            RenameColumn(table: "dbo.OrderBooks", name: "UserId", newName: "UsersId");
            RenameIndex(table: "dbo.OrderBooks", name: "IX_UserId", newName: "IX_UsersId");
            RenameIndex(table: "dbo.OrderBooks", name: "IX_BookId", newName: "IX_BooksId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.OrderBooks", name: "IX_BooksId", newName: "IX_BookId");
            RenameIndex(table: "dbo.OrderBooks", name: "IX_UsersId", newName: "IX_UserId");
            RenameColumn(table: "dbo.OrderBooks", name: "UsersId", newName: "UserId");
            RenameColumn(table: "dbo.OrderBooks", name: "BooksId", newName: "BookId");
        }
    }
}
