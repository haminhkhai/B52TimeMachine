using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Models
{
    public class CheckQuantityDetail
    {
        public int CheckQuantityDetailId { get; set; }
        public CheckQuantity CheckQuantity { get; set; }
        public Service Service { get; set; }
        public int ServiceId { get; set; }
        public int OldQuantity { get; set; }
        public int CurrentQuantity { get; set; }
        public int Margin { get; set; }
    }
}