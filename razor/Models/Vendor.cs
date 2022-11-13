using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace razor.Models
{
    public class Vendor
    {
        public int VendorID { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Mask cannot be longer than 15 characters.")]
        [Column("Vendor")]
        [Display(Name = "Vendor")]
        public string vendor { get; set; }
        public ICollection<NetworkAssignment> NetworkAssignments { get; set; }
    }
}