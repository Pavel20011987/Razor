using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace razor.Models
{
    public class Vlan
    {
        public int VlanID { get; set; }
        [Required]
        [Range(1221, 1250)]
        [Column("Vlan")]
        [Display(Name = "Vlan")]
        public int vlan { get; set; }
        public ICollection<NetworkAssignment> NetworkAssignments { get; set; }
    }
}