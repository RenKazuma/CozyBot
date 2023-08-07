using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
using ForeignKeyAttribute = System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute;

namespace Discord_Core.Database.Entities
{
    [Table("Poll")]
    public class Poll
    {
        [Key]
        [Column("Id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        [Required]
        public bool multiple_choice { get; set; }

        [Required]
        [Unique]
        public string messageId { get; set; }

        [Required]
        public string guildId { get; set; }
        
    }
}
