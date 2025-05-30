
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fin.Core.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string ExternalId { get; set; } = string.Empty; // External ID from the banking API used when syncing
        public string? Description { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public Account Account { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public bool IsDebit { get; set; } 
    }
}
