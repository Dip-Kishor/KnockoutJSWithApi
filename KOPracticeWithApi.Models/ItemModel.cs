using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOPracticeWithApi.Models
{
    public class ItemModel
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [Column(TypeName = "Decimal(18,2)")]
        
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsPublished { get; set; }
    }
}
