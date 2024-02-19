using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Curd_Task_Kolte.Models
{
    
        public class Contact
        {
            public int ContactId { get; set; }

            [Required(ErrorMessage = "Name is required.")]
            [StringLength(100)]
            public string Name { get; set; }

            [StringLength(100)]
            public string City { get; set; }

            [Required(ErrorMessage = "Email is required.")]
            [StringLength(100)]
            [EmailAddress(ErrorMessage = "Invalid email address.")]
            public string Email { get; set; }

            [Display(Name = "Date of Birth")]
            [DataType(DataType.Date)]
            public string DOB { get; set; }

            [Required(ErrorMessage = "Contact number is required.")]
            [StringLength(13)]
            public string ContactNo { get; set; }
        }
  
}