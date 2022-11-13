using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace razor.Models
{
    public class NetworkAssignment
    {
        public int NetworkAssignmentID { get; set; }
        public int NetworkID { get; set; }
        public int VlanID { get; set; }
        public int VendorID { get; set; }

        public Network Network { get; set; }
        public Vlan Vlan { get; set; }
        public Vendor Vendor { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }
        
        [Required]
        [StringLength(15, ErrorMessage = "IP cannot be longer than 15 characters.")]
        [RegularExpression(@"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$", ErrorMessage = "The field must match X.X.X.X (X = [0 - 255])")]
        [Column("MNGMNT")]
        [Display(Name = "MNGNMT")]
        public string networkMngmnt { get; set; }

        [Required]     
        [Column("InUse")]
        
        public Boolean InUse { get; set; } = true;   

        public string arp { get; set; }
        
        /* Arp int INT создал для нормальной натуральной сортировки*/
        [Column("arpInInt")]
        [Display(Name = "arpInInt")]        
        public int arpInInt { get; set; }
        public DateTime arpUpdate { get; set; }

    }
}