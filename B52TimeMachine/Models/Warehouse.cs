using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Models
{
    public class Warehouse
    {
        public int WarehouseId { get; set; }
        public Service Service { get; set; }
        public int Quantity { get; set; }
    }
}