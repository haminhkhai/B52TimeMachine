using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Models
{
    public class SplitTime
    {
        public int SplitTimeId { get; set; }

        [Required]
        public Rental Rental { get; set; }

        [Required]
        public DateTime TimeSplit { get; set; }
    }
}