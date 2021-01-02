namespace task_scheduler_data_access.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTask : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotificationFrequencies",
                c => new
                    {
                        NotificationFrequencyDALId = c.Guid(nullable: false),
                        Frequency = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.NotificationFrequencyDALId)
                .ForeignKey("dbo.TaskItems", t => t.NotificationFrequencyDALId)
                .Index(t => t.NotificationFrequencyDALId);
            
            CreateTable(
                "dbo.TaskItems",
                c => new
                    {
                        TaskItemDALId = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TaskItemDALId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotificationFrequencies", "NotificationFrequencyDALId", "dbo.TaskItems");
            DropIndex("dbo.NotificationFrequencies", new[] { "NotificationFrequencyDALId" });
            DropTable("dbo.TaskItems");
            DropTable("dbo.NotificationFrequencies");
        }
    }
}
