using MyERP.Domain.Entities.Finance;
using System.ComponentModel.DataAnnotations.Schema;

public class Payment
{
    public int Id { get; set; }

    [ForeignKey("Invoice")]
    public int InvoiceId { get; set; }
    public virtual Invoice Invoice { get; set; }

    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    // Accountability
    public int? ProcessedByUserId { get; set; }
}
