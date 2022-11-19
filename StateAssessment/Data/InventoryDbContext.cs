using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StateAssessment.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace StateAssessment.Data
{
    public class InventoryDbContext:DbContext
    {
        public InventoryDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Inventory> Inventories { get; set; }       

    }
}
