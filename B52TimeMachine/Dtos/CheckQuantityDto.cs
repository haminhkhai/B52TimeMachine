using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Dtos
{
    public class CheckQuantityDto
    {
        public int CheckQuantityId { get; set; }

        public int Margin { get; set; }

        public int TotalQuantity { get; set; }

        public string CheckDate { get; set; }
    }
}