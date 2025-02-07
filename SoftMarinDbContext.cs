using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SoftMarine.Model;


namespace SoftMarine
{
    public class SoftMarinDbContext : DbContext
    {
        public SoftMarinDbContext() : base("SOFT-MARINE") { }

        public DbSet<Inspection> Inspections { get; set; }
        public DbSet<Remark> Remarks { get; set; }
        public DbSet<Inspector> Inspectors { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Настройка связи один-ко-многим между Inspection и Remark
            modelBuilder.Entity<Inspection>()
                .HasMany(i => i.Remarks)
                .WithRequired(r => r.Inspection)
                .HasForeignKey(r => r.InspectionId);

            // Один ко многим: Inspector -> Inspections
            modelBuilder.Entity<Inspection>()
                .HasRequired(i => i.Inspector)
                .WithMany(inspector => inspector.Inspections)
                .HasForeignKey(i => i.InspectorId)
                .WillCascadeOnDelete(false); // Чтобы избежать циклического удаления

            base.OnModelCreating(modelBuilder);
        }
    }
}
