using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace razor.Models
{
    public class Network
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Pool")]
        public int NetworkID { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Mask cannot be longer than 15 characters.")]
        [RegularExpression(@"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$", ErrorMessage = "The field must match X.X.X.X (X = [0 - 255])")]
        [Column("Network")]
        [Display(Name = "Network")]
        public string network { get; set; }
        [Required]
        [Range(0, 25000)]
        [Column("AllocatedIP")]
        [Display(Name = "Allocated IP")]
        public int allocatedIP { get; set; }
        public int MaskID {get;set;}

        
        public Mask Mask { get; set; }

        [Required]
        public Boolean Nomadizm { get; set; } 

        [Required]        
        public Boolean InUseNet { get; set; } = false;
        public ICollection<NetworkAssignment> NetworkAssignments { get; set; }
    }
}