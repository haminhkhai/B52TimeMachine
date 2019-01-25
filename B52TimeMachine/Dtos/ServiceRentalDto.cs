using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Dtos
{
    public class ServiceRentalDto
    {
        public int RentalId { get; set; }
        public List<ServiceRentalAddingProps> ServiceRentals { get; set; }
    }

    public class ServiceRentalAddingProps
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int SumPrice { get; set; }
    }
}