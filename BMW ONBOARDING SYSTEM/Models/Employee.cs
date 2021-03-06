using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMW_ONBOARDING_SYSTEM.Models
{
    public class Employee
    {
        public Employee()
        {
            Onboarder = new List<Onboarder>();
        }

        [Key]
        public int EmployeeID { get; set; }

        [Column("DepartmentID")]
        public int? DepartmentId { get; set; }
        [Column("GenderID")]
        public int? GenderId { get; set; }
        [Column("AddressID")]
        public int? AddressId { get; set; }
        [Column("EmployeeCalendarID")]
        public int? EmployeeCalendarId { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [Column("IDNumber", TypeName = "numeric(18, 0)")]
        public decimal? Idnumber { get; set; }
        [StringLength(50)]
        public string EmailAddress { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal? ContactNumber { get; set; }
        [StringLength(50)]
        public string EmployeeJobTitle { get; set; }
        [Column("TitleID")]
        public int? TitleId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        [InverseProperty("Employee")]
        public virtual Department Department { get; set; }
        [ForeignKey(nameof(TitleId))]
        [InverseProperty("Employee")]
        public virtual Title Title { get; set; }


        public virtual List<Onboarder> Onboarder { get; set; }
    }
}
