using KOPracticeWithApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOPracticeWithApi.Data
{
    public class KOPracticeContext : DbContext
    {
        public KOPracticeContext(DbContextOptions<KOPracticeContext> options) : base (options) 
        { }
        public DbSet<ItemModel> Items { get; set; }
    }
}
