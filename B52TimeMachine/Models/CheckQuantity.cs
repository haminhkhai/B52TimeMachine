using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Models
{
    public class CheckQuantity
    {
        public int CheckQuantityId { get; set; }

        [Required]
        public int Margin { get; set; }

        [Required]
        public int TotalQuantity { get; set; }

        [Required]
        public DateTime CheckDate { get; set; }
    }
}