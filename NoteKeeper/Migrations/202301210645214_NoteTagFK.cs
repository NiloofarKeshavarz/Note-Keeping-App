namespace NoteKeeper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoteTagFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Note", "UserId", "dbo.User");
            DropForeignKey("dbo.NoteTag", "noteId", "dbo.Note");
            DropForeignKey("dbo.NoteTag", "tagId", "dbo.Tag");
            DropPrimaryKey("dbo.Note");
            DropPrimaryKey("dbo.Tag");
            DropPrimaryKey("dbo.User");
            AlterColumn("dbo.Note", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Tag", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.User", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Note", "Id");
            AddPrimaryKey("dbo.Tag", "Id");
            AddPrimaryKey("dbo.User", "Id");
            AddForeignKey("dbo.Note", "UserId", "dbo.User", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NoteTag", "noteId", "dbo.Note", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NoteTag", "tagId", "dbo.Tag", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NoteTag", "tagId", "dbo.Tag");
            DropForeignKey("dbo.NoteTag", "noteId", "dbo.Note");
            DropForeignKey("dbo.Note", "UserId", "dbo.User");
            DropPrimaryKey("dbo.User");
            DropPrimaryKey("dbo.Tag");
            DropPrimaryKey("dbo.Note");
            AlterColumn("dbo.User", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Tag", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Note", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.User", "Id");
            AddPrimaryKey("dbo.Tag", "Id");
            AddPrimaryKey("dbo.Note", "Id");
            AddForeignKey("dbo.NoteTag", "tagId", "dbo.Tag", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NoteTag", "noteId", "dbo.Note", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Note", "UserId", "dbo.User", "Id");
        }
    }
}
