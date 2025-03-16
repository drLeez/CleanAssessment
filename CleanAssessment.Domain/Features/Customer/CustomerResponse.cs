using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Domain.Features.Customer
{
    public class CustomerResponse
    {
        public int CustomerId { get; set; }
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }
        public int NameNumber { get; set; }
        public string FullName
        {
            get
            {
                var ret = string.IsNullOrEmpty(MiddleName) ? $"{FirstName} {LastName}" : $"{FirstName} {MiddleName} {LastName}";
                if (NameNumber > 1) ret += $" - #{NameNumber}";
                return ret;
            }
        }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
    }
}
