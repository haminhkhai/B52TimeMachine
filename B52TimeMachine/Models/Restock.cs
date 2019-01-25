using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Models
{
    public class Restock
    {
        public int RestockId { get; set; }

        [Required]
        public int Total { get; set; }

        [Required]
        public DateTime RestockDate { get; set; }
    }
}