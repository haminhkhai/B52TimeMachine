using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Models
{
    public class ServiceRental
    {
        public int ServiceRentalId { get; set; }

        public Service Service { get; set; }

        public int ServiceId { get; set; }

        public Rental Rental { get; set; }

        public int RentalId { get; set; }

        public int Quantity { get; set; }

    }
}