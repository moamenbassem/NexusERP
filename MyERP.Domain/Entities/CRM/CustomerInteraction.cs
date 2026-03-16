using MyERP.Domain.Entities.HR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyERP.Domain.Entities.CRM
{
    public class CustomerInteraction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey("Employee")]
        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Note { get; set; } // e.g., "Customer called to check order status"

        public InteractionType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum InteractionType { Call, Email, Meeting, Note }
}
