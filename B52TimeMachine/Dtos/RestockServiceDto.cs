using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Dtos
{
    public class RestockServiceDto
    {
        public int RestockServiceId { get; set; }
        public string RestockDate { get; set; }
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
    }
}