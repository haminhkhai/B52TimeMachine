using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Models
{
    public class RestockService
    {
        public int RestockServiceId { get; set; }
        public Restock Restock { get; set; }
        public Service Service { get; set; }
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
    }
}