using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Dtos
{
    public class RentalDto
    {
        public int RentalId { get; set; }
        public int PsId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int ServiceFee { get; set; }
        public int RentalFee { get; set; }
    }
}