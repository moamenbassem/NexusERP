using MyERP.Domain.Entities.Inventory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyERP.Domain.Entities.Finance
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required]
        public string InvoiceNumber { get; set; } 

        public decimal AmountDue { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }

        public InvoiceStatus Status { get; set; } = InvoiceStatus.Unpaid;

        public virtual Payment? Payment { get; set; }

        public bool IsSettled { get; set; } = false;

    }

    public enum InvoiceStatus
    {
        Unpaid,
        Paid,
        Overdue,
        Canceled
    }
}
