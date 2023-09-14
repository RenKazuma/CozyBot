using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
using ForeignKeyAttribute = System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute;
using System.Diagnostics.CodeAnalysis;

namespace Discord_Core.Database.Entities
{
    [Table("WaitingList")]
    public class WaitingList
    {
        [Key]
        [Column("Id", Order = 1)]
        public long Id { get; set; }
        
        [Required]
        [ForeignKey("User")]
        public long UserId {get; set;}
        
        [Required]
        [ForeignKey("Buff")]
        public long BuffId {get; set;}
        
        [Required]
        [ForeignKey("Timer")]
        public long TimerId {get; set;}

        //Keys
        
        [ForeignKey("UserId")]
        public User User {get; set;}

        [ForeignKey("BuffId")]
        public Buff Buff {get; set;}

        [ForeignKey("TimerId")]
        public Timer Timer {get; set;}
    }
}
