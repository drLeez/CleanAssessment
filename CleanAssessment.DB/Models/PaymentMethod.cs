using CleanAssessment.Shared.Bases;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CleanAssessment.DB.Models
{    
    [Serializable]
    public record PaymentMethod : Entity<int>
    {
        [Key]
        public int PaymentMethodId { get; set; }
        public string? NickName { get; set; }
        [Required]
        public int PaymentMethodTypeId { get; set; }
        [Required]
        public int OwnerId { get; set; }
        [Required]
        public int ExpirationDateId { get; set; }
        [Required]
        public required string PaymentMethodTypeCode { get; set; }
        [Required]
        public required string PaymentMethodTypeDesc { get; set; }
    }
}
