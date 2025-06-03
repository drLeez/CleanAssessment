using CleanAssessment.Shared.Attributes;
using CleanAssessment.Shared.Bases;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.DB.Models
{
    [Serializable]
    public record Customer : Entity<int>
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public int DuplicateNumber { get; set; }
        [Required]
        public int AccDateId { get; set; }
        public int? Age { get; set; }
        public string? Address { get; set; }
    }
}
