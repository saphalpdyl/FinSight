using System.ComponentModel.DataAnnotations;

namespace Fin.Core.Entities
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        // Used to stale the cache for transactions
        public DateTime TransactionsCachedUntilDateTime { get; set; } = DateTime.MinValue;

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
