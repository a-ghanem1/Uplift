using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Uplift.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderHeaderId { get; set; }

        [ForeignKey(nameof(OrderHeaderId))]
        public OrderHeader OrderHeader { get; set; }


        [Required]
        public int ServiceId { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public Service Service { get; set; }

        [Required]
        [StringLength(50)]
        public string ServiceName { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
