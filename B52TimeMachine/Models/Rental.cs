using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Models
{
    public class Rental
    {
        public int RentalId { get; set; }

        [Required]
        public Ps Ps { get; set; }

        public DateTime From { get; set; }

        public DateTime? To { get; set; }

        public int RentalFee { get; set; }

        public int ServiceFee { get; set; }
    }
}