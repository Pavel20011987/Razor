    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace razor.Models
    {
        public class Mask
        {
            public int MaskID { get; set; }
            [Required]
            [StringLength(15, ErrorMessage = "Mask cannot be longer than 15 characters.")]
            [Column("Mask")]
            [Display(Name = "Mask")]
            public string mask { get; set; }
            public ICollection<Network> Networks { get; set; }
        }
    }