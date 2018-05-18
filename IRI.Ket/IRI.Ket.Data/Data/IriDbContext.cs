using IRI.Ket.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Data.Data
{
    public class IriDbContext : DbContext
    {
        const string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"E:\\Programming\\100. IRI.Japey\\IRI.Ket\\IRI.Ket.Data\\Data\\IRI.Db.mdf\";Integrated Security=True;";

        public IriDbContext() : base(connectionString)
        {
            Database.SetInitializer(new IriDbInitializer());

            Database.Initialize(true);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<District>().HasOptional(m => m.Center).WithOptionalPrincipal();

            //modelBuilder.Entity<City>().HasOptional(m => m.District).WithOptionalDependent();

            //modelBuilder.Entity<City>().HasRequired(m=>m.District).WithMany(d=>d.ci)
        }

        public DbSet<Model.City> Cities93Wm { get; set; }

        public DbSet<Model.Province> Provinces93Wm { get; set; }

        public DbSet<Model.County> Counties93Wm { get; set; }

        public DbSet<Model.District> Districts93Wm { get; set; }

        public DbSet<Model.RuralDistrict> RuralDistricts93Wm { get; set; }

        public DbSet<Model.Village> Villages93Wm { get; set; }
    }

    public class IriDbInitializer : DropCreateDatabaseAlways<IriDbContext>
    {
        protected override void Seed(IriDbContext context)
        {
            base.Seed(context);

            context.Provinces93Wm.AddRange(Feeds.AdminFeeder.GetProvinces93Wm());

            context.Counties93Wm.AddRange(Feeds.AdminFeeder.GetCounties93Wm());

            context.Districts93Wm.AddRange(Feeds.AdminFeeder.GetDistricts93Wm());

            context.SaveChanges();
        }
    }
}
