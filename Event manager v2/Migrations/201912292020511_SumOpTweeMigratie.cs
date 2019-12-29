namespace Event_manager_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SumOpTweeMigratie : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activiteit",
                c => new
                    {
                        activiteit_id = c.Int(nullable: false, identity: true),
                        naam = c.String(nullable: false, maxLength: 100, unicode: false),
                        beschrijving = c.String(nullable: false, unicode: false, storeType: "text"),
                        evenement = c.Int(nullable: false),
                        begintijd = c.DateTime(nullable: false),
                        eindtijd = c.DateTime(nullable: false),
                        evenement_beheerder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.activiteit_id)
                .ForeignKey("dbo.Evenement", t => t.evenement)
                .ForeignKey("dbo.EvenementBeheerder", t => t.evenement_beheerder)
                .Index(t => t.evenement)
                .Index(t => t.evenement_beheerder);
            
            CreateTable(
                "dbo.Evenement",
                c => new
                    {
                        evenement_id = c.Int(nullable: false, identity: true),
                        naam = c.String(nullable: false, maxLength: 100, unicode: false),
                        beschrijving = c.String(nullable: false, unicode: false, storeType: "text"),
                        begindatum = c.DateTime(nullable: false, storeType: "date"),
                        einddatum = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.evenement_id);
            
            CreateTable(
                "dbo.Deelnemer",
                c => new
                    {
                        deelnemer_id = c.Int(nullable: false, identity: true),
                        voornaam = c.String(nullable: false, maxLength: 50, unicode: false),
                        achternaam = c.String(nullable: false, maxLength: 50, unicode: false),
                        email = c.String(nullable: false, maxLength: 100, unicode: false),
                        evenement = c.Int(nullable: false),
                        goedgekeurd = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.deelnemer_id)
                .ForeignKey("dbo.Evenement", t => t.evenement)
                .Index(t => t.evenement);
            
            CreateTable(
                "dbo.EvenementBeheerder",
                c => new
                    {
                        evenement_beheerder_id = c.Int(nullable: false, identity: true),
                        evenement = c.Int(nullable: false),
                        beheerder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.evenement_beheerder_id)
                .ForeignKey("dbo.Beheerder", t => t.beheerder)
                .ForeignKey("dbo.Evenement", t => t.evenement)
                .Index(t => t.evenement)
                .Index(t => t.beheerder);
            
            CreateTable(
                "dbo.Beheerder",
                c => new
                    {
                        beheerder_id = c.Int(nullable: false, identity: true),
                        voornaam = c.String(nullable: false, maxLength: 50, unicode: false),
                        achternaam = c.String(nullable: false, maxLength: 50, unicode: false),
                        gebruikersnaam = c.String(nullable: false, maxLength: 50, unicode: false),
                        wachtwoord = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.beheerder_id);
            
            CreateTable(
                "dbo.Wijziging",
                c => new
                    {
                        wijziging_id = c.Int(nullable: false, identity: true),
                        beheerder = c.Int(nullable: false),
                        type = c.Int(nullable: false),
                        naam = c.String(nullable: false, maxLength: 100, unicode: false),
                        beschrijving = c.String(nullable: false, unicode: false, storeType: "text"),
                        jsonData = c.String(nullable: false, unicode: false),
                        jsonClassType = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.wijziging_id)
                .ForeignKey("dbo.WijzigingsType", t => t.type)
                .ForeignKey("dbo.EvenementBeheerder", t => t.beheerder)
                .Index(t => t.beheerder)
                .Index(t => t.type);
            
            CreateTable(
                "dbo.WijzigingsType",
                c => new
                    {
                        type_id = c.Int(nullable: false, identity: true),
                        naam = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.type_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EvenementBeheerder", "evenement", "dbo.Evenement");
            DropForeignKey("dbo.Wijziging", "beheerder", "dbo.EvenementBeheerder");
            DropForeignKey("dbo.Wijziging", "type", "dbo.WijzigingsType");
            DropForeignKey("dbo.EvenementBeheerder", "beheerder", "dbo.Beheerder");
            DropForeignKey("dbo.Activiteit", "evenement_beheerder", "dbo.EvenementBeheerder");
            DropForeignKey("dbo.Deelnemer", "evenement", "dbo.Evenement");
            DropForeignKey("dbo.Activiteit", "evenement", "dbo.Evenement");
            DropIndex("dbo.Wijziging", new[] { "type" });
            DropIndex("dbo.Wijziging", new[] { "beheerder" });
            DropIndex("dbo.EvenementBeheerder", new[] { "beheerder" });
            DropIndex("dbo.EvenementBeheerder", new[] { "evenement" });
            DropIndex("dbo.Deelnemer", new[] { "evenement" });
            DropIndex("dbo.Activiteit", new[] { "evenement_beheerder" });
            DropIndex("dbo.Activiteit", new[] { "evenement" });
            DropTable("dbo.WijzigingsType");
            DropTable("dbo.Wijziging");
            DropTable("dbo.Beheerder");
            DropTable("dbo.EvenementBeheerder");
            DropTable("dbo.Deelnemer");
            DropTable("dbo.Evenement");
            DropTable("dbo.Activiteit");
        }
    }
}
