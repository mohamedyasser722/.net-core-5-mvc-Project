using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name can not exceed 50 characters")]
        [MinLength(5, ErrorMessage = "Name must be at least 5 characters")]
        public string Name { get; set; }

        [Range(22, 35, ErrorMessage = "Age must be between 22 and 35")]
        public int? Age { get; set; }
        [RegularExpression(@"^\d{1,5}-[A-Za-z0-9\s]+-[A-Za-z0-9\s]+-[A-Za-z0-9\s]+$"
                , ErrorMessage = "Address Must Be Like 123-Street-City-Country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        //[Column(TypeName = "decimal(18,2)")] 
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }

        public DateTime HireDate { get; set; }
        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        [InverseProperty("Employees")]
        public Department Department { get; set; }

    }
}
