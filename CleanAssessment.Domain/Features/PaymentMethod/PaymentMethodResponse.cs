using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Domain.Features.PaymentMethod
{
    public class PaymentMethodResponse
    {
        public int PaymentMethodId { get; set; }
        public string? NickName { get; set; }
        public int PaymentMethodTypeId { get; set; }
        public int OwnerId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public required string PaymentMethodTypeCode { get; set; }
        public required string PaymentMethodTypeDesc { get; set; }
    }
}
