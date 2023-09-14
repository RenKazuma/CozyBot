using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
using ForeignKeyAttribute = System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute;
using System.Diagnostics.CodeAnalysis;

namespace Discord_Core.Database.Entities
{
    [Table("Timer")]
    public class Timer
    {
        [Key]
        [Column("Id", Order = 1)]
        public long Id { get; set; }
        
        [Unique]
        [Required]
        [NotNull]
        public string Name {get; set;}
        
        [Required]
        [NotNull]
        public int TimerAmount {get; set;}
        
    }
}
