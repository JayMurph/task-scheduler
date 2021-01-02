namespace task_scheduler_data_access.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lessAnnotations : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.NotificationFrequencies", newName: "NotificationFrequencyDALs");
            RenameTable(name: "dbo.TaskItems", newName: "TaskItemDALs");
            AlterColumn("dbo.TaskItemDALs", "Title", c => c.String());
            AlterColumn("dbo.TaskItemDALs", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaskItemDALs", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.TaskItemDALs", "Title", c => c.String(nullable: false, maxLength: 200));
            RenameTable(name: "dbo.TaskItemDALs", newName: "TaskItems");
            RenameTable(name: "dbo.NotificationFrequencyDALs", newName: "NotificationFrequencies");
        }
    }
}
