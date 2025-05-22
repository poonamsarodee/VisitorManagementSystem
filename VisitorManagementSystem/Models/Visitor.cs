using System;
using System.ComponentModel.DataAnnotations;

namespace VisitorManagementSystem.Models
{
    public class Visitor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9._%+-]*@(gmail\.com|yahoo\.com|hotmail\.com|outlook\.com|rediffmail\.com|icloud\.com)$",
    ErrorMessage = "Email must be valid and from domains like gmail.com, yahoo.com, hotmail.com, etc.")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        [RegularExpression(@"^[6-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit mobile number starting with 6-9")]
        public string ContactNumber { get; set; }


        [Required(ErrorMessage = "Visit Date is required")]
        [DataType(DataType.Date)]
        [CustomDateValidation(ErrorMessage = "Visit Date cannot be in the future")]
        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "Purpose is required")]
        public string Purpose { get; set; }

        public string PhotoPath { get; set; }

        // ✅ Custom validator for VisitDate
        public class CustomDateValidationAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value is DateTime date)
                {
                    return date.Date <= DateTime.Now.Date;
                }
                return false;
            }
        }
    }
}
