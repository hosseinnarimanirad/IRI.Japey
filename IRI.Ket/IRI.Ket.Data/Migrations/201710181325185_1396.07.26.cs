namespace IRI.Ket.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13960726 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BoundaryWm = c.Geometry(),
                        Name = c.String(nullable: false),
                        LocationWm = c.Geometry(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Counties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ProvinceName = c.String(),
                        BoundaryWm = c.Geometry(nullable: false),
                        Center_Id = c.Int(),
                        Province_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.Center_Id)
                .ForeignKey("dbo.Provinces", t => t.Province_Id)
                .Index(t => t.Center_Id)
                .Index(t => t.Province_Id);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ProvinceName = c.String(),
                        CountryName = c.String(),
                        BoundaryWm = c.Geometry(),
                        Center_Id = c.Int(),
                        County_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.Center_Id)
                .ForeignKey("dbo.Counties", t => t.County_Id)
                .Index(t => t.Center_Id)
                .Index(t => t.County_Id);
            
            CreateTable(
                "dbo.RuralDistricts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CountryName = c.String(),
                        ProvinceName = c.String(),
                        DistrictName = c.String(),
                        BoundaryWm = c.Geometry(nullable: false),
                        Center_Id = c.Int(),
                        District_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Villages", t => t.Center_Id)
                .ForeignKey("dbo.Districts", t => t.District_Id)
                .Index(t => t.Center_Id)
                .Index(t => t.District_Id);
            
            CreateTable(
                "dbo.Villages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BoundaryWm = c.Geometry(),
                        Name = c.String(nullable: false),
                        ProvinceName = c.String(),
                        CountryName = c.String(),
                        DistrictName = c.String(),
                        RuralDistrictName = c.String(),
                        LocationWm = c.Geometry(nullable: false),
                        RuralDistrict_Id = c.Int(),
                        RuralDistrict_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RuralDistricts", t => t.RuralDistrict_Id)
                .ForeignKey("dbo.RuralDistricts", t => t.RuralDistrict_Id1)
                .Index(t => t.RuralDistrict_Id)
                .Index(t => t.RuralDistrict_Id1);
            
            CreateTable(
                "dbo.Provinces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        BoundaryWm = c.Geometry(nullable: false),
                        Center_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.Center_Id)
                .Index(t => t.Center_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Counties", "Province_Id", "dbo.Provinces");
            DropForeignKey("dbo.Provinces", "Center_Id", "dbo.Cities");
            DropForeignKey("dbo.Districts", "County_Id", "dbo.Counties");
            DropForeignKey("dbo.Villages", "RuralDistrict_Id1", "dbo.RuralDistricts");
            DropForeignKey("dbo.RuralDistricts", "District_Id", "dbo.Districts");
            DropForeignKey("dbo.RuralDistricts", "Center_Id", "dbo.Villages");
            DropForeignKey("dbo.Villages", "RuralDistrict_Id", "dbo.RuralDistricts");
            DropForeignKey("dbo.Districts", "Center_Id", "dbo.Cities");
            DropForeignKey("dbo.Counties", "Center_Id", "dbo.Cities");
            DropIndex("dbo.Provinces", new[] { "Center_Id" });
            DropIndex("dbo.Villages", new[] { "RuralDistrict_Id1" });
            DropIndex("dbo.Villages", new[] { "RuralDistrict_Id" });
            DropIndex("dbo.RuralDistricts", new[] { "District_Id" });
            DropIndex("dbo.RuralDistricts", new[] { "Center_Id" });
            DropIndex("dbo.Districts", new[] { "County_Id" });
            DropIndex("dbo.Districts", new[] { "Center_Id" });
            DropIndex("dbo.Counties", new[] { "Province_Id" });
            DropIndex("dbo.Counties", new[] { "Center_Id" });
            DropTable("dbo.Provinces");
            DropTable("dbo.Villages");
            DropTable("dbo.RuralDistricts");
            DropTable("dbo.Districts");
            DropTable("dbo.Counties");
            DropTable("dbo.Cities");
        }
    }
}
