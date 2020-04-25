using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Uplift.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(11)]
        public string Phone { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }
        
        [Required]
        [StringLength(256)]
        public string Address { get; set; }
        
        [Required]
        [StringLength(50)]
        public string City { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ZipCode { get; set; }
        
        public DateTime OrderDate { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; }
        
        [StringLength(500)]
        public string Comments { get; set; }
        
        public int ServiceCount { get; set; }
    }
}
