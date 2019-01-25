using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Dtos
{
    public class CheckQuantityDetailDto
    {
        public int ServiceId { get; set; }
        public int OldQuantity { get; set; }
        public int CurrentQuantity { get; set; }
        public int Margin { get; set; }
    }
}