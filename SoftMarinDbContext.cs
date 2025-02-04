using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace SoftMarine
{
    public class SoftMarinDbContext : DbContext
    {
        public SoftMarinDbContext() : base("SOFT-MARINE") { }

        public DbSet<Inspection> Inspections { get; set; }
        public DbSet<Remark> Remarks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Настройка связи один-ко-многим между Inspection и Remark
            modelBuilder.Entity<Inspection>()
                .HasMany(i => i.Remarks)
                .WithRequired(r => r.Inspection)
                .HasForeignKey(r => r.InspectionId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
