using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using B52TimeMachine.Models;

namespace B52TimeMachine.Dtos
{
    public class ServiceCheckoutDto
    {
        public ServiceDto Service { get; set; }
        public int RentalId { get; set; }
        public int Quantity { get; set; }
        
    }
}