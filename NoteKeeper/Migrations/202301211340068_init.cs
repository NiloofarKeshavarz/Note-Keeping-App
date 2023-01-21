namespace NoteKeeper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Note",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Body = c.String(nullable: false, unicode: false, storeType: "text"),
                        CreationTime = c.DateTime(nullable: false),
                        LastModificationDate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NoteTag",
                c => new
                    {
                        noteId = c.Int(nullable: false),
                        tagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.noteId, t.tagId })
                .ForeignKey("dbo.Note", t => t.noteId, cascadeDelete: true)
                .ForeignKey("dbo.Tag", t => t.tagId, cascadeDelete: true)
                .Index(t => t.noteId)
                .Index(t => t.tagId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Note", "UserId", "dbo.User");
            DropForeignKey("dbo.NoteTag", "tagId", "dbo.Tag");
            DropForeignKey("dbo.NoteTag", "noteId", "dbo.Note");
            DropIndex("dbo.NoteTag", new[] { "tagId" });
            DropIndex("dbo.NoteTag", new[] { "noteId" });
            DropIndex("dbo.Note", new[] { "UserId" });
            DropTable("dbo.NoteTag");
            DropTable("dbo.User");
            DropTable("dbo.Tag");
            DropTable("dbo.Note");
        }
    }
}
