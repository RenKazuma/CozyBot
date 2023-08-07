using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
using ForeignKeyAttribute = System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute;

namespace Discord_Core.Database.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        [Column("Id", Order = 1)]
        public long Id { get; set; }
        
        [ForeignKey("Level")]
        public long Current_Level { get; set; }

        public int Current_Exp { get; set; }

        [Required]
        [Unique]
        public string DiscordId { get; set; }

        public int Coins { get; set; }
        
    }
}
